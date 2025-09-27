using CG.Web.MegaApiClient;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reactive;
using System.Reactive.Linq;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TrailsHelper.Models;
using TrailsHelper.Support;
using TrailsHelper.Support.WakeScope;

namespace TrailsHelper.ViewModels
{
    public class InstallViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit, bool> InstallCommand { get; }

        private double _progressValue;
        public double ProgressValue
        {
            get => _progressValue;
            private set => this.RaiseAndSetIfChanged(ref _progressValue, value);
        }

        private bool _inProgress;
        public bool IsInProgress
        {
            get => _inProgress;
            private set => this.RaiseAndSetIfChanged(ref _inProgress, value);
        }


        private string? _downloadStatus;
        public string? Status
        {
            get => _downloadStatus;
            set => this.RaiseAndSetIfChanged(ref _downloadStatus, value);
        }

        readonly ObservableAsPropertyHelper<string> _progressPercentString;
        public string ProgressPercentString => _progressPercentString.Value;

        public string WindowTitle { get; }

        public CancellationTokenSource InstallCancel { get; private set; } = new();

        public string GamePath { get; }
        public bool IsSteam { get; }

        private static async Task<Stream?> DoDownloadFromDirect(InstallViewModel @this, SoraVoiceInstallModel client, DownloadManifest manifest, CancellationToken cancel)
        {
            try
            {
                // try download from direct links first
                @this.Status = "Downloading voice data...";
                var voiceStream = await client.DownloadVoiceFromDirect(manifest, cancel);
                cancel.ThrowIfCancellationRequested();
                return voiceStream;
            }
            catch (Exception e) when (e is ArgumentException || e is HttpRequestException)
            {
                return null;
            }
        }

        private static async Task<Stream?> DoDownloadFromMega(InstallViewModel @this, SoraVoiceInstallModel client, DownloadManifest manifest, CancellationToken cancel)
        {
            try
            {
                @this.Status = "Downloading voice data...";
                var voiceStream = await client.DownloadVoiceFromMega(manifest, cancel);
                cancel.ThrowIfCancellationRequested();
                return voiceStream;
            }
            // permissible failure situations
            catch (Exception megaException) when (megaException is NotSupportedException || megaException is ApiException
                || megaException is HttpRequestException
                || (megaException is AggregateException a && a.InnerException is HttpRequestException))
            {
                return null;
            }
        }

        private static async Task<Stream?> DoDownloadFromTorrent(InstallViewModel @this, SoraVoiceInstallModel client, DownloadManifest manifest, CancellationToken cancel)
        {
            // torrent has no permissible failure situations.
            @this.Status = "Downloading metadata...";
            var torrent = await client.DownloadVoiceTorrentInfo(manifest, cancel);
            cancel.ThrowIfCancellationRequested();

            @this.Status = "Downloading voice data...";

            void voiceDataStatusHandler(object? _, long speed)
            {
                if (!cancel.IsCancellationRequested)
                {
                    @this.Status = $"Downloading voice data ({speed / 1024} KB/s)...";
                }
            }

            client.SpeedChangedEvent += voiceDataStatusHandler;
            var voiceStream = await client.DownloadTorrent(manifest, torrent!, cancel);
            client.SpeedChangedEvent -= voiceDataStatusHandler;
            cancel.ThrowIfCancellationRequested();
            return voiceStream;
        }

        private bool DoCleanup()
        {
            this.GameModel.Game.Clean(new DirectoryInfo(this.GamePath));
            return false;
        }

        private static readonly Dictionary<string, Func<InstallViewModel, SoraVoiceInstallModel, DownloadManifest, CancellationToken, Task<Stream?>>> VoiceDownloadStrategies = new()
        {
            { "direct", InstallViewModel.DoDownloadFromDirect },
            { "mega", InstallViewModel.DoDownloadFromMega },
            { "torrent", InstallViewModel.DoDownloadFromTorrent },
        };

        private async Task<Stream> TryBestVoiceDownloadStrategy(SoraVoiceInstallModel client, DownloadManifest manifest, CancellationToken cancel)
        {
            foreach (var strategy in VoiceDownloadStrategies.Values)
            {
                var voiceArchive = await strategy(this, client, manifest, cancel);
                if (voiceArchive != null)
                    return voiceArchive;
            }
            throw new Exception("Failed to download voice files.");
        }

        private async Task<Func<InstallViewModel, SoraVoiceInstallModel, DownloadManifest, CancellationToken, Task<Stream?>>?> TryGetPreferredVoiceDownloadStrategy()
        {
            try
            {
                var path = Path.Combine(Environment.CurrentDirectory, "voicedl.txt");
                if (!File.Exists(path))
                    return null;

                string key = await File.ReadAllTextAsync(path);
                if (VoiceDownloadStrategies.TryGetValue(key.Trim().ToLowerInvariant(), out var strategy))
                    return strategy;
                return null;
            }
            catch
            {
                return null;
            }
        }

