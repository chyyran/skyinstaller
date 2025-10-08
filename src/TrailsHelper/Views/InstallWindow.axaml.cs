using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using System;
using TrailsHelper.Support;
using TrailsHelper.ViewModels;


namespace TrailsHelper.Views
{
    public partial class InstallWindow : Window, IDisposable
    {
        TaskbarProgressManager? _taskbar;

        public InstallWindow()
        {
            InitializeComponent();

            this._taskbar = TaskbarProgressManager.GetAvaloniaTaskbar(this);
            this.Closing += InstallWindow_Closing;

            this.DataContextChanged += (sender, e) =>
            {
                if (DataContext is InstallViewModel viewModel)
                {
                    viewModel.PropertyChanged += (sender, e) => Dispatcher.UIThread.Invoke(() => ViewModel_PropertyChanged(sender, e));
                }
            };

#if DEBUG
            this.AttachDevTools();
#endif
        }

        public void Dispose()
        {
            this._taskbar?.Dispose();
        }

        private async void InstallWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.DataContext is not InstallViewModel viewModel || !viewModel.IsInProgress)
            {
                return;
            }
            e.Cancel = true;

            var result = await MsBox.Avalonia.MessageBoxManager.GetMessageBoxStandard(new()
            {
                ContentTitle = "Cancel installation?",
                ContentMessage = "Are you sure you want to cancel the installation?",
                ButtonDefinitions = MsBox.Avalonia.Enums.ButtonEnum.YesNo,
                WindowIcon = this.Icon!,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                SystemDecorations = SystemDecorations.BorderOnly
            }).ShowWindowDialogAsync(this);


            if (result != MsBox.Avalonia.Enums.ButtonResult.Yes)
            {
                e.Cancel = true;
            }
            else
            {
                e.Cancel = false;
                viewModel.DownloadStatus = "Cleaning up...";
                viewModel.InstallCancel.Cancel();
            }
        }

        private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(InstallViewModel.ProgressValue) &&
                DataContext is InstallViewModel viewModel &&
                viewModel.IsInProgress)
            {
                this._taskbar.ProgressPercentage = viewModel.ProgressValue / 100.0;
            }
        }
    }
}
