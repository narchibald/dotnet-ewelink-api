namespace EWeLink.Api.Models.EventParameters
{
    using Newtonsoft.Json;

    public interface ITHOriginParameters : IThermostatSwitchParameters
    {
        public int TemperatureCorrection { get; set; }

        public int HumidityCorrection { get; set; }
    }

    [EventDeviceIdentifier(181)]
    public class THOriginParameters : ThermostatSwitchParameters, ITHOriginParameters
    {
        [JsonProperty("tempUnit")]
        public TemperatureUnit TemperatureUnits { get; set; }

        [JsonProperty("tempCorrection")]
        public int TemperatureCorrection { get; set; }

        [JsonProperty("humCorrection")]
        public int HumidityCorrection { get; set; }

        [JsonProperty("autoControlEnabled")]
        public bool AutoControlEnabled { get; set; }
    }
}