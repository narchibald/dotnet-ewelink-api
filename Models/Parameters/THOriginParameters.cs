namespace EWeLink.Api.Models.Parameters
{
    using Newtonsoft.Json;

    public class THOriginParameters : ThermostatSwitchParameters
    {
        [JsonProperty("tempUnit")]
        public TemperatureUnit TemperatureUnits { get; set; }

        [JsonProperty("tempCorrection")]
        public int TemperatureCorrection { get; set; }

        [JsonProperty("humCorrection")]
        public int HumidityCorrection { get; set; }

        [JsonProperty("autoControlEnabled")]
        public bool AutoControlEnabled { get; set; }

        [JsonProperty("tempHumiType")]
        public int TemperatureHumidityType { get; set; }
    }
}