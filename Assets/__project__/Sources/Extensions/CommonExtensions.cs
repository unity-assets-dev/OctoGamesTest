using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CommonExtensions {
    private static System.Random _rnd = new();

    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, int count = 1) {
        var array = source.ToArray();
        for (var i = 0; i < count; i++) {
            var n = array.Length;
            while (n > 1) 
            {
                var k = _rnd.Next(n--);
                // Swap elements using a tuple
                (array[n], array[k]) = (array[k], array[n]);
            }    
        }

        return array;
    }

    public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> onItemSelect) {
        var array = source.ToArray();
        foreach (var item in array) {
            onItemSelect?.Invoke(item);
        }
        
        return array;
    }

    public static IEnumerable<T> ForEachNonAlloc<T>(this IEnumerable<T> source, Action<T> onItemSelect) {
        foreach (var item in source) {
            onItemSelect?.Invoke(item);
        }
        
        return source;
    }
    
}
