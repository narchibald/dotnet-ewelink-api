namespace EWeLink.Api.Models.Parameters
{
    using EWeLink.Api.Models.Converters;

    using Newtonsoft.Json;

    public class SnZbThermostatParmeters : SnZbParameters, IThermostatParameters
    {
        [JsonProperty("temperature")]
        [JsonConverter(typeof(ThermostatConverter))]
        public decimal? Temperature { get; set; }

        [JsonProperty("humidity")]
        [JsonConverter(typeof(ThermostatConverter))]
        public decimal? Humidity { get; set; }
    }
}
