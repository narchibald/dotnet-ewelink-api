namespace EWeLink.Api
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using EWeLink.Api.Models;
    using EWeLink.Api.Models.Devices;
    using EWeLink.Api.Models.EventParameters;
    using EWeLink.Api.Models.LightThemes;

    public interface ILink
    {
        event Action<ILinkEvent<IEventParameters>>? LanParametersUpdated;

        Uri ApiUri { get; }

        Uri OtaUri { get; }

        Uri ApiWebSocketUri { get; }

        Task<SensorData?> GetDeviceCurrentSensorData(string deviceId);

        Task<(string? Email, string? Region)> GetRegion();

        Task<IDevice?> GetDevice(string deviceId);

        Task<IDevice?> GetDevice(string deviceId, bool noCacheLoad);

        int GetDeviceChannelCountByUuid(int uuid);

        Task ToggleDevice(string deviceId, ChannelId channel = ChannelId.One);

        Task<int> GetDeviceChannelCount(string deviceId);

        Task SetDevicePowerState(string deviceId, SwitchStateChange state, ChannelId channel = ChannelId.One);

        Task SetLightColor(string deviceId, LightBrightness value);

        Task<ILinkWebSocket> OpenWebSocket(CancellationToken cancellationToken = default);

        void EnableLanControl();

        Task<List<IDevice>> GetDevices();

        Task<List<UpdateCheckResult>> CheckAllDeviceUpdates();

        Task<List<UpgradeInfo>> CheckDeviceUpdates(IEnumerable<IDevice> devices);

        Task<UpdateCheckResult> CheckDeviceUpdate(string deviceId);

        Task<string?> GetFirmwareVersion(string deviceId);

        Task<Credentials> GetCredentials();
    }

    public interface ILinkAuthorization
    {
        event Action<OAuhToken>? OAuthTokenUpdated;

        (string Signature, long Seq, string AppId) GetAuthorization();

        Task<OAuhToken> GetAccessToken(string code, string redirectUri);
    }
}