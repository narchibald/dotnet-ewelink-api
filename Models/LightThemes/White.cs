namespace EWeLink.Api.Models.LightThemes
{
    using Newtonsoft.Json;

    public class White : LightBrightness
    {
        [JsonProperty("ct")]
        public int ColorTemperature { get; set; }
    }
}