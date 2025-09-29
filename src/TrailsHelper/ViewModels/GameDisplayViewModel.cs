using Avalonia;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using TrailsHelper.Models;

namespace TrailsHelper.ViewModels
{
    public partial class GameDisplayViewModel : ViewModelBase
    {
        GameModel _game;
        public GameModel Game => _game;
        public AsyncRelayCommand InstallForSteamGameCommand { get; }
        public AsyncRelayCommand BrowseThenInstallGameCommand { get; }

        public Interaction<InstallViewModel, bool> ShowInstallDialog { get; }
        public Interaction<GameDisplayViewModel, DirectoryInfo?> BrowseInstallFolderDialog { get; }

        private string _installWindowIcon { get; }
        public WindowIcon InstallWindowIcon => new(AssetLoader.Open(new(_installWindowIcon)));
        public GameDisplayViewModel(Models.GameModel model, string installIcon)
        {
            _game = model;
            _installWindowIcon = installIcon;
            var ico = new WindowIcon(AssetLoader.Open(new(_installWindowIcon)));

            this.ShowInstallDialog = new();
            this.BrowseInstallFolderDialog = new();

            this.InstallForSteamGameCommand = new AsyncRelayCommand(async () =>
            {
                var install = new InstallViewModel(this, this.SteamPath, true);
                var installResult = await ShowInstallDialog.Handle(install);
                //return this;
            });
            this.BrowseThenInstallGameCommand = new AsyncRelayCommand(async () =>
            {
                var browseResult = await BrowseInstallFolderDialog.Handle(this);
                if (browseResult == null)
                {
                    return;
                }

                var install = new InstallViewModel(this, browseResult.FullName, false);
                var installResult = await ShowInstallDialog.Handle(install);
            });
        }

        [ObservableProperty]
        private Bitmap? _coverArt;


        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(InstallButtonText))]
        private bool _isInstalled = false;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(InstallButtonText))]
        private bool _isSteamReady = false;

        [ObservableProperty]
        private bool _isSteamDisabled = false;

        [ObservableProperty]
        private bool _isLoaded = false;

        [ObservableProperty]
        private string _steamPath = "Game not found.";


        public string InstallButtonText
        {
            get
            {
                if (!this.IsSteamReady)
                    return "Waiting for Steam...";

                return this.IsInstalled ? "Install to Steam version" : "Game not installed";
            }
        }

        public string Title => _game.Title;

        public string Prefix => _game.ScriptPrefix;

        public string BattleVoiceFile => _game.BattleVoiceFile;

        public async Task LoadCover()
        {
            await using var imageStream = await _game.LoadCoverBitmapAsync();
            this.CoverArt = await Task.Run(() => Bitmap.DecodeToWidth(imageStream, 400));
            this.IsLoaded = true;
        }

        public async Task LoadSteam()
        {
            this.IsInstalled = _game.Locator.IsInstalled();
            if (this.IsInstalled)
                this.SteamPath = _game.Locator.GetInstallDirectory()!.FullName;
        }
    }
}
