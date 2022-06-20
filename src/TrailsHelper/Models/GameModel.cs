using System;
using System.Collections.Generic;
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

        public GameModel(GameLocator locator, string scriptPrefix, string battleVoiceFname)
        {
            this.Locator = locator;
            this.ScriptPrefix = scriptPrefix;
            this.BattleVoiceFile = battleVoiceFname;
        }

        public async Task<Stream> LoadCoverBitmapAsync()
        {
            var data = await httpClient.GetByteArrayAsync(this.Locator.GetCoverUri());
            return new MemoryStream(data);
        }

        public string Title => this.Locator.Name;
        public string ScriptPrefix { get; }
        public string BattleVoiceFile { get; }
    }
}
