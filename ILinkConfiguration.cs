namespace EWeLink.Api
{
    using System;
    using EWeLink.Api.Models;

    public interface ILinkConfiguration
    {
        string? Email { get; }

        string? Password { get; }

        string? PhoneNumber { get; }

        string? Region { get; }

        string? At { get; }

        string? ApiKey { get; }

        OAuhToken? AuhToken { get; }

        string? AppId { get; }
        
        string? AppSecret { get; }

        TimeSpan? DeviceCacheTimeout { get; }
    }
}