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
}