namespace ReinCore
{
    using System;

    using UnityEngine;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static class ComponentExtensions
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static Boolean HasComponent<T>( this GameObject g ) where T : Component => g.GetComponent( typeof( T ) ) != null;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static Boolean HasComponent<T>( this MonoBehaviour m ) where T : Component => m.GetComponent( typeof( T ) ) != null;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static Boolean HasComponent<T>( this Transform t ) where T : Component => t.GetComponent( typeof( T ) ) != null;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member




#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static Int32 ComponentCount<T>( this GameObject g ) where T : Component => g.GetComponents( typeof( T ) ).Length;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static Int32 ComponentCount<T>( this MonoBehaviour m ) where T : Component => m.GetComponents( typeof( T ) ).Length;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static Int32 ComponentCount<T>( this Transform t ) where T : Component => t.GetComponents( typeof( T ) ).Length;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member




#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static T GetComponent<T>( this GameObject g, Int32 index ) where T : Component => g.GetComponents( typeof( T ) )[index] as T;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static T GetComponent<T>( this MonoBehaviour m, Int32 index ) where T : Component => m.gameObject.GetComponents( typeof( T ) )[index] as T;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static T GetComponent<T>( this Transform t, Int32 index ) where T : Component => t.GetComponents( typeof( T ) )[index] as T;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member




#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static T AddComponent<T>( this MonoBehaviour m ) where T : Component => m.gameObject.AddComponent( typeof( T ) ) as T;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static T AddComponent<T>( this Transform t ) where T : Component => t.gameObject.AddComponent( typeof( T ) ) as T;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member




#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static T AddOrGetComponent<T>( this GameObject g ) where T : Component => ( g.HasComponent<T>() ? g.GetComponent( typeof( T ) ) : g.AddComponent( typeof( T ) ) ) as T;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static T AddOrGetComponent<T>( this MonoBehaviour m ) where T : Component => ( m.HasComponent<T>() ? m.GetComponent( typeof( T ) ) : m.gameObject.AddComponent( typeof( T ) ) ) as T;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static T AddOrGetComponent<T>( this Transform t ) where T : Component => ( t.HasComponent<T>() ? t.GetComponent( typeof( T ) ) : t.gameObject.AddComponent( typeof( T ) ) ) as T;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
