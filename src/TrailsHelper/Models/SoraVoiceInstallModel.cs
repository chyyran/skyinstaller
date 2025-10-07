using Octokit;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using SharpCompress.Archives;
using SharpCompress.Readers;
using MonoTorrent;
using MonoTorrent.Client;
using System.Text.Json;
using TrailsHelper.Support;
using System.Threading;
using System.Collections.Generic;
using Amazon.S3;
using Amazon.Runtime;
using Amazon;
using System.Linq;
using Gameloop.Vdf;
using Gameloop.Vdf.Linq;
using TrailsHelper.Support.HttpProgressHandler;

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
                CacheDirectory = Path.Combine(Path.GetTempPath(), $"skyinst_cache"),
                StaleRequestTimeout = TimeSpan.FromSeconds(120),
                WebSeedConnectionTimeout = TimeSpan.FromSeconds(90),
                // always use webseeds to support IA torrents
                WebSeedDelay = TimeSpan.Zero,
                WebSeedSpeedTrigger = int.MaxValue,
            }.ToSettings()); ;
        }, false);

        public SoraVoiceInstallModel(string modPrefix, string gamePath, string battleVoiceFile, uint gameId)
        {
            this.GameSteamId = gameId;
            this.ScriptPrefix = modPrefix;
            this.BattleVoiceFile = battleVoiceFile;
            this.GamePath = gamePath;
            this.GithubClient = new GitHubClient(new ProductHeaderValue("trailshelper"));
            var handler = new HttpClientHandler() { AllowAutoRedirect = true };
            var ph = new ProgressMessageHandler(handler);

            ph.HttpReceiveProgress += (_, args) => this.ProgressChangedEvent?.Invoke(this, ((double)args.BytesTransferred / args.TotalBytes) * 100 ?? 0);
            this.HttpClient = new HttpClient(ph);
        }

        public uint GameSteamId { get; }
        public string ScriptPrefix { get; }
        public string BattleVoiceFile { get; }
        public string GamePath { get; }
        public GitHubClient GithubClient { get; }
        public HttpClient HttpClient { get; }

        public event EventHandler<double>? ProgressChangedEvent;
        public event EventHandler<long>? SpeedChangedEvent;

        private static Uri? GetS3HttpUrl(DownloadManifest manifest, Uri s3, int hours = 12)
        {
            if (manifest.S3 == null || s3.Scheme != "s3")
                return null;

            try
            {

                AmazonS3Config config = !string.IsNullOrWhiteSpace(manifest.S3.Endpoint) 
                ? new ()
                {
                    // serviceurl takes precedence over RegionEndpoint
                    ServiceURL = manifest.S3.Endpoint,
                   
                }
                : new()
                {
                    // Default to us-east-1
                    RegionEndpoint = RegionEndpoint.GetBySystemName(manifest.S3.Region ?? "us-east-1")
                };

                var client = new AmazonS3Client(new BasicAWSCredentials(manifest.S3.AccessKey, manifest.S3.SecretKey), config);

                return new(client.GetPreSignedURL(new()
                {
                    BucketName = manifest.S3.Bucket,
                    Key = $"{s3.Host}{s3.AbsolutePath}",
                    Expires = DateTime.Now.AddHours(hours),
                    Verb = HttpVerb.GET,
                    
                }));
            } 
            catch
            {
                return null;
            }
        }
        private async Task<Stream> TryBestDownloadHttp(DownloadManifest manifest, IEnumerable<string> uris, CancellationToken cancel = default)
        {
            foreach (var uri in uris)
            {
                try
                {
                    var downloadUri = new Uri(uri.FormatTemplateString(this), new UriCreationOptions() { DangerousDisablePathAndQueryCanonicalization = false });
                    if (downloadUri.Scheme == "s3")
                        downloadUri = GetS3HttpUrl(manifest, downloadUri);
                    var stream = await this.HttpClient.GetStreamAsync(downloadUri, cancel);
                    return stream;
                }
                catch
                {
                    continue;
                }
            }
            throw new HttpRequestException("None of the provided URIs were available.");
        }

        public async Task<DownloadManifest> DownloadManifest(CancellationToken cancel = default)
        {
            if (File.Exists(Path.Combine(Environment.CurrentDirectory, "manifest.json")))
            {
                try
                {
                    var localStream = File.OpenRead(Path.Combine(Environment.CurrentDirectory, "manifest.json"));
                    var localManifest = await JsonSerializer.DeserializeAsync(localStream, JsonContext.Default.DownloadManifest, cancel);
                    return localManifest!;
                }
                catch
                {

                }
            }

            var stream = await this.HttpClient.GetStreamAsync(SoraVoiceInstallModel.ManifestUri, cancel);
            cancel.ThrowIfCancellationRequested();

            var manifest = await JsonSerializer.DeserializeAsync(stream, JsonContext.Default.DownloadManifest, cancel);
            return manifest!;
        }

        private async Task<IEnumerable<string>> TryGetGithubUrls(Github github)
        {
            var urls = new List<string>();

            try
            {
                var releases = await this.GithubClient.Repository.Release.GetLatest(github.Owner, github.Repository);
                var asset = releases.Assets.First(s => s.Name.StartsWith(github.Asset.FormatTemplateString(this)));
                urls.Add(asset.BrowserDownloadUrl);
            }
            catch
            {
                // Github failed and we have no backup.
                if (!github.DirectUris.Any())
                    throw;
            }

            return urls.Concat(github.DirectUris);
        }

        public async Task<Stream> DownloadLatestMod(DownloadManifest manifest, CancellationToken cancel = default)
        {
            var urls = await TryGetGithubUrls(manifest.Mod);
            var stream = await this.TryBestDownloadHttp(manifest, urls, cancel);
            cancel.ThrowIfCancellationRequested();

            var outStream = new MemoryStream();
            await stream.CopyToAsync(outStream, cancel);
            await outStream.FlushAsync(cancel);
            outStream.Seek(0, SeekOrigin.Begin);
            return outStream;
        }

        public async Task<Stream> DownloadLatestScripts(DownloadManifest manifest, CancellationToken cancel = default)
        {
            var urls = await TryGetGithubUrls(manifest.Scripts);
            var stream = await this.TryBestDownloadHttp(manifest, urls, cancel);
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

            var stream = await this.TryBestDownloadHttp(manifest, directUris.Select(uri => uri.FormatTemplateString(this).Replace("$ext", ext)
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

            var stream = await this.TryBestDownloadHttp(manifest, manifest.Voice.DirectUris
               .Select(uri => uri.FormatTemplateString(this)), cancel);
                    cancel.ThrowIfCancellationRequested();
            string filename = Path.Combine(Path.GetTempPath(), $"skyinst_voices_{this.ScriptPrefix}_{Random.Shared.Next(100000, 1000000)}.7z");

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
                await TorrentClient.Value.RemoveAsync(torrentInfo, RemoveMode.KeepAllData);
            }

            var torrent = await TorrentClient.Value.AddAsync(
                torrentInfo, 
                Path.Combine(Path.GetTempPath(), $"skyinst_{this.ScriptPrefix}"),
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
                this.ProgressChangedEvent?.Invoke(this, Math.Min((bytesUncompressed / (double)totalSize) * 99, 99.8));
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

                this.ProgressChangedEvent?.Invoke(this, 100);
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
                this.ProgressChangedEvent?.Invoke(this, Math.Min((bytesUncompressed / (double)totalSize) * 99, 99.8));
            };

            await Task.Run(() =>
            {
                reader.WriteAllToDirectory(Path.Combine(this.GamePath, "voice"), new()
                {
                    ExtractFullPath = true,
                    Overwrite = true,
                });
                this.ProgressChangedEvent?.Invoke(this, 100);
            }, cancel);
        }

        public async Task<bool> WriteSteamArgsOnLinux()
        {
            if (Steam.GetSteamDir() is not string steamDir)
                return false;

            var localConfigPath = Path.Combine(steamDir, "userdata", Steamworks.SteamClient.SteamId.AccountId.ToString(), "config", "localconfig.vdf");
           
            try
            {
                await using (var steamScope = SteamKillScope.WithoutSteamRunning())
                {
                    string localconfig = await File.ReadAllTextAsync(localConfigPath);
                    long fileSize = new FileInfo(localConfigPath).Length;
                    int maxTokenSize = (int)fileSize / 5; // arbitrary maybe need to use some better heuristics
                    VdfSerializerSettings vdfSerializerSettings = new()
                    {
                        MaximumTokenSize = maxTokenSize,
                        UsesEscapeSequences = true,
                        IsLinux = true,
                        IsWindows = false,
                        IsWin32 = false
                    };

                    var value = VdfConvert.Deserialize(localconfig, vdfSerializerSettings);
                    // On Steam Deck, the keys are titlecased. On Windows (or perhaps legacy), the keys are lowercase, but we only need to do this for Linux anyways.
                    //Console.WriteLine(value.Value["Software"]["Valve"]["Steam"]["apps"][this.GameSteamId.ToString()]);

                    if (value.Value?["Software"]?["Valve"]?["Steam"]?["apps"]?[this.GameSteamId.ToString()] is VObject appOptions)
                    {
                        appOptions["LaunchOptions"] = new VValue("WINEDLLOVERRIDES=\"dinput8=n,b\" %command%");
                    }

                    string serialized = VdfConvert.Serialize(value, vdfSerializerSettings);
                    await File.WriteAllTextAsync(localConfigPath, serialized);
                }
                return true;
            } 
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
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
