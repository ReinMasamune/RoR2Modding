namespace ReinCore
{
    using UnityEngine.Networking;

    public interface ISerializer<TObject>
    {
        void Serialize( NetworkWriter writer, TObject target );
        void Deserialize( NetworkReader reader, TObject target );
    }

    public static class ISerializerExtensions
    {
        public static void Write<TObject>( this NetworkWriter writer, TObject target, ISerializer<TObject> serializer ) => serializer.Serialize( writer, target );

        public static TObject Read<TObject>( this NetworkReader reader, ref TObject destination, ISerializer<TObject> serializer )
        {
            serializer.Deserialize( reader, destination );
            return destination;
        }
    }
}

