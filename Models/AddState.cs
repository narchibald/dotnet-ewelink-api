namespace EWeLink.Api.Models
{
    using System.Runtime.Serialization;

    public enum AddState
    {
        [EnumMember(Value = "off")]
        Off,

        [EnumMember(Value = "on")]
        On,
    }
}