using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrailsHelper.Support
{
    internal class SteamKillScope : IAsyncDisposable
    {
        private SteamKillScope(bool reinit)
        {
            this.ReinitializeApi = reinit;
        }

        public static SteamKillScope WithoutSteamRunning(bool reinitApi = true) {
            foreach (var proc in Process.GetProcessesByName("steam"))
            {
                proc.Kill();
            }
            return new(reinitApi);
        }
        public bool ReinitializeApi { get; }

        public async ValueTask DisposeAsync()
        {
            await Steam.StartSteam();
            if (this.ReinitializeApi)
            {
                await Steam.LoopInit();
            }
        }
    }
}
