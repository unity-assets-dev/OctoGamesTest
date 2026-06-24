using UnityEngine;

public abstract class SerializableData<T> {

    private static string Key => typeof(T).Name;
    
    public static void Serialize(T data) {
        var source = data.Serialize(); // Serialize into the string (by extension for JSON.net);
        
#if UNITY_EDITOR
        PlayerPrefs.SetString(Key, source);
#else
        ES3.Save(source, Key);
#endif
    }

    public static T Deserialize(T defaults = default) {
        if (!PlayerPrefs.HasKey(Key))
            return defaults;

        var source = PlayerPrefs.GetString(Key);
        
        return source.IsNullOrEmpty() ? defaults : source.Deserialize<T>();
    }
}
