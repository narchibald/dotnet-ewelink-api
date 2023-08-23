namespace EWeLink.Api.Models.Parameters
{
    using EWeLink.Api.Models.Converters;
    using Newtonsoft.Json;

    public class SnZbThermostatDisplayParameters : SnZbThermostatParameters
    {
        [JsonProperty("tempUnit")]
        public TemperatureUnit TemperatureUnits { get; set; }

        [JsonProperty("tempComfortStatus")]
        public int TemperatureComfortStatus { get; set; }

        [JsonProperty("tempComfortUpper")]
        [JsonConverter(typeof(ThermostatConverter))]
        public decimal TemperatureComfortUpper { get; set; }

        [JsonProperty("tempComfortLower")]
        [JsonConverter(typeof(ThermostatConverter))]
        public decimal TemperatureComfortLower { get; set; }

        [JsonProperty("humiComfortStatus")]
        public int HumidityComfortStatus { get; set; }

        [JsonProperty("humiComfortUpper")]
        [JsonConverter(typeof(ThermostatConverter))]
        public decimal HumidityComfortUpper { get; set; }

        [JsonProperty("humiComfortLower")]
        [JsonConverter(typeof(ThermostatConverter))]
        public decimal HumidityComfortLower { get; set; }
    }
}