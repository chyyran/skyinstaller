using ReactiveUI;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using TrailsHelper.ViewModels;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Controls.ApplicationLifetimes;

namespace TrailsHelper.Views
{
    public partial class GameDisplay : ReactiveUserControl<GameDisplayViewModel>
    {
        public GameDisplay()
        {
            InitializeComponent();
            this.WhenActivated(d => d(this.ViewModel!.ShowInstallDialog.RegisterHandler(DoShowInstallDialogAsync)));
        }

        private async Task DoShowInstallDialogAsync(InteractionContext<InstallViewModel, bool> interaction)
        {
            var dialog = new InstallWindow
            {
                DataContext = interaction.Input
            };

            if (Application.Current!.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var mainIcon = desktop.MainWindow.Icon;
                desktop.MainWindow.WindowState = WindowState.Minimized;
                desktop.MainWindow.Icon = dialog.Icon;

                var result = await dialog.ShowDialog<bool>(desktop.MainWindow);
                interaction.SetOutput(result);
                
                desktop.MainWindow.WindowState = WindowState.Normal;
                desktop.MainWindow.Icon = mainIcon;
            }
        }
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
