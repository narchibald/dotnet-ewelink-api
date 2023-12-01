namespace EWeLink.Api.Models.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using EWeLink.Api.Models.Devices;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class DeviceConverter : JsonConverter
    {
        private static readonly Dictionary<int, Type> DeviceTypes;

        [ThreadStatic]
        private static bool disabled;

        static DeviceConverter()
        {
            DeviceTypes = typeof(DeviceConverter).Assembly.ExportedTypes
                .SelectMany(x => x.GetCustomAttributes<DeviceIdentifierAttribute>().Select(a => new { Attribute = a, Type = x }))
                .Where(x => x.Attribute != null).ToDictionary(x => x.Attribute!.Uiid, v => v.Type);
        }

        // Disables the converter in a thread-safe manner.

        /// <inheritdoc/>
        public override bool CanWrite => false;

        /// <inheritdoc/>
        public override bool CanRead => !this.Disabled;

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
            if (!jsonObject.Value<int?>("uiid").HasValue && jsonObject["extra"]?.Value<int?>("uiid") != null)
            {
                jsonObject["uiid"] = jsonObject["extra"]?.Value<int>("uiid");
            }

            var deviceUiid = jsonObject.Value<int>("uiid");
            if (!DeviceTypes.TryGetValue(deviceUiid, out var deviceType))
            {
                deviceType = typeof(GenericDevice);
            }

            try
            {
                Disabled = true;
                return jsonObject.ToObject(deviceType);
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
