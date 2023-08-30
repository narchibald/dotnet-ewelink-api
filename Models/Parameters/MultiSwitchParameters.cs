namespace EWeLink.Api.Models.Parameters
{
    using System;
    using System.Linq;

    using Newtonsoft.Json;

    public class MultiSwitchParameters : Parameters
    {
        [JsonProperty("switches")]
        public LinkSwitch[] Switches { get; set; } = Array.Empty<LinkSwitch>();

        [JsonProperty("configure")]
        public LinkSwitchConfiguration[] Configuration { get; set; } = Array.Empty<LinkSwitchConfiguration>();

        public override int? Update(string jsonData)
        {
            var updateInfo = JsonConvert.DeserializeObject<MultiSwitchParameters>(jsonData);
            int? triggeredOutlet = null;
            foreach (var switchUpdateInfo in updateInfo?.Switches ?? Array.Empty<LinkSwitch>())
            {
                var @switch = Switches.FirstOrDefault(x => x.Outlet == @switchUpdateInfo.Outlet);
                if (@switch != null)
                {
                    if (@switch.Switch != switchUpdateInfo.Switch)
                    {
                        triggeredOutlet = @switch.Outlet;
                    }

                    @switch.Switch = @switch.Switch;
                }
            }

            return triggeredOutlet;
        }
    }
}
