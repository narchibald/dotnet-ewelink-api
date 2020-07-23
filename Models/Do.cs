namespace EWeLink.Api.Models
{
    using Newtonsoft.Json;

    public class Do
    {
        [JsonProperty("switch")]
        public SwitchState? Switch { get; set; }
    }
}