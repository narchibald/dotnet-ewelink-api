namespace EWeLink.Api.Models
{
    using System.Runtime.Serialization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    [JsonConverter(typeof(StringEnumConverter))]
    public enum LightType
    {
        [EnumMember(Value = "white")]
        White,

        [EnumMember(Value = "color")]
        Color,

        [EnumMember(Value = "read")]
        Read,

        [EnumMember(Value = "party")]
        Party,
    }
}