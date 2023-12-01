namespace EWeLink.Api.Models.Parameters
{
    using Newtonsoft.Json;

    public class SnZbMotionParameters : SnZbBatteryParameters
    {
        [JsonProperty("motion")]
        public Motion Motion { get; set; }
    }
}
