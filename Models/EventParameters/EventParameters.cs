namespace EWeLink.Api.Models.EventParameters
{
    using System;
    using EWeLink.Api.Models.Converters;
    using Newtonsoft.Json;

    public interface IEventParameters
    {
        Type Type { get; }

        DateTimeOffset? TriggerTime { get; set; }
    }

    public class EventParameters
        : IEventParameters
    {
        [JsonIgnore]
        public Type Type => this.GetType();

        [JsonProperty("trigTime")]
        [JsonConverter(typeof(UnixTimeMillisecondsConverter))]
        public DateTimeOffset? TriggerTime { get; set; }
    }
}