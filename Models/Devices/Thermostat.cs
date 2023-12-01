namespace EWeLink.Api.Models.Devices
{
    using EWeLink.Api.Models.Parameters;

    [DeviceIdentifier(15)]
    public class Thermostat : Device<ThermostatSwitchParameters, Tags>, ISingleSwitchDevice, IThermostatDevice
    {
        SwitchParameters ISingleSwitchDevice.Parameters => this.Parameters;

        IThermostatParameters IThermostatDevice.Parameters => this.Parameters;
    }
}
