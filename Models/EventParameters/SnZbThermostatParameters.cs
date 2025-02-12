namespace EWeLink.Api.Models.EventParameters
{
    using EWeLink.Api.Models.Converters;
    using Newtonsoft.Json;

    public interface ISnZbThermostatEventParameters
        : ISnZbEventParameters
    {
        decimal? Temperature { get; set; }

        decimal? Humidity { get; set; }
    }

    [EventDeviceIdentifier(1770, 7014)]
    public class SnZbThermostatEventParameters
        : SnZbEventParameters, ISnZbThermostatEventParameters
    {
        [JsonProperty("temperature")]
        [JsonConverter(typeof(ThermostatConverter))]
        public decimal? Temperature { get; set; }

        [JsonProperty("humidity")]
        [JsonConverter(typeof(ThermostatConverter))]
        public decimal? Humidity { get; set; }
    }
}