namespace EWeLink.Api.Models.Parameters
{
    using Newtonsoft.Json;

    public class BuzzerAlarm
    {
        [JsonProperty("test")]
        public bool Test { get; set; }

        [JsonProperty("mode")]
        public string? Mode { get; set; }
    }
}