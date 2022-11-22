namespace EWeLink.Api.Models.Devices
{
    using EWeLink.Api.Models.Parameters;

    public interface IMultiSwitchDevice
    {
        MultiSwitchParameters Parameters { get; }
    }

    public interface ITwoSwitchDevice
    {
        TwoSwitchParameters Parameters { get; }
    }

    public interface IThreeSwitchDevice
    {
        ThreeSwitchParameters Parameters { get; }
    }

    public interface IFourSwitchDevice
    {
        FourSwitchParameters Parameters { get; }
    }
}
