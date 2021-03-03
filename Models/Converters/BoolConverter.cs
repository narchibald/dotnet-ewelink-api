using System;
using System.Collections.Generic;
using System.Text;

namespace EWeLink.Api.Models.Converters
{
    using Newtonsoft.Json;

    public class BoolConverter: JsonConverter
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

        public override object? ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var nullableType = Nullable.GetUnderlyingType(objectType);
            if (reader.Value is null)
            {
                if (nullableType == null)
                {
                    return Activator.CreateInstance(objectType);
                }

                return null;
            }

            bool value = false;
            if (reader.Value is long i)
            {
                value = i != 0;
            }

            return Convert.ChangeType(value, nullableType ?? objectType);
        }

        public override bool CanConvert(Type objectType)
        {
            var nullableType = Nullable.GetUnderlyingType(objectType);
            return (nullableType ?? objectType) == typeof(bool);
        }
    }
}
