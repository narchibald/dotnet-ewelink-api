namespace EWeLink.Api.Models.Devices
{
    using EWeLink.Api.Models.Parameters;

    interface IThermostatDevice
    {
        IThermostatParameters Parameters { get; }
    }
}
