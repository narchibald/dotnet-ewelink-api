using System;

namespace EWeLink.Api.Extensions
{
    using EWeLink.Api.Models;
    using EWeLink.Api.Models.EventParameters;

    public static class SwitchExtensions
    {
        public static LinkSwitch? One(this IMultiSwitchEventParameters parameters) => Channel(parameters, ChannelId.One);

        public static LinkSwitch? Two(this IMultiSwitchEventParameters parameters) => Channel(parameters, ChannelId.Two);

        public static LinkSwitch? Three(this IMultiSwitchEventParameters parameters) => Channel(parameters, ChannelId.Three);

        public static LinkSwitch? Four(this IMultiSwitchEventParameters parameters) => Channel(parameters, ChannelId.Four);

        public static SwitchStateChange ToStateChange(this SwitchState state)
        {
            return state switch
            {
                SwitchState.Off => SwitchStateChange.Off,
                SwitchState.On => SwitchStateChange.On,
                _ => throw new ArgumentOutOfRangeException(nameof(state))
            };
        }

        public static SwitchStateChange Invert(this SwitchStateChange state)
        {
            return state switch
            {
                SwitchStateChange.Off => SwitchStateChange.On,
                SwitchStateChange.On => SwitchStateChange.Off,
                _ => throw new ArgumentOutOfRangeException(nameof(state))
            };
        }

        public static SwitchState Invert(this SwitchState state)
        {
            return state switch
            {
                SwitchState.Off => SwitchState.On,
                SwitchState.On => SwitchState.Off,
                _ => throw new ArgumentOutOfRangeException(nameof(state))
            };
        }

        public static LinkSwitch? Channel(this IMultiSwitchEventParameters parameters, ChannelId channel)
        {
            var switches = parameters.Switches;
            var index = (int)channel;
            if (index < switches.Length)
            {
                return switches[index];
            }

            return null;
        }
    }
}
