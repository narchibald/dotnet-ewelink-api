namespace EWeLink.Api.Models.EventParameters
{
    using System;

    public interface IMultiSwitchEventParameters
        : IEventParameters
    {
        LinkSwitch[] Switches { get; set; }
    }

    public class MultiSwitchEventParameters
        : EventParameters, IMultiSwitchEventParameters
    {
        public LinkSwitch[] Switches { get; set; } = Array.Empty<LinkSwitch>();
    }

    public interface ITwoSwitchEventParameters
        : IEventParameters
    {
        LinkSwitch One { get; }

        LinkSwitch Two { get; }
    }

    [EventDeviceIdentifier(7)]
    public class TwoSwitchEventParameters
        : MultiSwitchEventParameters, ITwoSwitchEventParameters
    {
        public LinkSwitch One => Switches[0];

        public LinkSwitch Two => Switches[1];
    }

    public interface IThreeSwitchEventParameters
        : ITwoSwitchEventParameters
    {
        LinkSwitch Three { get; }
    }

    [EventDeviceIdentifier(8)]
    public class ThreeSwitchEventParameters
        : TwoSwitchEventParameters, IThreeSwitchEventParameters
    {
        public LinkSwitch Three => Switches[2];
    }

    public interface IFourSwitchEventParameters
        : IThreeSwitchEventParameters
    {
        LinkSwitch Four { get; }
    }

    [EventDeviceIdentifier(9)]
    public class FourSwitchEventParameters
        : ThreeSwitchEventParameters, IFourSwitchEventParameters
    {
        public LinkSwitch Four => Switches[3];
    }
}