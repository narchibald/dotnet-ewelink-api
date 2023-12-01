namespace EWeLink.Api.Models.Devices
{
    using EWeLink.Api.Models.Parameters;

    [DeviceIdentifier(104)]
    public class Bulb05B : Device<ColorLightParameters, Tags>, ISingleSwitchDevice
    {
        SwitchParameters ISingleSwitchDevice.Parameters => this.Parameters;
    }
}
