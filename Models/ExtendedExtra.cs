namespace EWeLink.Api.Models
{
    using Newtonsoft.Json;

    public class ExtendedExtra
    {

        [JsonProperty("uiid")]
        public int Uiid { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("brandId")]
        public string BrandId { get; set; }

        [JsonProperty("apmac")]
        public string Apmac { get; set; }

        [JsonProperty("mac")]
        public string Mac { get; set; }

        [JsonProperty("ui")]
        public string Ui { get; set; }

        [JsonProperty("modelInfo")]
        public string ModelInfo { get; set; }

        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("manufacturer")]
        public string Manufacturer { get; set; }

        [JsonProperty("chipid")]
        public string Chipid { get; set; }

        [JsonProperty("staMac")]
        public string StaMac { get; set; }

    }
}