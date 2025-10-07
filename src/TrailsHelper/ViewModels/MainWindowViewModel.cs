using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using TrailsHelper.Support;

namespace TrailsHelper.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        private bool _steamInitComplete = false;

        [ObservableProperty]
        private bool _steamDisabled = false;

        public MainWindowViewModel()
        {
            Dispatcher.UIThread.Post(this.ActivateSteam);
            Dispatcher.UIThread.Post(this.LoadCover);

            this.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(SteamInitComplete) && this.SteamInitComplete)
                {
                    Dispatcher.UIThread.Post(this.LoadSteamStatus);
                }
            };
        }

        public GameDisplayViewModel ContextFC { get; } = new(new(GameLocator.TRAILS_IN_THE_SKY_FC, "fc", "ED6_DT1A", ["ed6_win.exe", "ed6_win_DX9.exe"]), "avares://SkyInstaller/Assets/fc.ico");
        public GameDisplayViewModel ContextSC { get; } = new(new(GameLocator.TRAILS_IN_THE_SKY_SC, "sc", "ED6_DT37", ["ed6_win2.exe", "ed6_win2_DX9.exe"]), "avares://SkyInstaller/Assets/sc.ico");
        public GameDisplayViewModel Context3rd { get; } = new(new(GameLocator.TRAILS_IN_THE_SKY_3RD, "3rd", "ED6_DT37", ["ed6_win3.exe", "ed6_win3_DX9.exe"]), "avares://SkyInstaller/Assets/3rd_config.ico");

        public async void ActivateSteam()
        {
            bool steamInstalled = await Steam.StartSteam();
            if (steamInstalled)
            {
                await Steam.LoopInit();
            }
            // even if steam init fails, we need to show the main menu.
            this.SteamInitComplete = true;
            this.SteamDisabled = !steamInstalled;
        }

        private async void LoadCover()
        {
            await this.ContextFC.LoadCover();
            await this.ContextSC.LoadCover();
            await this.Context3rd.LoadCover();
        }

        private void LoadSteamStatus()
        {
            this.ContextFC.IsSteamDisabled = this.SteamDisabled;
            this.ContextSC.IsSteamDisabled = this.SteamDisabled;
            this.Context3rd.IsSteamDisabled = this.SteamDisabled;

            this.ContextFC.IsSteamReady = true;
            this.ContextSC.IsSteamReady = true;
            this.Context3rd.IsSteamReady = true;

            this.ContextFC.LoadSteam();
            this.ContextSC.LoadSteam();
            this.Context3rd.LoadSteam();
        }
    }
}
