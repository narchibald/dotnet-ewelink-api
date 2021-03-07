namespace EWeLink.Api.Models.Devices
{
    using EWeLink.Api.Models.Parameters;

    public interface IThermostatDevice
    {
        IThermostatParameters Parameters { get; }
    }
}
