using Avalonia.Threading;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TrailsHelper.Models;

namespace TrailsHelper.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private bool _isSteamRunning = false;
        public bool SteamApiReady { get => _isSteamRunning; set => this.RaiseAndSetIfChanged(ref _isSteamRunning, value); }

        public MainWindowViewModel()
        {
            Dispatcher.UIThread.Post(async () => await this.ActivateSteam(), DispatcherPriority.Background);

            this.WhenAnyValue(x => x.SteamApiReady)
                .Where(x => x)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(this.LoadAll!);
        }

        public GameDisplayViewModel ContextFC { get; } = new(new (GameLocator.TRAILS_IN_THE_SKY_FC));
        public GameDisplayViewModel ContextSC { get; } = new(new(GameLocator.TRAILS_IN_THE_SKY_SC));
        public GameDisplayViewModel Context3rd { get; } = new(new(GameLocator.TRAILS_IN_THE_SKY_3RD));

        public async Task ActivateSteam()
        {
            await Steam.StartSteam();
            await Steam.LoopInit();

            this.SteamApiReady = true;
        }

        private async void LoadAll(bool s)
        {
            await this.ContextFC.Load();
            await this.ContextSC.Load();
            await this.Context3rd.Load();
            Steam.Shutdown();
        }

        //private async Task LoadCover(CancellationToken cancellationToken)
        //{
        //    //await al
        //    //foreach (var album in SearchResults.ToList())
        //    //{
        //    //    await album.LoadCover();

        //    //    if (cancellationToken.IsCancellationRequested)
        //    //    {
        //    //        return;
        //    //    }
        //    //}
        //}
    }
}
