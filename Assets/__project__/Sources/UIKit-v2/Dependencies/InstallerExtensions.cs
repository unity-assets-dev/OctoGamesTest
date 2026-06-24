using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using Object = System.Object;

public static class InstallerExtensions {

    public static IEnumerable<T> EachNonAlloc<T>(this IEnumerable<T> source, Action<T> onItemSelect) {
        if (source == null || source.Count() == 0) return source;
        
        foreach (var item in source) {
            onItemSelect?.Invoke(item);
        }
        
        return source;
    }
    
    public static IEnumerable<T> Each<T>(this IEnumerable<T> source, Action<T> onItemSelect) {
        if (source == null || source.Count() == 0) return source;
        
        var buffer = source.ToArray();
        
        foreach (var item in buffer) {
            onItemSelect?.Invoke(item);
        }
        
        return buffer;
    }
    
    public static IEnumerable<T> For<T>(this IEnumerable<T> source, Action<T, int> onItemSelect) {
        if (source == null || source.Count() == 0) return source;
        
        var buffer = source.ToArray();

        for (var index = 0; index < buffer.Length; index++) {
            onItemSelect?.Invoke(buffer[index], index);
        }

        return buffer;
    }

    public static bool TryGetItem<TSource, TItem>(this IEnumerable<TSource> source, out TItem item) where TItem : TSource {
        item = source.OfType<TItem>().FirstOrDefault();

        return item != null;
    }
    
    public static Type[] GetAssembliesTypes(this AppDomain domain) {
        return domain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => t.IsClass && !t.IsAbstract)
            .ToArray();
    }
    
    public static void OnType<TType>(this IEnumerable<Type> source, Action<Type> onItem) {
        var interfaceType = typeof(TType);
        
        source
            .Where(type => !type.IsAbstract && type.IsClass && interfaceType.IsAssignableFrom(type))
            .Each(t => onItem?.Invoke(t));
    }

    public static void BindAsSingle<TType, TService>(this DiContainer container) where TType: class where TService: class, TType {
        container
            .Bind<TType>()
            .To<TService>()
            .AsSingle();
    }
    
    public static void BindAsSingle<TType>(this DiContainer container) where TType : class  {
        container
            .BindInterfacesAndSelfTo<TType>()
            .AsSingle();
    }

    public static void BindAsSingleFromInstanceMono<TType>(this DiContainer container, bool nonLazy = false) where TType : MonoBehaviour {
        container.BindAsSingleFromInstance(UnityEngine.Object.FindAnyObjectByType<TType>(FindObjectsInactive.Include), nonLazy);
    }
    
    public static void BindAsSingleFromInstance<TType>(this DiContainer container, TType instance, bool nonLazy = false) {
        container.BindAsSingleFromInstanceType(instance, nonLazy);
    }
    
    public static void BindAsSingleFromInstanceType(this DiContainer container, object instance, bool nonLazy = false) {
        var result = container
            .BindInterfacesAndSelfTo(instance.GetType())
            .FromInstance(instance);
        
        if (nonLazy) {
            result.NonLazy();
        }
    }
    
    public static bool TryGetAttribute<TAttribute>(this Type type, out TAttribute attribute)
        where TAttribute : Attribute {
        attribute = type.GetCustomAttributes(false).OfType<TAttribute>().FirstOrDefault();

        return attribute != null;
    }
    
}