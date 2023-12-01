namespace EWeLink.Api.Models.Devices
{
    using EWeLink.Api.Models.Parameters;

    [DeviceIdentifier(8)]
    public class Switch3 : MultiSwitchDevice<ThreeSwitchParameters>, IThreeSwitchDevice
    {
    }
}
