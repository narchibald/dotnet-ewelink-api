namespace EWeLink.Api.Models.Devices
{
    using EWeLink.Api.Models.Parameters;

    public interface ISingleSwitchDevice
    {
        SwitchParameters Parameters { get; }
    }
}
