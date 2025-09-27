using Avalonia.Controls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tmds.DBus.Protocol;
using TrailsHelper.Support.WakeScope.Native;

namespace TrailsHelper.Support.WakeScope
{
    internal sealed class WakeScope : IDisposable
    {
        IWakeHandler? WakeHandler { get; }

        private WakeScope()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                this.WakeHandler = new WindowsWakeHandler();
            }
            // We need the session bus
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && Address.System != null)
            {
                this.WakeHandler = new DbusWakeHandler(Address.System);
            }
            else
            {
                this.WakeHandler = null;
            }
        }


        public async static ValueTask<WakeScope> PreventSleep()
        {
            var wakeScope = new WakeScope();

            if (wakeScope.WakeHandler != null)
            {
                await wakeScope.WakeHandler.PreventSleep();
            }
            
            return wakeScope;
        }


        public void Dispose()
        {
            // We use this as a scope guard, there's no real need to dispose of unmanaged resources here.
            this.WakeHandler?.RestoreSleep();
            this.WakeHandler?.Dispose();
        }
    }
}
