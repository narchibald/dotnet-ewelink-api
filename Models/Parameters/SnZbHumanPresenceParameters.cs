namespace EWeLink.Api.Models.Parameters
{
    using Newtonsoft.Json;

    public class SnZbHumanPresenceParameters : SnZbParameters
    {
        [JsonProperty("human")]
        public int Human { get; set; }

        [JsonProperty("brState")]
        public string BrState { get; set; }

        [JsonProperty("judgeTime")]
        public int DetectionDuration { get; set; }

        [JsonProperty("sensitivity")]
        public int Sensitivity { get; set; }
    }
}
