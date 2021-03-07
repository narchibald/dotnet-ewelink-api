namespace EWeLink.Api.Models.Parameters
{
    using EWeLink.Api.Models.Converters;

    using Newtonsoft.Json;

    public class SnZbDoorWindowParameters : SnZbParameters
    {
        [JsonProperty("lock")]
        public bool Open { get; set; }
    }
}
