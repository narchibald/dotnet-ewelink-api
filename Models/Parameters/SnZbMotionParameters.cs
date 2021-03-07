namespace EWeLink.Api.Models.Parameters
{
    using Newtonsoft.Json;

    public class SnZbMotionParameters : SnZbParameters
    {
        [JsonProperty("motion")]
        public Motion Motion { get; set; }
    }
}
