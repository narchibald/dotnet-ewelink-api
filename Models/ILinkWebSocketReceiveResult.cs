namespace EWeLink.Api.Models
{
    public interface ILinkWebSocketReceiveResult
    {
        bool SocketClosed { get; }

        ILinkEvent<EventParameters.IEventParameters>? Event { get; }
    }
}