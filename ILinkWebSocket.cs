namespace EWeLink.Api
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using EWeLink.Api.Models;

    public interface ILinkWebSocket : IDisposable
    {
        Task Open(string wssLoginPayload, Uri apiWebSocketUri, CancellationToken cancellationToken);

        Task<LinkWebSocketReceiveResult?> Receive(CancellationToken cancellationToken);
    }
}