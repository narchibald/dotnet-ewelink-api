namespace EWeLink.Api.Models
{
    using Newtonsoft.Json;

    public class MatterInfoData
    {
        [JsonProperty("matterFabricId")]
        public string MatterFabricId { get; set; }

        [JsonProperty("matterNodeId")]
        public string matterNodeId { get; set; }
    }
}