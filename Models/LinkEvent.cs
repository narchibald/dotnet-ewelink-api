namespace EWeLink.Api.Models
{
    using EWeLink.Api.Models.Converters;
    using Newtonsoft.Json;

    public interface ILinkEvent<out T>
        where T : EventParameters.IEventParameters
    {
        EventAction Action { get; set; }

        public string DeviceId { get; }

        public int Uiid { get; }

        public string Apikey { get; }

        public string UserAgent { get; }

        public int DSeq { get; }

        public T Parameters { get; }

        public string From { get; }
    }

    public class LinkEvent<T> : ILinkEvent<T>
        where T : EventParameters.IEventParameters
    {
        [JsonProperty("action")]
        public EventAction Action { get; set; }

        [JsonProperty("deviceid")]
        public string DeviceId { get; set; }

        [JsonProperty("uiid")]
        public int Uiid { get; set; }

        [JsonProperty("apikey")]
        public string Apikey { get; set; }

        [JsonProperty("userAgent")]
        public string UserAgent { get; set; }

        [JsonProperty("d_seq")]
        public int DSeq { get; set; }

        [JsonProperty("params")]
        public T Parameters { get; set; }

        [JsonProperty("from")]
        public string From { get; set; }
    }
}