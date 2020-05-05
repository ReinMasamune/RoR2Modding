using System;
using System.Collections.Generic;
using System.Text;
using ReinCore;
using RoR2;
using Sniper.Modules;
using UnityEngine;

namespace Sniper.Effects
{
    internal static partial class EffectCreator
    {
        internal static GameObject CreateBaseAmmoTracer( Material mainMat, Material trailMat )
        {
            GameObject obj = PrefabsCore.CreatePrefab("Standard Tracer", false );

            Transform tracerHead = new GameObject("TracerHead").transform;
            tracerHead.parent = obj.transform;
            tracerHead.localPosition = Vector3.zero;
            tracerHead.localRotation = Quaternion.identity;
            //tracerHead.localEulerAngles = new Vector3( 0f, 90f, 0f );
            tracerHead.localScale = Vector3.one;

            Transform tracerTail = new GameObject("TracerTail").transform;
            tracerTail.parent = obj.transform;
            tracerTail.localPosition = Vector3.zero;
            tracerTail.localRotation = Quaternion.identity;
            tracerTail.localScale = Vector3.one;



            Transform tailRotate = new GameObject( "tailRotate" ).transform;
            tailRotate.parent = tracerTail;
            tailRotate.localPosition = Vector3.zero;
            tailRotate.localRotation = Quaternion.identity;
            tailRotate.localScale = Vector3.one;

            Transform trail = new GameObject( "trail" ).transform;
            trail.parent = tailRotate;
            trail.localPosition = Vector3.zero;
            trail.localRotation = Quaternion.identity;
            trail.localScale = Vector3.one;


            Transform headBeam = new GameObject( "HeadBeam" ).transform;
            headBeam.parent = tracerTail;
            headBeam.localPosition = Vector3.zero;
            headBeam.localEulerAngles = new Vector3( 0f, 90f, 0f );
            headBeam.localScale = Vector3.one;

            EffectComponent effectComp = obj.AddComponent<EffectComponent>();

            Tracer tracer = obj.AddComponent<Tracer>();

            VFXAttributes vfxAtrib = obj.AddComponent<VFXAttributes>();

            Rigidbody headRb = tracerHead.AddComponent<Rigidbody>();

            Rigidbody tailRb = tracerTail.AddComponent<Rigidbody>();

            RotateObject rotator = tailRotate.AddComponent<RotateObject>();

            ParticleSystem mainPs = headBeam.AddComponent<ParticleSystem>();

            ParticleSystemRenderer mainPsr = headBeam.AddOrGetComponent<ParticleSystemRenderer>();

            ParticleSystem trailPs = trail.AddComponent<ParticleSystem>();

            ParticleSystemRenderer trailPsr = trail.AddOrGetComponent<ParticleSystemRenderer>();

            DestroyTracerOnDelay cleanup = obj.AddComponent<DestroyTracerOnDelay>();
            cleanup.delay = 2f;
            cleanup.tracer = tracer;

            ZeroTracerLengthOverDuration zeroLength = obj.AddComponent<ZeroTracerLengthOverDuration>();
            zeroLength.tracer = tracer;

            var timer = obj.AddComponent<DestroyOnTimer>();
            timer.duration = 10f;


            effectComp.effectIndex = EffectIndex.Invalid;
            effectComp.positionAtReferencedTransform = false;
            effectComp.parentToReferencedTransform = false;
            effectComp.applyScale = false;
            effectComp.soundName = null;
            effectComp.disregardZScale = false;


            tracer.startTransform = null;
            tracer.beamObject = null;
            tracer.beamDensity = 0f;
            tracer.speed = 600f;
            tracer.headTransform = tracerHead;
            tracer.tailTransform = tracerTail;
            tracer.length = 20f;
            tracer.reverse = false;

            rotator.rotationSpeed = new Vector3( 0f, 0f, 1440f );


            headRb.isKinematic = true;
            headRb.useGravity = false;

            tailRb.isKinematic = true;
            tailRb.useGravity = false;

            vfxAtrib.optionalLights = null;
            vfxAtrib.secondaryParticleSystem = null;
            vfxAtrib.vfxIntensity = VFXAttributes.VFXIntensity.Low;
            vfxAtrib.vfxPriority = VFXAttributes.VFXPriority.Always;

            mainPs.PlayOnStart();
            mainPs.SetupTracerMain();
            mainPs.SetupTracerEmission();
            mainPs.SetupTracerShape();
            mainPs.SetupTracerColorOverLifetime();
            mainPs.SetupTracerSizeOverLifetime();
            mainPsr.SetupTracerRenderer( mainMat );

            ParticleSystem.VelocityOverLifetimeModule mainPsVol = mainPs.velocityOverLifetime;
            mainPsVol.enabled = false;
            ParticleSystem.LimitVelocityOverLifetimeModule mainPsLvol = mainPs.limitVelocityOverLifetime;
            mainPsLvol.enabled = false;
            ParticleSystem.InheritVelocityModule mainPsIvel = mainPs.inheritVelocity;
            mainPsIvel.enabled = false;
            ParticleSystem.ForceOverLifetimeModule mainPsFol = mainPs.forceOverLifetime;
            mainPsFol.enabled = false;
            ParticleSystem.ColorBySpeedModule mainPsCbs = mainPs.colorBySpeed;
            mainPsCbs.enabled = false;
            ParticleSystem.SizeBySpeedModule mainPsSbs = mainPs.sizeBySpeed;
            mainPsSbs.enabled = false;
            ParticleSystem.RotationOverLifetimeModule mainPsRol = mainPs.rotationOverLifetime;
            mainPsRol.enabled = false;
            ParticleSystem.RotationBySpeedModule mainPsRbs = mainPs.rotationBySpeed;
            mainPsRbs.enabled = false;
            ParticleSystem.ExternalForcesModule mainPsExt = mainPs.externalForces;
            mainPsExt.enabled = false;
            ParticleSystem.NoiseModule mainPsNoise = mainPs.noise;
            mainPsNoise.enabled = false;
            ParticleSystem.CollisionModule mainPsColl = mainPs.collision;
            mainPsColl.enabled = false;
            ParticleSystem.TriggerModule mainPsTrig = mainPs.trigger;
            mainPsTrig.enabled = false;
            ParticleSystem.SubEmittersModule mainPsSub = mainPs.subEmitters;
            mainPsSub.enabled = false;
            ParticleSystem.TextureSheetAnimationModule mainPsTex = mainPs.textureSheetAnimation;
            mainPsTex.enabled = false;
            ParticleSystem.LightsModule mainPsLigh = mainPs.lights;
            mainPsLigh.enabled = false;
            ParticleSystem.TrailModule mainPsTrail = mainPs.trails;
            mainPsTrail.enabled = false;
            ParticleSystem.CustomDataModule mainPsData = mainPs.customData;
            mainPsData.enabled = false;


            trailPs.PlayOnStart();

            trailPs.SetupTracerTrailMain();
            trailPs.SetupTracerTrailEmission();
            trailPs.SetupTracerTrailTrail();
            trailPsr.SetupTracerTrailRenderer( trailMat );

            ParticleSystem.ShapeModule trailPsShape = trailPs.shape;
            trailPsShape.enabled = false;
            ParticleSystem.VelocityOverLifetimeModule trailPsVol = trailPs.velocityOverLifetime;
            trailPsVol.enabled = false;
            ParticleSystem.LimitVelocityOverLifetimeModule trailPsLvol = trailPs.limitVelocityOverLifetime;
            trailPsLvol.enabled = false;
            ParticleSystem.InheritVelocityModule trailPsIvel = trailPs.inheritVelocity;
            trailPsIvel.enabled = false;
            ParticleSystem.ForceOverLifetimeModule trailPsFol = trailPs.forceOverLifetime;
            trailPsFol.enabled = false;
            ParticleSystem.ColorOverLifetimeModule trailPsCol = trailPs.colorOverLifetime;
            trailPsCol.enabled = false;
            ParticleSystem.ColorBySpeedModule trailPsCbs = trailPs.colorBySpeed;
            trailPsCbs.enabled = false;
            ParticleSystem.SizeOverLifetimeModule trailPsSol = trailPs.sizeOverLifetime;
            trailPsSol.enabled = false;
            ParticleSystem.SizeBySpeedModule trailPsSbs = trailPs.sizeBySpeed;
            trailPsSbs.enabled = false;
            ParticleSystem.RotationOverLifetimeModule trailPsRol = trailPs.rotationOverLifetime;
            trailPsRol.enabled = false;
            ParticleSystem.RotationBySpeedModule trailPsRbs = trailPs.rotationBySpeed;
            trailPsRbs.enabled = false;
            ParticleSystem.ExternalForcesModule trailPsExt = trailPs.externalForces;
            trailPsExt.enabled = false;
            ParticleSystem.NoiseModule trailPsNoise = trailPs.noise;
            trailPsNoise.enabled = false;
            ParticleSystem.CollisionModule trailPsColl = trailPs.collision;
            trailPsColl.enabled = false;
            ParticleSystem.TriggerModule trailPsTrig = trailPs.trigger;
            trailPsTrig.enabled = false;
            ParticleSystem.SubEmittersModule trailPsSub = trailPs.subEmitters;
            trailPsSub.enabled = false;
            ParticleSystem.TextureSheetAnimationModule trailPsTex = trailPs.textureSheetAnimation;
            trailPsTex.enabled = false;
            ParticleSystem.LightsModule trailPsLigh = trailPs.lights;
            trailPsLigh.enabled = false;
            ParticleSystem.CustomDataModule trailPsData = trailPs.customData;
            trailPsData.enabled = false;





            trail.localEulerAngles = new Vector3( 90f, 0f, 0f );

            var trail2 = UnityEngine.Object.Instantiate<GameObject>( trail.gameObject, tailRotate ).transform;
            trail2.localEulerAngles = new Vector3( -30f, -90f, 0f );

            var trail3 = UnityEngine.Object.Instantiate<GameObject>( trail.gameObject, tailRotate ).transform;
            trail3.localEulerAngles = new Vector3( -150f, -90f, 0f );

            return obj;
        }

