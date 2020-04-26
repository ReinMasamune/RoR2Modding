using System;
using System.Collections.Generic;
using System.Text;
using ReinCore;
using RoR2;
using UnityEngine;
using Sniper.Modules;

namespace Sniper.Effects
{
    internal static partial class EffectCreator
    {
        internal static GameObject CreateStandardAmmoTracer()
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

            Transform headBeam = new GameObject( "HeadBeam" ).transform;
            headBeam.parent = tracerHead;
            headBeam.localPosition = Vector3.zero;
            headBeam.localEulerAngles = new Vector3( 0f, 90f, 0f );
            headBeam.localScale = Vector3.one;

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


            EffectComponent effectComp = obj.AddComponent<EffectComponent>();

            Tracer tracer = obj.AddComponent<Tracer>();

            //BeamPointsFromTransforms beamPoints = obj.AddComponent<BeamPointsFromTransforms>();

            //LineRenderer line = obj.AddComponent<LineRenderer>();

            EventFunctions eventFuncs = obj.AddComponent<EventFunctions>();

            VFXAttributes vfxAtrib = obj.AddComponent<VFXAttributes>();

            Rigidbody headRb = tracerHead.AddComponent<Rigidbody>();

            Rigidbody tailRb = tracerTail.AddComponent<Rigidbody>();


            RotateObject rotator = tailRotate.AddComponent<RotateObject>();


            ParticleSystem mainPs = headBeam.AddComponent<ParticleSystem>();

            ParticleSystemRenderer mainPsr = headBeam.AddOrGetComponent<ParticleSystemRenderer>();


            ParticleSystem trailPs = trail.AddComponent<ParticleSystem>();

            ParticleSystemRenderer trailPsr = trail.AddOrGetComponent<ParticleSystemRenderer>();




            effectComp.effectIndex = EffectIndex.Invalid;
            effectComp.positionAtReferencedTransform = false;
            effectComp.parentToReferencedTransform = false;
            effectComp.applyScale = false;
            effectComp.soundName = "";
            effectComp.disregardZScale = false;

            tracer.startTransform = null;
            tracer.beamObject = null;
            tracer.beamDensity = 0f;
            tracer.speed = 600f;
            tracer.headTransform = tracerHead;
            tracer.tailTransform = tracerTail;
            tracer.length = 1f;
            tracer.reverse = false;
            //tracer.onTailReachedDestination = new UnityEngine.Events.UnityEvent();
            //tracer.onTailReachedDestination.AddListener( new UnityEngine.Events.UnityAction( () => eventFuncs.UnparentTransform(smokeBeam) ) );
            //tracer.onTailReachedDestination.AddListener( new UnityEngine.Events.UnityAction( () => eventFuncs.DestroySelf() ) );

            //beamPoints.target = line;
            //beamPoints._SetPointTransforms( tracerHead, tracerTail );

            // TODO: Line Renderer setup

            rotator.rotationSpeed = new Vector3( 0f, 0f, 2880f );


            headRb.isKinematic = true;
            headRb.useGravity = false;

            tailRb.isKinematic = true;
            tailRb.useGravity = false;

            vfxAtrib.optionalLights = null;
            vfxAtrib.secondaryParticleSystem = null;
            vfxAtrib.vfxIntensity = VFXAttributes.VFXIntensity.Low;
            vfxAtrib.vfxPriority = VFXAttributes.VFXPriority.Always;



            ParticleSystem.MainModule mainPsMain = mainPs.main;
            mainPsMain.duration = 2f;
            mainPsMain.loop = true;
            mainPsMain.prewarm = false;
            mainPsMain.startDelay = 0f;
            mainPsMain.startLifetime = new ParticleSystem.MinMaxCurve( 0.4f, 0.42f );
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
            mainPsMain.simulationSpace = ParticleSystemSimulationSpace.Local;
            mainPsMain.simulationSpeed = 1f;
            mainPsMain.useUnscaledTime = false;
            mainPsMain.playOnAwake = true;
            mainPsMain.emitterVelocityMode = ParticleSystemEmitterVelocityMode.Transform;
            mainPsMain.maxParticles = 20000;
            mainPsMain.stopAction = ParticleSystemStopAction.None;
            mainPsMain.cullingMode = ParticleSystemCullingMode.AlwaysSimulate;
            mainPsMain.ringBufferMode = ParticleSystemRingBufferMode.Disabled;
            mainPsMain.scalingMode = ParticleSystemScalingMode.Hierarchy;

