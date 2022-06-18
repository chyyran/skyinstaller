using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;
using TrailsHelper.Models;

namespace TrailsHelper.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string Greeting => "Welcome to Avalonia!";

        private bool _isSteamRunning = false;
        public bool IsSteamRunning { get => _isSteamRunning; set => this.RaiseAndSetIfChanged(ref _isSteamRunning, value); }

        public InstalledLocation FCLocation { get; set; } = new();
    }
}
