namespace EWeLink.Api.Models.Parameters
{
    using System;

    using Newtonsoft.Json;

    public class MultiSwitchParameters : Parameters
    {
        [JsonProperty("switches")]
        public LinkSwitch[] Switches { get; set; } = Array.Empty<LinkSwitch>();
    }

    public class TwoSwitchParameters : MultiSwitchParameters
    {
        public LinkSwitch One => Switches[0];

        public LinkSwitch Two => Switches[1];
    }

    public class ThreeSwitchParameters : TwoSwitchParameters
    {
        public LinkSwitch Three => Switches[2];
    }

    public class FourSwitchParameters : ThreeSwitchParameters
    {
        public LinkSwitch Four => Switches[3];
    }
}
