using Newtonsoft.Json;

namespace EWeLink.Api.Models.Parameters
{
    using System.Linq;

    public class TwoSwitchParameters : MultiSwitchParameters
    {
        [JsonIgnore]
        public LinkSwitch One => Switches[0];

        [JsonIgnore]
        public LinkSwitch Two => Switches[1];

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