        private static void SetupTracerMain( this ParticleSystem particles )
        {
            ParticleSystem.MainModule mainPsMain = particles.main;
            mainPsMain.duration = 2f;
            mainPsMain.loop = true;
            mainPsMain.prewarm = false;
            mainPsMain.startDelay = 0f;
            mainPsMain.startLifetime = new ParticleSystem.MinMaxCurve( 0.1f, 0.12f );
            mainPsMain.startSpeed = new ParticleSystem.MinMaxCurve( 0.1f, 0.1f );
            mainPsMain.startSize3D = true;
            mainPsMain.startSizeX = 4f;
            mainPsMain.startSizeY = 0.5f;
            mainPsMain.startSizeZ = 4f;
            mainPsMain.startRotation3D = true;
            mainPsMain.startRotationX = new ParticleSystem.MinMaxCurve( 0f, Mathf.PI * 2f );
            mainPsMain.startRotationY = 0f;
            mainPsMain.startRotationZ = 0f;
            mainPsMain.flipRotation = 0f;
            mainPsMain.startColor = Color.white;
            mainPsMain.gravityModifier = 0f;
            mainPsMain.simulationSpace = ParticleSystemSimulationSpace.World;
            mainPsMain.simulationSpeed = 1f;
            mainPsMain.useUnscaledTime = false;
            mainPsMain.playOnAwake = false;
            mainPsMain.emitterVelocityMode = ParticleSystemEmitterVelocityMode.Transform;
            mainPsMain.maxParticles = 20000;
            mainPsMain.stopAction = ParticleSystemStopAction.None;
            mainPsMain.cullingMode = ParticleSystemCullingMode.AlwaysSimulate;
            mainPsMain.ringBufferMode = ParticleSystemRingBufferMode.Disabled;
            mainPsMain.scalingMode = ParticleSystemScalingMode.Hierarchy;
        }

