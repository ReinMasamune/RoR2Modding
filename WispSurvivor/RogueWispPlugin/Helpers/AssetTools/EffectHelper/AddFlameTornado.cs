using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    internal static partial class EffectHelper
    {
        private static Dictionary<GameObject, UInt32> flameTornadoCounter = new Dictionary<GameObject, UInt32>();
        internal static ParticleSystem AddFlameTornado( GameObject mainObj, WispSkinnedEffect skin, MaterialType matType, Single lifetime, Single duration, Single radius, Single height )
        {
            if( !flameTornadoCounter.ContainsKey( mainObj ) ) flameTornadoCounter[mainObj] = 0u;
            var obj = new GameObject( "FlameTornado" + flameTornadoCounter[mainObj]++ );
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
            psMain.duration = duration;
            psMain.loop = true;
            psMain.prewarm = false;
            psMain.startDelay = 0.0f;
            psMain.startLifetime = new ParticleSystem.MinMaxCurve( lifetime * 0.75f, lifetime * 1.25f );
            psMain.startSpeed = 0f;
            psMain.startSize3D = true;
            psMain.startSizeX = new ParticleSystem.MinMaxCurve( radius * 0.75f, radius * 1.25f );
            psMain.startSizeY = new ParticleSystem.MinMaxCurve( radius * 0.75f, radius * 1.25f );
            psMain.startSizeZ = height;
            psMain.startRotation3D = true;
            psMain.startRotationX = Mathf.PI / -2f;
            psMain.startRotationY = new ParticleSystem.MinMaxCurve( 0f, Mathf.PI * 2f );
            psMain.startRotationZ = 0f;
            psMain.flipRotation = 0f;
            psMain.startColor = Color.white;
            psMain.gravityModifier = 0f;
            psMain.simulationSpace = ParticleSystemSimulationSpace.World;
            psMain.simulationSpeed = 1f;
            psMain.useUnscaledTime = false;
            psMain.scalingMode = ParticleSystemScalingMode.Local;
            psMain.playOnAwake = true;
            psMain.emitterVelocityMode = ParticleSystemEmitterVelocityMode.Transform;
            psMain.maxParticles = 1000;
            psMain.stopAction = ParticleSystemStopAction.None;
            psMain.cullingMode = ParticleSystemCullingMode.PauseAndCatchup;
            psMain.ringBufferMode = ParticleSystemRingBufferMode.Disabled;

            var psEmis = ps.emission;
            psEmis.enabled = true;
            //psEmis.burstCount = 1;
            //psEmis.SetBurst( 0, new ParticleSystem.Burst( 0f, 4, 4, 1, 0.01f ) );
            psEmis.rateOverTime = 5f;
            psEmis.rateOverDistance = 0f;

            //var psShape = ps.shape;
            //psShape.enabled = true;
            //psShape.shapeType = ParticleSystemShapeType.BoxEdge;
            //psShape.radius = 0.01f;
            //psShape.arcMode = ParticleSystemShapeMultiModeValue.Random;
            //psShape.arcSpread = 0f;
            //psShape.texture = null;
            //psShape.alignToDirection = true;
            //psShape.randomDirectionAmount = 0f;
            //psShape.sphericalDirectionAmount = 0f;


            var psCOL = ps.colorOverLifetime;
            psCOL.enabled = true;
            psCOL.color = new ParticleSystem.MinMaxGradient( new Gradient
            {
                mode = GradientMode.Blend,
                alphaKeys = new[]
                {
                    new GradientAlphaKey(0f, 0f ),
                    new GradientAlphaKey( 1f, 0.5f ),
                    new GradientAlphaKey(0f, 1f ),
                },
                colorKeys = new[]
                {
                    new GradientColorKey( Color.white, 0f ),
                    new GradientColorKey( Color.white, 1f ),
                }
            } );
            

            var psSOL = ps.sizeOverLifetime;
            psSOL.enabled = true;
            psSOL.separateAxes = false;
            psSOL.size = new ParticleSystem.MinMaxCurve( 1f, AnimationCurve.EaseInOut( 0f, 0.5f, 1f, 1f ) );
            //psSOL.x = new ParticleSystem.MinMaxCurve(1f, AnimationCurve.EaseInOut( 0f, 1f, 1f, 0f ));
            //psSOL.y = new ParticleSystem.MinMaxCurve( 1f, AnimationCurve.EaseInOut( 0f, 1f, 1f, 0f ) );
            //psSOL.z = new ParticleSystem.MinMaxCurve( 1f, AnimationCurve.EaseInOut( 0.5f, 1f, 1f, 0.75f ) );
            //psSOL.size = new ParticleSystem.MinMaxCurve( 1f, AnimationCurve.Linear( 0f, 0.5f, 1f, 1f ) );

            var psROL = ps.rotationOverLifetime;
            psROL.enabled = true;
            psROL.separateAxes = true;
            psROL.x = 0f;
            //psROL.y = new ParticleSystem.MinMaxCurve( Mathf.PI * 0.75f * 3f, Mathf.PI * 1.25f * 3f );
            psROL.y = 0f;
            psROL.z = Mathf.PI * 2f;


            psr.renderMode = ParticleSystemRenderMode.Mesh;
            psr.mesh = AssetLibrary<Mesh>.i[MeshIndex.TornadoMesh2];
            //psr.normalDirection = 1f;
            //psr.sortMode = ParticleSystemSortMode.None;
            //psr.minParticleSize = 0f;
            //psr.maxParticleSize = 1.04f;
            psr.alignment = ParticleSystemRenderSpace.World;
            //psr.flip = Vector3.zero;
            psr.enableGPUInstancing = true;
            //psr.allowRoll = true;
            //psr.pivot = Vector3.zero;
            psr.maskInteraction = SpriteMaskInteraction.None;
            //psr.SetActiveVertexStreams( null );
            //psr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            //psr.receiveShadows = true;
            //psr.shadowBias = 0f;
            //psr.motionVectorGenerationMode = MotionVectorGenerationMode.Object;
            //psr.sortingLayerID = default;
            //psr.sortingOrder = 0;
            //psr.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.BlendProbes;
            //psr.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Simple;
            //psr.probeAnchor = null;

            return ps;
        }
    }
}
