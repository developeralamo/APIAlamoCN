using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace APIAlamoCN.Converters
{
    public class IntNullableConverter : JsonConverter<int?>
    {
        public override int? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var str = reader.GetString();
                return int.TryParse(str, out var val) ? val : null;
            }
            if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetInt32();
            }
            return null;
        }

        public override void Write(Utf8JsonWriter writer, int? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
                writer.WriteNumberValue(value.Value);
            else
                writer.WriteNullValue();
        }
    }
}
