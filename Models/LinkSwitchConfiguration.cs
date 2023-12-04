namespace EWeLink.Api.Models
{
    using Newtonsoft.Json;

    public class LinkSwitchConfiguration
    {
        [JsonProperty("outlet")]
        public int Outlet { get; set; }

        [JsonProperty("startup")]
        public StartupState Startup { get; set; }
    }
}