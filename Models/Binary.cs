namespace EWeLink.Api.Models
{
    using Newtonsoft.Json;

    public class Binary
    {
        [JsonProperty("downloadUrl")]
        public string? DownloadUrl { get; set; }

        [JsonProperty("digest")]
        public string? Digest { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }
    }
}