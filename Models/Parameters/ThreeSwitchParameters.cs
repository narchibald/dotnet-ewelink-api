namespace EWeLink.Api.Models.Parameters
{
    public class ThreeSwitchParameters : TwoSwitchParameters
    {
        public LinkSwitch Three => Switches[2];
    }
}