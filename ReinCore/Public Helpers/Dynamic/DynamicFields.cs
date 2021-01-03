namespace ReinCore
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public struct DynamicFieldHandle<TOn, TField>
        where TOn : class
        where TField : class, new()
    { }

    public abstract class DynamicField<TOn, TSelf>
        where TSelf : DynamicField<TOn, TSelf>, new()
        where TOn : class
    {
        public static DynamicFieldHandle<TOn, TSelf> handle => new();
    }

    public static class DynamicFields
    {
        internal static class Storage<TOn, TField>
            where TOn : class
            where TField : DynamicField<TOn, TField>, new()
        {
            internal static readonly ConditionalWeakTable<TOn, TField> store = new();
        }

        public static Boolean TryGetDField<TOn, TField>(this TOn obj, out TField data)
            where TOn : class
            where TField : DynamicField<TOn, TField>, new()
            => Storage<TOn, TField>.store.TryGetValue(obj, out data);

        public static Boolean RemoveDField<TOn, TField>(this TOn obj, DynamicFieldHandle<TOn, TField> handle)
            where TOn : class
            where TField : DynamicField<TOn, TField>, new()
            => Storage<TOn, TField>.store.Remove(obj);

        public static TField GetOrCreateDField<TOn, TField>(this TOn obj, DynamicFieldHandle<TOn, TField> handle)
            where TOn : class
            where TField : DynamicField<TOn, TField>, new()
        {
            if(obj.TryGetDField(out TField f)) return f;
            var n = new TField();
            Storage<TOn, TField>.store.Add(obj, n);
            return n;
        }
    }
}
