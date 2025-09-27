using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tmds.DBus.Protocol;
using Connection = Tmds.DBus.Protocol.Connection;

namespace TrailsHelper.Support.WakeScope.Native
{
    internal class DbusWakeHandler : IWakeHandler
    {
        Connection DbusConnection { get; }
        bool _isConnected = false;
        SafeHandle? currentLock = null;
        public DbusWakeHandler(string address)
        {
          this.DbusConnection = new Connection(address);
        }


        public void Dispose()
        {
            this.DbusConnection.Dispose();
            this.currentLock?.Dispose();
        }

        public async ValueTask<bool> EnsureConnection()
        {
            if (!_isConnected)
            {
                try
                {
                    await this.DbusConnection.ConnectAsync();
                    return true;
                }
                catch
                {
                    Console.WriteLine("Warning: Unable to connect to DBUS, will not prevent sleep");
                    return true;
                }
            }
            return true;
        }


        public async ValueTask PreventSleep()
        {
            await this.EnsureConnection();
            try
            {
                var service = new login1Service(this.DbusConnection, "org.freedesktop.login1");
                var manager = service.CreateManager("/org/freedesktop/login1");

                this.currentLock = await manager.InhibitAsync("shutdown:idle:sleep", "SkyInstaller", "Installing SoraVoice mod", "block");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Warning: Failed to inhibit sleep: {ex}");
            }
        }

        public void RestoreSleep()
        {
            if (currentLock != null)
            {
                currentLock.Dispose();
                Console.WriteLine("Releasing inhibitor lock");
            }
        }
    }
}
