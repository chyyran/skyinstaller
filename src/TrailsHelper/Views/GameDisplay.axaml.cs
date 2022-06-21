using ReactiveUI;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using TrailsHelper.ViewModels;
using System.Threading.Tasks;
using Avalonia.Controls.ApplicationLifetimes;
using System.IO;

namespace TrailsHelper.Views
{
    public partial class GameDisplay : ReactiveUserControl<GameDisplayViewModel>
    {
        public GameDisplay()
        {
            InitializeComponent();
            this.WhenActivated(d => d(this.ViewModel!.ShowInstallDialog.RegisterHandler(DoShowInstallDialogAsync)));
            this.WhenActivated(d => d(this.ViewModel!.BrowseInstallFolderDialog.RegisterHandler(DoBrowseForInstallFolderAsync)));

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
                return;
            }
            interaction.SetOutput(false);
        }


        private void DoBrowseForInstallFolderAsync(InteractionContext<GameDisplayViewModel, DirectoryInfo?> interaction)
        {

            // todo: linux won't play nice with PresentationFramework..

            if (Application.Current!.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var mainIcon = desktop.MainWindow.Icon;
                desktop.MainWindow.Icon = interaction.Input.InstallWindowIcon;
                //desktop.MainWindow.Icon = dialog.Icon;

                var dialog = new Microsoft.Win32.OpenFileDialog()
                {
                    Multiselect = false,
                    Filter = $"{interaction.Input.Title}|{interaction.Input.Game.ExecutableName}",
                    Title = $"Find the installation of {interaction.Input.Title}",
                    CheckFileExists = true,
                    CheckPathExists = true
                };

          
                if (dialog.ShowDialog() == true && new FileInfo(dialog.FileName).Directory is DirectoryInfo directory && directory.Exists) 
                {
                    interaction.SetOutput(directory);
                }
                else
                {
                    interaction.SetOutput(null);
                }

                desktop.MainWindow.WindowState = WindowState.Normal;
                desktop.MainWindow.Icon = mainIcon;
                return;
            }
            interaction.SetOutput(null);
        }
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
