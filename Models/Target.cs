namespace EWeLink.Api.Models
{
    using Newtonsoft.Json;

    public class Target
    {
        [JsonProperty("targetHigh")]
        public decimal TargetHigh { get; set; }

        [JsonProperty("reaction")]
        public Reaction? Reaction { get; set; }

        [JsonProperty("targetLow")]
        public decimal TargetLow { get; set; }
    }
}