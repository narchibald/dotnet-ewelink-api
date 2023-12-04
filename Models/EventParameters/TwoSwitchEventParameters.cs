namespace EWeLink.Api.Models.EventParameters
{
    public interface ITwoSwitchEventParameters
        : IOneSwitchEventParameters
    {
        LinkSwitch One { get; }

        LinkSwitch Two { get; }
    }

    [EventDeviceIdentifier(7)]
    public class TwoSwitchEventParameters
        : OneSwitchEventParameters, ITwoSwitchEventParameters
    {
        public LinkSwitch Two => Switches[1];
    }
}