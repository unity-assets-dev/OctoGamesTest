using System;
using Newtonsoft.Json;
using UnityEngine;

public static class KitExtensions {
    
    public static T Deserialize<T>(this string source) => JsonConvert.DeserializeObject<T>(source);

    public static string Serialize<T>(this T source) => JsonConvert.SerializeObject(source);

    public static bool TryDeserialize<T>(this string json, out T result) {
        if (string.IsNullOrWhiteSpace(json)) {
            result = default;
            return false;
        }

        try {
            var hasError = false;
            result = JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings {
                Error = (sender, args) =>  {
                    Debug.LogWarning($"[JsonError] {args.ErrorContext.Error.Message}");
                    hasError = true;
                    args.ErrorContext.Handled = true; // Подавляем исключение
                },
                MissingMemberHandling = MissingMemberHandling.Ignore
            });

            return !hasError && result != null;
        }
        catch (Exception ex) {
            Debug.LogError($"[JsonFatal] {ex.Message}");
            result = default;
            return false;
        }
    }
    
    
    
    public static bool IsNullOrEmpty(this string value) => string.IsNullOrEmpty(value);
}