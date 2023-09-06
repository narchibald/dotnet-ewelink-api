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

        [JsonIgnore]
        protected virtual int SwitchCount => 4;

        public override int? Update(string jsonData)
        {
            // Workout which switch has changed its switch state
            var updateInfo = JsonConvert.DeserializeObject<MultiSwitchParameters>(jsonData);
            int? triggeredOutlet = null;
            foreach (var switchUpdateInfo in updateInfo?.Switches.Where(x => x.Outlet < SwitchCount) ?? Array.Empty<LinkSwitch>())
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

            // Only update the parameter after we have worked out the switched state.
            base.Update(jsonData);

            return triggeredOutlet;
        }
    }
}
