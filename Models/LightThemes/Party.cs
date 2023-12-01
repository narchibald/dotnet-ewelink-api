namespace EWeLink.Api.Models.LightThemes
{
    using Newtonsoft.Json;

    public class Party : Color
    {
        [JsonProperty("tf")]
        public int Tf { get; set; }

        [JsonProperty("sp")]
        public int Sp { get; set; }
    }
}