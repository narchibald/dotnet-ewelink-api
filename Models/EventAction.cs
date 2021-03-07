namespace EWeLink.Api.Models
{
    using System.Runtime.Serialization;

    public enum EventAction
    {
        [EnumMember(Value = "")]
        Unknown,

        [EnumMember(Value = "update")]
        Update,
    }
}