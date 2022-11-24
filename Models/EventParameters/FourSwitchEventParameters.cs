namespace EWeLink.Api.Models.EventParameters
{
    public interface IFourSwitchEventParameters
        : IThreeSwitchEventParameters
    {
        LinkSwitch Four { get; }
    }

    [EventDeviceIdentifier(9)]
    public class FourSwitchEventParameters
        : ThreeSwitchEventParameters, IFourSwitchEventParameters
    {
        public LinkSwitch Four => Switches[3];
    }
}