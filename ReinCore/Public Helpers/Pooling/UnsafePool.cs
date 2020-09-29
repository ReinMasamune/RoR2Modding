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


    public static class Pool<T, TInitItem, TCleanItem>
        where TInitItem : struct, IInitItem<T>
        where TCleanItem : struct, ICleanItem<T>
    {
        public static T item
        {
            get => elems.TryDequeue(out var res) ? res : InitNewItem();
            set => elems.Enqueue(PipeCleaner(value));
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
}
