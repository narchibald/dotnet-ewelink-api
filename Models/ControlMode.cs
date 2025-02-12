namespace EWeLink.Api.Models
{
    using System.Runtime.Serialization;

    public enum ControlMode
    {
        [EnumMember(Value = "manual")]
        Manual,

        [EnumMember(Value = "auto")]
        Auto,
    }
}