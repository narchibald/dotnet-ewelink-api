namespace EWeLink.Api.Models.Parameters
{
    using Newtonsoft.Json;

    public class SnZbButtonParameters : SnZbParameters
    {
        [JsonProperty("key")]
        public KeyTrigger Key { get; set; }
    }
}
