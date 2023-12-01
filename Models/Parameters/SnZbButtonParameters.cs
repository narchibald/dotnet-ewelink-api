namespace EWeLink.Api.Models.Parameters
{
    using Newtonsoft.Json;

    public class SnZbButtonParameters : SnZbBatteryParameters
    {
        [JsonProperty("key")]
        public KeyTrigger Key { get; set; }
    }
}
