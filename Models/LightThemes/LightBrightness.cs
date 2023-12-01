namespace EWeLink.Api.Models.LightThemes
{
    using Newtonsoft.Json;

    public abstract class LightBrightness
    {
          [JsonProperty("br")]
          public int Brightness { get; set; }
    }
}