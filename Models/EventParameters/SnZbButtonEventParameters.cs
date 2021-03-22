namespace EWeLink.Api.Models.EventParameters
{
    using EWeLink.Api.Models.Parameters;

    using Newtonsoft.Json;

    public interface ISnZbButtonEventParameters
        : ISnZbEventParameters
    {
        KeyTrigger? Key { get; set; }
    }

    [EventDeviceIdentifierAttribute(1000)]
    public class SnZbButtonEventParameters
        : SnZbEventParameters, ISnZbButtonEventParameters
    {
        [JsonProperty("key", NullValueHandling = NullValueHandling.Ignore)]
        public KeyTrigger? Key { get; set; }
    }
}