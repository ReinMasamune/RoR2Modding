namespace ReinCore
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Text;

    using UnityEngine;

    public static class ArrayPool<T, TSizeDef>
        where TSizeDef : struct, IArraySizeDef
    {
        private static readonly UInt32 size = new TSizeDef().size;
        public static T[] item
        {
            get => Pool<T[], Init, Clean>.item;
            set => Pool<T[], Init, Clean>.item = value;
        }

        private struct Init : IInitItem<T[]>
        {
            public T[] InitItem() => new T[size];
        }
        private struct Clean : ICleanItem<T[]>
        {
            public void CleanItem(T[] item)
            {
                for(UInt32 i = 0; i < size; ++i)
                {
                    item[i] = default;
                }
            }
        }
    }
    public interface IArraySizeDef
    {
        UInt32 size { get; }
    }
}
