namespace EWeLink.Api.Models.Parameters
{
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public class ZbBridgeParameters : Parameters
    {
        [JsonProperty("subDevNum")]
        public int SubDeviceNumber { get; set; }

        [JsonProperty("subDevMaxNum")]
        public int SubDeviceMaximumNumber { get; set; }

        [JsonProperty("zled")]
        public string? Zled { get; set; }

        [JsonProperty("subDevices")]
        public List<SubDevice> SubDevices { get; set; } = new ();

        [JsonProperty("addSubDevState")]
        public AddState AddSubDevState { get; set; }
    }
}
