namespace EWeLink.Api.Models.EventParameters
{
    using EWeLink.Api.Models.LightThemes;
    using Newtonsoft.Json;

    [EventDeviceIdentifier(104)]
    public class ColorLightParameters
        : SwitchEventParameters
    {
        [JsonProperty("ltype")]
        public LightType LightType { get; set; }

        [JsonProperty("white")]
        public White? White { get; set; }

        [JsonProperty("color")]
        public Color? Color { get; set; }

        [JsonProperty("read")]
        public Read? Read { get; set; }

        [JsonProperty("party")]
        public Party? Party { get; set; }
    }
}