namespace EWeLink.Api.Models.EventParameters
{
    using Newtonsoft.Json;

    public interface ISnZbHumanPresenceEventParameters
        : ISnZbEventParameters
    {
        int Human { get; set; }
    }

    [EventDeviceIdentifier(7016)]
    public class SnZbHumanPresenceEventParameters
        : SnZbEventParameters, ISnZbHumanPresenceEventParameters
    {
        [JsonProperty("human")]
        public int Human { get; set; }
    }
}