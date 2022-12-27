namespace EWeLink.Api.Models
{
    using Newtonsoft.Json;

    public class LinkSwitch
    {
        [JsonProperty(Order = 1)]
        public int Outlet { get; set; }

        [JsonProperty(Order = 2)]
        public SwitchState Switch { get; set; }
    }
}