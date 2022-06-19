using ReactiveUI;
using System.Reactive;
using TrailsHelper.Models;
namespace TrailsHelper.ViewModels
{
    public class InstallViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit, Unit> InstallCommand { get; }

        private double _progressValue;
        public double ProgressValue
        {
            get => _progressValue;
            private set => this.RaiseAndSetIfChanged(ref _progressValue, value);
        }

        private bool _progressIndeterminate;
        public bool ProgressIndeterminate
        {
            get => _progressIndeterminate;
            private set => this.RaiseAndSetIfChanged(ref _progressIndeterminate, value);
        }


        private string _downloadStatus;
        public string Status
        {
            get => _downloadStatus;
            private set => this.RaiseAndSetIfChanged(ref _downloadStatus, value);
        }

        public InstallViewModel(GameDisplayViewModel gameModel)
        {
            this.GameModel = gameModel;
          
            this.InstallCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                using var client = new SoraVoiceInstallModel(gameModel.Prefix, gameModel.Path);
                client.ProgressChangedEvent += (_, percent) => this.ProgressValue = percent;

                this.Status = "Downloading SoraVoiceLite...";
                var modArchive = await client.DownloadLatestMod();
                this.Status = "Extracting SoraVoiceLite...";
                client.ExtractMod(modArchive);

                this.Status = "Downloading script files...";
                var scriptArchive = await client.DownloadLatestScripts();
                this.Status = "Extracting scripts...";
                client.ExtractScript(scriptArchive);

                this.Status = "Done";
                return Unit.Default;
            });
        }

        public GameDisplayViewModel GameModel { get; }
    }
}
