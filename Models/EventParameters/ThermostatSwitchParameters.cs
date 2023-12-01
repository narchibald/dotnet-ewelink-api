namespace EWeLink.Api.Models.EventParameters
{
    using EWeLink.Api.Models.Converters;
    using Newtonsoft.Json;

    public interface IThermostatSwitchParameters
        : IEventParameters
    {
        decimal? Temperature { get; set; }

        decimal? Humidity { get; set; }

        string? SensorType { get; set; }
    }

    [EventDeviceIdentifier(15)]
    public class ThermostatSwitchParameters
        : SwitchEventParameters, IThermostatSwitchParameters
    {
        [JsonProperty("currentTemperature")]
        [JsonConverter(typeof(SensorJsonConverter))]
        public decimal? Temperature { get; set; }

        [JsonProperty("currentHumidity")]
        [JsonConverter(typeof(SensorJsonConverter))]
        public decimal? Humidity { get; set; }

        [JsonProperty("sensorType")]
        public string? SensorType { get; set; }
    }
}