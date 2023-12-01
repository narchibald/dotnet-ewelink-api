namespace EWeLink.Api.Models.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Newtonsoft.Json;

    public class UnixTimeMillisecondsConverter : JsonConverter
    {
        /// <inheritdoc/>
        public override bool CanWrite => true;

        /// <inheritdoc/>
        public override bool CanRead => true;

        /// <inheritdoc/>
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value is null)
            {
                writer.WriteNull();
            }
            else if (value is DateTimeOffset dateTime)
            {
                writer.WriteValue(dateTime.ToUnixTimeMilliseconds());
            }
            else
            {
                writer.WriteNull();
            }
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
