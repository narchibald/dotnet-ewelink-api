namespace EWeLink.Api.Models.EventParameters
{
    using Newtonsoft.Json;

    public class SnZbBatteryEventParameter
        : SnZbEventParameters
    {
        [JsonProperty("battery")]
        public int Battery { get; set; }
    }
}