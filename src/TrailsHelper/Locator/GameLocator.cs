using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steamworks;
namespace TrailsHelper
{
    public sealed class GameLocator
    {
        public static readonly GameLocator TRAILS_IN_THE_SKY_FC = new("Trails in the Sky", 251150);
        public static readonly GameLocator TRAILS_IN_THE_SKY_SC = new("Trails in the Sky SC", 251290);
        public static readonly GameLocator TRAILS_IN_THE_SKY_3RD = new("Trails in the Sky the 3rd", 436670);

        private GameLocator(string name, uint appid)
        {
            this.Name = name;
            this.AppId = new() { Value = appid };
        }

        public string Name { get; }
        public AppId AppId { get; }

        public bool IsInstalled()
        {
            return SteamApps.IsAppInstalled(this.AppId);
        }

        public DirectoryInfo GetInstallDirectory()
        {
            return new DirectoryInfo(SteamApps.AppInstallDir(this.AppId));
        }

        public string GetCoverUri()
        {
            return $"https://steamcdn-a.akamaihd.net/steam/apps/{this.AppId.Value}/library_600x900_2x.jpg";
        }
    }
}
