namespace Rein.Sniper.Effects
{
    using Rein.Sniper.Modules;

    using ReinCore;

    using RoR2;

    using UnityEngine;

    internal static partial class EffectCreator
    {
        internal static GameObject CreatePlasmaBurnPrefab()
        {
            var obj = PrefabsCore.CreatePrefab("PlasmaBurn", false);

            var holder = new GameObject("Particles");
            holder.transform.parent = obj.transform;

            var mainPs = holder.AddComponent<ParticleSystem>();
            var mainPsr = holder.AddOrGetComponent<ParticleSystemRenderer>();

            mainPs
                .BurnMain()
                .BurnEmission()
                .BurnShape()
                .BurnCOL()
                .BurnSOL();

            mainPsr
                .BurnRenderer();


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






            return obj;
        }

        private static ParticleSystem BurnMain(this ParticleSystem ps)
        {
            var m = ps.main;
            m.duration = 2f;
            m.loop = true;
            m.prewarm = false;
            m.startDelay = 0f;
            m.startLifetime = 0.2f;
            m.startSpeed = 20f;
            m.startSize3D = true;
            m.startSizeX = 2f;
            m.startSizeY = 0.5f;
            m.startSizeZ = 2f;
            m.startRotation3D = true;
            m.startRotationX = new ParticleSystem.MinMaxCurve(0f, Mathf.PI * 2f);
            m.startRotationY = Mathf.PI / 2f;
            m.startRotationZ = 0f;
            m.flipRotation = 0f;
            m.startColor = Color.white;
            m.gravityModifier = 0f;
            m.simulationSpace = ParticleSystemSimulationSpace.Local;
            m.simulationSpeed = 1f;
            m.useUnscaledTime = false;
            m.playOnAwake = true;
            m.emitterVelocityMode = ParticleSystemEmitterVelocityMode.Transform;
            m.maxParticles = 20000;
            m.stopAction = ParticleSystemStopAction.None;
            m.cullingMode = ParticleSystemCullingMode.AlwaysSimulate;
            m.ringBufferMode = ParticleSystemRingBufferMode.Disabled;
            m.scalingMode = ParticleSystemScalingMode.Local;


            return ps;
        }
        private static ParticleSystem BurnEmission(this ParticleSystem ps)
        {
            var e = ps.emission;
            e.enabled = true;
            e.rateOverTime = 100f;
            e.rateOverDistance = 0f;
            e.burstCount = 0;

            return ps;
        }

        private static ParticleSystem BurnShape(this ParticleSystem ps)
        {
            var s = ps.shape;
            s.enabled = false;
            s.shapeType = ParticleSystemShapeType.SingleSidedEdge;
            s.radius = 0.1f;
            s.radiusMode = ParticleSystemShapeMultiModeValue.Random;
            s.arcSpread = 0f;
            s.texture = null;
            s.position = Vector3.zero;
            s.rotation = Vector3.zero;
            s.scale = Vector3.one;
            s.alignToDirection = false;
            s.randomDirectionAmount = 0f;
            s.randomPositionAmount = 0f;
            s.sphericalDirectionAmount = 0f;


            return ps;
        }

        private static ParticleSystem BurnCOL(this ParticleSystem ps)
        {
            var c = ps.colorOverLifetime;
            c.enabled = true;
            c.color = new ParticleSystem.MinMaxGradient(new Gradient
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
            });

            return ps;
        }

        private static ParticleSystem BurnSOL(this ParticleSystem ps)
        {
            var s = ps.sizeOverLifetime;

            s.enabled = true;
            s.separateAxes = true;
            s.x = 1f;
            s.y = new ParticleSystem.MinMaxCurve(1f, AnimationCurve.EaseInOut(0f, 1f, 1f, 0f));
            s.z = 1f;

            return ps;
        }


        private static ParticleSystemRenderer BurnRenderer(this ParticleSystemRenderer psr)
        {
            psr.renderMode = ParticleSystemRenderMode.Mesh;
            psr.mesh = AssetsCore.LoadAsset<Mesh>(MeshIndex.Quad);
            psr.material = MaterialModule.GetPlasmaBurnMaterial().material;
            psr.trailMaterial = null;
            psr.sortMode = ParticleSystemSortMode.None;
            psr.sortingFudge = 0;
            psr.alignment = ParticleSystemRenderSpace.Local;
            psr.flip = Vector3.zero;
            psr.enableGPUInstancing = true;
            psr.pivot = Vector3.zero;
            psr.maskInteraction = SpriteMaskInteraction.None;
            psr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            psr.receiveShadows = false;
            psr.motionVectorGenerationMode = MotionVectorGenerationMode.Object;
            psr.sortingLayerID = LayerIndex.defaultLayer.intVal;
            psr.sortingOrder = 0;
            psr.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
            psr.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;

            return psr;
        }
    }
}
