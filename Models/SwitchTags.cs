namespace EWeLink.Api.Models
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class SwitchTags : Tags
    {
        [JsonProperty("ck_channel_name")]
        public Dictionary<ChannelId, string> ChannelNames { get; set; } = new Dictionary<ChannelId, string>();

        [JsonProperty("channelSplitIcon")]
        public Dictionary<ChannelId, int> ChannelSplitIcons { get; set; } = new Dictionary<ChannelId, int>();

        [JsonProperty("channelSplit")]
        public Dictionary<ChannelId, ChannelSplit> ChannelSplits { get; set; } = new Dictionary<ChannelId, ChannelSplit>();
    }
}