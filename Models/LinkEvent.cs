namespace EWeLink.Api.Models
{
    using Newtonsoft.Json;

    public class LinkEvent
    {
        public EventAction Action { get; set; }

        public string DeviceId { get; set; } = null!;

        public string? ApiKey { get; set; }

        public string? UserAgent { get; set; }

        [JsonProperty("ts")]
        public long Timestamp { get; set; }

        [JsonProperty("proxyMsgTime")]
        public long? ProxyMessageTime { get; set; }

        [JsonProperty("params")]
        public EventParameters Parameters { get; set; } = null!;

        public string? From { get; set; }

        public long Sequence { get; set; }

        public long Seq { get; set; }

        public string? PartnerApikey { get; set; }

        public string? TempRec { get; set; }
    }
}