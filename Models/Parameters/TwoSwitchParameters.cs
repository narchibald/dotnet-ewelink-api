namespace EWeLink.Api.Models.Parameters
{
    public class TwoSwitchParameters : MultiSwitchParameters
    {
        public LinkSwitch One => Switches[0];

        public LinkSwitch Two => Switches[1];
    }
}