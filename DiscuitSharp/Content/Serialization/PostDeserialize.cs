using System.Text.Json.Serialization;
using System.Text.Json;
using DiscuitSharp.Core.Content;

namespace DiscuitSharp.Core.Group.Serialization
{
    public class PostJsonConverter : JsonConverter<Post>
    {
        public override Post Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Create a copy of the reader to use it for deserialization
            var jsonDocument = JsonDocument.ParseValue(ref reader);
            var jsonObject = jsonDocument.RootElement;

            // Determine the type based on the "type" property
            var type = jsonObject.GetProperty("type").GetString();
            if (type == null)
                throw new JsonException($"Unsupported or Missing post type: {type}");

            Post? post = type switch
            {
                "link" => JsonSerializer.Deserialize<LinkPost>(jsonObject.GetRawText(), options),
                "text" => JsonSerializer.Deserialize<TextPost>(jsonObject.GetRawText(), options),
                "image" => JsonSerializer.Deserialize<ImagePost>(jsonObject.GetRawText(), options),
                _ => throw new JsonException($"Unsupported post type: {type}")
            };
            if (post == null)
                throw new JsonException($"Unable to Deserialize: {type}");

            return post;
        }

        public override void Write(Utf8JsonWriter writer, Post value, JsonSerializerOptions options)
        {
            // Use the JsonSerializer to write the derived type
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}
