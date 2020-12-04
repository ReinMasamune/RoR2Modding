namespace ReinCore
{
    using System;

    using UnityEngine;

    internal static class MeshInitializer
    {
        private static readonly Boolean completedProperly = false;
        internal static Boolean Initialize() => completedProperly;


        static MeshInitializer()
        {
            #region Unity Primatives
            new AssetAccessor<Mesh>( MeshIndex.Sphere, () =>
            {
                var obj = GameObject.CreatePrimitive( PrimitiveType.Sphere );
                Mesh mesh = obj.GetComponent<MeshFilter>().mesh;
                UnityEngine.Object.Destroy( obj );
                return mesh;
            } ).RegisterAccessor();

            new AssetAccessor<Mesh>( MeshIndex.Capsule, () =>
            {
                var obj = GameObject.CreatePrimitive( PrimitiveType.Capsule );
                Mesh mesh = obj.GetComponent<MeshFilter>().mesh;
                UnityEngine.Object.Destroy( obj );
                return mesh;
            } ).RegisterAccessor();

            new AssetAccessor<Mesh>( MeshIndex.Cylinder, () =>
            {
                var obj = GameObject.CreatePrimitive( PrimitiveType.Cylinder );
                Mesh mesh = obj.GetComponent<MeshFilter>().mesh;
                UnityEngine.Object.Destroy( obj );
                return mesh;
            } ).RegisterAccessor();

            new AssetAccessor<Mesh>( MeshIndex.Cube, () =>
            {
                var obj = GameObject.CreatePrimitive( PrimitiveType.Cube );
                Mesh mesh = obj.GetComponent<MeshFilter>().mesh;
                UnityEngine.Object.Destroy( obj );
                return mesh;
            } ).RegisterAccessor();

            new AssetAccessor<Mesh>( MeshIndex.Plane, () =>
            {
                var obj = GameObject.CreatePrimitive( PrimitiveType.Plane );
                Mesh mesh = obj.GetComponent<MeshFilter>().mesh;
                UnityEngine.Object.Destroy( obj );
                return mesh;
            } ).RegisterAccessor();

            new AssetAccessor<Mesh>( MeshIndex.Quad, () =>
            {
                var obj = GameObject.CreatePrimitive( PrimitiveType.Quad );
                Mesh mesh = obj.GetComponent<MeshFilter>().mesh;
                UnityEngine.Object.Destroy( obj );
                return mesh;
            } ).RegisterAccessor();
            #endregion
            #region Hopoo Meshes       
            new AssetAccessor<Mesh>( MeshIndex.Spiral1, () =>
            {
                Transform trans = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refWillOWispExplosion).transform;
                ParticleSystemRenderer rend = trans.Find( "Flames, Tube" ).GetComponent<ParticleSystemRenderer>();
                Int32 count = rend.meshCount;
                if( count > 1 )
                {
                    var meshes = new Mesh[count];
                    _ = rend.GetMeshes( meshes );
                    return meshes[0];
                }
                return rend.mesh;
            }, PrefabIndex.refWillOWispExplosion ).RegisterAccessor();

            new AssetAccessor<Mesh>( MeshIndex.TornadoMesh, () =>
            {
                Transform obj = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refFireTornadoGhost).transform.Find("TornadoSmokeMesh");
                return obj.GetComponent<ParticleSystemRenderer>().mesh;
            }, PrefabIndex.refFireTornadoGhost ).RegisterAccessor();

            new AssetAccessor<Mesh>( MeshIndex.TornadoMesh2, () =>
            {
                Transform obj = AssetLibrary<GameObject>.GetAsset(PrefabIndex.refFireTornadoGhost).transform.Find("TornadoMesh");
                return obj.GetComponent<ParticleSystemRenderer>().mesh;
            }, PrefabIndex.refFireTornadoGhost ).RegisterAccessor();

            new AssetAccessor<Mesh>( MeshIndex.MdlTriTip, () =>
            {
                MeshFilter obj = AssetLibrary<GameObject>.GetAsset( PrefabIndex.refPickupTriTip ).transform.Find( "mslTriTip" ).GetComponent<MeshFilter>();
                return obj.sharedMesh;
            }, PrefabIndex.refPickupTriTip ).RegisterAccessor();

            new AssetAccessor<Mesh>(MeshIndex.Donut2, 
                () => AssetLibrary<GameObject>.GetAsset(PrefabIndex.refMercSwordSlashWhirlwind).transform.Find("SwingTrail").GetComponent<ParticleSystemRenderer>().mesh,
                PrefabIndex.refMercSwordSlashWhirlwind).RegisterAccessor();

            #endregion

            completedProperly = true;
        }
    }
}
