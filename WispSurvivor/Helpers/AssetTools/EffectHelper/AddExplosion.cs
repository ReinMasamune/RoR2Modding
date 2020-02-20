using R2API;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    internal static partial class EffectHelper
    {
        private static Dictionary<GameObject, UInt32> explosionCounter = new Dictionary<GameObject, UInt32>();
        internal static ParticleSystem AddExplosion( GameObject mainObj, WispSkinnedEffect skin, MaterialType matType, Int32 count, Single lifetime, Single speed, Single size )
        {
            if( !explosionCounter.ContainsKey( mainObj ) ) explosionCounter[mainObj] = 0u;
            var obj = new GameObject( "Explosion" + explosionCounter[mainObj]++ );
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
            psMain.duration = lifetime * 2;
            psMain.loop = false;
            psMain.startDelay = 0f;
            psMain.startLifetime = new ParticleSystem.MinMaxCurve( lifetime * 0.75f, lifetime * 1.25f );
            psMain.startSpeed = new ParticleSystem.MinMaxCurve( speed * 0.75f, speed * 1.25f );
            psMain.startSize3D = false;
            psMain.startSize = new ParticleSystem.MinMaxCurve( size * 0.75f, size * 1.25f );
            psMain.startRotation3D = false;
            psMain.startRotation = new ParticleSystem.MinMaxCurve( 0f, 360f );
            psMain.flipRotation = 0f;
            psMain.startColor = Color.white;
            psMain.gravityModifier = 0f;
            psMain.simulationSpace = ParticleSystemSimulationSpace.Local;
            psMain.useUnscaledTime = false;
            psMain.scalingMode = ParticleSystemScalingMode.Hierarchy;
            psMain.playOnAwake = true;
            psMain.emitterVelocityMode = ParticleSystemEmitterVelocityMode.Transform;
            psMain.maxParticles = count * 2;
            psMain.stopAction = ParticleSystemStopAction.None;
            psMain.cullingMode = ParticleSystemCullingMode.PauseAndCatchup;
            psMain.ringBufferMode = ParticleSystemRingBufferMode.Disabled;

            var psEmis = ps.emission;
            psEmis.enabled = true;
            psEmis.burstCount = 1;
            psEmis.SetBurst( 0, new ParticleSystem.Burst( 0f, count ) );
            psEmis.rateOverTime = 0f;
            psEmis.rateOverDistance = 0f;

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

            var psCOL = ps.colorOverLifetime;
            psCOL.enabled = true;
            psCOL.color = new ParticleSystem.MinMaxGradient(new Gradient
            {
                mode = GradientMode.Blend,
                alphaKeys = new[]
                {
                    new GradientAlphaKey( 1f, 0f ),
                    new GradientAlphaKey( 0f, 1f ),
                },
                colorKeys = new[]
                {
                    new GradientColorKey( Color.white, 0f ),
                    new GradientColorKey( Color.white, 1f ),
                },
            } );

            var psSOL = ps.sizeOverLifetime;
            psSOL.enabled = true;
            psSOL.size = new ParticleSystem.MinMaxCurve( 1f, AnimationCurve.Linear( 0f, 0.75f, 1f, 1f ) );


            psr.renderMode = ParticleSystemRenderMode.Billboard;
            psr.normalDirection = 1f;
            psr.sortMode = ParticleSystemSortMode.None;
            psr.minParticleSize = 0f;
            psr.maxParticleSize = 1.04f;
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
