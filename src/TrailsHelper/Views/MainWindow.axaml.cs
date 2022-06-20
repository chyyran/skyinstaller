using System;
using Avalonia.Controls;
using Avalonia.Threading;
using ReactiveUI;
using System.Reactive.Linq;
using System.Threading.Tasks;
using TrailsHelper.ViewModels;

namespace TrailsHelper.Views
{
    public partial class MainWindow : Avalonia.ReactiveUI.ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
            this.WhenActivated(d => d(this.ViewModel.WhenAnyValue(x => x!.SteamApiReady)
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
