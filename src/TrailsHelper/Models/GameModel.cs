using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TrailsHelper.Support;

namespace TrailsHelper.Models
{
    public class GameModel
    {
        public GameLocator Locator { get; }

        private static readonly HttpClient httpClient = new();
        public GameModel(GameLocator locator, string scriptPrefix, string battleVoiceFname, string exeName)
        {
            this.Locator = locator;
            this.ScriptPrefix = scriptPrefix;
            this.BattleVoiceFile = battleVoiceFname;
            this.ExecutableName = exeName;
        }

        public async Task<Stream> LoadCoverBitmapAsync()
        {
            var data = await httpClient.GetByteArrayAsync(this.Locator.GetCoverUri());
            return new MemoryStream(data);
        }

        public string Title => this.Locator.Name;
        public string ScriptPrefix { get; }
        public string BattleVoiceFile { get; }
        public string ExecutableName { get; }

        public void Clean(DirectoryInfo startPath)
        {
            if (File.Exists(Path.Combine(startPath.FullName, "dinput8.dll")))
            {
                try
                {
                    File.Delete(Path.Combine(startPath.FullName, "dinput8.dll"));
                }
                catch { }
            }
            if (File.Exists(Path.Combine(startPath.FullName, $"{this.BattleVoiceFile}.dat")))
            {
                try
                {
                    File.Delete(Path.Combine(startPath.FullName, $"{this.BattleVoiceFile}.dat"));
                } catch { }
            }
            if (File.Exists(Path.Combine(startPath.FullName, $"{this.BattleVoiceFile}.dir")))
            {
                try
                {
                    File.Delete(Path.Combine(startPath.FullName, $"{this.BattleVoiceFile}.dir"));
                }
                catch { }
            }
        }
    }
}
