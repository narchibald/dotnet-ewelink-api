namespace EWeLink.Api.Models.EventParameters
{
    using EWeLink.Api.Models.Converters;
    using Newtonsoft.Json;

    [EventDeviceIdentifierAttribute(1770)]
    public class SnZbThermostatParameters
        : SnZbEventParameters
    {
        [JsonProperty("temperature")]
        [JsonConverter(typeof(ThermostatConverter))]
        public decimal? Temperature { get; set; }

        [JsonProperty("humidity")]
        [JsonConverter(typeof(ThermostatConverter))]
        public decimal? Humidity { get; set; }
    }
}