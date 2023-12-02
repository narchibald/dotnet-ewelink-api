namespace EWeLink.Api.Models.EventParameters
{
    using EWeLink.Api.Models;
    using Newtonsoft.Json;

    public interface ISnZbHumanPresenceEventParameters
        : ISnZbEventParameters
    {
        Presence Human { get; set; }
    }

    [EventDeviceIdentifier(7016)]
    public class SnZbHumanPresenceEventParameters
        : SnZbEventParameters, ISnZbHumanPresenceEventParameters
    {
        [JsonProperty("human")]
        public Presence Human { get; set; }
    }
}