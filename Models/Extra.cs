namespace EWeLink.Api.Models
{
    using System;

    using Newtonsoft.Json;

    public class Extra
    {

        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("apikey")]
        public string Apikey { get; set; }

        [JsonProperty("extra")]
        public ExtendedExtra Extended { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("expiredAt")]
        public DateTime ExpiredAt { get; set; }

        [JsonProperty("secretKey")]
        public string SecretKey { get; set; }

        [JsonProperty("deviceid")]
        public string Deviceid { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

    }
}