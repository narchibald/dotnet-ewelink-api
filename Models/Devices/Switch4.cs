namespace EWeLink.Api.Models.Devices
{
    using EWeLink.Api.Models.Parameters;

    [DeviceIdentifier(9)]
    public class Switch4 : Device<FourSwitchParameters>, IFourSwitchDevice
    {
    }
}
