namespace EWeLink.Api.Models.EventParameters
{
    using Newtonsoft.Json;

    [EventDeviceIdentifierAttribute(3026)]
    public class SnZbDoorWindowEventParameters
        : SnZbEventParameters
    {
        [JsonProperty("lock")]
        public bool Open { get; set; }
    }
}