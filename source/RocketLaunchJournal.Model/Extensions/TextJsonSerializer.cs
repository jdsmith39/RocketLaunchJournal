﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

public static class TextJsonSerializer
{
    public static string SerializeJson<T>(this T toSerialize, JsonSerializerOptions options = null)
    {
        options = options ?? defaultOptions;
        return JsonSerializer.Serialize<T>(toSerialize, options);
    }

    public static T DeserializeJson<T>(this string json, JsonSerializerOptions options = null)
    {
        if (json is null)
            return default;

        options = options ?? defaultOptions;
        return JsonSerializer.Deserialize<T>(json, options);
    }

    public static object DeserializeJson(this string json, Type type, JsonSerializerOptions options = null)
    {
        if (json is null)
            return default;

        options = options ?? defaultOptions;
        return JsonSerializer.Deserialize(json, type, options);
    }

    private static JsonSerializerOptions defaultOptions => new JsonSerializerOptions() 
    {
        IgnoreNullValues = true
    };
}
