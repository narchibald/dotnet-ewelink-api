namespace EWeLink.Api.Models.Parameters
{
    using Newtonsoft.Json;

    public class SubDevice
    {
        [JsonProperty("subDevId")]
        public string SubDevId { get; set; }

        [JsonProperty("deviceid")]
        public string Deviceid { get; set; }

        [JsonProperty("uiid")]
        public string Uiid { get; set; }
    }
}