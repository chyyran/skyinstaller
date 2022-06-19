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
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
