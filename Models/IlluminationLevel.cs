namespace EWeLink.Api.Models;

using System.Runtime.Serialization;

public enum IlluminationLevel
{
    [EnumMember(Value="darker")]
    Darker,

    [EnumMember(Value="brighter")]
    Brighter,
}