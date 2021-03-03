namespace EWeLink.Api.Models.Parameters
{
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public class LinkParamaters : Paramaters
    {
        [JsonProperty("init")]
        public int Init { get; set; }

        [JsonProperty("rssi")]
        public int Rssi { get; set; }

        [JsonProperty("staMac")]
        public string StaMac { get; set; }

        [JsonProperty("timers")]
        public List<Timer> Timers { get; set; }

        [JsonProperty("swMode")]
        public int SwMode { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("startup")]
        public string? Startup { get; set; }

        [JsonProperty("sledOnline")]
        public string? SledOnline { get; set; }

        [JsonProperty("fwVersion")]
        public string? FirmWareVersion { get; set; }

        [JsonProperty("partnerApikey")]
        public string? PartnerApikey { get; set; }

        [JsonProperty("only_device")]
        public OnlyDevice OnlyDevice { get; set; }
    }
}