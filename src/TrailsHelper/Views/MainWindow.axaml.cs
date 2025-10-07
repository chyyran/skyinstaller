using System;
using System.ComponentModel;
using Avalonia.Controls;
using TrailsHelper.ViewModels;

namespace TrailsHelper.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.DataContextChanged += (sender, e) =>
            {
                if (DataContext is MainWindowViewModel viewModel)
                {
                    viewModel.PropertyChanged += ViewModel_PropertyChanged;
                }
            };
        }

        private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MainWindowViewModel.SteamInitComplete) &&
                DataContext is MainWindowViewModel viewModel &&
                viewModel.SteamInitComplete)
            {
                this.Activate();
            }
        }
    }
}
