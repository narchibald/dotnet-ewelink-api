namespace EWeLink.Api.Models.Converters
{
    using System;
    using System.Linq;

    using Newtonsoft.Json;

    public class ThermostatConverter : JsonConverter
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
            else if (value is decimal decimalValue)
            {
                writer.WriteValue(decimalValue.ToString().Replace(".", string.Empty));
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
            string? val = reader.Value as string;
            if (string.IsNullOrEmpty(val))
            {
                if (nullableType == null)
                {
                    return Activator.CreateInstance(objectType);
                }

                return null;
            }

            var value = decimal.Parse(new string(val.Take(val.Length - 2).Concat(new[] { '.' }).Concat(val.Skip(val.Length - 2)).ToArray()));
            return Convert.ChangeType(value, nullableType ?? objectType);
        }

        /// <inheritdoc/>
        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }
}