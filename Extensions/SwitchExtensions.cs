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