        private static void SetupTracerEmission( this ParticleSystem particles )
        {
            ParticleSystem.EmissionModule mainPsEmis = particles.emission;
            mainPsEmis.enabled = true;
            mainPsEmis.rateOverTime = 0f;
            mainPsEmis.rateOverDistance = 14f;
            mainPsEmis.burstCount = 0;
        }

        private static void SetupTracerShape( this ParticleSystem particles )
        {
            ParticleSystem.ShapeModule mainPsShape = particles.shape;
            mainPsShape.enabled = true;
            mainPsShape.shapeType = ParticleSystemShapeType.SingleSidedEdge;
            mainPsShape.radius = 0.1f;
            mainPsShape.radiusMode = ParticleSystemShapeMultiModeValue.Random;
            mainPsShape.arcSpread = 0f;
            mainPsShape.texture = null;
            mainPsShape.position = Vector3.zero;
            mainPsShape.rotation = Vector3.zero;
            mainPsShape.scale = Vector3.one;
            mainPsShape.alignToDirection = false;
            mainPsShape.randomDirectionAmount = 0f;
            mainPsShape.randomPositionAmount = 0f;
            mainPsShape.sphericalDirectionAmount = 0f;
        }

        private static void SetupTracerColorOverLifetime( this ParticleSystem particles )
        {
            ParticleSystem.ColorOverLifetimeModule mainPsCol = particles.colorOverLifetime;
            mainPsCol.enabled = true;
            mainPsCol.color = new ParticleSystem.MinMaxGradient( new Gradient
            {
                mode = GradientMode.Blend,
                alphaKeys = new[]
                {
                    new GradientAlphaKey( 1f, 0f ),
                    new GradientAlphaKey( 1f, 0.5f ),
                    new GradientAlphaKey( 0f, 1f ),
                },
                colorKeys = new[]
                {
                    new GradientColorKey( Color.white, 0f ),
                    new GradientColorKey( Color.white, 1f ),
                }
            } );
        }

