namespace EWeLink.Api.Models.Parameters
{
    using Newtonsoft.Json;

    public class SnZbMotionPParameters : SnZbMotionParameters
    {
        [JsonProperty("detectInterval")]
        public int DetectInterval { get; set; }

        [JsonProperty("brState")]
        public string BrState { get; set; } = string.Empty;
    }
}
