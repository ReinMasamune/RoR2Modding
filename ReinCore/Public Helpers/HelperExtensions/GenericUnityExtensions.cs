namespace ReinCore
{
    using System;
    using System.Collections;

    using UnityEngine;

    public static class GenericUnityExtensions
    {
        public static TObject Instantiate<TObject>( this TObject obj ) where TObject : UnityEngine.Object => UnityEngine.Object.Instantiate( obj );

        public static void Destroy( this UnityEngine.Object self )
        {
            UnityEngine.Object.Destroy( self );
        }

        public static void DestroyImmediate( this UnityEngine.Object self )
        {
            UnityEngine.Object.DestroyImmediate( self );
        }

        public static IEnumerator DestroyOnTimer( this MonoBehaviour self, Single timer )
        {
            yield return new WaitForSeconds( timer );
            self.Destroy();
        }
    }
}
