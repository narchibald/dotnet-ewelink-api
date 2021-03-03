namespace EWeLink.Api.Models.Devices
{
    using EWeLink.Api.Models.Parameters;

    interface IMultiSwitchDevice
    {
        MultiSwitchParameters Parameters { get; }
    }
}
