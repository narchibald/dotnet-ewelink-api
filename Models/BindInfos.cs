namespace EWeLink.Api.Models
{
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public class BindInfos
    {
        [JsonProperty("gaction")]
        public List<string>? Gaction { get; set; }

        [JsonProperty("iftttTriggerCnt")]
        public int IftttTriggerCount { get; set; }
    }
}