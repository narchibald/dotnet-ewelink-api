namespace EWeLink.Api.Models
{
    using System;
    using System.Net.WebSockets;

    public class LinkWebSocketReceiveResult : ILinkWebSocketReceiveResult
    {
        private readonly WebSocketReceiveResult socketReceiveResult;

        public LinkWebSocketReceiveResult(WebSocketReceiveResult socketReceiveResult, ILinkEvent<EventParameters.IEventParameters>? @event = null)
        {
            this.socketReceiveResult = socketReceiveResult;
            this.Event = @event;
        }

        public bool SocketClosed => this.socketReceiveResult.MessageType == WebSocketMessageType.Close;

        public ILinkEvent<EventParameters.IEventParameters>? Event { get; }
    }
}