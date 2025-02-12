namespace EWeLink.Api.Models
{
    using Newtonsoft.Json;

    public class Ethernet
    {
        [JsonProperty("ipv4")]
        public string Ipv4 { get; set; }
    }
}