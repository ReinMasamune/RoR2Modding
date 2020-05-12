namespace ReinCore
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static class GenericUnityExtensions
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static TObject Instantiate<TObject>( this TObject obj ) where TObject : UnityEngine.Object => UnityEngine.Object.Instantiate( obj );
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
