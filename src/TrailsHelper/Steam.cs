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
                // Linux Steam API doesn't like Init with hardcoded int so we use a steam_appid.txt hack.
                FileStream? steamId = null;
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    steamId = File.Open(Path.Combine(Environment.CurrentDirectory, "steam_appid.txt"), new FileStreamOptions()
                    {
                        Mode = System.IO.FileMode.OpenOrCreate,
                        Access = FileAccess.Write,
                        BufferSize = 512,
                        Options = FileOptions.DeleteOnClose,
                    });

                    steamId.Write(Encoding.ASCII.GetBytes("228980"));
                    steamId.Flush();
                }


                while (true)
                {
                    try
                    {
                        Steamworks.SteamClient.Init(228980);
                        break;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        Thread.Sleep(1);
                    }
                }

                Console.WriteLine("Steam Initialized");

                steamId?.Dispose();
            });
        }

        public static string? GetSteamExePath()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Valve\\Steam");
                return key?.GetValue("SteamExe") as string;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return "/usr/bin/steam";
            }

            Console.WriteLine($"WARNING: Unknown OS {RuntimeInformation.OSDescription}");

            // if we're on a different platform, just force manual browsing.
            return null;
        }

        public static string? GetSteamDir()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Valve\\Steam");
                return key?.GetValue("SteamPath") as string;
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                var steam_link = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".steam", "steam");
                if (Directory.Exists(steam_link))
                {
                    var steam_dir = Directory.ResolveLinkTarget(steam_link, true);
                    if (steam_dir is DirectoryInfo info) { return info.FullName; }
                }
            }
            return null;
        }

        public static bool IsSteamInstalled()
        {
            return GetSteamExePath() != null;
        }

        public static async Task<bool> StartSteam()
        {
            var steamPath = GetSteamExePath();
  
            if (steamPath == null || !File.Exists(steamPath)
                || File.Exists(Path.Combine(Environment.CurrentDirectory, "nosteam.txt"))) {
                Console.WriteLine($"Could not find Steam at {steamPath}");
                return false;
            }

   
            return await Task.Run(() =>
            {
                while (!Process.GetProcessesByName("steam").Any())
                {
                    var process = Process.Start(new ProcessStartInfo(steamPath)
                    {
                        Arguments = "-silent",
                        UseShellExecute = RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
                    });
                    Thread.Sleep(1000);
                }
                return true;
            });
        }
    }
}
