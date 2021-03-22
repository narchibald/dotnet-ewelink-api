namespace EWeLink.Api.Models.EventParameters
{
    using Newtonsoft.Json;

    public interface ISnZbDoorWindowEventParameters
        : ISnZbEventParameters
    {
        bool Open { get; set; }
    }

    [EventDeviceIdentifierAttribute(3026)]
    public class SnZbDoorWindowEventParameters
        : SnZbEventParameters, ISnZbDoorWindowEventParameters
    {
        [JsonProperty("lock")]
        public bool Open { get; set; }
    }
}