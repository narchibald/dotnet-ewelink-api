namespace EWeLink.Api.Models.Parameters
{
    using System.Linq;
    using Newtonsoft.Json;

    public class TwoSwitchParameters : OneSwitchParameters
    {
        [JsonIgnore]
        public LinkSwitch Two => Switches[1];

        [JsonIgnore]
        protected override int SwitchCount => 2;

        public override Parameters CreateParameters()
        {
            var parameters = new TwoSwitchParameters
            {
                Switches = Switches.Take(2).Select(x =>
                    new LinkSwitch { Switch = x.Switch, Outlet = x.Outlet }).ToArray(),
            };
            return parameters;
        }
    }
}