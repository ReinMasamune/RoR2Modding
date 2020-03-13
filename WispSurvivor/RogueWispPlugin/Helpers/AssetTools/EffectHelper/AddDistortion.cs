using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    internal static partial class EffectHelper
    {
        private static Dictionary<GameObject, UInt32> distortionCounter = new Dictionary<GameObject, UInt32>();
        internal static ParticleSystem AddDistortion( GameObject mainObj, WispSkinnedEffect skin, MaterialType matType, Single radius, Single duration, Single spinSpeed )
        { 
            if( !distortionCounter.ContainsKey( mainObj ) ) distortionCounter[mainObj] = 0u;
            var obj = new GameObject( "Distortion" + distortionCounter[mainObj]++ );
            obj.transform.parent = mainObj.transform;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            obj.transform.localRotation = Quaternion.identity;

            var ps = obj.AddComponent<ParticleSystem>();
            var psr = obj.AddOrGetComponent<ParticleSystemRenderer>();
            if( matType != MaterialType.Constant )
            {
                skin.AddRenderer( psr, matType );
            }
            

            BasicSetup( ps );

            ps.useAutoRandomSeed = true;

            var psMain = ps.main;
            psMain.duration = duration * 2f;
            psMain.loop = false;
            psMain.startDelay = 0f;
            psMain.startLifetime = new ParticleSystem.MinMaxCurve( duration * 0.75f, duration * 1.25f );
            psMain.startSpeed = 0f;
            psMain.startSize3D = false;
            psMain.startSize = new ParticleSystem.MinMaxCurve( radius * 0.75f, radius * 1.25f );
            psMain.startRotation3D = false;
            psMain.startRotation = 0f;
            psMain.flipRotation = 0f;
            psMain.startColor = Color.white;
            psMain.gravityModifier = 0f;
            psMain.simulationSpace = ParticleSystemSimulationSpace.World;
            psMain.useUnscaledTime = false;
            psMain.scalingMode = ParticleSystemScalingMode.Hierarchy;
            psMain.playOnAwake = true;
            psMain.emitterVelocityMode = ParticleSystemEmitterVelocityMode.Transform;
            psMain.maxParticles = 100000;
            psMain.stopAction = ParticleSystemStopAction.None;
            psMain.cullingMode = ParticleSystemCullingMode.AlwaysSimulate;
            psMain.ringBufferMode = ParticleSystemRingBufferMode.Disabled;

            var psEmis = ps.emission;
            psEmis.enabled = false;
            psEmis.burstCount = 0;
            psEmis.rateOverDistance = 0f;
            psEmis.rateOverTime = 0f;


            var psShape = ps.shape;
            psShape.enabled = false;
            psShape.arcMode = ParticleSystemShapeMultiModeValue.Random;
            psShape.arcSpread = 0f;
            psShape.texture = null;
            psShape.position = Vector3.zero;
            psShape.rotation = Vector3.zero;
            psShape.scale = Vector3.one;
            psShape.randomDirectionAmount = 0f;
            psShape.randomPositionAmount = 0f;
            psShape.sphericalDirectionAmount = 0f;
            //psShape.


            var psSOL = ps.sizeOverLifetime;
            psSOL.enabled = true;
            psSOL.size = new ParticleSystem.MinMaxCurve( radius, AnimationCurve.EaseInOut( 0f, 1f, 1f, 0f ) );

            var psROL = ps.rotationOverLifetime;
            psROL.enabled = true;
            psROL.separateAxes = true;
            psROL.x = 0f;
            psROL.y = 0f;
            psROL.z = spinSpeed;
           
            

            psr.renderMode = ParticleSystemRenderMode.Billboard;
            psr.normalDirection = 1f;
            psr.sortMode = ParticleSystemSortMode.None;
            psr.minParticleSize = 0f;
            psr.maxParticleSize = radius * 2f;
            psr.alignment = ParticleSystemRenderSpace.View;
            psr.flip = Vector3.zero;
            psr.allowRoll = true;
            psr.pivot = Vector3.zero;
            psr.maskInteraction = SpriteMaskInteraction.None;
            //psr.SetActiveVertexStreams( null );
            psr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            psr.receiveShadows = true;
            psr.shadowBias = 0f;
            psr.motionVectorGenerationMode = MotionVectorGenerationMode.Object;
            psr.sortingLayerID = default;
            psr.sortingOrder = 0;
            psr.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.BlendProbes;
            psr.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Simple;
            psr.probeAnchor = null;

            return ps;
        }
    }
}
