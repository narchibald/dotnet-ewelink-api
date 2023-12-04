namespace EWeLink.Api.Models
{
    using System.Runtime.Serialization;

    public enum CurtainAction
    {
        [EnumMember(Value = "close")]
        Close,

        [EnumMember(Value = "open")]
        Open,

        [EnumMember(Value = "stop")]
        Stop,

        [EnumMember(Value = "pause")]
        Pause,
    }
}