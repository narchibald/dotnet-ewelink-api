namespace EWeLink.Api.Models.EventParameters
{
    public interface ITwoSwitchEventParameters
        : IMultiSwitchEventParameters
    {
        LinkSwitch One { get; }

        LinkSwitch Two { get; }
    }

    [EventDeviceIdentifier(7)]
    public class TwoSwitchEventParameters
        : MultiSwitchEventParameters, ITwoSwitchEventParameters
    {
        public LinkSwitch One => Switches[0];

        public LinkSwitch Two => Switches[1];
    }
}