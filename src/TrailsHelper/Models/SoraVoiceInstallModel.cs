using Octokit;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Handlers;
using System.Threading.Tasks;
using SharpCompress.Archives;
using SharpCompress.Readers;
using MonoTorrent;
using MonoTorrent.Client;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using TrailsHelper.Support;
using System.Threading;
using CG.Web.MegaApiClient;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

namespace TrailsHelper.Models
{
    static class SoraVoiceModelStringExtensions
    {
        public static string FormatTemplateString(this string @this, SoraVoiceInstallModel model)
            => @this.Replace("$prefix", model.ScriptPrefix)
            .Replace("$fname", model.BattleVoiceFile);
    }

    internal class SoraVoiceInstallModel : IDisposable
    {
        private static readonly string ManifestUri = "https://github.com/chyyran/skyinstaller/releases/latest/download/manifest.json";
        private bool disposedValue;

        private static Lazy<ClientEngine> TorrentClient = new(() =>
        {
            return new ClientEngine(new EngineSettingsBuilder()
            {
                CacheDirectory = Path.Combine(Environment.CurrentDirectory, $"skyinst_cache"),
            }.ToSettings());
        }, false);

        public SoraVoiceInstallModel(string modPrefix, string gamePath, string battleVoiceFile)
        {
            this.ScriptPrefix = modPrefix;
            this.BattleVoiceFile = battleVoiceFile;
            this.GamePath = gamePath;
            this.GithubClient = new GitHubClient(new ProductHeaderValue("trailshelper"));
            var handler = new HttpClientHandler() { AllowAutoRedirect = true };
            var ph = new ProgressMessageHandler(handler);

            ph.HttpReceiveProgress += (_, args) => this.ProgressChangedEvent?.Invoke(this, ((double)args.BytesTransferred / args.TotalBytes) * 100 ?? 0);
            this.HttpClient = new HttpClient(ph);
        }

        public string ScriptPrefix { get; }
        public string BattleVoiceFile { get; }
        public string GamePath { get; }
        public GitHubClient GithubClient { get; }
        public HttpClient HttpClient { get; }

        public event EventHandler<double>? ProgressChangedEvent;
        public event EventHandler<long>? SpeedChangedEvent;

        private async Task<Stream> TryBestDownloadHttp(IEnumerable<string> uris, CancellationToken cancel = default)
        {
            foreach (var uri in uris)
            {
                try
                {
                    var stream = await this.HttpClient.GetStreamAsync(uri, cancel);
                    return stream;
                }
                catch
                {
                    continue;
                }
            }
            throw new HttpRequestException("None of the provided URIs were avaialble.");
        }

        public async Task<DownloadManifest> DownloadManifest(CancellationToken cancel = default)
        {
            var stream = await this.HttpClient.GetStreamAsync(SoraVoiceInstallModel.ManifestUri, cancel);
            cancel.ThrowIfCancellationRequested();

            var manifest = await JsonSerializer.DeserializeAsync(stream, JsonContext.Default.DownloadManifest, cancel);
            return manifest!;
        }

        public async Task<Stream> DownloadLatestMod(DownloadManifest manifest, CancellationToken cancel = default)
        {
            var releases = await this.GithubClient.Repository.Release.GetLatest(manifest.Mod.Owner, manifest.Mod.Repository);
            var asset = releases.Assets.First(s => s.Name.StartsWith(manifest.Mod.Asset.FormatTemplateString(this)));
            var stream = await this.HttpClient.GetStreamAsync(asset.BrowserDownloadUrl, cancel);
            cancel.ThrowIfCancellationRequested();

            var outStream = new MemoryStream();
            await stream.CopyToAsync(outStream, cancel);
            await outStream.FlushAsync(cancel);
            outStream.Seek(0, SeekOrigin.Begin);
            return outStream;
        }

        public async Task<Stream> DownloadLatestScripts(DownloadManifest manifest, CancellationToken cancel = default)
        {
            var releases = await this.GithubClient.Repository.Release.GetLatest(manifest.Scripts.Owner, manifest.Scripts.Repository);
            var asset = releases.Assets.First(s => s.Name.StartsWith(manifest.Scripts.Asset.FormatTemplateString(this)));
            var stream = await this.HttpClient.GetStreamAsync(asset.BrowserDownloadUrl, cancel);
            cancel.ThrowIfCancellationRequested();
            
            var outStream = new MemoryStream();
            await stream.CopyToAsync(outStream, cancel);
            await outStream.FlushAsync(cancel);
            outStream.Seek(0, SeekOrigin.Begin);
            return outStream;
        }

