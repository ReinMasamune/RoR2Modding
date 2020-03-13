using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using ReinCore;

namespace RogueWispPlugin.Helpers
{
    internal static partial class EffectHelper
    {
        private static Dictionary<GameObject, UInt32> meshIndCounter = new Dictionary<GameObject, UInt32>();
        internal static Renderer AddMeshIndicator( GameObject mainObj, WispSkinnedEffect skin, MaterialType matType, MeshIndex mesh, Boolean useParticle = false, Boolean particleScaleToSize = false, Single timeToFullsize = 1f, Single duration = 10f, Boolean scaleX = true, Boolean scaleY = true, Boolean scaleZ = true )
        {
            if( !meshIndCounter.ContainsKey( mainObj ) ) meshIndCounter[mainObj] = 0u;
            var obj = new GameObject( "MeshIndicator" + meshIndCounter[mainObj]++ );
            obj.transform.parent = mainObj.transform;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            obj.transform.localRotation = Quaternion.identity;

            Renderer renderer = null;
            Mesh m = AssetsCore.LoadAsset<Mesh>(mesh);

            if( useParticle )
            {
                var ps = obj.AddComponent<ParticleSystem>();
                var psr = obj.AddOrGetComponent<ParticleSystemRenderer>();
                renderer = psr;
                psr.renderMode = ParticleSystemRenderMode.Mesh;
                psr.alignment = ParticleSystemRenderSpace.World;
                psr.mesh = m;

                BasicSetup( ps );

                ps.useAutoRandomSeed = true;

                var psMain = ps.main;
                psMain.duration = duration;
                psMain.loop = false;
                psMain.startDelay = 0f;
                psMain.startLifetime = duration;
                psMain.startSpeed = 0f;
                psMain.startSize3D = false;
                psMain.startSize = 1f;
                psMain.startRotation3D = false;
                psMain.startRotation = 0f;
                psMain.flipRotation = 0f;
                psMain.startColor = Color.white;
                psMain.gravityModifier = 0f;
                psMain.simulationSpace = ParticleSystemSimulationSpace.Local;
                psMain.useUnscaledTime = false;
                psMain.scalingMode = ParticleSystemScalingMode.Hierarchy;
                psMain.playOnAwake = true;
                psMain.emitterVelocityMode = ParticleSystemEmitterVelocityMode.Transform;
                psMain.maxParticles = 1;
                psMain.stopAction = ParticleSystemStopAction.None;
                psMain.cullingMode = ParticleSystemCullingMode.PauseAndCatchup;
                psMain.ringBufferMode = ParticleSystemRingBufferMode.Disabled;

                var psEmis = ps.emission;
                psEmis.enabled = true;
                psEmis.burstCount = 1;
                psEmis.SetBurst( 0, new ParticleSystem.Burst( 0f, 1 ) );
                psEmis.rateOverTime = 0f;
                psEmis.rateOverDistance = 0f;

                if( particleScaleToSize )
                {
                    var frac = timeToFullsize / duration;
                    var psSOL = ps.sizeOverLifetime;
                    psSOL.enabled = true;
                    if( scaleX == false && scaleY == false && scaleZ == false )
                    {
                        psSOL.separateAxes = false;
                        psSOL.size = new ParticleSystem.MinMaxCurve( 1f, AnimationCurve.EaseInOut( 0f, 0f, frac, 1f ) );
                    } else
                    {
                        psSOL.separateAxes = true;
                        psSOL.x = scaleX ? new ParticleSystem.MinMaxCurve( 1f, AnimationCurve.EaseInOut( 0f, 0f, frac, 1f ) ) : 1f;
                        psSOL.y = scaleX ? new ParticleSystem.MinMaxCurve( 1f, AnimationCurve.EaseInOut( 0f, 0f, frac, 1f ) ) : 1f;
                        psSOL.z = scaleX ? new ParticleSystem.MinMaxCurve( 1f, AnimationCurve.EaseInOut( 0f, 0f, frac, 1f ) ) : 1f;
                    }

                    
                }

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