            ParticleSystem.EmissionModule mainPsEmis = mainPs.emission;
            mainPsEmis.enabled = true;
            mainPsEmis.rateOverTime = 0f;
            mainPsEmis.rateOverDistance = 9f;
            mainPsEmis.burstCount = 0;

            ParticleSystem.ShapeModule mainPsShape = mainPs.shape;
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

            ParticleSystem.VelocityOverLifetimeModule mainPsVol = mainPs.velocityOverLifetime;
            mainPsVol.enabled = false;

            ParticleSystem.LimitVelocityOverLifetimeModule mainPsLvol = mainPs.limitVelocityOverLifetime;
            mainPsLvol.enabled = false;

            ParticleSystem.InheritVelocityModule mainPsIvel = mainPs.inheritVelocity;
            mainPsIvel.enabled = false;

            ParticleSystem.ForceOverLifetimeModule mainPsFol = mainPs.forceOverLifetime;
            mainPsFol.enabled = false;

            ParticleSystem.ColorOverLifetimeModule mainPsCol = mainPs.colorOverLifetime;
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

            ParticleSystem.ColorBySpeedModule mainPsCbs = mainPs.colorBySpeed;
            mainPsCbs.enabled = false;

            ParticleSystem.SizeOverLifetimeModule mainPsSol = mainPs.sizeOverLifetime;
            mainPsSol.enabled = true;
            mainPsSol.separateAxes = true;
            mainPsSol.x = 1f;
            mainPsSol.y = new ParticleSystem.MinMaxCurve( 1f, AnimationCurve.EaseInOut( 0f, 1f, 1f, 0f ) );
            mainPsSol.z = 1f;

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

            CloudMaterial mainMat = MaterialModule.GetStandardTracerMaterial();
            obj.AddComponent<CloudMaterialHolder>().cloudMaterial = mainMat;

            mainPsr.renderMode = ParticleSystemRenderMode.Mesh;
            mainPsr.mesh = AssetsCore.LoadAsset<Mesh>( MeshIndex.Quad );
            mainPsr.material = mainMat.material;
            mainPsr.trailMaterial = null;
            mainPsr.sortMode = ParticleSystemSortMode.None;
            mainPsr.sortingFudge = 0;
            mainPsr.alignment = ParticleSystemRenderSpace.Local;
            mainPsr.flip = Vector3.zero;
            mainPsr.enableGPUInstancing = true;
            mainPsr.pivot = Vector3.zero;
            mainPsr.maskInteraction = SpriteMaskInteraction.None;
            mainPsr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            mainPsr.receiveShadows = false;
            mainPsr.motionVectorGenerationMode = MotionVectorGenerationMode.Object;
            mainPsr.sortingLayerID = LayerIndex.defaultLayer.intVal;
            mainPsr.sortingOrder = 0;
            mainPsr.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
            mainPsr.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;



            trailPs.PlayOnStart();

            ParticleSystem.MainModule trailPsMain = trailPs.main;
            trailPsMain.duration = 5f;
            trailPsMain.loop = true;
            trailPsMain.prewarm = false;
            trailPsMain.startDelay = 0f;
            trailPsMain.startLifetime = 1.5f;
            trailPsMain.startSpeed = 0.35f;
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
            trailPsMain.maxParticles = 1000000;
            trailPsMain.stopAction = ParticleSystemStopAction.None;
            trailPsMain.cullingMode = ParticleSystemCullingMode.AlwaysSimulate;
            trailPsMain.ringBufferMode = ParticleSystemRingBufferMode.Disabled;
            trailPsMain.scalingMode = ParticleSystemScalingMode.Hierarchy;

            ParticleSystem.EmissionModule trailPsEmis = trailPs.emission;
            trailPsEmis.enabled = true;
            trailPsEmis.rateOverTime = 0f;
            trailPsEmis.rateOverDistance = 3f;
            trailPsEmis.burstCount = 0;

            ParticleSystem.ShapeModule trailPsShape = trailPs.shape;
            trailPsShape.enabled = false;

            ParticleSystem.VelocityOverLifetimeModule trailPsVol = trailPs.velocityOverLifetime;
            trailPsVol.enabled = false;
            trailPsVol.orbitalOffsetX = 0f;
            trailPsVol.orbitalOffsetY = 0.083f;
            trailPsVol.orbitalOffsetZ = 0f;
            trailPsVol.orbitalX = 10f;
            trailPsVol.orbitalY = 0f;
            trailPsVol.orbitalZ = 0f;
            trailPsVol.radial = new ParticleSystem.MinMaxCurve( 0.001f, AnimationCurve.Linear( 0f, 0f, 1f, 1f ) );
            trailPsVol.space = ParticleSystemSimulationSpace.Local;
            trailPsVol.speedModifier = 10f;
            trailPsVol.x = 0f;
            trailPsVol.y = 0f;
            trailPsVol.z = 0f;

