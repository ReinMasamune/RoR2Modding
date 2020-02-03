
using RogueWispPlugin.Helpers;
using UnityEngine;

namespace RogueWispPlugin
{
    internal partial class Main
    {
        partial void CreateMeshAccessors()
        {
            #region Unity Primatives
            new GenericAccessor<Mesh>( MeshIndex.Sphere, () =>
            {
                var obj = GameObject.CreatePrimitive( PrimitiveType.Sphere );
                var mesh = obj.GetComponent<MeshFilter>().mesh;
                Destroy( obj );
                return mesh;
            }, false, ExecutionState.Constructor ).RegisterAccessor();

            new GenericAccessor<Mesh>( MeshIndex.Capsule, () =>
            {
                var obj = GameObject.CreatePrimitive( PrimitiveType.Capsule );
                var mesh = obj.GetComponent<MeshFilter>().mesh;
                Destroy( obj );
                return mesh;
            }, false, ExecutionState.Constructor ).RegisterAccessor();

            new GenericAccessor<Mesh>( MeshIndex.Cylinder, () =>
            {
                var obj = GameObject.CreatePrimitive( PrimitiveType.Cylinder );
                var mesh = obj.GetComponent<MeshFilter>().mesh;
                Destroy( obj );
                return mesh;
            }, false, ExecutionState.Constructor ).RegisterAccessor();

            new GenericAccessor<Mesh>( MeshIndex.Cube, () =>
            {
                var obj = GameObject.CreatePrimitive( PrimitiveType.Cube );
                var mesh = obj.GetComponent<MeshFilter>().mesh;
                Destroy( obj );
                return mesh;
            }, false, ExecutionState.Constructor ).RegisterAccessor();

            new GenericAccessor<Mesh>( MeshIndex.Plane, () =>
            {
                var obj = GameObject.CreatePrimitive( PrimitiveType.Plane );
                var mesh = obj.GetComponent<MeshFilter>().mesh;
                Destroy( obj );
                return mesh;
            }, false, ExecutionState.Constructor ).RegisterAccessor();

            new GenericAccessor<Mesh>( MeshIndex.Quad, () =>
            {
                var obj = GameObject.CreatePrimitive( PrimitiveType.Quad );
                var mesh = obj.GetComponent<MeshFilter>().mesh;
                Destroy( obj );
                return mesh;
            }, false, ExecutionState.Constructor ).RegisterAccessor();
            #endregion
            #region Hopoo Meshes       
            new GenericAccessor<Mesh>( MeshIndex.Spiral1,                       () =>
            {
                var trans = AssetLibrary<GameObject>.i[PrefabIndex.refWillOWispExplosion].transform;
                var rend = trans.Find( "Flames, Tube" ).GetComponent<ParticleSystemRenderer>();
                var count = rend.meshCount;
                if( count > 1 )
                {
                    var meshes = new Mesh[count];
                    rend.GetMeshes( meshes );
                    return meshes[0];
                }
                return rend.mesh;
            }, false, PrefabIndex.refWillOWispExplosion ).RegisterAccessor();
            #endregion
        }
    }
}
