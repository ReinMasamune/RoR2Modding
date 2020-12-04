namespace Rein.Sniper.Effects
{
    using System;

    using Rein.Sniper.Modules;

    using ReinCore;

    using RoR2;

    using UnityEngine;
    using UnityEngine.Experimental.VFX;

    internal static partial class EffectCreator
    {
        internal static GameObject CreateKnifePickupSlash(CloudMaterial mat)
        {
            var obj = AssetsCore.LoadAsset<GameObject>(PrefabIndex.refMercSwordSlashWhirlwind).ClonePrefab("Rein.SniperKnifeSlash", false);
            var effect = obj.GetComponent<EffectComponent>();
            effect.applyScale = true;
            effect.disregardZScale = true;

            var swing = obj.transform.Find("SwingTrail");
            var dist = obj.transform.Find("Distortion");

            var swingPs = swing.GetComponent<ParticleSystem>();
            var swingPsr = swing.GetComponent<ParticleSystemRenderer>();

            var distPs = dist.GetComponent<ParticleSystem>();
            var distPsr = dist.GetComponent<ParticleSystemRenderer>();

            //dist.localEulerAngles = swing.localEulerAngles = new(90f, 0f, 0f);

            var swingMain = swingPs.main;
            var distMain = distPs.main;

            distMain.scalingMode = swingMain.scalingMode = ParticleSystemScalingMode.Hierarchy;

            dist.localScale = swing.localScale = new(0.2f, 0.2f, 1f);


            swingPsr.sharedMaterial = mat.material;

            var swing2 = swing.gameObject.Instantiate().transform;
            swing2.parent = obj.transform;
            swing2.localScale = Vector3.Scale(swing2.localScale, new(0.5f, 0.5f, 1f));

            var swing3 = swing2.gameObject.Instantiate().transform;
            swing3.parent = obj.transform;
            swing3.localScale = Vector3.Scale(swing3.localScale, new(0.5f, 0.5f, 1f));

            //var obj = PrefabsCore.CreatePrefab("Rein.SniperKnifePickupSlash", false);
            //var effect = obj.AddComponent<EffectComponent>();
            //effect.effectIndex = EffectIndex.Invalid;
            //effect.positionAtReferencedTransform = false;
            //effect.parentToReferencedTransform = false;
            //effect.applyScale = false;
            //effect.disregardZScale = false;
            //effect.soundName = "";

            //obj.AddComponent<DestroyOnParticleEnd>();
            //var shake = obj.AddComponent<ShakeEmitter>();
            //shake.shakeOnStart = true;
            //shake.wave = new Wave
            //{
            //    amplitude = 0.2f,
            //    cycleOffset = 0.25f,
            //    frequency = 0.1f,
            //};
            //shake.duration = 0.1f;
            //shake.radius = 20f;
            //shake.scaleShakeRadiusWithLocalScale = true;
            //shake.amplitudeTimeDecay = true;

            //var vfx = obj.AddComponent<VFXAttributes>();
            //vfx.vfxPriority = VFXAttributes.VFXPriority.Always;
            //vfx.vfxIntensity = VFXAttributes.VFXIntensity.Medium;
            //vfx.optionalLights = Array.Empty<Light>();
            //vfx.secondaryParticleSystem = Array.Empty<ParticleSystem>();



            //#region Lines
            //var linesObj = new GameObject("Lines");
            //linesObj.transform.parent = obj.transform;
            //linesObj.transform.localPosition = Vector3.zero;
            //linesObj.transform.localScale = Vector3.one;
            //linesObj.transform.localRotation = Quaternion.identity;

            //var linePs = linesObj.AddComponent<ParticleSystem>();
            //var linePsr = linesObj.AddOrGetComponent<ParticleSystemRenderer>();

            //var lineMain = linePs.main;
            //lineMain.duration = 1.0f;
            //lineMain.loop = false;
            //lineMain.prewarm = false;
            //lineMain.startDelay = 0f;
            //lineMain.startLifetime = 0.25f;
            //lineMain.startSpeed = 40f;
            //lineMain.startSize = 0.05f;
            //lineMain.startRotation = 0f;
            //lineMain.flipRotation = 0f;
            //lineMain.startColor = Color.white;
            //lineMain.gravityModifier = 0f;
            //lineMain.simulationSpace = ParticleSystemSimulationSpace.World;
            //lineMain.simulationSpeed = 1f;
            //lineMain.scalingMode = ParticleSystemScalingMode.Hierarchy;
            //lineMain.playOnAwake = true;
            //lineMain.emitterVelocityMode = ParticleSystemEmitterVelocityMode.Rigidbody;
            //lineMain.maxParticles = 200;
            //lineMain.stopAction = ParticleSystemStopAction.None;
            //lineMain.cullingMode = ParticleSystemCullingMode.AlwaysSimulate;
            //lineMain.ringBufferMode = ParticleSystemRingBufferMode.Disabled;

            //var lineEmis = linePs.emission;
            //lineEmis.enabled = true;
            //lineEmis.rateOverTime = 0f;
            //lineEmis.rateOverDistance = 0f;
            //lineEmis.burstCount = 1;
            //lineEmis.SetBurst(0, new ParticleSystem.Burst(0f, 50, 1, 0.01f));

            //var lineShape = linePs.shape;
            //lineShape.enabled = true;
            //lineShape.shapeType = ParticleSystemShapeType.Sphere;
            //lineShape.radius = 5f;
            //lineShape.radiusThickness = 0f;
            //lineShape.arc = 360f; //TODO: Radians or degrees?
            //lineShape.arcMode = ParticleSystemShapeMultiModeValue.Random;
            //lineShape.arcSpread = 0f;
            //lineShape.texture = null;
            //lineShape.position = Vector3.zero;
            //lineShape.rotation = Vector3.zero;
            //lineShape.scale = Vector3.one;
            //lineShape.alignToDirection = false;
            //lineShape.randomDirectionAmount = 0f;
            //lineShape.sphericalDirectionAmount = 0f;
            //lineShape.randomPositionAmount = 0f;

            //var lineVol = linePs.velocityOverLifetime;
            //lineVol.enabled = false;
            //lineVol.speedModifier = new(1f, new AnimationCurve(new(0f, 1f, 0f, 0f), new(0.25f, 0f, 0f, 0f), new(0.5f, 0f, 0f, 0f), new(1f, -1f, 0f, 0f)));

            //linePsr.renderMode = ParticleSystemRenderMode.Stretch;
            //linePsr.cameraVelocityScale = 0f;
            //linePsr.velocityScale = 0.5f;
            //linePsr.lengthScale = 1f;
            //linePsr.normalDirection = 1f;
            //linePsr.material = AssetsCore.LoadAsset<Material>(MaterialIndex.refMatTracerBright);
            //#endregion

            //#region Distortion
            //var distortObj = new GameObject("Distortion");
            //distortObj.transform.parent = obj.transform;
            //distortObj.transform.localPosition = Vector3.zero;
            //distortObj.transform.localScale = Vector3.one;
            //distortObj.transform.localRotation = Quaternion.identity;

            //var distortPs = distortObj.AddComponent<ParticleSystem>();
            //var distortPsr = distortObj.AddOrGetComponent<ParticleSystemRenderer>();

            //var distortMain = distortPs.main;
            //distortMain.duration = 1.0f;
            //distortMain.loop = false;
            //distortMain.prewarm = false;
            //distortMain.startDelay = 0f;
            //distortMain.startLifetime = 0.1f;
            //distortMain.startSpeed = 0f;
            //distortMain.startSize = 10f;
            //distortMain.startRotation = 0f;
            //distortMain.flipRotation = 0f;
            //distortMain.startColor = Color.white;
            //distortMain.gravityModifier = 0f;
            //distortMain.simulationSpace = ParticleSystemSimulationSpace.World;
            //distortMain.simulationSpeed = 1f;
            //distortMain.scalingMode = ParticleSystemScalingMode.Hierarchy;
            //distortMain.playOnAwake = true;
            //distortMain.emitterVelocityMode = ParticleSystemEmitterVelocityMode.Rigidbody;
            //distortMain.maxParticles = 10;
            //distortMain.stopAction = ParticleSystemStopAction.None;
            //distortMain.cullingMode = ParticleSystemCullingMode.AlwaysSimulate;
            //distortMain.ringBufferMode = ParticleSystemRingBufferMode.Disabled;

            //var distortEmis = distortPs.emission;
            //distortEmis.enabled = true;
            //distortEmis.rateOverTime = 0f;
            //distortEmis.rateOverDistance = 0f;
            //distortEmis.burstCount = 1;
            //distortEmis.SetBurst(0, new ParticleSystem.Burst(0f, 1, 1, 0.01f));

            //var distortShape = distortPs.shape;
            //distortShape.enabled = false;

            //var distortSol = distortPs.sizeOverLifetime;
            //distortSol.enabled = true;
            //distortSol.size = new(1f, new AnimationCurve(new(0f, 0.5f, 1f, 1f), new(0.25f, 1f, 0f, 0f), new(0.5f, 1f, 0f, 0f), new(1f, 0f, 0f, 0f)));

            //distortPsr.renderMode = ParticleSystemRenderMode.Mesh;
            //distortPsr.mesh = AssetsCore.LoadAsset<Mesh>(MeshIndex.Sphere);
            //distortPsr.material = AssetsCore.LoadAsset<Material>(MaterialIndex.refMatDistortionFaded);
            //#endregion
            //var obj = Resources.Load<GameObject>("");



            return obj;
        }
    }
}
