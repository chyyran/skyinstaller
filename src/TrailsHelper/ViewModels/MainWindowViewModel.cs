using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using TrailsHelper.Models;
using TrailsHelper.Support;

namespace TrailsHelper.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private bool _steamInitComplete = false;
        public bool SteamInitComplete { get => _steamInitComplete; set => this.RaiseAndSetIfChanged(ref _steamInitComplete, value); }

        private bool _steamDisabled = false;
        public bool SteamDisabled { get => _steamDisabled; set => this.RaiseAndSetIfChanged(ref _steamDisabled, value); }

        public MainWindowViewModel()
        {
            RxApp.MainThreadScheduler.Schedule(this.ActivateSteam);
            RxApp.MainThreadScheduler.Schedule(this.LoadCover);
            this.WhenAnyValue(x => x.SteamInitComplete)
                .Where(x => x)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(this.LoadSteamStatus!);
        }

        public GameDisplayViewModel ContextFC { get; } = new(new(GameLocator.TRAILS_IN_THE_SKY_FC, "fc", "ED6_DT1A", "ed6_win_DX9.exe"), "avares://SkyInstaller/Assets/fc.ico");
        public GameDisplayViewModel ContextSC { get; } = new(new(GameLocator.TRAILS_IN_THE_SKY_SC, "sc", "ED6_DT37", "ed6_win2_DX9.exe"), "avares://SkyInstaller/Assets/sc.ico");
        public GameDisplayViewModel Context3rd { get; } = new(new(GameLocator.TRAILS_IN_THE_SKY_3RD, "3rd", "ED6_DT37", "ed6_win3_DX9.exe"), "avares://SkyInstaller/Assets/3rd_config.ico");

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

        private async void LoadSteamStatus(bool s)
        {
            this.ContextFC.IsSteamDisabled = this.SteamDisabled;
            this.ContextSC.IsSteamDisabled = this.SteamDisabled;
            this.Context3rd.IsSteamDisabled = this.SteamDisabled;

            this.ContextFC.IsSteamReady = true;
            this.ContextSC.IsSteamReady = true;
            this.Context3rd.IsSteamReady = true;

            await this.ContextFC.LoadSteam();
            await this.ContextSC.LoadSteam();
            await this.Context3rd.LoadSteam();
        }
    }
}
