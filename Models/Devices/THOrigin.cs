namespace EWeLink.Api.Models.Devices
{
    using EWeLink.Api.Models.Parameters;

    public interface ITHOrigin : IDevice<THOriginParameters>
    {
    }

    [DeviceIdentifier(181)]
    public class THOrigin : Device<THOriginParameters>, ITHOrigin, ISingleSwitchDevice, IThermostatDevice
    {
        SwitchParameters ISingleSwitchDevice.Parameters => this.Parameters;

        IThermostatParameters IThermostatDevice.Parameters => this.Parameters;
    }
}