namespace EWeLink.Api.Models.Parameters
{
    using Newtonsoft.Json;

    public class IHostParameters : Parameters
    {
        [JsonProperty("rooted")]
        public bool Rooted { get; set; }

        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("fwVersion")]
        public string? FirmwareVersion { get; set; }

        [JsonProperty("staMac")]
        public string? StaMac { get; set; }

        [JsonProperty("otaState")]
        public string? OtaState { get; set; }

        [JsonProperty("ip")]
        public string? IPAddress { get; set; }

        [JsonProperty("volume")]
        public int Volume { get; set; }

        [JsonProperty("sledOnline")]
        public string? SledOnline { get; set; }

        [JsonProperty("maskrom")]
        public bool MaskRom { get; set; }
    }
}