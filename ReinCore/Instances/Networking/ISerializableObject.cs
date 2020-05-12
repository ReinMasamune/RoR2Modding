namespace ReinCore
{
    using UnityEngine.Networking;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public interface ISerializableObject
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        void Serialize( NetworkWriter writer );
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        void Deserialize( NetworkReader reader );
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static class ISerializableObjectExtensions
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static void Write<TObject>( this NetworkWriter writer, TObject target ) where TObject : ISerializableObject => target.Serialize( writer );
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static TObject Read<TObject>( this NetworkReader reader, TObject destination ) where TObject : ISerializableObject
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            destination.Deserialize( reader );
            return destination;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static TObject ReadNew<TObject>( this NetworkReader reader ) where TObject : ISerializableObject, new()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            var obj = new TObject();
            obj.Deserialize( reader );
            return obj;
        }
    }

}