        private async Task<Stream> DoVoiceDownload(SoraVoiceInstallModel client, DownloadManifest manifest, CancellationToken cancel)
        {
            Stream voiceArchive;
            var preferredStrategy = await TryGetPreferredVoiceDownloadStrategy();
            if (preferredStrategy?.Invoke(this, client, manifest, cancel) is Task<Stream?> voiceStreamTask
                && (await voiceStreamTask) is Stream voiceStream)
            {
                voiceArchive = voiceStream;
            }
            else
            {
                voiceArchive = await this.TryBestVoiceDownloadStrategy(client, manifest, cancel);
            }
            return voiceArchive;
        }

        private async Task<bool> DoInstall(SoraVoiceInstallModel client, CancellationToken cancel)
        {
            this.Status = "Downloading manifest...";
            var manifest = await client.DownloadManifest(cancel);

            try
            {
                this.Status = "Downloading SoraVoiceLite...";
                using Stream modArchive = await client.DownloadLatestMod(manifest, cancel);

                this.Status = "Extracting SoraVoiceLite...";
                await client.ExtractToGameRoot(modArchive!, cancel);

                this.Status = "Downloading script files...";
                using Stream scriptArchive = await client.DownloadLatestScripts(manifest, cancel);

                this.Status = "Extracting scripts...";
                await client.ExtractToVoiceFolder(scriptArchive, cancel);

                this.Status = "Downloading battle voices...";
                await client.DownloadAndInstallBattleVoice(manifest, "dir", cancel);

                await client.DownloadAndInstallBattleVoice(manifest, "dat", cancel);

                using Stream voiceArchive = await DoVoiceDownload(client, manifest, cancel);

                this.Status = "Extracting voice data...";
                await client.ExtractToVoiceFolder(voiceArchive, cancel);

                if (this.IsSteam && Steam.IsSteamOS)
                {
                    this.Status = "Setting Proton Launch Arguments...";
                    await client.WriteSteamArgsOnLinux();
                }

                await voiceArchive.DisposeAsync();
                await scriptArchive.DisposeAsync();
                await modArchive.DisposeAsync();
                return true;
            }
            finally
            {

            }
        }

        public async Task<bool> DoInstall(CancellationToken cancel)
        {
            if (!Directory.Exists(this.GamePath))
            {
                this.Status = "Game directory not found";
                return false;
            }

            this.IsInProgress = true;
            using var client = new SoraVoiceInstallModel(this.GameModel.Prefix, this.GamePath, this.GameModel.BattleVoiceFile, this.GameModel.Game.Locator.AppId.Value);
            using var wakeScope = await WakeScope.PreventSleep();

            client.ProgressChangedEvent += (_, percent) => this.ProgressValue = percent;
            try
            {
                var result = await this.DoInstall(client, cancel);
                this.Status = "Installation complete";
                return result;
            }
            catch (Exception e) when (e is TaskCanceledException || e is OperationCanceledException
                || (e is InvalidOperationException && e.Message == "Reader has been cancelled."))
            {
                this.Status = "Installation cancelled";
                this.DoCleanup();
                return false;
            }
            // this is such a hack.
            catch (Exception e) when (e.Message == "Failed to download voice files.")
            {
                this.Status = "Downloading voice files failed.";
                this.DoCleanup();
                return false;
            }
            catch (Octokit.RateLimitExceededException)
            {
                this.Status = "GitHub rate limit exceeded";
                this.DoCleanup();
                return false;
            }
            catch (AggregateException e) when (e.InnerException != null)
            {
                this.Status = $"Unknown error: {e.InnerException.GetType().Name}, {e.Message}";
                this.DoCleanup();
                return false;
            }
            catch (Exception e)
            {
                this.Status = $"Unknown error: {e.GetType().Name}, {e.Message}";
                this.DoCleanup();
                return false;
            }
            finally
            {
                this.IsInProgress = false;
                this.InstallCancel = new();
            }
        }

        public InstallViewModel(GameDisplayViewModel gameModel, string gamePath, bool isSteam)
        {
            this.GameModel = gameModel;
            this.GamePath = gamePath;
            this.IsSteam = isSteam;

            this.WindowTitle = $"SkyInstaller — {this.GameModel.Title}";

            _progressPercentString = this.WhenAnyValue(x => x.ProgressValue)
               .Select(x => $"{x:F1}%")
               .ToProperty(this, x => x.ProgressPercentString);

            this.InstallCommand = ReactiveCommand.CreateFromTask(async () => {
                var cancel = this.InstallCancel.Token;
                var value = await this.DoInstall(cancel);
                return value;
             });
        }

        public GameDisplayViewModel GameModel { get; }
    }
}
