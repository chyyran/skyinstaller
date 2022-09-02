using System;
using ReactiveUI;
using System.Reactive.Linq;
using TrailsHelper.ViewModels;

namespace TrailsHelper.Views
{
    public partial class MainWindow : Avalonia.ReactiveUI.ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
            this.WhenActivated(d => d(this.ViewModel.WhenAnyValue(x => x!.SteamInitComplete)
                .Where(x => x)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(this.OnSteamReady!)));
        }

        public void OnSteamReady(bool s)
        {
            this.Activate();
        }
    }
}
