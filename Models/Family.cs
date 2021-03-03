namespace EWeLink.Api.Models
{
    using Newtonsoft.Json;

    public class Family
    {
        [JsonProperty("room")]
        public Room? Room { get; set; }

        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("index")]
        public int? Index { get; set; }
    }
}