using Avalonia.Controls;
using Avalonia.Threading;
using TrailsHelper.Locator;
using TrailsHelper.ViewModels;

namespace TrailsHelper.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Dispatcher.UIThread.Post(async () =>
            {
                await Steam.StartSteam();
                await Steam.LoopInit();

                var context = (this.DataContext as MainWindowViewModel);
                var fc = new FirstChapterLocator();

                context.FCLocation.IsInstalled = fc.IsInstalled();
                context.FCLocation.Path = fc.GetInstallDirectory().FullName;


                context.IsSteamRunning = true;


            }, DispatcherPriority.Background);
        }
    }
}
