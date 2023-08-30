namespace EWeLink.Api
{
    using System;
    using System.Linq;
    using System.Net.WebSockets;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using EWeLink.Api.Models;
    using EWeLink.Api.Models.Devices;
    using EWeLink.Api.Models.EventParameters;
    using EWeLink.Api.Models.Parameters;

    using Microsoft.Extensions.Logging;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class LinkWebSocket : ILinkWebSocket
    {
        private readonly IDeviceCache deviceCache;
        private readonly ILogger<LinkWebSocket> logger;
        private ClientWebSocket? websocket;

        public LinkWebSocket(IDeviceCache deviceCache, ILogger<LinkWebSocket> logger)
        {
            this.deviceCache = deviceCache;
            this.logger = logger;
        }

        public async Task Open(string wssLoginPayload, Uri apiWebSocketUri, CancellationToken cancellationToken)
        {
            websocket?.Dispose();
            this.websocket = new ();
            this.websocket.Options.KeepAliveInterval = TimeSpan.FromMilliseconds(120000);

            await this.websocket.ConnectAsync(apiWebSocketUri, CancellationToken.None);

            var data = new ArraySegment<byte>(Encoding.UTF8.GetBytes(wssLoginPayload));
            await this.websocket.SendAsync(data, WebSocketMessageType.Text, true, cancellationToken);

            var buffer = new ArraySegment<byte>(new byte[8192]);
            var result = await this.websocket.ReceiveAsync(buffer, cancellationToken);

            if (result.MessageType == WebSocketMessageType.Text && buffer.Array != null)
            {
                var text = Encoding.UTF8.GetString(buffer.ToArray(), 0, result.Count);
                var json = JsonConvert.DeserializeObject<dynamic>(text);
                int? error = json?.error;
                if (error > 0)
                {
                    throw new Exception($"Error: {error}");
                }
            }
        }

        public void Dispose()
        {
            this.websocket?.Dispose();
        }

        public async Task<LinkWebSocketReceiveResult?> Receive(CancellationToken cancellationToken)
        {
            if (this.websocket == null)
            {
                throw new ArgumentException("Websocket not opened. Make sure you call the Open' method before calling receive");
            }

            // wait for event
            var buffer = new ArraySegment<byte>(new byte[8192]);
            var socketReceiveResult = await this.websocket.ReceiveAsync(buffer, cancellationToken);

            switch (socketReceiveResult.MessageType)
            {
                case WebSocketMessageType.Close:
                    this.logger.LogInformation("Web socket closed. status:{CloseStatus}, description: {CloseStatusDescription}", socketReceiveResult.CloseStatus, socketReceiveResult.CloseStatusDescription);
                    return new LinkWebSocketReceiveResult(socketReceiveResult);
                case WebSocketMessageType.Text:
                    var linkEvent = DeserializeEvent(buffer.ToArray(), socketReceiveResult.Count);
                    if (linkEvent != null)
                    {
                        return new LinkWebSocketReceiveResult(socketReceiveResult, linkEvent);
                    }

                    return null;
                default:
                    this.logger.LogWarning("Unhandled web socket message type. messageType:{MessageType", socketReceiveResult.MessageType);
                    return null;
            }
        }

        private ILinkEvent<IEventParameters>? DeserializeEvent(byte[] buffer, int count)
        {
            var text = Encoding.UTF8.GetString(buffer, 0, count);

            this.logger.LogDebug("Received event: {Text}", text);
            var jsonObject = JObject.Parse(text);
            jsonObject.Add("eventSource", new JValue(EventSource.Cloud));
            var deviceId = jsonObject.Value<string>("deviceid");

            var deviceUiid = jsonObject.Value<int?>("uiid");
            if (!deviceUiid.HasValue)
            {
                deviceUiid = this.deviceCache.GetDevicesUiid(deviceId ?? string.Empty);
                if (deviceUiid.HasValue)
                {
                    jsonObject.Add("uiid", new JValue(deviceUiid.Value));
                }
            }

            Type deviceType = deviceUiid.HasValue ? this.deviceCache.GetEventParameterTypeForUiid(deviceUiid.Value) ?? typeof(EventParameters) : typeof(EventParameters);
            var jsonObjectParams = (JObject?)jsonObject.GetValue("params");
            if (jsonObjectParams != null)
            {
                var device = this.deviceCache.GetDevice(deviceId ?? string.Empty);
                if (device is IDevice<Parameters> typedDevice)
                {
                    var updateResult = typedDevice.Parameters.Update(jsonObjectParams.ToString());
                    if (typeof(IMultiSwitchEventParameters).IsAssignableFrom(deviceType) && updateResult.HasValue)
                    {
                        jsonObjectParams.Add("triggeredOutlet", updateResult.Value);
                    }
                }

                if (typeof(SnZbEventParameters).IsAssignableFrom(deviceType))
                {
                    if (jsonObjectParams.Count == 2 && new[] { "battery", "trigTime" }.All(jsonObjectParams.ContainsKey))
                    {
                        deviceType = typeof(SnZbBatteryEventParameter);
                    }
                }

                if (jsonObject.TryGetValue("proxyMsgTime", out var jvalue))
                {
                    jsonObjectParams.Add("trigTime", jvalue);
                }

                if (typeof(EventParameters).IsAssignableFrom(deviceType))
                {
                    if (!jsonObjectParams.ContainsKey("trigTime"))
                    {
                        jsonObjectParams.Add("trigTime", new JValue(DateTimeOffset.Now.ToUnixTimeMilliseconds()));
                    }
                }
            }

            var eventType = typeof(LinkEvent<>).MakeGenericType(deviceType);
            var linkEvent = jsonObject.ToObject(eventType) as ILinkEvent<IEventParameters>;
            return linkEvent;
        }
    }
}