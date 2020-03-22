using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ReinCore
{
    internal static class MeshInitializer
    {
        private static Boolean completedProperly = false;
        internal static Boolean Initialize()
        {
            return completedProperly;
        }


        static MeshInitializer()
        {
            #region Unity Primatives
            new AssetAccessor<Mesh>( MeshIndex.Sphere, () =>
            {
                var obj = GameObject.CreatePrimitive( PrimitiveType.Sphere );
                var mesh = obj.GetComponent<MeshFilter>().mesh;
                UnityEngine.Object.Destroy( obj );
                return mesh;
            }).RegisterAccessor();

            new AssetAccessor<Mesh>( MeshIndex.Capsule, () =>
            {
                var obj = GameObject.CreatePrimitive( PrimitiveType.Capsule );
                var mesh = obj.GetComponent<MeshFilter>().mesh;
                UnityEngine.Object.Destroy( obj );
                return mesh;
            }).RegisterAccessor();

            new AssetAccessor<Mesh>( MeshIndex.Cylinder, () =>
            {
                var obj = GameObject.CreatePrimitive( PrimitiveType.Cylinder );
                var mesh = obj.GetComponent<MeshFilter>().mesh;
                UnityEngine.Object.Destroy( obj );
                return mesh;
            }).RegisterAccessor();

            new AssetAccessor<Mesh>( MeshIndex.Cube, () =>
            {
                var obj = GameObject.CreatePrimitive( PrimitiveType.Cube );
                var mesh = obj.GetComponent<MeshFilter>().mesh;
                UnityEngine.Object.Destroy( obj );
                return mesh;
            }).RegisterAccessor();

            new AssetAccessor<Mesh>( MeshIndex.Plane, () =>
            {
                var obj = GameObject.CreatePrimitive( PrimitiveType.Plane );
                var mesh = obj.GetComponent<MeshFilter>().mesh;
                UnityEngine.Object.Destroy( obj );
                return mesh;
            }).RegisterAccessor();

            new AssetAccessor<Mesh>( MeshIndex.Quad, () =>
            {
                var obj = GameObject.CreatePrimitive( PrimitiveType.Quad );
                var mesh = obj.GetComponent<MeshFilter>().mesh;
                UnityEngine.Object.Destroy( obj );
                return mesh;
            }).RegisterAccessor();
            #endregion
            #region Hopoo Meshes       
            new AssetAccessor<Mesh>( MeshIndex.Spiral1, () =>
            {
                var trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refWillOWispExplosion).transform;
                var rend = trans.Find( "Flames, Tube" ).GetComponent<ParticleSystemRenderer>();
                var count = rend.meshCount;
                if( count > 1 )
                {
                    var meshes = new Mesh[count];
                    rend.GetMeshes( meshes );
                    return meshes[0];
                }
                return rend.mesh;
            }, PrefabIndex.refWillOWispExplosion ).RegisterAccessor();

            new AssetAccessor<Mesh>( MeshIndex.TornadoMesh, () =>
            {
                var obj = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refFireTornadoGhost).transform.Find("TornadoSmokeMesh");
                return obj.GetComponent<ParticleSystemRenderer>().mesh;
            }, PrefabIndex.refFireTornadoGhost ).RegisterAccessor();

            new AssetAccessor<Mesh>( MeshIndex.TornadoMesh2, () =>
            {
                var obj = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refFireTornadoGhost).transform.Find("TornadoMesh");
                return obj.GetComponent<ParticleSystemRenderer>().mesh;
            }, PrefabIndex.refFireTornadoGhost ).RegisterAccessor();

            new AssetAccessor<Mesh>( MeshIndex.MdlTriTip, () =>
            {
                var obj = AssetLibrary<GameObject>.GetAsset( PrefabIndex.refPickupTriTip ).transform.Find( "mslTriTip" ).GetComponent<MeshFilter>();
                return obj.sharedMesh;
            }, PrefabIndex.refPickupTriTip ).RegisterAccessor();

            #endregion

            completedProperly = true;
        }
    }
}
