namespace EWeLink.Api.Models.Parameters
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class RgbicLightStripParameters : RgbLightStripBaseParameters
    {
        [JsonProperty("lineSequence")]
        public Dictionary<string, string> LineSequence { get; set; }

        [JsonProperty("icNumber")]
        public int IcNumber { get; set; }

        [JsonProperty("icType")]
        public int IcType { get; set; }

        [JsonProperty("bright07")]
        public int Brightness07 { get; set; }

        [JsonProperty("speed07")]
        public int Speed07 { get; set; }

        [JsonProperty("colorTemp")]
        public int ColorTemperature { get; set; }
    }
}
