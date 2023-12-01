namespace EWeLink.Api.Models.EventParameters
{
    public interface IThreeSwitchEventParameters
        : ITwoSwitchEventParameters
    {
        LinkSwitch Three { get; }
    }

    [EventDeviceIdentifier(8)]
    public class ThreeSwitchEventParameters
        : TwoSwitchEventParameters, IThreeSwitchEventParameters
    {
        public LinkSwitch Three => Switches[2];
    }
}