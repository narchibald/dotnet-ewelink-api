namespace EWeLink.Api.Models.LightThemes
{
    using Newtonsoft.Json;

    public class Color : LightBrightness
    {
        [JsonProperty("r")]
        public int Red { get; set; }

        [JsonProperty("g")]
        public int Green { get; set; }

        [JsonProperty("b")]
        public int Blue { get; set; }
    }
}