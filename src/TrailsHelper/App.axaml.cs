using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using TrailsHelper.ViewModels;
using TrailsHelper.Views;

namespace TrailsHelper
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var mainViewModel = new MainWindowViewModel();

                desktop.MainWindow = new MainWindow
                {
                    DataContext = mainViewModel,
                };
            }
       
            base.OnFrameworkInitializationCompleted();
        }
    }
}
