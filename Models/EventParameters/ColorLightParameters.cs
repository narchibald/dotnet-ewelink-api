using Newtonsoft.Json;

namespace EWeLink.Api.Models.EventParameters
{
    [EventDeviceIdentifierAttribute(104)]
    public class ColorLightParameters
        : SwitchEventParameters
    {
        [JsonProperty("ltype")]
        public LightType LightType { get; set; }

        [JsonProperty("white")]
        public White White { get; set; }

        [JsonProperty("color")]
        public Color Color { get; set; }
    }
}