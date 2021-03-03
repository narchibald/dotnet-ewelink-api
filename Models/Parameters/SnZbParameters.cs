namespace EWeLink.Api.Models.Parameters
{
    using System;

    using EWeLink.Api.Models.Converters;

    using Newtonsoft.Json;

    public class SnZbParameters : Paramaters
    {
        [JsonProperty("subDevId")]
        public string SubDeviceId { get; set; }

        [JsonProperty("parentid")]
        public string ParentId { get; set; }

        [JsonProperty("battery")]
        public int Battery { get; set; }

        [JsonProperty("trigTime")]
        [JsonConverter(typeof(UnixTimeMillisecondsConverter))]
        public DateTime? TrigerTime { get; set; }
    }
}
