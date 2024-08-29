namespace EWeLink.Api.Models.Parameters
{
    using Newtonsoft.Json;

    public class ZbSmartWaterValveParameters : SnZbParameters
    {
        [JsonProperty("switch")]
        public SwitchState Switch { get; set; }
    }
}