namespace EWeLink.Api.Models.EventParameters
{
    using Newtonsoft.Json;

    public interface IOnlineEventParameters
        : IEventParameters
    {
        bool Online { get; set; }
    }

    public class OnlineEventParameters
        : EventParameters, IOnlineEventParameters
    {
        [JsonProperty("online")]
        public bool Online { get; set; }
    }
}