using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TrailsHelper.Support
{
#nullable disable
    public class Battle
    {
        [JsonPropertyName("uri")]
        public string Uri { get; set; }

        [JsonPropertyName("directs")]
        public List<string> DirectUris { get; set; }
    }

    public class Mod
    {
        [JsonPropertyName("owner")]
        public string Owner { get; set; }

        [JsonPropertyName("repository")]
        public string Repository { get; set; }

        [JsonPropertyName("asset")]
        public string Asset { get; set; }
    }

    public class S3
    {
        [JsonPropertyName("bucket")]
        public string Bucket { get; set; }

        [JsonPropertyName("region")]
        public string Region { get; set; }

        [JsonPropertyName("access")]
        public string AccessKey { get; set; }

        [JsonPropertyName("secret")]
        public string SecretKey { get; set; }
    }

    public class DownloadManifest
    {
        [JsonPropertyName("mod")]
        public Mod Mod { get; set; }

        [JsonPropertyName("scripts")]
        public Scripts Scripts { get; set; }

        [JsonPropertyName("battle")]
        public Battle Battle { get; set; }

        [JsonPropertyName("voice")]
        public Voice Voice { get; set; }

        [JsonPropertyName("s3")]
        public S3 S3 { get; set; }
    }

    public class Scripts
    {
        [JsonPropertyName("owner")]
        public string Owner { get; set; }

        [JsonPropertyName("repository")]
        public string Repository { get; set; }

        [JsonPropertyName("asset")]
        public string Asset { get; set; }
    }

    public class Voice
    {
        [JsonPropertyName("uri")]
        public string Uri { get; set; }

        [JsonPropertyName("directs")]
        public List<string> DirectUris { get; set; }

        [JsonPropertyName("asset")]
        public string Asset { get; set; }

        [JsonPropertyName("mega_api")]
        public string MegaApiKey { get; set; }

        [JsonPropertyName("mega")]
        public Dictionary<string, string> Mega { get; set; }
    }
#nullable enable
}
