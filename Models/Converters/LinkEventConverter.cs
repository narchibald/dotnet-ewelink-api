namespace EWeLink.Api.Models.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using EWeLink.Api.Models.EventParameters;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class LinkEventConverter : JsonConverter
    {
        private static readonly Dictionary<int, Type> DeviceTypes;

        [ThreadStatic]
        private static bool disabled;

        static LinkEventConverter()
        {
            DeviceTypes = typeof(LinkEventConverter).Assembly.ExportedTypes
                .Select(x => new { Attribute = x.GetCustomAttribute(typeof(EventDeviceIdentifierAttribute)) as EventDeviceIdentifierAttribute, Type = x })
                .Where(x => x.Attribute != null).SelectMany(x => x.Attribute!.Uiids.Select(i => new { Uiid = i, Type = x.Type }))
                .ToDictionary(x => x.Uiid, v => v.Type);
        }

        /// <inheritdoc/>
        public override bool CanWrite => false;

        /// <inheritdoc/>
        public override bool CanRead => !this.Disabled;

        // Disables the converter in a thread-safe manner.

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
            var deviceUiid = jsonObject.Value<int?>("uiid");
            Type eventType = objectType; // GetTypeForUiid(deviceUiid);

            try
            {
                this.Disabled = true;
                return jsonObject.ToObject(eventType);
            }
            finally
            {
                this.Disabled = false;
            }
        }

        /// <inheritdoc/>
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        private static Type GetTypeForUiid(int? deviceUiid)
        {
            if (!deviceUiid.HasValue || !DeviceTypes.TryGetValue(deviceUiid.Value, out var deviceType))
            {
                deviceType = typeof(EventParameters);
            }

            return typeof(LinkEvent<>).MakeGenericType(deviceType);
        }
    }
}
