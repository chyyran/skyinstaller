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
            this.TorrentClient = new ClientEngine(new EngineSettingsBuilder()
            {
                CacheDirectory = Path.Combine(Environment.CurrentDirectory, $"skyinst_cache_{this.ScriptPrefix}"),                
            }.ToSettings());
        }

        public string ScriptPrefix { get; }
        public string BattleVoiceFile { get; }
        public string GamePath { get; }
        public GitHubClient GithubClient { get; }
        public HttpClient HttpClient { get; }
        public ClientEngine TorrentClient { get; }

        public event EventHandler<double>? ProgressChangedEvent;

        public async Task<DownloadManifest> DownloadManifest(CancellationToken cancel = default)
        {
            var stream = await this.HttpClient.GetStreamAsync(SoraVoiceInstallModel.ManifestUri, cancel);
            cancel.ThrowIfCancellationRequested();

            var manifest = await JsonSerializer.DeserializeAsync(stream, JsonContext.Default.DownloadManifest);
            return manifest!;
        }

        public async Task<Stream> DownloadLatestMod(DownloadManifest manifest, CancellationToken cancel = default)
        {
            var releases = await this.GithubClient.Repository.Release.GetLatest(manifest.Mod.Owner, manifest.Mod.Repository);
            var asset = releases.Assets.First(s => s.Name.StartsWith(manifest.Mod.Asset.FormatTemplateString(this)));
            var stream = await this.HttpClient.GetStreamAsync(asset.BrowserDownloadUrl, cancel);
            cancel.ThrowIfCancellationRequested();

            var outStream = new MemoryStream();
            await stream.CopyToAsync(outStream);
            await outStream.FlushAsync();
            return outStream;
        }

        public async Task<Stream> DownloadLatestScripts(DownloadManifest manifest, CancellationToken cancel = default)
        {
            var releases = await this.GithubClient.Repository.Release.GetLatest(manifest.Scripts.Owner, manifest.Scripts.Repository);
            var asset = releases.Assets.First(s => s.Name.StartsWith(manifest.Scripts.Asset.FormatTemplateString(this)));
            var stream = await this.HttpClient.GetStreamAsync(asset.BrowserDownloadUrl);
            cancel.ThrowIfCancellationRequested();
            
            var outStream = new MemoryStream();
            await stream.CopyToAsync(outStream, cancel);
            await outStream.FlushAsync(cancel);
            return outStream;
        }

        public async Task DownloadAndInstallBattleVoice(DownloadManifest manifest, string ext, CancellationToken cancel = default)
        {
            var stream = await this.HttpClient.GetStreamAsync(manifest.Battle.Uri.FormatTemplateString(this).Replace("$ext", ext), cancel);
            cancel.ThrowIfCancellationRequested();

            using var outStream = File.Open(Path.Combine(this.GamePath, $"{this.BattleVoiceFile}.{ext}"), System.IO.FileMode.Create);
            await stream.CopyToAsync(outStream, cancel);
            await outStream.FlushAsync(cancel);
            return;
        }

        public async Task<TorrentManager> DownloadTorrentInfo(DownloadManifest manifest, CancellationToken cancel = default)
        {
            // http stream can't use random access.
            var torrentStreamHttp = await this.HttpClient.GetStreamAsync(manifest.Voice.Uri, cancel);
            cancel.ThrowIfCancellationRequested();

            using var torrentStream = new MemoryStream();
            await torrentStreamHttp.CopyToAsync(torrentStream, cancel);
            await torrentStream.FlushAsync(cancel);
            cancel.ThrowIfCancellationRequested();

            torrentStream.Seek(0, SeekOrigin.Begin);

            var torrent = await this.TorrentClient.AddAsync(
                await Torrent.LoadAsync(torrentStream), 
                Path.Combine(Environment.CurrentDirectory, $"skyinst_{this.ScriptPrefix}"),
                new TorrentSettingsBuilder()
                {
                    CreateContainingDirectory = false,
                    
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

        public async Task<Stream> DownloadVoiceTorrent(DownloadManifest manifest, TorrentManager torrent, CancellationToken cancel = default)
        {
            var asset = manifest.Voice.Asset.FormatTemplateString(this);

            return await Task.Run(async () =>
            {
                while (torrent.State != TorrentState.Stopped && torrent.State != TorrentState.Paused && torrent.State != TorrentState.Seeding)
                {
                    if (cancel.IsCancellationRequested)
                    {
                        await torrent.StopAsync();
                        cancel.ThrowIfCancellationRequested();
                    }

                    await Task.Delay(500);
                    if (torrent.Peers.Seeds == 0)
                    {
                        await torrent.TrackerManager.AnnounceAsync(cancel);
                    }
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
            });
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
            });
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.HttpClient.Dispose();
                    this.TorrentClient.Dispose();
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
