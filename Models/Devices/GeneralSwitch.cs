namespace EWeLink.Api.Models.Devices
{
    using EWeLink.Api.Models.Parameters;

    public interface IGeneralSwitch : IDevice<SwitchParameters>
    {
    }

    [DeviceIdentifier(1)]
    public class GeneralSwitch : Device<SwitchParameters>, IGeneralSwitch
    {
    }
}
