namespace EWeLink.Api.Models.EventParameters
{
    [EventDeviceIdentifierAttribute(7, 8, 9)]
    public class MultiSwitchEventParameters
        : EventParameters
    {
        public LinkSwitch[] Switches { get; set; } = new LinkSwitch[0];
    }
}