namespace EWeLink.Api.Models
{
    using Newtonsoft.Json;

    public class White
    {
        [JsonProperty("br")]
        public int Brightness { get; set; }

        [JsonProperty("ct")]
        public int ColorTemperature { get; set; }
    }
}