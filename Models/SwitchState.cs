namespace EWeLink.Api.Models
{
    using System.Runtime.Serialization;

    public enum SwitchState
    {
        [EnumMember(Value = "on")]
        On,

        [EnumMember(Value = "off")]
        Off,
    }

    public enum SwitchStateChange
    {
        Toggle,

        On,

        Off,
    }
}