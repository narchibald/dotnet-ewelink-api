namespace EWeLink.Api.Models.Devices
{
    using System.Collections.Generic;
    using EWeLink.Api.Models.Parameters;

    using Newtonsoft.Json;

    [DeviceIdentifier(243)]
    public class ZbBridgeUltra : Device<ZbBridgeUltraParameters>
    {
        [JsonProperty("matterInfoData")]
        public MatterInfoData MatterInfoData { get; set; } = new ();
    }
}
