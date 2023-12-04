namespace EWeLink.Api.Models.EventParameters
{
    public interface IOneSwitchEventParameters
        : IMultiSwitchEventParameters
    {
        LinkSwitch One { get; }
    }

    [EventDeviceIdentifier(77)]
    public class OneSwitchEventParameters
        : MultiSwitchEventParameters, IOneSwitchEventParameters
    {
        public LinkSwitch One => Switches[0];
    }
}