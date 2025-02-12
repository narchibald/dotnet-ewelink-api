namespace EWeLink.Api.Models.Parameters
{
    using Newtonsoft.Json;

    public class RgbLightStripBaseParameters : Parameters
    {
        [JsonProperty("switch")]
        public SwitchState Switch { get; set; }

        [JsonProperty("light_type")]
        public int LightType { get; set; }

        [JsonProperty("colorR")]
        public int Red { get; set; }

        [JsonProperty("colorG")]
        public int Green { get; set; }

        [JsonProperty("colorB")]
        public int Blue { get; set; }

        [JsonProperty("bright")]
        public int Brightness { get; set; }

        [JsonProperty("mode")]
        public int Mode { get; set; }
    }
}
