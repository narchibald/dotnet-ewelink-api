namespace EWeLink.Api.Models
{
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public class UpgradeInfo
    {
        [JsonProperty("bizRtnCode")]
        public int BizRtnCode { get; set; }

        [JsonProperty("deviceid")]
        public string? DeviceId { get; set; }

        [JsonProperty("model")]
        public string Model { get; set; } = string.Empty;

        [JsonProperty("version")]
        public string Version { get; set; } = string.Empty;

        [JsonProperty("upgradeText")]
        public string UpgradeText { get; set; } = string.Empty;

        [JsonProperty("binList")]
        public List<Binary> Binaries { get; set; } = new ();
    }
}