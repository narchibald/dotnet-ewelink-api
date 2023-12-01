namespace EWeLink.Api.Models.Devices
{
    using EWeLink.Api.Models.Parameters;

    public interface IGeneralSwitch : IDevice<SwitchParameters, Tags>
    {
    }

    [DeviceIdentifier(1)]
    public class GeneralSwitch : Device<SwitchParameters, Tags>, IGeneralSwitch
    {
    }
}
