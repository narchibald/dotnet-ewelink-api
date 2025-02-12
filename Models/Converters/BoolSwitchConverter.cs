namespace EWeLink.Api.Models.Converters
{
    using System;
    using Newtonsoft.Json;

    public class BoolSwitchConverter : JsonConverter
    {
        /// <inheritdoc/>
        public override bool CanWrite => false;

        /// <inheritdoc/>
        public override bool CanRead => true;

        /// <inheritdoc/>
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value is null)
            {
                writer.WriteNull();
            }
            else if (value is SwitchState state)
            {
                writer.WriteValue(state == SwitchState.On);
            }

            throw new NotImplementedException();
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
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
            if (reader.Value is bool boolVal)
            {
                value = boolVal;
            }

            return value ? SwitchState.On : SwitchState.Off;
        }

        public override bool CanConvert(Type objectType)
        {
            var nullableType = Nullable.GetUnderlyingType(objectType);
            return (nullableType ?? objectType) == typeof(bool);
        }
    }
}
