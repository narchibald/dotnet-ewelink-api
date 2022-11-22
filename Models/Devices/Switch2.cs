namespace EWeLink.Api.Models.Devices
{
    using EWeLink.Api.Models.Parameters;

    [DeviceIdentifier(7)]
    public class Switch2 : Device<TwoSwitchParameters>, ITwoSwitchDevice
    {
    }
}
