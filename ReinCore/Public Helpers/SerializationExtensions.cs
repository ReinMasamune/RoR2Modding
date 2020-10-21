namespace ReinCore
{
    using System;
    using System.ComponentModel;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    using BepInEx;

    using UnityEngine;
    using UnityEngine.Networking;

    /// <summary>
    /// Extensions for Serialization interfaces
    /// </summary>
    public static class SerializationExtensions
    {
        public unsafe static void WriteBits<T>(this NetworkWriter writer, T value)
            where T : unmanaged
        {
            var ptr = (Byte*)&value;
            writer.WriteBytesFromPtr(ptr, sizeof(T));
        }

        public unsafe static void ReadBits<T>(this NetworkReader reader, ref T target)
            where T : unmanaged
        {
            var temp = target;
            var ptr = (Byte*)&temp;
            reader.ReadBytesToPtr(ptr, sizeof(T));
            target = temp;
        }

        public unsafe static void WriteBytesFromPtr(this NetworkWriter writer, Byte* ptr, Int32 count)
        {
            while(count --> 0) writer.Write(ptr[count]);
        }

        public unsafe static void ReadBytesToPtr(this NetworkReader reader, Byte* ptr, Int32 count)
        {
            while(count --> 0) ptr[count] = reader.ReadByte();
        }

    }
}
