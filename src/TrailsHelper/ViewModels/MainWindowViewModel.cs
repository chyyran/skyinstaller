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
        private bool _isSteamRunning = false;
        public bool SteamInitComplete { get => _isSteamRunning; set => this.RaiseAndSetIfChanged(ref _isSteamRunning, value); }
        
        public MainWindowViewModel()
        {
            RxApp.MainThreadScheduler.Schedule(this.ActivateSteam);
            this.WhenAnyValue(x => x.SteamInitComplete)
                .Where(x => x)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(this.LoadAll!);
        }

        public GameDisplayViewModel ContextFC { get; } = new(new(GameLocator.TRAILS_IN_THE_SKY_FC, "fc", "ED6_DT1A", "ed6_win_DX9.exe"), "avares://SkyInstaller/Assets/fc.ico");
        public GameDisplayViewModel ContextSC { get; } = new(new(GameLocator.TRAILS_IN_THE_SKY_SC, "sc", "ED6_DT37", "ed6_win2_DX9.exe"), "avares://SkyInstaller/Assets/sc.ico");
        public GameDisplayViewModel Context3rd { get; } = new(new(GameLocator.TRAILS_IN_THE_SKY_3RD, "3rd", "ED6_DT37", "ed6_win3_DX9.exe"), "avares://SkyInstaller/Assets/3rd_config.ico");

        public async void ActivateSteam()
        {
            if (await Steam.StartSteam())
            {
                await Steam.LoopInit();
            };
            // even if steam init fails, we need to show the main menu.
            this.SteamInitComplete = true;
        }

        private async void LoadAll(bool s)
        {
            await this.ContextFC.Load();
            await this.ContextSC.Load();
            await this.Context3rd.Load();
        }
    }
}
