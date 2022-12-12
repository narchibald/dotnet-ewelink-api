namespace EWeLink.Api.Models.Parameters
{
    using System.Linq;
    using Newtonsoft.Json;

    public class FourSwitchParameters : ThreeSwitchParameters
    {
        [JsonIgnore]
        public LinkSwitch Four => Switches[3];

        public override Parameters CreateParameters()
        {
            var parameters = new FourSwitchParameters
            {
                Switches = Switches.Take(4).Select(x =>
                    new LinkSwitch { Switch = x.Switch, Outlet = x.Outlet }).ToArray(),
            };
            return parameters;
        }
    }
}