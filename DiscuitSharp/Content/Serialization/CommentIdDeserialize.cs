using System.Text.Json.Serialization;
using System.Text.Json;
using DiscuitSharp.Core.Content;
using System.Security.Cryptography;

namespace DiscuitSharp.Core.Group.Serialization
{
    public class CommentIdConverter : JsonConverter<CommentId>
    {
        public override CommentId Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options) {
                var mid = reader.GetString();
                    return new CommentId(mid);
            }

        public override void Write(
            Utf8JsonWriter writer,
            CommentId commentIdValue,
            JsonSerializerOptions options) =>
                writer.WriteStringValue(commentIdValue.Value);
    }
}
