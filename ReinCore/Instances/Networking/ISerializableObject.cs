namespace ReinCore
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.Serialization;

    using RoR2;

    using UnityEngine.Networking;

    public interface ISerializableObject
    {
        void Serialize( NetworkWriter writer );
        void Deserialize( NetworkReader reader );
    }

    public static class ISerializableObjectExtensions
    {
        public static NetworkWriter Write<TObject>(this NetworkWriter writer, TObject target) where TObject : ISerializableObject
        {
            target.Serialize(writer);
            return writer;
        }

        public static NetworkReader Read<TObject>(this NetworkReader reader, ref TObject destination)
            where TObject : ISerializableObject
        {
            destination.Deserialize(reader);
            return reader;
        }

        public static TObject Read<TObject>(this NetworkReader reader, TObject destination) where TObject : ISerializableObject
        {
            destination.Deserialize(reader);
            return destination;
        }

        public static TObject ReadNew<TObject>( this NetworkReader reader ) where TObject : ISerializableObject, new()
        {
            var obj = new TObject();
            obj.Deserialize( reader );
            return obj;
        }
    }

    public unsafe interface IByteSerializableObject
    {
        UInt32 size { get; }
        void Serialize(Byte* to);
        void Deserialize(Byte* from);
    }

    public static class IByteSerializableObjectExtensions
    {
        public static unsafe void Serialize<T>(this T obj, Byte[] to)
            where T : IByteSerializableObject
        {
            if(to.Length != obj.size) throw new ArgumentException("Bad array size");
            fixed(Byte* ptr = to)
            {
                //Log.Message(ptr[0]);
                obj.Serialize(ptr);

                //Log.Message("Sending bytes");
                //foreach(var b in to)
                //{
                //    Log.Message(b);
                //}
            }
        }
        public static unsafe T Deserialize<T>(this T obj, Byte[] from)
            where T : IByteSerializableObject
        {
            if(from.Length != obj.size) throw new ArgumentException("Bad array size");
            fixed(Byte* ptr = from)
            {
                //Log.Message(ptr[0]);
                obj.Deserialize(ptr);

                //Log.Message("Sending bytes");
                //foreach(var b in from)
                //{
                //    Log.Message(b);
                //}
            }
            return obj;
        }
    }


    public unsafe struct RWPtr
    {
        public RWPtr(void* ptr)
        {
            this.ptr = ptr;
        }
        internal void* ptr;
    }


    public static unsafe class PtrStreamExtensions
    {
        public static RWPtr WriteStruct<T>(this RWPtr str, T item)
            where T : unmanaged
        {
            var ptr = (T*)str.ptr;
            ptr[0] = item;
            ptr++;
            str.ptr = (void*)ptr;
            return str;
        }

        public static RWPtr ReadStruct<T>(this RWPtr str, out T item)
            where T : unmanaged
        {
            var ptr = (T*)str.ptr;
            item = ptr[0];
            ptr++;
            str.ptr = (void*)ptr;
            return str;
        }



        public static RWPtr WriteNetObj<T>(this RWPtr str, T netObj)
            where T : NetworkBehaviour
            => str.WriteStruct(netObj.netId);

        public static RWPtr ReadNetObj<T>(this RWPtr str, out T? netObj)
            where T : NetworkBehaviour
        {
            var res = str.ReadStruct(out NetworkInstanceId id);
            netObj = id.FindLocalObj<T>();
            return res;
        }

        public static RWPtr Write(this RWPtr str, HurtBox hurtbox) => str
            .WriteNetObj(hurtbox?.healthComponent?.body)
            .WriteStruct((Byte)(hurtbox.indexInGroup + 1));

        public static RWPtr Read(this RWPtr str, out HurtBox? hurtbox)
        {
            var res = str.ReadNetObj(out CharacterBody body).ReadStruct(out Byte by);
            var r = body?.hurtBoxGroup?.hurtBoxes;
            Int32 index = by;
            index--;
            hurtbox = r is not null && index < r.Length ? r[index] : null;
            return res;
        }
        private static TObj FindLocalObj<TObj>(this NetworkInstanceId id)
            where TObj : NetworkBehaviour
            => (NetworkServer.active ? NetworkServer.FindLocalObject(id) : ClientScene.FindLocalObject(id))?.GetComponent<TObj>();
    }

    
}
