namespace EWeLink.Api.Models
{
    using Newtonsoft.Json;

    public class LinkSwitch
    {
        [JsonProperty("outlet")]
        public int Outlet { get; set; }

        [JsonProperty("switch")]
        public SwitchState Switch { get; set; }
    }
}