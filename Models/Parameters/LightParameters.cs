namespace EWeLink.Api.Models.Parameters
{
    using System;
    using EWeLink.Api.Models.LightThemes;
    using Newtonsoft.Json;

    public abstract class LightParameters : SwitchParameters
    {
        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("fwVersion")]
        public string FirmwareVersion { get; set; } = string.Empty;

        [JsonProperty("sequence")]
        public long Sequence { get; set; }

        [JsonProperty("selfApikey")]
        public string? SelfApikey { get; set; }

        [JsonProperty("white")]
        public White? White { get; set; }
    }
}