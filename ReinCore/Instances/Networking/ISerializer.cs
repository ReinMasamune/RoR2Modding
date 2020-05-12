namespace ReinCore
{
    using System;
    using System.Collections.Generic;
    using BepInEx;
    using Mono.Cecil;
    using RoR2;
    using RoR2.Networking;
    using UnityEngine.Networking;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public interface ISerializer<TObject>
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        void Serialize( NetworkWriter writer, TObject target );
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        void Deserialize( NetworkReader reader, TObject target );
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static class ISerializerExtensions
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static void Write<TObject>( this NetworkWriter writer, TObject target, ISerializer<TObject> serializer ) => serializer.Serialize( writer, target );
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static TObject Read<TObject>( this NetworkReader reader, ref TObject destination, ISerializer<TObject> serializer )
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            serializer.Deserialize( reader, destination );
            return destination;
        }
    }
}

