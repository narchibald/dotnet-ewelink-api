namespace EWeLink.Api.Models.Parameters
{
    using System;

    using Newtonsoft.Json;

    public class MultiSwitchParameters : Parameters
    {
        [JsonProperty("switches")]
        public LinkSwitch[] Switches { get; set; } = Array.Empty<LinkSwitch>();

        [JsonProperty("configure")]
        public LinkSwitchConfiguration[] Configuration { get; set; } = Array.Empty<LinkSwitchConfiguration>();
    }
}
