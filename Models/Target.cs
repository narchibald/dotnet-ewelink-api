namespace EWeLink.Api.Models
{
    using Newtonsoft.Json;

    public class Target
    {
        [JsonProperty("targetHigh")]
        public string TargetHigh { get; set; }

        [JsonProperty("reaction")]
        public Reaction Reaction { get; set; }

        [JsonProperty("targetLow")]
        public string TargetLow { get; set; }
    }
}