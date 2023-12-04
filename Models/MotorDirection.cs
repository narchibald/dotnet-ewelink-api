namespace EWeLink.Api.Models
{
    using System.Runtime.Serialization;

    public enum MotorDirection
    {
        [EnumMember(Value = "forward")]
        Forward,

        [EnumMember(Value = "backward")]
        Backward,
    }
}