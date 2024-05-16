using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace DiscuitSharp.Core.Common.Serialization
{
    public class CursorConverter<T> : JsonConverter<Cursor<T>>
    {
        public override Cursor<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException("Expected StartObject token.");
            }

            var result = new Cursor<T>();
            string parentProperty = string.Empty;
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return result;
                }

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    var propertyName = reader.GetString();
                    reader.Read();  // Move to the value.
                    switch (propertyName)
                    {
                        case "posts":
                            result.Records = JsonSerializer.Deserialize<List<T>>(ref reader, options);
                            parentProperty = "posts";
                            break;
                        case "comments":
                            result.Records = JsonSerializer.Deserialize<List<T>>(ref reader, options);
                            break;
                        case "next":
                            result.Next = (parentProperty == "posts") 
                                ? reader.GetInt64().ToString()
                                : reader.GetString();
                            break;
                        default:
                            throw new JsonException($"Property {propertyName} is not expected.");
                    }
                }
            }

            throw new JsonException("Expected EndObject token.");
        }

        public override void Write(Utf8JsonWriter writer, Cursor<T> value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("items"); // Use a generic name or you might need to adjust based on the type of T
            JsonSerializer.Serialize(writer, value.Records, options);
            writer.WriteEndObject();
        }
    }
}
