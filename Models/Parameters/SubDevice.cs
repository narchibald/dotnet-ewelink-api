namespace EWeLink.Api.Models.Parameters
{
    using Newtonsoft.Json;

    public class SubDevice
    {
        [JsonProperty("subDevId")]
        public string? SubDeviceId { get; set; }

        [JsonProperty("deviceid")]
        public string? Deviceid { get; set; }

        [JsonProperty("uiid")]
        public int Uiid { get; set; }

        [JsonProperty("index")]
        public int Index { get; set; }
    }
}