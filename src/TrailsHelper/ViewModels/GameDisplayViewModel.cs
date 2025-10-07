using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TrailsHelper.Models;
using TrailsHelper.Views;

namespace TrailsHelper.ViewModels
{
    public partial class GameDisplayViewModel : ViewModelBase
    {
        GameModel _game;
        public GameModel Game => _game;
        public AsyncRelayCommand InstallForSteamGameCommand { get; }
        public AsyncRelayCommand BrowseThenInstallGameCommand { get; }

        private string _installWindowIcon { get; }
        public WindowIcon InstallWindowIcon => new(AssetLoader.Open(new(_installWindowIcon)));

        public GameDisplayViewModel(Models.GameModel model, string installIcon)
        {
            _game = model;
            _installWindowIcon = installIcon;
            var ico = new WindowIcon(AssetLoader.Open(new(_installWindowIcon)));

            this.InstallForSteamGameCommand = new AsyncRelayCommand(async () =>
            {

                var install = new InstallViewModel(this, this.SteamPath, true);
                var installResult = await install.ShowInstallDialog();
            });

            this.BrowseThenInstallGameCommand = new AsyncRelayCommand(async () =>
            {
                var browseResult = await this.BrowseInstallFolder();
                if (browseResult == null)
                {
                    return;
                }

                var install = new InstallViewModel(this, browseResult.FullName, false);
                var installResult = await install.ShowInstallDialog();
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

        public void LoadSteam()
        {
            this.IsInstalled = _game.Locator.IsInstalled();
            if (this.IsInstalled)
                this.SteamPath = _game.Locator.GetInstallDirectory()!.FullName;
        }

        public async Task<DirectoryInfo?> BrowseInstallFolder()
        {

            // todo: linux won't play nice with PresentationFramework..

            if (Application.Current!.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var mainIcon = desktop.MainWindow!.Icon;
                desktop.MainWindow.Icon = this.InstallWindowIcon;

                var dialogResult = await desktop.MainWindow.StorageProvider.OpenFilePickerAsync(new()
                {
                    AllowMultiple = false,
                    Title = $"Search for the installation of {this.Title}",
                    FileTypeFilter = new List<FilePickerFileType>()
                    {
                        new(this.Title)
                        {
                            Patterns = this.Game.ExecutableNames
                        }
                    }
                });

                desktop.MainWindow.WindowState = WindowState.Normal;
                desktop.MainWindow.Icon = mainIcon;

                if (dialogResult.Count == 1 && dialogResult.Single().TryGetLocalPath() is string fileUri
                    && new FileInfo(fileUri).Directory is DirectoryInfo directory && directory.Exists)
                {
                    return directory;
                }
                else
                {
                    return null;
                }
            }
            return null;
        }
    }
}