        private static void SetupTracerSizeOverLifetime( this ParticleSystem particles )
        {
            ParticleSystem.SizeOverLifetimeModule mainPsSol = particles.sizeOverLifetime;
            mainPsSol.enabled = true;
            mainPsSol.separateAxes = true;
            mainPsSol.x = 1f;
            mainPsSol.y = new ParticleSystem.MinMaxCurve( 1f, AnimationCurve.EaseInOut( 0f, 1f, 1f, 0f ) );
            mainPsSol.z = 1f;
        }

        private static void SetupTracerRenderer( this ParticleSystemRenderer renderer, Material material )
        {
            renderer.renderMode = ParticleSystemRenderMode.Mesh;
            renderer.mesh = AssetsCore.LoadAsset<Mesh>( MeshIndex.Quad );
            renderer.material = material;
            renderer.trailMaterial = null;
            renderer.sortMode = ParticleSystemSortMode.None;
            renderer.sortingFudge = 0;
            renderer.alignment = ParticleSystemRenderSpace.Local;
            renderer.flip = Vector3.zero;
            renderer.enableGPUInstancing = true;
            renderer.pivot = Vector3.zero;
            renderer.maskInteraction = SpriteMaskInteraction.None;
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            renderer.receiveShadows = false;
            renderer.motionVectorGenerationMode = MotionVectorGenerationMode.Object;
            renderer.sortingLayerID = LayerIndex.defaultLayer.intVal;
            renderer.sortingOrder = 0;
            renderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
            renderer.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
        }

        private static void SetupTracerTrailMain( this ParticleSystem particles )
        {
            ParticleSystem.MainModule trailPsMain = particles.main;
            trailPsMain.duration = 5f;
            trailPsMain.loop = true;
            trailPsMain.prewarm = false;
            trailPsMain.startDelay = 0f;
            trailPsMain.startLifetime = 0.35f;
            trailPsMain.startSpeed = new ParticleSystem.MinMaxCurve( 0.8f, 1.7f );
            trailPsMain.startSize3D = false;
            trailPsMain.startSize = 1f;
            trailPsMain.startRotation3D = false;
            trailPsMain.startRotation = 0f;
            trailPsMain.flipRotation = 0f;
            trailPsMain.startColor = Color.white;
            trailPsMain.gravityModifier = 0f;
            trailPsMain.simulationSpace = ParticleSystemSimulationSpace.World;
            trailPsMain.simulationSpeed = 1f;
            trailPsMain.useUnscaledTime = false;
            trailPsMain.playOnAwake = false;
            trailPsMain.emitterVelocityMode = ParticleSystemEmitterVelocityMode.Rigidbody;
            trailPsMain.maxParticles = 10000;
            trailPsMain.stopAction = ParticleSystemStopAction.None;
            trailPsMain.cullingMode = ParticleSystemCullingMode.Automatic;
            trailPsMain.ringBufferMode = ParticleSystemRingBufferMode.Disabled;
            trailPsMain.scalingMode = ParticleSystemScalingMode.Hierarchy;
        }

        private static void SetupTracerTrailEmission( this ParticleSystem particles )
        {
            ParticleSystem.EmissionModule trailPsEmis = particles.emission;
            trailPsEmis.enabled = true;
            trailPsEmis.rateOverTime = 0f;
            trailPsEmis.rateOverDistance = 4f;
            trailPsEmis.burstCount = 0;
        }

        private static void SetupTracerTrailTrail( this ParticleSystem particles )
        {
            ParticleSystem.TrailModule trailPsTrail = particles.trails;
            trailPsTrail.enabled = true;
            trailPsTrail.mode = ParticleSystemTrailMode.Ribbon;
            trailPsTrail.ribbonCount = 1;
            trailPsTrail.splitSubEmitterRibbons = false;
            trailPsTrail.attachRibbonsToTransform = false;
            trailPsTrail.textureMode = ParticleSystemTrailTextureMode.Tile;
            trailPsTrail.sizeAffectsWidth = true;
            trailPsTrail.inheritParticleColor = false;
            trailPsTrail.colorOverLifetime = new ParticleSystem.MinMaxGradient( new Gradient
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
                }
            } );
            trailPsTrail.colorOverTrail = Color.white;
            trailPsTrail.widthOverTrail = 0.25f;
            trailPsTrail.generateLightingData = true;
            trailPsTrail.shadowBias = 0.5f;
        }

        private static void SetupTracerTrailRenderer( this ParticleSystemRenderer renderer, Material material )
        {
            renderer.renderMode = ParticleSystemRenderMode.None;
            renderer.material = null;
            renderer.trailMaterial = material;
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            renderer.receiveShadows = false;
            renderer.motionVectorGenerationMode = MotionVectorGenerationMode.Object;
            renderer.sortingLayerID = LayerIndex.defaultLayer.intVal;
            renderer.sortingOrder = 0;
            renderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
            renderer.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
        }
    }
}
