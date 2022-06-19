﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TrailsHelper.Models
{
    public class GameModel
    {
        public GameLocator Locator { get; }

        private static HttpClient httpClient = new();


        public GameModel(GameLocator locator)
        {
            this.Locator = locator;
        }

        public async Task<Stream> LoadCoverBitmapAsync()
        {
            var data = await httpClient.GetByteArrayAsync(this.Locator.GetCoverUri());
            return new MemoryStream(data);
        }

        public string Title => this.Locator.Name;
    }
}