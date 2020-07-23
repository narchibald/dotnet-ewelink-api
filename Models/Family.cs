namespace EWeLink.Api.Models
{
    using Newtonsoft.Json;

    public class Family
    {
        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("index")]
        public int? Index { get; set; }
    }
}