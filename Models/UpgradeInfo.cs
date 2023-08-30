namespace EWeLink.Api.Models
{
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public class UpgradeInfo
    {
        [JsonProperty("deviceid")]
        public string? DeviceId { get; set; }

        [JsonProperty("model")]
        public string Model { get; set; } = string.Empty;

        [JsonProperty("version")]
        public string Version { get; set; } = string.Empty;

        [JsonProperty("type")]
        public string Type { get; set; } = string.Empty;

        [JsonProperty("forceTime")]
        public string ForceTime { get; set; } = string.Empty;

        [JsonProperty("binList")]
        public List<Binary> Binaries { get; set; } = new ();
    }
}