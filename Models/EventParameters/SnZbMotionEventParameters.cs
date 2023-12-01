namespace EWeLink.Api.Models.EventParameters
{
    using EWeLink.Api.Models.Parameters;

    using Newtonsoft.Json;

    public interface ISnZbMotionEventParameters
        : ISnZbEventParameters
    {
        Motion Motion { get; set; }
    }

    [EventDeviceIdentifier(2026)]
    public class SnZbMotionEventParameters
        : SnZbEventParameters, ISnZbMotionEventParameters
    {
        [JsonProperty("motion")]
        public Motion Motion { get; set; }
    }
}