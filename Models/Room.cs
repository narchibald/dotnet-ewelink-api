namespace EWeLink.Api.Models
{
    using Newtonsoft.Json;

    public class Room
    {
        [JsonProperty("id")]
        public string? Id { get; set; }
    }
}