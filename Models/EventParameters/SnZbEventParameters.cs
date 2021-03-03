namespace EWeLink.Api.Models.EventParameters
{
    using System;

    using EWeLink.Api.Models.Converters;

    using Newtonsoft.Json;

    public class SnZbEventParameters : EventParameters
    {
        [JsonProperty("trigTime")]
        [JsonConverter(typeof(UnixTimeMillisecondsConverter))]
        public DateTime? TrigerTime { get; set; }
    }
}