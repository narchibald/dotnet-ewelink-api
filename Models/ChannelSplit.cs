namespace EWeLink.Api.Models
{
    using Newtonsoft.Json;

    public class ChannelSplit
    {
        [JsonProperty("index")]
        public double Index { get; set; }

        [JsonProperty("roomId")]
        public string RoomId { get; set; } = string.Empty;
    }
}