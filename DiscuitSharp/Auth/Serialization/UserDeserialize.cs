using System.Text.Json.Serialization;
using System.Text.Json;
using DiscuitSharp.Core.Content;
using DiscuitSharp.Core.Auth;
using DiscuitSharp.Core.Utils;

namespace DiscuitSharp.Core.Group.Serialization
{
    public class DiscuitUserJsonConverter : JsonConverter<DiscuitUser>
    {
        public override DiscuitUser Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            // Create a copy of the reader to use it for deserialization
            var jsonDocument = JsonDocument.ParseValue(ref reader);
            var jsonObject = jsonDocument.RootElement;
            var converter = new JsonSerializerOptions(options);
            converter.Converters.Add(new UserIdJsonConverter());
            converter.PropertyNamingPolicy = new LowercaseNamingPolicy();
            DiscuitUser? usr = JsonSerializer.Deserialize<DiscuitUser>(jsonObject.GetRawText(), converter);

            if (usr == null)
                throw new JsonException($"Unable to Deserialize: {typeToConvert}");

            return usr;
        }

        public override void Write(Utf8JsonWriter writer, DiscuitUser value, JsonSerializerOptions options)
        {
            // Use the JsonSerializer to write the derived type
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}
