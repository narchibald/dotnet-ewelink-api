namespace EWeLink.Api.Models.EventParameters
{
    public interface IMultiSwitchEventParameters
        : IEventParameters
    {
        LinkSwitch[] Switches { get; set; }
    }

    [EventDeviceIdentifierAttribute(7, 8, 9)]
    public class MultiSwitchEventParameters
        : EventParameters, IMultiSwitchEventParameters
    {
        public LinkSwitch[] Switches { get; set; } = new LinkSwitch[0];
    }
}