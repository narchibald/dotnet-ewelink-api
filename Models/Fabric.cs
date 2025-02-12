namespace EWeLink.Api.Models
{
    using System;
    using EWeLink.Api.Models.Converters;
    using Newtonsoft.Json;

    public class Fabric
    {
        [JsonProperty("compressedFabricId")]
        public string CompressedFabricId { get; set; }

        [JsonProperty("vendorId")]
        public string VendorId { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("createAt")]
        [JsonConverter(typeof(UnixTimeMillisecondsConverter))]
        public DateTimeOffset? CreateAt { get; set; }
    }
}