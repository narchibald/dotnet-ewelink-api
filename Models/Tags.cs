namespace EWeLink.Api.Models
{
    using System;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public class Tags
    {
        [JsonProperty("m_760c_arch")]
        public string M760cArch { get; set; }

        [JsonProperty("disable_timers")]
        public List<object> DisableTimers { get; set; }

        [JsonProperty("ck_channel_name")]
        public CkChannelName ck_channel_name { get; set; }

    }
}
