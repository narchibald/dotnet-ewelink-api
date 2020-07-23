namespace EWeLink.Api.Models
{
    using Newtonsoft.Json;

    public class Settings
    {

        [JsonProperty("opsNotify")]
        public int OpsNotify { get; set; }

        [JsonProperty("opsHistory")]
        public int OpsHistory { get; set; }

        [JsonProperty("alarmNotify")]
        public int AlarmNotify { get; set; }

        [JsonProperty("wxAlarmNotify")]
        public int WxAlarmNotify { get; set; }

        [JsonProperty("wxOpsNotify")]
        public int WxOpsNotify { get; set; }

        [JsonProperty("wxDoorbellNotify")]
        public int WxDoorbellNotify { get; set; }

        [JsonProperty("appDoorbellNotify")]
        public int AppDoorbellNotify { get; set; }

    }
}