namespace EWeLink.Api.Models
{
    using Newtonsoft.Json;

    public class Reaction
    {
        [JsonProperty("switch")]
        public SwitchState Switch { get; set; }
    }
}