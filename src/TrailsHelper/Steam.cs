using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TrailsHelper
{
    internal class Steam
    {
        public static Task LoopInit()
        {
            return Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        Steamworks.SteamClient.Init(480);
                        break;
                    }
                    catch
                    {
                        Thread.Sleep(1);
                    }
                }
            });
        }

        public static void Shutdown()
        {
            Steamworks.SteamClient.Shutdown();
        }

        public static string? GetSteamPath()
        {
            // todo: linux?
            var key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Valve\\Steam");
            return key?.GetValue("SteamExe") as string;
        }

        public static Task StartSteam()
        {
            var steamPath = GetSteamPath();
  
            return Task.Run(() =>
            {
                while (!Process.GetProcessesByName("steam").Any())
                {
                    var process = Process.Start(new ProcessStartInfo(steamPath)
                    {
                        Arguments = "-silent"
                    });
                    Thread.Sleep(100);
                }
            });
        }
    }
}