            ParticleSystem.LimitVelocityOverLifetimeModule trailPsLvol = trailPs.limitVelocityOverLifetime;
            trailPsLvol.enabled = false;

            ParticleSystem.InheritVelocityModule trailPsIvel = trailPs.inheritVelocity;
            trailPsIvel.enabled = false;

            ParticleSystem.ForceOverLifetimeModule trailPsFol = trailPs.forceOverLifetime;
            trailPsFol.enabled = false;

            ParticleSystem.ColorOverLifetimeModule trailPsCol = trailPs.colorOverLifetime;
            trailPsCol.enabled = true;
            trailPsCol.color = new ParticleSystem.MinMaxGradient( new Gradient
            {
                mode = GradientMode.Blend,
                alphaKeys = new[]
                {
                    new GradientAlphaKey( 1f, 0f ),
                    new GradientAlphaKey( 1f, 0.35f ),
                    new GradientAlphaKey( 0f, 1f ),
                },
                colorKeys = new[]
                {
                    new GradientColorKey( Color.white, 0f ),
                    new GradientColorKey( Color.white, 1f ),
                }
            } );

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
            trailPsNoise.separateAxes = true;
            trailPsNoise.strengthX = 0f;
            trailPsNoise.strengthY = 1f;
            trailPsNoise.strengthZ = 1f;
            trailPsNoise.frequency = 0.25f;
            trailPsNoise.scrollSpeed = 0.25f;
            trailPsNoise.damping = false;
            trailPsNoise.octaveCount = 1;
            trailPsNoise.octaveMultiplier = 0.5f;
            trailPsNoise.octaveScale = 2f;
            trailPsNoise.quality = ParticleSystemNoiseQuality.High;
            trailPsNoise.remapEnabled = false;
            trailPsNoise.positionAmount = 0.25f;
            trailPsNoise.sizeAmount = 0f;


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

            ParticleSystem.TrailModule trailPsTrail = trailPs.trails;
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
            //trailPsTrail.colorOverTrail = new ParticleSystem.MinMaxGradient( new Gradient
            //{
            //    mode = GradientMode.Blend,
            //    alphaKeys = new[]
            //    {
            //        new GradientAlphaKey( 1f, 0f ),
            //        new GradientAlphaKey( 1f, 0.5f ),
            //        new GradientAlphaKey( 0f, 1f ),
            //    },
            //    colorKeys = new[]
            //    {
            //        new GradientColorKey( Color.white, 0f ),
            //        new GradientColorKey( Color.white, 1f ),
            //    }
            //} );
            trailPsTrail.colorOverLifetime = Color.white;
            trailPsTrail.colorOverTrail = Color.white;
            trailPsTrail.widthOverTrail = 0.25f;
            trailPsTrail.generateLightingData = true;
            trailPsTrail.shadowBias = 0.5f;

            ParticleSystem.CustomDataModule trailPsData = trailPs.customData;
            trailPsData.enabled = false;

            CloudMaterial trailMat = MaterialModule.GetStandardTracerTrailMaterial();
            obj.AddComponent<CloudMaterialHolder>().cloudMaterial = trailMat;

            trailPsr.renderMode = ParticleSystemRenderMode.None;
            trailPsr.material = null;
            trailPsr.trailMaterial = trailMat.material;
            trailPsr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            trailPsr.receiveShadows = false;
            trailPsr.motionVectorGenerationMode = MotionVectorGenerationMode.Object;
            trailPsr.sortingLayerID = LayerIndex.defaultLayer.intVal;
            trailPsr.sortingOrder = 0;
            trailPsr.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
            trailPsr.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;


            trail.localEulerAngles = new Vector3( 90f, 0f, 0f );

            var trail2 = UnityEngine.Object.Instantiate<GameObject>( trail.gameObject, tailRotate ).transform;
            trail2.localEulerAngles = new Vector3( -30f, -90f, 0f );

            var trail3 = UnityEngine.Object.Instantiate<GameObject>( trail.gameObject, tailRotate ).transform;
            trail3.localEulerAngles = new Vector3( -150f, -90f, 0f );

            //var trail4 = UnityEngine.Object.Instantiate<GameObject>( trail.gameObject, tailRotate ).transform;
            //trail4.localEulerAngles = new Vector3( -90f, 0f, 0f );

            return obj;
        }
    }
}
