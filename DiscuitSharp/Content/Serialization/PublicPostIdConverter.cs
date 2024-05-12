using System.Text.Json.Serialization;
using System.Text.Json;
using DiscuitSharp.Core.Content;

namespace DiscuitSharp.Core.Group.Serialization
{
    public class PublicPostIdConverter : JsonConverter<PublicPostId>
    {
        public override PublicPostId Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options) => new PublicPostId(reader.GetString()!);

        public override void Write(
            Utf8JsonWriter writer,
            PublicPostId postIdValue,
            JsonSerializerOptions options) =>
                writer.WriteStringValue(postIdValue.Value);
    }
}
