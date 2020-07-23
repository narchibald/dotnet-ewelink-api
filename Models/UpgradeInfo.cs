namespace EWeLink.Api.Models
{
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public class UpgradeInfo
    {

        [JsonProperty("bizRtnCode")]
        public int BizRtnCode { get; set; }

        [JsonProperty("deviceid")]
        public string DeviceId { get; set; }

        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("upgradeText")]
        public string UpgradeText { get; set; }

        [JsonProperty("binList")]
        public List<Binary> Binaries { get; set; }
    }
}