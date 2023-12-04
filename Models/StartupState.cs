namespace EWeLink.Api.Models
{
    using System.Runtime.Serialization;

    public enum StartupState
    {
        [EnumMember(Value = "on")]
        On,

        [EnumMember(Value = "off")]
        Off,

        [EnumMember(Value = "stay")]
        Stay,
    }
}