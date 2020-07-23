namespace EWeLink.Api.Models
{
    using System;

    using Newtonsoft.Json;

    public class SensorJsonConverter : JsonConverter
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
            if (reader.Value is string val && val == "unavailable")
            {
                if (nullableType == null)
                {
                    return Activator.CreateInstance(objectType);
                }

                return null;
            }

            return Convert.ChangeType(reader.Value, nullableType ?? objectType);
        }

        /// <inheritdoc/>
        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }
}