        public async Task DownloadAndInstallBattleVoice(DownloadManifest manifest, string ext, CancellationToken cancel = default)
        {
            var directUris = manifest.Battle.DirectUris;
            if (directUris == null || !directUris.Any())
            {
                directUris = new() { manifest.Battle.Uri };
            }

            var stream = await this.TryBestDownloadHttp(directUris.Select(uri => uri.FormatTemplateString(this).Replace("$ext", ext)
                ), cancel);
            cancel.ThrowIfCancellationRequested();

            using var outStream = File.Open(Path.Combine(this.GamePath, $"{this.BattleVoiceFile}.{ext}"), System.IO.FileMode.Create);
            await stream.CopyToAsync(outStream, cancel);
            await outStream.FlushAsync(cancel);
            return;
        }

        public async Task<Stream> DownloadVoiceFromDirect(DownloadManifest manifest, CancellationToken cancel = default)
        {
            if (manifest.Voice.DirectUris == null || !manifest.Voice.DirectUris.Any())
            {
                throw new ArgumentException("Direct download URIs were not found.");
            }

            var stream = await this.TryBestDownloadHttp(manifest.Voice.DirectUris
               .Select(uri => uri.FormatTemplateString(this)), cancel);
                    cancel.ThrowIfCancellationRequested();
            string filename = Path.Combine(Environment.CurrentDirectory, $"skyinst_voices_{this.ScriptPrefix}_{Random.Shared.Next(100000, 1000000)}.7z");

            var outStream = File.Open(filename, new FileStreamOptions()
            {
                Mode = System.IO.FileMode.Create,
                Access = FileAccess.ReadWrite,
                BufferSize = 8096,
                Options = FileOptions.DeleteOnClose,
            });
            await stream.CopyToAsync(outStream, cancel);
            await outStream.FlushAsync(cancel);
            outStream.Seek(0, SeekOrigin.Begin);
            return outStream;

        }

        public async Task<Stream> DownloadVoiceFromMega(DownloadManifest manifest, CancellationToken cancel = default)
        {
            var key = manifest.Voice.Asset.FormatTemplateString(this);
            string megakey = manifest.Voice.Mega[key];
            string filename = Path.Combine(Environment.CurrentDirectory, $"skyinst_voices_{key}_{Random.Shared.Next(100000, 1000000)}.7z");
            var megaClient = new MegaApiClient(new Options(manifest.Voice.MegaApiKey));
            var megaUri = new Uri($"https://mega.nz/file/{megakey}");
            await megaClient.LoginAnonymousAsync();
            var megaNode = await megaClient.GetNodeFromLinkAsync(megaUri);
            await megaClient.DownloadFileAsync(megaNode, filename, new Progress<double>(
                d => this.ProgressChangedEvent?.Invoke(this, d)), cancel);
            return File.Open(filename, new FileStreamOptions()
            {
                Mode = System.IO.FileMode.Open,
                Access = FileAccess.Read,
                BufferSize = 8096,
                Options = FileOptions.DeleteOnClose,
            });
        }

