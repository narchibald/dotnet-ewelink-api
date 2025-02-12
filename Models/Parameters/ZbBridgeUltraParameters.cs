namespace EWeLink.Api.Models.Parameters
{
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public class ZbBridgeUltraParameters : Parameters
    {
        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("network")]
        public List<string> Network { get; set; } = new ();

        [JsonProperty("ethernet")]
        public Ethernet Ethernet { get; set; } = new ();

        [JsonProperty("wlan")]
        public object Wlan { get; set; } = new ();

        [JsonProperty("mac")]
        public string MacAddress { get; set; }

        [JsonProperty("sledOnline")]
        public string? SledOnline { get; set; }

        [JsonProperty("ssid")]
        public string Ssid { get; set; }

        [JsonProperty("bssid")]
        public string Bssid { get; set; }

        [JsonProperty("fabricList")]
        public List<Fabric> FabricList { get; set; } = new ();

        [JsonProperty("tzInfo")]
        public TimeZoneInfo TimeZoneInfo { get; set; }

        [JsonProperty("staMac")]
        public string StaMacAddress { get; set; }

        [JsonProperty("otaState")]
        public string OtaState { get; set; }

        [JsonProperty("supportMatterSubDevices")]
        public List<string> SupportMatterSubDevices { get; set; } = new ();

        [JsonProperty("rssi")]
        public int Rssi { get; set; }

        [JsonProperty("addSubDevState")]
        public AddState AddSubDevState { get; set; }

        [JsonProperty("supportUIIDToMatter")]
        public List<string> SupportUIIDToMatter { get; set; } = new ();

        [JsonProperty("relatedDevices")]
        public List<string> RelatedDevices { get; set; } = new ();

        [JsonProperty("gatewayBindRelations")]
        public List<string> GatewayBindRelations { get; set; } = new ();

        [JsonProperty("supportMatterDeviceType")]
        public List<string> SupportMatterDeviceType { get; set; } = new ();

        [JsonProperty("supportMatterHub")]
        public bool SupportMatterHub { get; set; }
    }
}
