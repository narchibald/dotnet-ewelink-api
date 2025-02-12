namespace EWeLink.Api.Models
{
    using Newtonsoft.Json;

    public class DeviceFeature
    {
        [JsonProperty("scenes")]
        public string[]? Scenes { get; set; } = null;
    }
}