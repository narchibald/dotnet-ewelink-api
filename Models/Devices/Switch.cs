namespace EWeLink.Api.Models.Devices
{
    using EWeLink.Api.Models.Parameters;

    [DeviceIdentifier(6)]
    public class Switch : Device<SwitchParameters, Tags>, ISingleSwitchDevice
    {
    }
}
