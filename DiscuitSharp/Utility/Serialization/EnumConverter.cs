using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using DiscuitSharp.Core.Content;

namespace DiscuitSharp.Core.Utility.Serialization
{
    public class EnumConverter<T> : JsonConverter<T> where T : Enum
    {
        public override bool CanConvert(Type typeToConvert) => typeof(T).IsAssignableFrom(typeToConvert);

        public override T Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options) {
            var str = reader.GetString()!;
            return Enum.GetValues(typeof(T)).Cast<T>().Single(v => v.Description() == str);
        }

        public override void Write(
            Utf8JsonWriter writer,
            T enumValue,
            JsonSerializerOptions options) =>
                writer.WriteStringValue(enumValue.Description());

    }
    public class NullableEnumConverter<T> : JsonConverter<T?> where T : Enum
    {
        public override bool CanConvert(Type typeToConvert) => typeof(T).IsAssignableFrom(typeToConvert);

        public override T? Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            var str = reader.GetString()!;
            if(str == "null") 
                return default;
            return Enum.GetValues(typeof(T)).Cast<T>().Single(v => v.Description() == str);
        }

        public override void Write(
            Utf8JsonWriter writer,
            T? enumValue,
            JsonSerializerOptions options) =>
                writer.WriteStringValue(enumValue?.Description() ?? default);

    }
}
