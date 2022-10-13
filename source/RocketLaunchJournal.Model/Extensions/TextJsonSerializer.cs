using System;
using System.Text.Json;

public static class TextJsonSerializer
{
    public static string SerializeJson<T>(this T toSerialize, JsonSerializerOptions? options = null)
    {
        options = options ?? defaultOptions;
        return JsonSerializer.Serialize<T>(toSerialize, options);
    }

    public static T? DeserializeJson<T>(this string json, JsonSerializerOptions? options = null)
    {
        if (json is null)
            return default;

        options = options ?? defaultOptions;
        return JsonSerializer.Deserialize<T>(json, options);
    }

    public static object? DeserializeJson(this string json, Type type, JsonSerializerOptions? options = null)
    {
        if (json is null)
            return default;

        options = options ?? defaultOptions;
        return JsonSerializer.Deserialize(json, type, options);
    }

    private static JsonSerializerOptions defaultOptions => new JsonSerializerOptions() 
    {
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    };

    public class DBNullConverter : System.Text.Json.Serialization.JsonConverter<DBNull>
    {
        public static DBNullConverter Instance => new DBNullConverter();

        public override DBNull Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Implement custom logic.
            // Debug.Assert(typeToConvert == typeof(DBNull));

            // if (reader.TokenType != JsonTokenType.Null)
            // {
            //     throw new JsonException();
            // }
            // return DBNull.Value;
            throw new NotSupportedException();
        }

        public override void Write(Utf8JsonWriter writer, DBNull value, JsonSerializerOptions options)
        {
            writer.WriteNullValue();
        }
    }
}
