namespace EWeLink.Api.Models
{
    using System.Runtime.Serialization;

    public enum Motion
    {
        [EnumMember(Value = "none")]
        None = 0,

        [EnumMember(Value = "detected")]
        Detected = 1,
    }
}