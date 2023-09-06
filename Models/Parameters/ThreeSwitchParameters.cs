namespace EWeLink.Api.Models.Parameters
{
    using System.Linq;
    using Newtonsoft.Json;

    public class ThreeSwitchParameters : TwoSwitchParameters
    {
        [JsonIgnore]
        public LinkSwitch Three => Switches[2];

        [JsonIgnore]
        protected override int SwitchCount => 3;

        public override Parameters CreateParameters()
        {
            var parameters = new ThreeSwitchParameters
            {
                Switches = Switches.Take(3).Select(x =>
                    new LinkSwitch { Switch = x.Switch, Outlet = x.Outlet }).ToArray(),
            };
            return parameters;
        }
    }
}