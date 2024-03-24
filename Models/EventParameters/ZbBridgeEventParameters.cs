namespace EWeLink.Api.Models.EventParameters
{
    using Newtonsoft.Json;

    public interface IZbBridgeEventParameters
        : IEventParameters
    {
        int Rssi { get; set; }
    }

    [EventDeviceIdentifier(66)]
    public class ZbBridgeEventParameters : EventParameters
    {
        [JsonProperty("rssi")]
        public int Rssi { get; set; }
    }
}