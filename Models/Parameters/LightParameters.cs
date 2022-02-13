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
        public string FirmwareVersion { get; set; }

        [JsonProperty("sequence")]
        public string Sequence { get; set; }

        [JsonProperty("selfApikey")]
        public string SelfApikey { get; set; }

        [JsonProperty("white")]
        public White? White { get; set; }

        public override dynamic CreateParameters()
        {
            var parameters = base.CreateParameters();
            parameters.sequence = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();
            return parameters;
        }
    }
}