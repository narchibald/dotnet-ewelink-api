namespace EWeLink.Api
{
    using System;
    using System.Collections.Generic;
    using EWeLink.Api.Models.Devices;

    public interface IDeviceCache
    {
        int? GetDevicesUiid(string deviceId);

        Device? GetDevice(string deviceId);

        bool TryGetDevice(string deviceId, out Device? device);

        Type? GetEventParameterTypeForUiid(int uiid);

        bool TryGetEventParameterTypeForUiid(int uiid, out Type? type);

        IEnumerable<Device> UpdateCache(IEnumerable<Device> devices);

        Device UpdateCache(Device device);
    }
}