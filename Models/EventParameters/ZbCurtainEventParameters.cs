namespace EWeLink.Api.Models.EventParameters
{
    using Newtonsoft.Json;

    public interface IZbCurtainEventParameters
        : ISnZbEventParameters
    {
        int CurrentPercent { get; set; }

        MotorDirection MotorDirection { get; set; }
    }

    [EventDeviceIdentifier(7006, 1514)]
    public class ZbCurtainEventParameters : SnZbEventParameters
    {
        [JsonProperty("curPercent")]
        public int CurrentPercent { get; set; }

        [JsonProperty("motorDir")]
        public MotorDirection MotorDirection { get; set; }

        [JsonProperty("motorClb")]
        public string MotorClb { get; set; }
    }
}