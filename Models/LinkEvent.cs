namespace EWeLink.Api.Models
{
    using EWeLink.Api.Models.Converters;

    using Newtonsoft.Json;

    [JsonConverter(typeof(LinkEventConverter))]
    public class LinkEvent<T>
        where T : EventParameters.EventParameters
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
        public T Parameters { get; set; } = default;

        public string? From { get; set; }

        public long Sequence { get; set; }

        public long Seq { get; set; }

        public string? PartnerApikey { get; set; }

        public string? TempRec { get; set; }
    }
}