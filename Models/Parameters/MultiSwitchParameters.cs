﻿namespace EWeLink.Api.Models.Parameters
{
    using Newtonsoft.Json;

    public class MultiSwitchParameters : Parameters
    {
        [JsonProperty("switches")]
        public LinkSwitch[] Switches { get; set; } = new LinkSwitch[0];
    }
}
