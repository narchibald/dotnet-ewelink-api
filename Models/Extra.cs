namespace EWeLink.Api.Models
{
    using System;

    using Newtonsoft.Json;

    public class Extra
    {
        [JsonProperty("_id")]
        public string? Id { get; set; }

        [JsonProperty("apikey")]
        public string? Apikey { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; } = string.Empty;

        [JsonProperty("expiredAt")]
        public DateTime ExpiredAt { get; set; }

        [JsonProperty("secretKey")]
        public string? SecretKey { get; set; }

        [JsonProperty("deviceid")]
        public string DeviceId { get; set; } = string.Empty;

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("uiid")]
        public int Uiid { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; } = string.Empty;

        [JsonProperty("brandId")]
        public string BrandId { get; set; } = string.Empty;

        [JsonProperty("apmac")]
        public string? Apmac { get; set; }

        [JsonProperty("mac")]
        public string? Mac { get; set; }

        [JsonProperty("ui")]
        public string Ui { get; set; } = string.Empty;

        [JsonProperty("modelInfo")]
        public string ModelInfo { get; set; } = string.Empty;

        [JsonProperty("model")]
        public string Model { get; set; } = string.Empty;

        [JsonProperty("manufacturer")]
        public string Manufacturer { get; set; } = string.Empty;

        [JsonProperty("chipid")]
        public string Chipid { get; set; } = string.Empty;

        [JsonProperty("staMac")]
        public string? StaMac { get; set; }
    }
}