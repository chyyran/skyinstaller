using Avalonia;
using Avalonia.Controls;
using TrailsHelper.ViewModels;


namespace TrailsHelper.Views
{
    public partial class InstallWindow : Window
    {
        public InstallWindow()
        {
            InitializeComponent();
            this.Closing += InstallWindow_Closing;
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private async void InstallWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DataContext is not InstallViewModel viewModel || !viewModel.IsInProgress)
            {
                return;
            }
            e.Cancel = true;

            var result = await MsBox.Avalonia.MessageBoxManager.GetMessageBoxStandard(new()
            {
                ContentTitle = "Cancel installation?",
                ContentMessage = "Are you sure you want to cancel the installation?",
                ButtonDefinitions = MsBox.Avalonia.Enums.ButtonEnum.YesNo,
                WindowIcon = this.Icon,
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
    }
}
