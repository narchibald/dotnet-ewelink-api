namespace EWeLink.Api
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using EWeLink.Api.Models.Devices;
    using EWeLink.Api.Models.EventParameters;

    public class DeviceCache : IDeviceCache
    {
        private static readonly Dictionary<int, Type> UiidToEventParameterTypes;
        private readonly Dictionary<string, IDevice> deviceIdToDevices = new ();
        private readonly Dictionary<string, int> deviceIdTUiid = new ();

        static DeviceCache()
        {
            UiidToEventParameterTypes = typeof(DeviceCache).Assembly.ExportedTypes
                .Select(x => new { Attribute = x.GetCustomAttribute(typeof(EventDeviceIdentifierAttribute)) as EventDeviceIdentifierAttribute, Type = x })
                .Where(x => x.Attribute != null).SelectMany(x => x.Attribute!.Uiids.Select(i => new { Uiid = i, Type = x.Type }))
                .ToDictionary(x => x.Uiid, v => v.Type);
        }

        public int? GetDevicesUiid(string deviceId)
        {
            if (this.deviceIdTUiid.TryGetValue(deviceId, out var uiid))
            {
                return uiid;
            }

            return null;
        }

        public bool TryGetDevice(string deviceId, out IDevice? device)
        {
            device = null;
            if (this.deviceIdToDevices.TryGetValue(deviceId, out var val))
            {
                device = val;
                return true;
            }

            return false;
        }

        public Type? GetEventParameterTypeForUiid(int uiid)
        {
            if (UiidToEventParameterTypes.TryGetValue(uiid, out var type))
            {
                return type;
            }

            return null;
        }

        public bool TryGetEventParameterTypeForUiid(int uiid, out Type? type)
        {
            type = null;
            if (UiidToEventParameterTypes.TryGetValue(uiid, out var val))
            {
                type = val;
                return true;
            }

            return false;
        }

        public IDevice? GetDevice(string deviceId)
        {
            if (this.deviceIdToDevices.TryGetValue(deviceId, out var device))
            {
                return device;
            }

            return null;
        }

        public IEnumerable<IDevice> UpdateCache(IEnumerable<IDevice> devices)
        {
            foreach (var device in devices)
            {
                UpdateCache(device);
            }

            return devices;
        }

        public IDevice UpdateCache(IDevice device)
        {
            lock (this.deviceIdToDevices)
            {
                if (device is { DeviceId: not null })
                {
                    if (!this.deviceIdToDevices.ContainsKey(device.DeviceId))
                    {
                        this.deviceIdToDevices.Add(device.DeviceId, device);
                        this.deviceIdTUiid.Add(device.DeviceId, device.Uiid);
                    }
                    else
                    {
                        this.deviceIdToDevices[device.DeviceId] = device;
                        this.deviceIdTUiid[device.DeviceId] = device.Uiid;
                    }
                }

                return device;
            }
        }
    }
}