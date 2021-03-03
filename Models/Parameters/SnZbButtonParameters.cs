namespace EWeLink.Api.Models.Parameters
{
    using Newtonsoft.Json;

    public enum KeyTrigger
    {
        Single = 0,

        Double = 1,

        Long = 2
    }

    public class SnZbButtonParameters : SnZbParameters
    {
        [JsonProperty("key")]
        public KeyTrigger Key { get; set; }
    }
}
