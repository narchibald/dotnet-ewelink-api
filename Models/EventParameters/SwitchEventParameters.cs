namespace EWeLink.Api.Models.EventParameters
{
    public interface ISwitchEventParameters
        : IEventParameters
    {
        SwitchState Switch { get; set; }
    }

    [EventDeviceIdentifier(1, 6)]
    public class SwitchEventParameters
        : EventParameters, ISwitchEventParameters
    {
        public SwitchState Switch { get; set; }
    }
}