using ReactiveUI;
using System;
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
            private set => this.RaiseAndSetIfChanged(ref _downloadStatus, value);
        }


        readonly ObservableAsPropertyHelper<string> _progressPercentString;
        public string ProgressPercentString => _progressPercentString.Value;

        public string WindowTitle { get; }

        public CancellationTokenSource InstallCancel { get; private set; } = new();

        private bool CancelInstall()
        {
            this.GameModel.Game.Clean();
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

            this.Status = "Downloading metadata...";
            var torrent = await client.DownloadTorrentInfo(manifest, cancel);
            cancel.ThrowIfCancellationRequested();


            this.Status = "Downloading voice data...";
            var voiceArchive = await client.DownloadVoiceTorrent(manifest, torrent!, cancel);
            cancel.ThrowIfCancellationRequested();


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

        public InstallViewModel(GameDisplayViewModel gameModel)
        {
            this.GameModel = gameModel;

            this.WindowTitle = $"SkyInstaller — {this.GameModel.Title}";

            _progressPercentString = this.WhenAnyValue(x => x.ProgressValue)
               .Select(x => $"{x:F1}%")
               .ToProperty(this, x => x.ProgressPercentString);

            this.InstallCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                this.IsInProgress = true;
                using var client = new SoraVoiceInstallModel(gameModel.Prefix, gameModel.Path, gameModel.BattleVoiceFile);
                client.ProgressChangedEvent += (_, percent) => this.ProgressValue = percent;

                var cancel = this.InstallCancel.Token;
                try
                {
                    var result = await this.DoInstall(client, cancel);
                    this.Status = "Installation complete";
                    return result;
                } 
                catch (TaskCanceledException)
                {
                    this.Status = "Installation cancelled";
                    this.CancelInstall();
                    return false;
                }
                catch (OperationCanceledException)
                {
                    this.Status = "Installation cancelled";
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
