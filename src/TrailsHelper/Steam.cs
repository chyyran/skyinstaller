using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TrailsHelper.Support;

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
                        Steamworks.SteamClient.Init(228980);
                        break;
                    }
                    catch
                    {
                        Thread.Sleep(1);
                    }
                }
            });
        }

        public static string? GetSteamPath()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Valve\\Steam");
                return key?.GetValue("SteamExe") as string;
            }
            // if we're on a different platform, just force manual browsing.
            return null;
        }

        public static bool IsSteamInstalled()
        {
            return GetSteamPath() != null;
        }

        public static async Task<bool> StartSteam()
        {
            var steamPath = GetSteamPath();
  
            if (steamPath == null || !File.Exists(steamPath)) {
                return false;
            }

            return await Task.Run(() =>
            {
                while (!Process.GetProcessesByName("steam").Any())
                {
                    var process = Process.Start(new ProcessStartInfo(steamPath)
                    {
                        Arguments = "-silent"
                    });
                    Thread.Sleep(100);
                }
                return true;
            });
        }
    }
}
