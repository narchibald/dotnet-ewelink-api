namespace EWeLink.Api.Models
{
    using Newtonsoft.Json;

    public class Color
    {
        [JsonProperty("r")]
        public int Red { get; set; }

        [JsonProperty("g")]
        public int Green { get; set; }

        [JsonProperty("b")]
        public int Blue { get; set; }

        [JsonProperty("br")]
        public int Brightness { get; set; }
    }
}