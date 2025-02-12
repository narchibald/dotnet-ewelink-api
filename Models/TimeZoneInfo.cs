namespace EWeLink.Api.Models
{
    using Newtonsoft.Json;

    public class TimeZoneInfo
    {
        [JsonProperty("tzName")]
        public string TimeZoneName { get; set; }

        [JsonProperty("utcOffset")]
        public int UtcOffset { get; set; }

        [JsonProperty("dstOffset")]
        public int DestinationOffset { get; set; }

        [JsonProperty("dstStartRule")]
        public string DestinationStartRule { get; set; }

        [JsonProperty("dstEndRule")]
        public string DestinationEndRule { get; set; }
    }
}