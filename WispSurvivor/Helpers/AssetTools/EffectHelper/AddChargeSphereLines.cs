using R2API;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    internal static partial class EffectHelper
    {
        private static Dictionary<GameObject, UInt32> chargeSphereLinesCounter = new Dictionary<GameObject, UInt32>();
        internal static ParticleSystem AddChargeSphereLines( GameObject mainObj, WispSkinnedEffect skin, MaterialType matType, Single radius, Single moveTime, Single size, Single rate )
        {
            if( !chargeSphereLinesCounter.ContainsKey( mainObj ) ) chargeSphereLinesCounter[mainObj] = 0u;
            var obj = new GameObject( "ChargeSphereLines" + chargeSphereLinesCounter[mainObj]++ );
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

            var calcSpeed = radius / moveTime;

            var psMain = ps.main;
            psMain.duration = 2f;
            psMain.loop = true;
            psMain.startDelay = 0f;
            psMain.startLifetime = moveTime;
            psMain.startSpeed = -calcSpeed;
            psMain.startSize3D = false;
            psMain.startSize = size;
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
            psMain.maxParticles = Mathf.CeilToInt(moveTime * rate * 2);
            psMain.stopAction = ParticleSystemStopAction.None;
            psMain.cullingMode = ParticleSystemCullingMode.PauseAndCatchup;
            psMain.ringBufferMode = ParticleSystemRingBufferMode.Disabled;

            var psEmis = ps.emission;
            psEmis.enabled = true;
            psEmis.rateOverTime = rate;
            psEmis.rateOverDistance = 0f;

            var psShape = ps.shape;
            psShape.enabled = true;
            psShape.arcMode = ParticleSystemShapeMultiModeValue.Random;
            psShape.arcSpread = 0f;
            psShape.texture = null;
            psShape.position = Vector3.zero;
            psShape.rotation = Vector3.zero;
            psShape.scale = Vector3.one;
            psShape.randomDirectionAmount = 0f;
            psShape.randomPositionAmount = 0f;
            psShape.sphericalDirectionAmount = 0f;
            psShape.shapeType = ParticleSystemShapeType.Sphere;
            psShape.radiusThickness = 0f;
            psShape.radius = radius;

            var psCOL = ps.colorOverLifetime;
            psCOL.enabled = false;
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
            psSOL.enabled = false;
            psSOL.separateAxes = true;
            psSOL.x = new ParticleSystem.MinMaxCurve( 1f, AnimationCurve.EaseInOut( 0f, 1f, 1f, 0f ) );
            psSOL.y = 1f;
            psSOL.z = 1f;


            psr.renderMode = ParticleSystemRenderMode.Stretch;
            psr.cameraVelocityScale = 0f;
            psr.velocityScale = 1f;
            psr.lengthScale = 0.5f;
            psr.normalDirection = 1f;
            psr.sortMode = ParticleSystemSortMode.None;
            psr.sortingFudge = -20;
            psr.minParticleSize = 0f;
            psr.maxParticleSize = 1.04f;
            psr.flip = Vector3.zero;
            psr.pivot = new Vector3( 0f, -2.03f, 0f );
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
