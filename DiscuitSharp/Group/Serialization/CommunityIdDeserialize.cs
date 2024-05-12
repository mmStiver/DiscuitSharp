using System.Text.Json.Serialization;
using System.Text.Json;

namespace DiscuitSharp.Core.Group.Serialization
{
    public class CommunityIdJsonConverter : JsonConverter<CommunityId>
    {
        public override CommunityId Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options) => new CommunityId(reader.GetString()!);

        public override void Write(
            Utf8JsonWriter writer,
            CommunityId dateTimeValue,
            JsonSerializerOptions options) =>
                writer.WriteStringValue(dateTimeValue.Id);
    }
}
