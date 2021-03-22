namespace EWeLink.Api.Models.EventParameters
{
    using Newtonsoft.Json;

    public interface ISnZbBatteryEventParameter
        : ISnZbEventParameters
    {
        int Battery { get; set; }
    }

    public class SnZbBatteryEventParameter
        : SnZbEventParameters, ISnZbBatteryEventParameter
    {
        [JsonProperty("battery")]
        public int Battery { get; set; }
    }
}