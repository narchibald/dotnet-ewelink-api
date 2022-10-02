namespace EWeLink.Api.Models.EventParameters
{
    using Newtonsoft.Json;

    [EventDeviceIdentifier(59)]
    public class RgbLightStripParameters : SwitchEventParameters
    {
        [JsonProperty("mode")]
        public int Mode { get; set; }

        [JsonProperty("colorR")]
        public int Red { get; set; }

        [JsonProperty("colorB")]
        public int Blue { get; set; }

        [JsonProperty("light_type")]
        public int LightType { get; set; }

        [JsonProperty("controlType")]
        public int ControlType { get; set; }

        [JsonProperty("bright")]
        public int Brightness { get; set; }

        [JsonProperty("colorG")]
        public int Green { get; set; }
    }
}
