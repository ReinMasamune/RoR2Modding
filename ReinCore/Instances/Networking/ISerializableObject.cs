namespace ReinCore
{
    using UnityEngine.Networking;

    public interface ISerializableObject
    {
        void Serialize( NetworkWriter writer );
        void Deserialize( NetworkReader reader );
    }

    public static class ISerializableObjectExtensions
    {
        public static void Write<TObject>( this NetworkWriter writer, TObject target ) where TObject : ISerializableObject => target.Serialize( writer );

        public static TObject Read<TObject>( this NetworkReader reader, TObject destination ) where TObject : ISerializableObject
        {
            destination.Deserialize( reader );
            return destination;
        }

        public static TObject ReadNew<TObject>( this NetworkReader reader ) where TObject : ISerializableObject, new()
        {
            var obj = new TObject();
            obj.Deserialize( reader );
            return obj;
        }
    }

}
