using System.Text.Json.Serialization;
using System.Text.Json;
using DiscuitSharp.Core.Media;

namespace DiscuitSharp.Core.Group.Serialization
{
    public class ImageConverter : JsonConverter<Image>
    {
        public override Image Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException("Expected StartObject token.");
            

            var image = new Image();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return image;
                }

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    var propertyName = reader.GetString();
                    reader.Read();
                    switch (propertyName)
                    {
                        case "id":
                            image.Id = reader.TokenType != JsonTokenType.Null ? reader.GetString() : null;
                            break;
                        case "format":
                            image.Format = reader.TokenType != JsonTokenType.Null ? reader.GetString() : null;
                            break;
                        case "mimetype":
                            image.Mimetype = reader.TokenType != JsonTokenType.Null ? reader.GetString() : null;
                            break;
                        case "width":
                            image.Width = reader.GetInt32();
                            break;
                        case "height":
                            image.Height = reader.GetInt32();
                            break;
                        case "size":
                            image.Size = reader.GetInt32();
                            break;
                        case "averageColor":
                            image.AverageColor = reader.TokenType != JsonTokenType.Null ? reader.GetString() : null;
                            break;
                        case "url":
                            image.Url = reader.TokenType != JsonTokenType.Null ? reader.GetString() : null;
                            break;
                        case "copies":
                            if (reader.TokenType != JsonTokenType.Null)
                            {
                                image.Copies = JsonSerializer.Deserialize<List<ImageCopy>>(ref reader, options);
                            }
                            break;
                        default:
                            reader.Skip(); // Skip any unknown elements
                            break;
                    }
                }
            }

            throw new JsonException("Expected EndObject token.");
        }

        public override void Write(Utf8JsonWriter writer, Image value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("id", value.Id);
            writer.WriteString("format", value.Format);
            writer.WriteString("mimetype", value.Mimetype);
            writer.WriteNumber("width", value.Width);
            writer.WriteNumber("height", value.Height);
            writer.WriteNumber("size", value.Size);
            writer.WriteString("averageColor", value.AverageColor);
            writer.WriteString("url", value.Url);
            if (value.Copies != null)
            {
                writer.WritePropertyName("copies");
                JsonSerializer.Serialize(writer, value.Copies, options);
            }
            else
            {
                writer.WriteNull("copies");
            }
            writer.WriteEndObject();
        }
    }
}