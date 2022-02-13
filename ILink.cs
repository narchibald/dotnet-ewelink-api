using EWeLink.Api.Models.LightThemes;

namespace EWeLink.Api
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using EWeLink.Api.Models;
    using EWeLink.Api.Models.Devices;

    public interface ILink
    {
        Uri ApiUri { get; }

        Uri OtaUri { get; }

        Uri ApiWebSocketUri { get; }

        Task<SensorData?> GetDeviceCurrentSensorData(string deviceId);

        Task<(string? Email, string? Region)> GetRegion();

        Task<Device> GetDevice(string deviceId);

        int GetDeviceChannelCountByUuid(int uuid);

        Task ToggleDevice(string deviceId, int channel = 1);

        Task<int> GetDeviceChannelCount(string deviceId);

        Task SetDevicePowerState(string deviceId, SwitchStateChange state, int channel = 1);

        Task SetLightColor(string deviceId, LightBrightness value);

        Task<ILinkWebSocket> OpenWebSocket(CancellationToken cancellationToken = default);

        Task<List<Device>> GetDevices();

        Task<List<UpdateCheckResult>> CheckAllDeviceUpdates();

        Task<List<UpgradeInfo>> CheckDeviceUpdates(IEnumerable<Device> devices);

        Task<UpdateCheckResult> CheckDeviceUpdate(string deviceId);

        Task<string?> GetFirmwareVersion(string deviceId);

        Task<Credentials> GetCredentials();
    }
}