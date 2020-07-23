namespace EWeLink.Api.Models
{
    using Newtonsoft.Json;

    public class AppInfo
    {
        [JsonProperty("os")]
        public string? Os { get; set; }

        [JsonProperty("appVersion")]
        public string? AppVersion { get; set; }
    }
}