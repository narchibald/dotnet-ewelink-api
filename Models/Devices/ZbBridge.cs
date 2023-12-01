namespace EWeLink.Api.Models.Devices
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using EWeLink.Api.Models.Parameters;

    using Newtonsoft.Json;

    [DeviceIdentifier(66)]
    public class ZbBridge : Device<ZbBridgeParameters, Tags>
    {
        [JsonProperty("relational")]
        public List<object> Relational { get; set; } = new ();
    }
}
