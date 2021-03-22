namespace EWeLink.Api.Models.EventParameters
{
    using EWeLink.Api.Models.Converters;
    using Newtonsoft.Json;

    public interface ISnZbThermostatParameters
        : ISnZbEventParameters
    {
        decimal? Temperature { get; set; }

        decimal? Humidity { get; set; }
    }

    [EventDeviceIdentifierAttribute(1770)]
    public class SnZbThermostatParameters
        : SnZbEventParameters, ISnZbThermostatParameters
    {
        [JsonProperty("temperature")]
        [JsonConverter(typeof(ThermostatConverter))]
        public decimal? Temperature { get; set; }

        [JsonProperty("humidity")]
        [JsonConverter(typeof(ThermostatConverter))]
        public decimal? Humidity { get; set; }
    }
}