namespace EWeLink.Api.Models.Parameters
{
    using Newtonsoft.Json;

    public class RgbLightStripParameters : RgbLightStripBaseParameters
    {
        [JsonProperty("speed")]
        public int Speed { get; set; }

        [JsonProperty("sensitive")]
        public int Sensitive { get; set; }
    }
}
