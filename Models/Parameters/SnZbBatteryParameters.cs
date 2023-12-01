namespace EWeLink.Api.Models.Parameters
{
    using Newtonsoft.Json;

    public class SnZbBatteryParameters : SnZbParameters
    {
        [JsonProperty("battery")]
        public int Battery { get; set; }
    }
}
