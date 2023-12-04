namespace EWeLink.Api.Models.Parameters
{
    using System.Linq;
    using Newtonsoft.Json;

    public class OneSwitchParameters : MultiSwitchParameters
    {
        [JsonIgnore]
        public LinkSwitch One => Switches[0];

        [JsonIgnore]
        protected override int SwitchCount => 1;

        public override Parameters CreateParameters()
        {
            var parameters = new OneSwitchParameters
            {
                Switches = Switches.Take(1).Select(x =>
                    new LinkSwitch { Switch = x.Switch, Outlet = x.Outlet }).ToArray(),
            };
            return parameters;
        }
    }
}