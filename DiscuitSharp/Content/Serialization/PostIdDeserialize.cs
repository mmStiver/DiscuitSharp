using System.Text.Json.Serialization;
using System.Text.Json;
using DiscuitSharp.Core.Content;

namespace DiscuitSharp.Core.Group.Serialization
{
    public class PostIdJsonConverter : JsonConverter<PostId>
    {
        public override PostId Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options) => new PostId(reader.GetString()!);

        public override void Write(
            Utf8JsonWriter writer,
            PostId postIdValue,
            JsonSerializerOptions options) =>
                writer.WriteStringValue(postIdValue.Value);
    }
}
