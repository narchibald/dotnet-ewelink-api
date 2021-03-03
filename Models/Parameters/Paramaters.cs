namespace EWeLink.Api.Models.Parameters
{
    using Newtonsoft.Json;

    public class Paramaters
    {
        [JsonProperty("bindInfos")]
        public BindInfos BindInfos { get; set; }
    }
}