namespace EWeLink.Api.Models
{
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public class Tags
    {

        [JsonProperty("m_760c_arch")]
        public string M760cArch { get; set; }

        [JsonProperty("disable_timers")]
        public List<object> DisableTimers { get; set; }

    }
}