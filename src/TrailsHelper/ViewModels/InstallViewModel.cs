using CG.Web.MegaApiClient;
using ReactiveUI;
using System;
using System.IO;
using System.Net.Http;
using System.Reactive;
using System.Reactive.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TrailsHelper.Models;
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

        private bool CancelInstall()
        {
            this.GameModel.Game.Clean(new DirectoryInfo(this.GamePath));
            return false;
        }

        private async Task<bool> DoInstall(SoraVoiceInstallModel client, CancellationToken cancel)
        {
            this.Status = "Downloading manifest...";
            var manifest = await client.DownloadManifest(cancel);
            cancel.ThrowIfCancellationRequested();

            this.Status = "Downloading SoraVoiceLite...";
            var modArchive = await client.DownloadLatestMod(manifest, cancel);
            cancel.ThrowIfCancellationRequested();


            this.Status = "Extracting SoraVoiceLite...";
            await client.ExtractToGameRoot(modArchive!, cancel);
            cancel.ThrowIfCancellationRequested();


            this.Status = "Downloading script files...";
            var scriptArchive = await client.DownloadLatestScripts(manifest, cancel);
            cancel.ThrowIfCancellationRequested();

            this.Status = "Extracting scripts...";
            await client.ExtractToVoiceFolder(scriptArchive, cancel);
            cancel.ThrowIfCancellationRequested();


            Stream voiceArchive;
            try
            {
                this.Status = "Downloading voice data...";
                voiceArchive = await client.DownloadVoiceFromMega(manifest, cancel);
                cancel.ThrowIfCancellationRequested();
            }
            catch (Exception e) when (e is NotSupportedException || e is ApiException 
                || e is HttpRequestException
                || (e is AggregateException a && a.InnerException is HttpRequestException))
            {
                this.Status = "Downloading metadata...";
                var torrent = await client.DownloadVoiceTorrentInfo(manifest, cancel);
                cancel.ThrowIfCancellationRequested();

                this.Status = "Downloading voice data...";

                void voiceDataStatusHandler(object? _, long speed)
                {
                    if (!cancel.IsCancellationRequested)
                    {
                        this.Status = $"Downloading voice data ({speed / 1024} KB/s)...";
                    }
                }

                client.SpeedChangedEvent += voiceDataStatusHandler;
                voiceArchive = await client.DownloadTorrent(manifest, torrent!, cancel);
                client.SpeedChangedEvent -= voiceDataStatusHandler;
                cancel.ThrowIfCancellationRequested();
            }

            this.Status = "Extracting voice data...";
            await client.ExtractToVoiceFolder(voiceArchive, cancel);
            cancel.ThrowIfCancellationRequested();

            this.Status = "Downloading battle voices...";
            await client.DownloadAndInstallBattleVoice(manifest, "dir", cancel);
            cancel.ThrowIfCancellationRequested();


            await client.DownloadAndInstallBattleVoice(manifest, "dat", cancel);
            cancel.ThrowIfCancellationRequested();

            return true;
        }

        public InstallViewModel(GameDisplayViewModel gameModel, string gamePath)
        {
            this.GameModel = gameModel;
            this.GamePath = gamePath;

            this.WindowTitle = $"SkyInstaller — {this.GameModel.Title}";

            _progressPercentString = this.WhenAnyValue(x => x.ProgressValue)
               .Select(x => $"{x:F1}%")
               .ToProperty(this, x => x.ProgressPercentString);

            this.InstallCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                if (!Directory.Exists(this.GamePath))
                {
                    this.Status = "Game directory not found";
                    return false;
                }

                this.IsInProgress = true;
                using var client = new SoraVoiceInstallModel(gameModel.Prefix, this.GamePath, gameModel.BattleVoiceFile);
                client.ProgressChangedEvent += (_, percent) => this.ProgressValue = percent;
                var cancel = this.InstallCancel.Token;
                try
                {
                    var result = await this.DoInstall(client, cancel);
                    this.Status = "Installation complete";
                    return result;
                } 
                catch (Exception e) when (e is TaskCanceledException || e is OperationCanceledException)
                {
                    this.Status = "Installation cancelled";
                    this.CancelInstall();
                    return false;
                }
                catch (Octokit.RateLimitExceededException)
                {
                    this.Status = "GitHub rate limit exceeded";
                    this.CancelInstall();
                    return false;
                }
                catch (AggregateException e) when (e.InnerException != null)
                {
                    this.Status = $"Unknown error: {e.InnerException.GetType().Name}";
                    this.CancelInstall();
                    return false;
                }
                catch (Exception e)
                {
                    this.Status = $"Unknown error: {e.GetType().Name}";
                    this.CancelInstall();
                    return false;
                }
                finally
                {
                    this.IsInProgress = false;
                    this.InstallCancel = new();
                }
            });
        }

        public GameDisplayViewModel GameModel { get; }
    }
}
