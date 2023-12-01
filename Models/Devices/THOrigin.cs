namespace EWeLink.Api.Models.Devices
{
    using EWeLink.Api.Models.Parameters;

    public interface ITHOrigin : IDevice<THOriginParameters, Tags>
    {
    }

    [DeviceIdentifier(181)]
    public class THOrigin : Device<THOriginParameters, Tags>, ITHOrigin, ISingleSwitchDevice, IThermostatDevice
    {
        SwitchParameters ISingleSwitchDevice.Parameters => this.Parameters;

        IThermostatParameters IThermostatDevice.Parameters => this.Parameters;
    }
}