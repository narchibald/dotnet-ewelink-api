namespace EWeLink.Api.Models.EventParameters
{
    using EWeLink.Api.Models.Converters;
    using Newtonsoft.Json;

    public interface IThermostatParameters
        : IEventParameters
    {
        decimal? Temperature { get; set; }

        decimal? Humidity { get; set; }
    }

    [EventDeviceIdentifierAttribute(15)]
    public class ThermostatParameters
        : EventParameters, IThermostatParameters
    {
        [JsonProperty("currentTemperature")]
        [JsonConverter(typeof(SensorJsonConverter))]
        public decimal? Temperature { get; set; }

        [JsonProperty("currentHumidity")]
        [JsonConverter(typeof(SensorJsonConverter))]
        public decimal? Humidity { get; set; }
    }
}