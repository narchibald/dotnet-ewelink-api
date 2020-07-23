namespace EWeLink.Api.Models
{
    using Newtonsoft.Json;

    public class Credentials
    {
        [JsonProperty("at")]
        public string? At { get; set; }

        [JsonProperty("rt")]
        public string? Rt { get; set; }

        [JsonProperty("user")]
        public User? User { get; set; }

        [JsonProperty("region")]
        public string? Region { get; set; }
    }
}