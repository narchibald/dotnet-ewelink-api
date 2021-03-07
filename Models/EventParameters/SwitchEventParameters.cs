namespace EWeLink.Api.Models.EventParameters
{
    [EventDeviceIdentifierAttribute(6)]
    public class SwitchEventParameters
        : EventParameters
    {
        public SwitchState Switch { get; set; }
    }
}