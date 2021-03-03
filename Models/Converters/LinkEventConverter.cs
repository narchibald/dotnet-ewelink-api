namespace EWeLink.Api.Models.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using EWeLink.Api.Models.Devices;
    using EWeLink.Api.Models.EventParameters;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class LinkEventConverter : JsonConverter
    {
        private static readonly Dictionary<int, Type> DeviceTypes;
        private static Dictionary<string, int> _deviceIdsToUiid;

        [ThreadStatic]
        private static bool disabled;

        static LinkEventConverter()
        {
            DeviceTypes = typeof(LinkEventConverter).Assembly.ExportedTypes
                .Select(x => new { Attribute = x.GetCustomAttribute(typeof(EventDeviceIdentifierAttribute)) as EventDeviceIdentifierAttribute, Type = x })
                .Where(x => x.Attribute != null).SelectMany(x => x.Attribute.Uiids.Select(i => new { Uiid = i, Type = x.Type }))
                .ToDictionary(x => x.Uiid, v => v.Type);
        }

        public static void SetDeviceIdToUiid(Dictionary<string, int> o)
        {
            lock (DeviceTypes)
            {
                _deviceIdsToUiid = o;
            }
        }

        public static Type GetTypeForDeviceId(string deviceId)
        {
            Type deviceType = typeof(GenericDevice);
            lock (DeviceTypes)
            {
                if (_deviceIdsToUiid.TryGetValue(deviceId, out var deviceUiid))
                {
                    DeviceTypes.TryGetValue(deviceUiid, out deviceType);
                }
            }

            return typeof(LinkEvent<>).MakeGenericType(deviceType);
        }

        // Disables the converter in a thread-safe manner.

        /// <inheritdoc/>
        public override bool CanWrite => false;

        /// <inheritdoc/>
        public override bool CanRead => !Disabled;

        private bool Disabled
        {
            get => disabled;
            set => disabled = value;
        }

        /// <inheritdoc/>
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");
        }

        /// <inheritdoc/>
        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);
            var deviceId = jsonObject.Value<string>("deviceid");
            Type eventType = GetTypeForDeviceId(deviceId);

            try
            {
                Disabled = true;
                return jsonObject.ToObject(eventType);
            }
            finally
            {
                Disabled = false;
            }
        }

        /// <inheritdoc/>
        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }
}
