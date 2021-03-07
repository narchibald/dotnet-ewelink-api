namespace EWeLink.Api.Models.Devices
{
    using EWeLink.Api.Models.Parameters;

    public interface IMultiSwitchDevice
    {
        MultiSwitchParameters Parameters { get; }
    }
}
