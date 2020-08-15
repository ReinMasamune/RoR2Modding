//using System;
//using System.Collections.Generic;
//using System.Reflection;
//using BF = System.Reflection.BindingFlags;

//public abstract class PluginMetadataExtension : System.Attribute { }

//internal static class PluginMetadata
//{
//    internal static event Action onAllMetadataRetreived;

//    internal static void AddMetadata(PluginMetadataExtension data)
//    {
//        if(data is null) return;
//        var type = data.GetType();
//        if(!delegateCache.TryGetValue(type, out var del))
//        {
//            delegateCache[type] = del = (AddMetadataDelegate)AddMetadataMethod.MakeGenericMethod(type).CreateDelegate(typeof(AddMetadataDelegate));
//        }
//        del(data);
//    }

//    private static MethodInfo AddMetadataMethod = typeof(PluginMetadata).GetMethod("AddMetadata", BF.NonPublic | BF.Static);
//    private static void AddMetatada<T>(PluginMetadataExtension data)
//        where T : PluginMetadataExtension
//    {
//        if(data is not T tData) return;
//        PluginMetadataAccess<T>.discoveredMetadata.Add(tData);
//    }

//    private delegate void AddMetadataDelegate(PluginMetadataExtension data);
//    private static readonly Dictionary<Type,AddMetadataDelegate> delegateCache = new Dictionary<Type, AddMetadataDelegate>();
//}

//public static class PluginMetadataAccess<T>
//    where T : PluginMetadataExtension
//{
//    public static event Action<IEnumerable<T>> onMetadataLoaded;

//    static PluginMetadataAccess()
//    {
//        PluginMetadata.onAllMetadataRetreived += FireTypeSpecificEvent;
//    }

//    private static void FireTypeSpecificEvent()
//    {
//        onMetadataLoaded?.Invoke(discoveredMetadata);
//    }

//    internal static readonly List<T> discoveredMetadata = new List<T>();
//}