namespace EWeLink.Api.Models.EventParameters
{
    using EWeLink.Api.Models.Parameters;

    using Newtonsoft.Json;

    [EventDeviceIdentifierAttribute(1000)]
    public class SnZbButtonEventParameters : EventParameters
    {
        [JsonProperty("key")]
        public KeyTrigger Key { get; set; }
    }
}