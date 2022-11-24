namespace EWeLink.Api.Models.Parameters
{
    public class FourSwitchParameters : ThreeSwitchParameters
    {
        public LinkSwitch Four => Switches[3];
    }
}