namespace EWeLink.Api.Models.Parameters
{
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public class ZbBridgeParameters : Paramaters
    {
        [JsonProperty("subDevNum")]
        public int SubDeviceNumber { get; set; }

        [JsonProperty("subDevMaxNum")]
        public int SubDeviceMaximumNumber { get; set; }

        [JsonProperty("zled")]
        public string Zled { get; set; }

        [JsonProperty("subDevices")]
        public List<SubDevice> SubDevices { get; set; } = new ();

        [JsonProperty("addSubDevState")]
        public string AddSubDevState { get; set; }
    }

    public class SubDevice
    {
        [JsonProperty("subDevId")]
        public string SubDevId { get; set; }

        [JsonProperty("deviceid")]
        public string Deviceid { get; set; }

        [JsonProperty("uiid")]
        public string Uiid { get; set; }
    }
}
