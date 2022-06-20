using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using TrailsHelper.ViewModels;

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

        private void InstallWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = this.ViewModel!.IsInProgress;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
