namespace EWeLink.Api.Models
{
    using Newtonsoft.Json;

    public class ClientInfo
    {
        [JsonProperty("model")]
        public string? Model { get; set; }

        [JsonProperty("os")]
        public string? Os { get; set; }

        [JsonProperty("imei")]
        public string? Imei { get; set; }

        [JsonProperty("romVersion")]
        public string? RomVersion { get; set; }

        [JsonProperty("appVersion")]
        public string? AppVersion { get; set; }
    }
}