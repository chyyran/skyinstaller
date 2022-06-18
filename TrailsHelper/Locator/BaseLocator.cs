using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steamworks;
namespace TrailsHelper
{
    internal abstract class BaseLocator
    {
        public BaseLocator(uint appid)
        {
            this.AppId = new() { Value = appid };
        }

        public AppId AppId { get; }
    
        public bool IsInstalled()
        {
            return SteamApps.IsAppInstalled(this.AppId);
        }

        public DirectoryInfo GetInstallDirectory()
        {
            return new DirectoryInfo(SteamApps.AppInstallDir(this.AppId));
        }

    }
}
