namespace EWeLink.Api.Models.Parameters
{
    using Newtonsoft.Json;

    public class BuzzerAlarm
    {
        [JsonProperty("test")]
        public bool Test { get; set; }

        [JsonProperty("mode")]
        public string? Mode { get; set; }

        [JsonProperty("fileName")]
        public string FileName { get; set; }

        [JsonProperty("volume")]
        public int Volume { get; set; }

        [JsonProperty("duration")]
        public int Duration { get; set; }
    }
}