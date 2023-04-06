using ReactiveUI;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using TrailsHelper.ViewModels;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;

namespace TrailsHelper.Views
{
    public partial class InstallWindow : Avalonia.ReactiveUI.ReactiveWindow<InstallViewModel>
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
            if (!this.ViewModel!.IsInProgress)
            {
                return;
            }
            e.Cancel = true;

            var result = await MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(new()
            {
                ContentTitle = "Cancel installation?", 
                ContentMessage = "Are you sure you want to cancel the installation?",
                ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.YesNo,
                WindowIcon = this.Icon,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                SystemDecorations = SystemDecorations.BorderOnly
            }).ShowDialog(this);


            if (result != MessageBox.Avalonia.Enums.ButtonResult.Yes)
            {
                e.Cancel = true;
            }
            else
            {
                e.Cancel = false;
                this.ViewModel!.Status = "Cleaning up...";
                this.ViewModel!.InstallCancel.Cancel();
            }
        }
    }
}
