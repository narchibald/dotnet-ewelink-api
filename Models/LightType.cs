namespace EWeLink.Api.Models
{
    using System.Runtime.Serialization;

    public enum LightType
    {
        [EnumMember(Value = "white")]
        White,

        [EnumMember(Value = "color")]
        Color,
    }
}