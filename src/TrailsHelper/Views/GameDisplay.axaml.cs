using ReactiveUI;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using TrailsHelper.ViewModels;
using System.Threading.Tasks;
using Avalonia.Controls.ApplicationLifetimes;
using System.IO;
using System.Collections.Generic;
using Avalonia.Platform.Storage;
using System.Linq;
using System;

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

        private async Task DoShowInstallDialogAsync(IInteractionContext<InstallViewModel, bool> interaction)
{
            var dialog = new InstallWindow
            {
                DataContext = interaction.Input
            };

            if (Application.Current!.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var mainIcon = desktop.MainWindow!.Icon;
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


        private async Task DoBrowseForInstallFolderAsync(IInteractionContext<GameDisplayViewModel, DirectoryInfo?> interaction)
        {

            // todo: linux won't play nice with PresentationFramework..

            if (Application.Current!.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var mainIcon = desktop.MainWindow!.Icon;
                desktop.MainWindow.Icon = interaction.Input.InstallWindowIcon;

                var dialogResult = await desktop.MainWindow.StorageProvider.OpenFilePickerAsync(new()
                {
                    AllowMultiple = false,
                    Title = $"Search for the installation of {interaction.Input.Title}",
                    FileTypeFilter = new List<FilePickerFileType>()
                    {
                        new(interaction.Input.Title)
                        {
                            Patterns = interaction.Input.Game.ExecutableNames
                        }
                    }
                });

                if (dialogResult.Count == 1 && dialogResult.Single().TryGetLocalPath() is string fileUri
                    && new FileInfo(fileUri).Directory is DirectoryInfo directory && directory.Exists)
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
    }
}
