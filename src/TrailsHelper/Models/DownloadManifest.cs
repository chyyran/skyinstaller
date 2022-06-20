using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TrailsHelper.Models
{
    public record Battle(
        [property: JsonPropertyName("uri")] string Uri
    );

    public record Mod(
        [property: JsonPropertyName("owner")] string Owner,
        [property: JsonPropertyName("repository")] string Repository,
        [property: JsonPropertyName("asset")] string Asset
    );

    public record DownloadManifest(
        [property: JsonPropertyName("mod")] Mod Mod,
        [property: JsonPropertyName("scripts")] Scripts Scripts,
        [property: JsonPropertyName("battle")] Battle Battle,
        [property: JsonPropertyName("voice")] Voice Voice
    );

    public record Scripts(
        [property: JsonPropertyName("owner")] string Owner,
        [property: JsonPropertyName("repository")] string Repository,
        [property: JsonPropertyName("asset")] string Asset
    );

    public record Voice(
        [property: JsonPropertyName("uri")] string Uri,
        [property: JsonPropertyName("asset")] string Asset
    );


}
