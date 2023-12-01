namespace EWeLink.Api.Models.Devices
{
    using EWeLink.Api.Models.Parameters;

    [DeviceIdentifier(9)]
    [DeviceIdentifier(162)]
    public class Switch4 : MultiSwitchDevice<FourSwitchParameters>, IFourSwitchDevice
    {
    }
}
