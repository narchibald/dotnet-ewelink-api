namespace EWeLink.Api
{
    using System;
    using System.Collections.Generic;
    using EWeLink.Api.Models.Devices;

    public interface IDeviceCache
    {
        int? GetDevicesUiid(string deviceId);

        IDevice? GetDevice(string deviceId);

        bool TryGetDevice(string deviceId, out IDevice? device);

        Type? GetEventParameterTypeForUiid(int uiid);

        bool TryGetEventParameterTypeForUiid(int uiid, out Type? type);

        IEnumerable<IDevice> UpdateCache(IEnumerable<IDevice> devices);

        IDevice UpdateCache(IDevice device);
    }
}