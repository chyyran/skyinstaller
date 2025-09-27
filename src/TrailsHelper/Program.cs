using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.ReactiveUI;
using System;
using System.Reflection;
using System.Threading;

namespace TrailsHelper
{
    internal class Program
    {
        private static Mutex? singleInstanceMutex = null;

        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args)
        {
            singleInstanceMutex = new Mutex(true, Assembly.GetExecutingAssembly().GetName().Name, out bool isNew);
            if (!isNew) 
            {
                return;
            }

            BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
            Steamworks.SteamClient.Shutdown();
        }
        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace()
                .UseReactiveUI()
                .With(new Win32PlatformOptions()
                {
                });
    }
}