        public async Task<TorrentManager> DownloadVoiceTorrentInfo(DownloadManifest manifest, CancellationToken cancel = default)
        {
            // http stream can't use random access.
            var torrentStreamHttp = await this.HttpClient.GetStreamAsync(manifest.Voice.Uri, cancel);
            cancel.ThrowIfCancellationRequested();

            using var torrentStream = new MemoryStream();
            await torrentStreamHttp.CopyToAsync(torrentStream, cancel);
            await torrentStream.FlushAsync(cancel);
            cancel.ThrowIfCancellationRequested();

            torrentStream.Seek(0, SeekOrigin.Begin);

            // since we are sharing the torrentclient make sure that
            // we don't re-add the torrent.
            var torrentInfo = await Torrent.LoadAsync(torrentStream);
            if (TorrentClient.Value.Contains(torrentInfo))
            {
                await TorrentClient.Value.RemoveAsync(torrentInfo);
            }

            var torrent = await TorrentClient.Value.AddAsync(
                await Torrent.LoadAsync(torrentStream), 
                Path.Combine(Environment.CurrentDirectory, $"skyinst_{this.ScriptPrefix}"),
                new TorrentSettingsBuilder()
                {
                    CreateContainingDirectory = false,
                    // always use webseeds to support IA torrents
                    WebSeedDelay = TimeSpan.Zero,
                    WebSeedSpeedTrigger = int.MaxValue,
                }.ToSettings());
            this.ProgressChangedEvent?.Invoke(this, torrent.Progress);

            var asset = manifest.Voice.Asset.FormatTemplateString(this);
            foreach (var file in torrent.Files)
            {
                if (!file.Path.StartsWith(asset))
                {
                    await torrent.SetFilePriorityAsync(file, Priority.DoNotDownload);
                }
            }
            await torrent.StartAsync();
            await torrent.WaitForMetadataAsync(cancel);
            return torrent;
        }

        public async Task<Stream> DownloadTorrent(DownloadManifest manifest, TorrentManager torrent, CancellationToken cancel = default)
        {
            var asset = manifest.Voice.Asset.FormatTemplateString(this);

            return await Task.Run(async () =>
            {
                while (torrent.State != TorrentState.Stopped && torrent.State != TorrentState.Paused && torrent.State != TorrentState.Seeding)
                {
                    if (cancel.IsCancellationRequested)
                    {
                        await torrent.StopAsync();
                        this.SpeedChangedEvent = null;
                        cancel.ThrowIfCancellationRequested();
                        break;
                    }
                    await Task.Delay(1000, cancel);
                    if (torrent.OpenConnections == 0)
                    {
                        await torrent.TrackerManager.AnnounceAsync(cancel);
                        await torrent.TrackerManager.ScrapeAsync(cancel);
                    }
                    this.SpeedChangedEvent?.Invoke(this, torrent.Monitor.DownloadRate);
                    this.ProgressChangedEvent?.Invoke(this, torrent.PartialProgress);
                }

                var voiceFiles = torrent.Files.Single(f => f.Path.StartsWith(asset));
                return File.OpenRead(voiceFiles.FullPath);
            });
        }

        public async Task ExtractToGameRoot(Stream modStream, CancellationToken cancel = default)
        {
            var archive = ArchiveFactory.Open(modStream);
            using var reader = archive.ExtractAllEntries();
            long totalSize = archive.TotalUncompressSize;
            long bytesUncompressed = 0;
            reader.EntryExtractionProgress += (_, args) =>
            {
                if (cancel.IsCancellationRequested)
                {
                    reader.Cancel();
                }
                bytesUncompressed += args.ReaderProgress?.BytesTransferred ?? 0;
                this.ProgressChangedEvent?.Invoke(this, bytesUncompressed / (double)totalSize);
            };

            await Task.Run(() =>
            {
                if (cancel.IsCancellationRequested)
                {
                    reader.Cancel();
                }

                reader.WriteAllToDirectory(this.GamePath, new()
                {
                    ExtractFullPath = true,
                    Overwrite = true,
                });
            }, cancel);
        }

        public async Task ExtractToVoiceFolder(Stream modStream, CancellationToken cancel = default)
        {
            var archive = ArchiveFactory.Open(modStream);
            using var reader = archive.ExtractAllEntries();
            long totalSize = archive.TotalUncompressSize;
            long bytesUncompressed = 0;
            reader.EntryExtractionProgress += (_, args) =>
            {
                if (cancel.IsCancellationRequested)
                {
                    reader.Cancel();
                }
                bytesUncompressed += args.ReaderProgress?.BytesTransferred ?? 0;
                this.ProgressChangedEvent?.Invoke(this, bytesUncompressed / (double)totalSize);
            };

            await Task.Run(() =>
            {
                reader.WriteAllToDirectory(Path.Combine(this.GamePath, "voice"), new()
                {
                    ExtractFullPath = true,
                    Overwrite = true,
                });
            }, cancel);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.HttpClient.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~SoraVoiceInstallModel()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
