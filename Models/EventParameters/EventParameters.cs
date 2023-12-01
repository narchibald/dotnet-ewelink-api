namespace EWeLink.Api.Models.EventParameters
{
    using System;
    using EWeLink.Api.Models.Converters;
    using Newtonsoft.Json;

    public interface IEventParameters
    {
        DateTimeOffset? TriggerTime { get; set; }
    }

    public class EventParameters
        : IEventParameters
    {
        [JsonProperty("trigTime")]
        [JsonConverter(typeof(UnixTimeMillisecondsConverter))]
        public DateTimeOffset? TriggerTime { get; set; }

        public Type GetUnderlingType() => this.GetType();
    }
}