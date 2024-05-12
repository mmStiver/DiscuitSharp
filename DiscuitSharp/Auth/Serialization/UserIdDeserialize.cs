using System.Text.Json.Serialization;
using System.Text.Json;
using DiscuitSharp.Core.Content;
using DiscuitSharp.Core.Auth;

namespace DiscuitSharp.Core.Group.Serialization
{
    public class UserIdJsonConverter : JsonConverter<UserId>
    {
        public override UserId Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options) => new UserId(reader.GetString()!);

        public override void Write(
            Utf8JsonWriter writer,
            UserId userIdValue,
            JsonSerializerOptions options) =>
                writer.WriteStringValue(userIdValue.Value);
    }
}
