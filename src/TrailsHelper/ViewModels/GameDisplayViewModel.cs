using Avalonia.Media.Imaging;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using TrailsHelper.Models;

namespace TrailsHelper.ViewModels
{
    public class GameDisplayViewModel : ViewModelBase
    {
        GameModel _game;

        public ReactiveCommand<Unit, GameDisplayViewModel> InstallForGameCommand { get; }
        public Interaction<InstallViewModel, Unit> ShowInstallDialog { get; }

        public GameDisplayViewModel(Models.GameModel model)
        {
            _game = model;
            _installButtonText = this.WhenAnyValue(x => x.IsInstalled)
                .Select(x => x ? "Install SoraVoice" : "Game not installed")
                .ToProperty(this, x => x.InstallButtonText);
            this.ShowInstallDialog = new();
            this.InstallForGameCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var install = new InstallViewModel(this);
                var result = await ShowInstallDialog.Handle(install);
                return this;
            });
        }

        private Bitmap? _cover;
        public Bitmap? CoverArt
        {
            get => _cover;
            private set => this.RaiseAndSetIfChanged(ref _cover, value);
        }

        private bool _isInstalled = false;
        public bool IsInstalled { get => _isInstalled; set => this.RaiseAndSetIfChanged(ref _isInstalled, value); }

        private bool _isLoaded = false;
        public bool IsLoaded { get => _isLoaded; set => this.RaiseAndSetIfChanged(ref _isLoaded, value); }

        private string _path = "Game not found.";
        public string Path { get => _path; set => this.RaiseAndSetIfChanged(ref _path, value); }

        readonly ObservableAsPropertyHelper<string> _installButtonText;
        public string InstallButtonText => _installButtonText.Value;

        public string Title => _game.Title;

        private async Task LoadCover()
        {
            await using var imageStream = await _game.LoadCoverBitmapAsync();
            this.CoverArt = await Task.Run(() => Bitmap.DecodeToWidth(imageStream, 400));
        }

        public async Task Load()
        {
            await this.LoadCover();
            this.IsInstalled = _game.Locator.IsInstalled();
            if (this.IsInstalled) 
                this.Path = _game.Locator.GetInstallDirectory().FullName;
            this.IsLoaded = true;
        }
    }
}
