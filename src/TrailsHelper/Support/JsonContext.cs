using MonoTorrent.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TrailsHelper.Support
{
    [JsonSerializable(typeof(DownloadManifest))]
    internal partial class JsonContext : JsonSerializerContext
    {
    }
}
