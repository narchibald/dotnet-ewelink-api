namespace EWeLink.Api.Models
{
    using Newtonsoft.Json;

    public class OnlyDevice
    {
        [JsonProperty("ota")]
        public string Ota { get; set; }
    }
}