namespace EWeLink.Api.Models.Devices
{
    using EWeLink.Api.Models.Parameters;

    public abstract class MultiSwitchDevice<T> : Device<T, SwitchTags>
        where T : MultiSwitchParameters
    {
    }
}