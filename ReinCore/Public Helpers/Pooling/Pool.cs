namespace ReinCore
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using System.Text;

    using BepInEx;
    using BepInEx.Bootstrap;


    using UnityEngine;

    public static class Pool<T, TInitItem, TCleanItem>
        where TInitItem : struct, IInitItem<T>
        where TCleanItem : struct, ICleanItem<T>
    {
        public static T item
        {
            get => elems.TryDequeue(out var res) ? res : InitNewItem();
            set
            {
                if(value is null) return;
                elems.Enqueue(PipeCleaner(value));
            }
        }

        private static T InitNewItem() => new TInitItem().InitItem();

        //hehe puns
        private static T PipeCleaner(T item)
        {
            new TCleanItem().CleanItem(item);
            return item;
        }

        private static readonly ConcurrentQueue<T> elems = new();
    }

    public interface IInitItem<T>
    {
        T InitItem();
    }
    public interface ICleanItem<T>
    {
        void CleanItem(T item);
    }

    public struct SimpleInitItem<T> : IInitItem<T>
        where T : new()
    {
        public T InitItem() => new();
    }

    public struct SimpleCleanItem<T> : ICleanItem<T>
    {
        public static event Action<T> onClean;

        public void CleanItem(T item) => onClean?.Invoke(item);
    }

    public struct CollectionCleanItem<TCollection, TItem> : ICleanItem<TCollection>
        where TCollection : ICollection<TItem>
    {
        public void CleanItem(TCollection item) => item.Clear();
    }

    public struct QueueCleanItem<TQueue, TItem> : ICleanItem<TQueue>
        where TQueue : Queue<TItem>
    {
        public void CleanItem(TQueue item) => item.Clear();
    }

    public struct StackCleanItem<TStack, TItem> : ICleanItem<TStack>
        where TStack : Stack<TItem>
    {
        public void CleanItem(TStack item) => item.Clear();
    }

    public static class SimplePool<T>
        where T : new()
    {
        public static T item
        {
            get => Pool<T, SimpleInitItem<T>, SimpleCleanItem<T>>.item;
            set => Pool<T, SimpleInitItem<T>, SimpleCleanItem<T>>.item = value;
        }
    }

    public static class CustomInitPool<T, TInit>
        where TInit : struct, IInitItem<T>
    {
        public static T item
        {
            get => Pool<T, TInit, SimpleCleanItem<T>>.item;
            set => Pool<T, TInit, SimpleCleanItem<T>>.item = value;
        }
    }

    public static class CustomCleanPool<T, TClean>
        where T : new()
        where TClean : struct, ICleanItem<T>
    {
        public static T item
        {
            get => Pool<T, SimpleInitItem<T>, TClean>.item;
            set => Pool<T, SimpleInitItem<T>, TClean>.item = value;
        }
    }

    public static class CollectionPool<TItem, TCollection>
        where TCollection : ICollection<TItem>, new()
    {
        public static TCollection item
        {
            get => CustomCleanPool<TCollection, CollectionCleanItem<TCollection, TItem>>.item;
            set => CustomCleanPool<TCollection, CollectionCleanItem<TCollection, TItem>>.item = value;
        }
    }

    public static class ListPool<T>
    {
        public static List<T> item
        {
            get => CollectionPool<T, List<T>>.item;
            set => CollectionPool<T, List<T>>.item = value;
        }
    }
    public static class QueuePool<T>
    {
        public static Queue<T> item
        {
            get => CustomCleanPool<Queue<T>, QueueCleanItem<Queue<T>, T>>.item;
            set => CustomCleanPool<Queue<T>, QueueCleanItem<Queue<T>, T>>.item = value;
        }
    }
    public static class StackPool<T>
    {
        public static Stack<T> item
        {
            get => CustomCleanPool<Stack<T>, StackCleanItem<Stack<T>, T>>.item;
            set => CustomCleanPool<Stack<T>, StackCleanItem<Stack<T>, T>>.item = value;
        }
    }

    public static class HashSetPool<T>
    {
        public static HashSet<T> item
        {
            get => CollectionPool<T, HashSet<T>>.item;
            set => CollectionPool<T, HashSet<T>>.item = value;
        }
    }

    public static class DictionaryPool<TKey, TValue>
    {
        public static Dictionary<TKey, TValue> item
        {
            get => CollectionPool<KeyValuePair<TKey, TValue>, Dictionary<TKey, TValue>>.item;
            set => CollectionPool<KeyValuePair<TKey, TValue>, Dictionary<TKey, TValue>>.item = value;
        }
    }
}
