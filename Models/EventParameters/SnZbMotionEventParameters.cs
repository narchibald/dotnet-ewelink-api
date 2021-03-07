namespace EWeLink.Api.Models.EventParameters
{
    using EWeLink.Api.Models.Parameters;

    using Newtonsoft.Json;

    [EventDeviceIdentifierAttribute(2026)]
    public class SnZbMotionEventParameters
        : SnZbEventParameters
    {
        [JsonProperty("motion")]
        public Motion Motion { get; set; }
    }
}