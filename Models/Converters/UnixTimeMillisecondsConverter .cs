namespace EWeLink.Api.Models.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Newtonsoft.Json;

    public class UnixTimeMillisecondsConverter : JsonConverter
    {
        /// <inheritdoc/>
        public override bool CanWrite => false;

        /// <inheritdoc/>
        public override bool CanRead => true;

        /// <inheritdoc/>
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");
        }

        /// <inheritdoc/>
        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var nullableType = Nullable.GetUnderlyingType(objectType);
            if (reader.Value is null || (reader.Value is string val && string.IsNullOrEmpty(val)))
            {
                if (nullableType == null)
                {
                    return Activator.CreateInstance(objectType);
                }

                return null;
            }

            var unixTicks = (long)Convert.ChangeType(reader.Value, typeof(long));
            var dateTime = DateTimeOffset.FromUnixTimeMilliseconds(unixTicks).ToLocalTime();

            object? value = dateTime;
            if ((nullableType ?? objectType) == typeof(DateTime))
            {
                value = dateTime.DateTime;
            }

            return Convert.ChangeType(value, nullableType ?? objectType);
        }

        /// <inheritdoc/>
        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }
}
