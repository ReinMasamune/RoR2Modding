using R2API;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    internal static partial class EffectHelper
    {
        private static Dictionary<GameObject, UInt32> meshIndCounter = new Dictionary<GameObject, UInt32>();
        internal static Renderer AddMeshIndicator( GameObject mainObj, WispSkinnedEffect skin, MaterialType matType, MeshIndex mesh, Boolean useParticle = false )
        {
            if( !meshIndCounter.ContainsKey( mainObj ) ) meshIndCounter[mainObj] = 0u;
            var obj = new GameObject( "MeshIndicator" + meshIndCounter[mainObj]++ );
            obj.transform.parent = mainObj.transform;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            obj.transform.localRotation = Quaternion.identity;

            Renderer renderer = null;
            Mesh m = AssetLibrary<Mesh>.i[mesh];

            if( useParticle )
            {
                var ps = obj.AddComponent<ParticleSystem>();
                var psr = obj.AddOrGetComponent<ParticleSystemRenderer>();
                renderer = psr;
                psr.renderMode = ParticleSystemRenderMode.Mesh;
                psr.mesh = m;
            } else
            {
                var meshRend = obj.AddComponent<MeshRenderer>();
                var meshFilter = obj.AddOrGetComponent<MeshFilter>();
                renderer = meshRend;
                meshFilter.sharedMesh = m;
            }

            if( matType != MaterialType.Constant )
            {
                skin.AddRenderer( renderer, matType );
            }            
            return renderer;
        }
    }
}
