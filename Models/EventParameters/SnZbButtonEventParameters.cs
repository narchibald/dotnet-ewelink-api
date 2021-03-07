namespace EWeLink.Api.Models.EventParameters
{
    using EWeLink.Api.Models.Parameters;

    using Newtonsoft.Json;

    [EventDeviceIdentifierAttribute(1000)]
    public class SnZbButtonEventParameters
        : SnZbEventParameters
    {
        [JsonProperty("key", NullValueHandling = NullValueHandling.Ignore)]
        public KeyTrigger? Key { get; set; }
    }
}