namespace EWeLink.Api.Models.Parameters
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using EWeLink.Api.Models.Converters;

    using Newtonsoft.Json;

    public class ThermostatSwitchParameters : SwitchParameters, IThermostatParameters
    {
        [JsonProperty("targets")]
        public List<Target> Targets { get; set; } = new();

        [JsonProperty("currentTemperature")]
        [JsonConverter(typeof(SensorJsonConverter))]
        public decimal? Temperature { get; set; }

        [JsonProperty("currentHumidity")]
        [JsonConverter(typeof(SensorJsonConverter))]
        public decimal? Humidity { get; set; }
    }
}
