namespace EWeLink.Api
{
    public interface ILinkConfiguration
    {
        string? Email { get; }

        string? Password { get; }

        string? PhoneNumber { get; }

        string? Region { get; }

        string? At { get; }

        string? ApiKey { get; }
    }
}