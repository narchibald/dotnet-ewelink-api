namespace EWeLink.Api.Models
{
    using System.Runtime.Serialization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    [JsonConverter(typeof(StringEnumConverter))]
    public enum EventAction
    {
        [EnumMember(Value = "")]
        Unknown,

        [EnumMember(Value = "update")]
        Update,

        [EnumMember(Value = "sysmsg")]
        SystemMessage,

        [EnumMember(Value = "reportSubDevice")]
        ReportSubDevice,
    }
}