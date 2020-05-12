#if ANCIENTWISP
using Rein.RogueWispPlugin.Helpers;

using ReinCore;

using RoR2;
using RoR2.Projectile;

using UnityEngine;

namespace Rein.RogueWispPlugin
{
    internal partial class Main
    {
        internal static GameObject AW_primaryProjGhost;
        internal static GameObject AW_primaryChargeEffect;
        internal static GameObject AW_primaryExplosionEffect;

        internal static GameObject AW_secondaryInitialPrediction;
        internal static GameObject AW_secondaryInitialExplosion;
        internal static GameObject AW_secondaryPrediction;
        internal static GameObject AW_secondaryExplosion;

        internal static GameObject AW_utilityProjGhost;
        internal static GameObject AW_utilityZoneGhost;
        internal static GameObject AW_utilityChargeEffect;
        internal static GameObject AW_utilityOrbEffect;

        partial void AW_CreateEffects()
        {
            this.Load += this.AW_PrimaryProjGhost;
            this.Load += this.AW_PrimaryChargeEffect;
            this.Load += this.AW_PrimaryExplosionEffect;

            this.Load += this.AW_SecondaryPredictionEffect;
            this.Load += this.AW_SecondaryExplosion;
            this.Load += this.AW_SecondaryInitialPredictionEffect;
            this.Load += this.AW_SecondaryInitialExplosion;

            this.Load += this.AW_UtilityProjGhost;
            this.Load += this.AW_UtilityZoneGhost;
            this.Load += this.AW_UtilityChargeEffect;
            this.Load += this.AW_UtilityOrbEffect;
        }

        private void AW_UtilityOrbEffect()
        {
            var obj = PrefabsCore.CreatePrefab("LeechOrb", false);

            var effComp = obj.AddComponent<EffectComponent>();
            effComp.positionAtReferencedTransform = false;
            effComp.parentToReferencedTransform = false;
            effComp.applyScale = true;
            effComp.soundName = "Play_gravekeeper_attack1_fire";

            var skin = obj.AddComponent<WispSkinnedEffect>();


            var vfxAtrib = obj.AddComponent<VFXAttributes>();
            vfxAtrib.vfxPriority = VFXAttributes.VFXPriority.Always;
            vfxAtrib.vfxIntensity = VFXAttributes.VFXIntensity.Low;

            var eventFuncs = obj.AddComponent<EventFunctions>();

            var orb = obj.AddComponent<WispOrbEffect>();
            orb.startVelocity1 = new Vector3( -10f, 10f, -10f );
            orb.startVelocity2 = new Vector3( 10f, 0f, 10f );
            orb.endVelocity1 = new Vector3( -4f, 0f, -4f );
            orb.endVelocity2 = new Vector3( 4f, 0f, 10f );
            orb.movementCurve = AnimationCurve.EaseInOut( 0f, 0f, 1f, 1f );
            orb.faceMovement = true;
            orb.callArrivalIfTargetIsGone = false;
            orb.soundString = "Play_treeBot_m1_hit_heal";


            var fireParticles = EffectHelper.AddFire( obj, skin, MaterialType.Flames, 2f, 0.3f, 20f, 10f, 0f, true );



            AW_utilityOrbEffect = obj;
            RegisterEffect( AW_utilityOrbEffect );
        }

        private void AW_UtilityChargeEffect()
        {
            var obj = PrefabsCore.CreatePrefab( "UtilityChargeEffect", false );

            var skinner = obj.AddComponent<WispSkinnedEffect>();

            var light = EffectHelper.AddLight( obj, skinner, true, 4f, 2f );

            var chargeLines = EffectHelper.AddChargeSphereLines( obj, skinner, MaterialType.Tracer, 0.75f, 0.15f, 0.05f, 30f );

            var arcCircle = EffectHelper.AddArcaneCircle( obj, skinner, MaterialType.ArcaneCircle, 5f, 2f );
            var arcMain = arcCircle.main;
            arcMain.simulationSpace = ParticleSystemSimulationSpace.Local;
            arcMain.emitterVelocityMode = ParticleSystemEmitterVelocityMode.Transform;
            var arcEmis = arcCircle.emission;
            arcEmis.burstCount = 1;
            arcEmis.SetBurst( 0, new ParticleSystem.Burst( 0f, 1f, 1, 0.01f ) );
            var arcShape = arcCircle.shape;
            arcShape.enabled = false;
            var arcCOL = arcCircle.colorOverLifetime;
            arcCOL.enabled = true;
            arcCOL.color = new ParticleSystem.MinMaxGradient( new Gradient
            {
                mode = GradientMode.Blend,
                alphaKeys = new[]
                {
                    new GradientAlphaKey(0f, 0f ),
                    new GradientAlphaKey(1f, 1f ),
                },
                colorKeys = new[]
                {
                    new GradientColorKey(Color.white, 0f ),
                    new GradientColorKey(Color.white, 0f ),
                },
            } );
            var arcSOL = arcCircle.sizeOverLifetime;
            arcSOL.enabled = true;
            arcSOL.separateAxes = false;
            arcSOL.size = new ParticleSystem.MinMaxCurve( 1f, AnimationCurve.Linear( 0f, 1f, 1f, 0f ) );
            var arcROL = arcCircle.rotationOverLifetime;
            arcROL.enabled = true;
            arcROL.z = 2f;




            AW_utilityChargeEffect = obj;
        }

        private void AW_UtilityZoneGhost()
        {
            var obj = PrefabsCore.CreatePrefab( "PrimaryProjGhost", false );

            var sound = obj.AddComponent<StartEndSound>();
            sound.startSound = "Play_lemurianBruiser_m2_loop";
            sound.endSound = "Stop_lemurianBruiser_m2_loop";
            sound.mult = 5;
            //sound.startSound = "Play_magmaWorm_idle_burn_loop";
            //sound.endSound = "Stop_magmaWorm_idle_burn_loop";

            var ghostControl = obj.AddComponent<ProjectileGhostController>();
            var skinner = obj.AddComponent<WispSkinnedEffect>();

            var vecScale = obj.AddComponent<EffectVectorScale>();
            vecScale.scaleOverTime = true;
            vecScale.durationFrac = 1f;
            vecScale.duration = 10f;
            vecScale.startScale = new Vector3( 1f, 1f, 1f );
            vecScale.endScale = new Vector3( 150f, 150f, 150f );
            vecScale.useEffectComponent = false;

            //var light = EffectHelper.AddLight( obj, skinner, true, 8f, 4f );

            //var trail1 = EffectHelper.AddTrail( obj, skinner, MaterialType.Tracer, 0.5f, 1f, 0f, 0.5f, false );
            //var trail2 = EffectHelper.AddTrail( obj, skinner, MaterialType.Tracer, 0.5f, 1f, 0f, 0.5f, false );

            //var rotator = EffectHelper.AddRotator( obj, new Vector3( 0f, 0f, 360f ), Vector3.forward, 1f, trail1.transform, trail2.transform );

            //var flame = EffectHelper.AddFire( obj, skinner, MaterialType.Flames, 10f, 0.3f, 5f, 1f, 0f, true );

            //var tornado = EffectHelper.AddFlameTornado( obj, skinner, MaterialType.FlameTornado, 1f , 10f, 10f, 5f );

            var indicator = EffectHelper.AddMeshIndicator( obj, skinner, MaterialType.BossAreaExplosion, MeshIndex.Sphere, false );

            AW_utilityZoneGhost = obj;
        }

        private void AW_UtilityProjGhost()
        {
            var obj = PrefabsCore.CreatePrefab( "PrimaryProjGhost", false );

            var ghostControl = obj.AddComponent<ProjectileGhostController>();
            var skinner = obj.AddComponent<WispSkinnedEffect>();

            var sound = obj.AddComponent<StartEndSound>();
            sound.startSound = "Play_lemurianBruiser_m1_fly_loop";
            sound.endSound = "Stop_lemurianBruiser_m1_fly_loop";
            sound.mult = 2;
            //Play_greater_wisp_active_loop
            //Stop_greater_wisp_active_loop
            var light = EffectHelper.AddLight( obj, skinner, true, 8f, 4f );

            //var trail1 = EffectHelper.AddTrail( obj, skinner, MaterialType.Tracer, 0.5f, 1f, 0f, 0.5f, false );
            //var trail2 = EffectHelper.AddTrail( obj, skinner, MaterialType.Tracer, 0.5f, 1f, 0f, 0.5f, false );

            //var rotator = EffectHelper.AddRotator( obj, new Vector3( 0f, 0f, 360f ), Vector3.forward, 1f, trail1.transform, trail2.transform );

            var flame = EffectHelper.AddFire( obj, skinner, MaterialType.Flames, 10f, 0.3f, 5f, 1f, 0f, true );

            AW_utilityProjGhost = obj;
        }

        private void AW_SecondaryInitialExplosion()
        {
            var obj = PrefabsCore.CreatePrefab( "SecondaryExplosionEffect", false );
            var effComp = obj.AddComponent<EffectComponent>();
            effComp.positionAtReferencedTransform = false;
            effComp.parentToReferencedTransform = false;
            effComp.applyScale = false;
            effComp.soundName = "";

            var sound = obj.AddComponent<StartEndSound>();
            sound.startSound = "Play_item_proc_laserTurbine_explode";
            sound.mult = 3;



            var skin = obj.AddComponent<WispSkinnedEffect>();

            var vecScale = obj.AddComponent<EffectVectorScale>();
            vecScale.effectComp = effComp;
            vecScale.applyX = true;
            vecScale.applyY = false;
            vecScale.applyZ = true;
            vecScale.durationFrac = 0f;
            vecScale.scaleOverTime = false;

            var vfxAtrib = obj.AddComponent<VFXAttributes>();
            vfxAtrib.vfxPriority = VFXAttributes.VFXPriority.Always;
            vfxAtrib.vfxIntensity = VFXAttributes.VFXIntensity.Low;

            var timer = obj.AddComponent<DestroyOnTimer>();
            timer.duration = 1f;

            var flash1 = EffectHelper.AddFlash( obj, skin, MaterialType.Tracer );
            var flash1Main = flash1.main;
            flash1Main.scalingMode = ParticleSystemScalingMode.Local;


            var pillar = EffectHelper.AddFlamePillar( obj, skin, MaterialType.FlamePillar, 100f, 3f, 0.75f  );


            var sparks = EffectHelper.AddSparks( obj, skin, MaterialType.Tracer, 10000, 0.15f, 1.2f );
            var sparkMain = sparks.main;
            sparkMain.scalingMode = ParticleSystemScalingMode.Shape;
            sparkMain.maxParticles = 10000;
            var sparkShape = sparks.shape;
            sparkShape.enabled = true;
            sparkShape.shapeType = ParticleSystemShapeType.ConeVolume;
            sparkShape.angle = 0f;
            sparkShape.radius = 2f;
            sparkShape.length = 1000f;
            sparkShape.rotation = new Vector3( -90f, 0f, 0f );



            var flashLines = EffectHelper.AddFlashLines( obj, skin, MaterialType.Tracer, 10, 0.2f );
            var flashLinesMain = flashLines.main;
            flashLinesMain.scalingMode = ParticleSystemScalingMode.Shape;
            var flashLineShape = flashLines.shape;
            flashLineShape.enabled = true;
            flashLineShape.shapeType = ParticleSystemShapeType.Cone;
            flashLineShape.radius = 1f;
            flashLineShape.length = 5f;
            flashLineShape.angle = 30f;
            flashLineShape.rotation = new Vector3( -90f, 0f, 0f );




            //var explosion = EffectHelper.AddExplosion( obj, skin, MaterialType.Explosion, 20, 0.3f, 5, 5f );
            //var explShape = explosion.shape;
            //explShape.enabled = true;
            //explShape.shapeType = ParticleSystemShapeType.Hemisphere;
            //explShape.radius = 0.5f;
            //explShape.rotation = new Vector3( -90f, 0f, 0f );


            //var light = EffectHelper.AddLight( obj, skin, true, 20f, 100f );
            //light.transform.localPosition += new Vector3( 0f, 3f, 0f );
            //EffectHelper.EditLightOverTime( light, 2f, AnimationCurve.EaseInOut( 0f, 1f, 1f, 0f ), AnimationCurve.EaseInOut( 0f, 1f, 1f, 0f ) );


            //var distortion = EffectHelper.AddDistortion( obj, skin, MaterialType.Distortion, 8f, 0.3f, 0f );

            AW_secondaryInitialExplosion = obj;
            RegisterEffect( AW_secondaryInitialExplosion );
        }

        private void AW_SecondaryInitialPredictionEffect()
        {
            var obj = PrefabsCore.CreatePrefab( "SecondaryPredictionEffect", false );

            var effComp = obj.AddComponent<EffectComponent>();
            effComp.positionAtReferencedTransform = false;
            effComp.parentToReferencedTransform = false;
            effComp.applyScale = false;
            effComp.soundName = "";

            var skinner = obj.AddComponent<WispSkinnedEffect>();

            var vecScale = obj.AddComponent<EffectVectorScale>();
            vecScale.effectComp = effComp;
            vecScale.applyX = true;
            vecScale.applyY = false;
            vecScale.applyZ = true;
            vecScale.scaleOverTime = true;
            vecScale.durationFrac = 1f;
            vecScale.reverse = true;

            var vfxAtrib = obj.AddComponent<VFXAttributes>();
            vfxAtrib.vfxPriority = VFXAttributes.VFXPriority.Always;
            vfxAtrib.vfxIntensity = VFXAttributes.VFXIntensity.Low;

            var timer = obj.AddComponent<DestroyOnEffectTimer>();
            timer.effectComp = effComp;

            var indicator = EffectHelper.AddMeshIndicator( obj, skinner, MaterialType.BossAreaIndicator, MeshIndex.Cylinder, false, false, 0.5f, 1f, true, false, true );
            indicator.transform.localScale = new Vector3( 1f, 1000f, 1f );
            indicator.transform.localPosition = new Vector3( 0f, 995f, 0f );
            //var indPS = indicator.GetComponent<ParticleSystem>();
            //timer.AddLifetimeParticle( indPS );
            //var indPSMain = indPS.main;
            //indPSMain.maxParticles = 100;
            //var indPSCOL = indPS.colorOverLifetime;
            //indPSCOL.enabled = false;
            //indPSCOL.color = new ParticleSystem.MinMaxGradient( new Gradient
            //{
            //    mode = GradientMode.Blend,
            //    alphaKeys = new[]
            //    {
            //        new GradientAlphaKey( 0f, 0f ),
            //        new GradientAlphaKey( 1f, 0.1f ),
            //        new GradientAlphaKey( 1f, 1f ),
            //    },
            //    colorKeys = new[]
            //    {
            //        new GradientColorKey( Color.white, 0f ),
            //        new GradientColorKey( Color.white, 1f ),
            //    },
            //} );






            AW_secondaryInitialPrediction = obj;
            RegisterEffect( AW_secondaryInitialPrediction );
        }



        private void AW_SecondaryExplosion()
        {
            var obj = PrefabsCore.CreatePrefab( "SecondaryExplosionEffect", false );
            var effComp = obj.AddComponent<EffectComponent>();
            effComp.positionAtReferencedTransform = false;
            effComp.parentToReferencedTransform = false;
            effComp.applyScale = false;
            effComp.soundName = "";

            var skin = obj.AddComponent<WispSkinnedEffect>();

            var vecScale = obj.AddComponent<EffectVectorScale>();
            vecScale.effectComp = effComp;
            vecScale.applyX = true;
            vecScale.applyY = false;
            vecScale.applyZ = true;
            vecScale.durationFrac = 0f;
            vecScale.scaleOverTime = false;

            var vfxAtrib = obj.AddComponent<VFXAttributes>();
            vfxAtrib.vfxPriority = VFXAttributes.VFXPriority.Always;
            vfxAtrib.vfxIntensity = VFXAttributes.VFXIntensity.Low;

            var timer = obj.AddComponent<DestroyOnTimer>();
            timer.duration = 1f;

            var sounds = obj.AddComponent<EffectSoundPlayer>();
            sounds.AddSound( new SoundEvent( 0f, "Play_gravekeeper_attack2_shoot", 1f ) );


            var flash1 = EffectHelper.AddFlash( obj, skin, MaterialType.Tracer );
            var flash1Main = flash1.main;
            flash1Main.scalingMode = ParticleSystemScalingMode.Local;


            var pillar = EffectHelper.AddFlamePillar( obj, skin, MaterialType.FlamePillar, 1000f, 3f, 0.75f  );


            var sparks = EffectHelper.AddSparks( obj, skin, MaterialType.Tracer, 10000, 0.15f, 1.2f );
            var sparkMain = sparks.main;
            sparkMain.scalingMode = ParticleSystemScalingMode.Shape;
            sparkMain.maxParticles = 10000;
            var sparkShape = sparks.shape;
            sparkShape.enabled = true;
            sparkShape.shapeType = ParticleSystemShapeType.ConeVolume;
            sparkShape.angle = 0f;
            sparkShape.radius = 2f;
            sparkShape.length = 1000f;
            sparkShape.rotation = new Vector3( -90f, 0f, 0f );



            var flashLines = EffectHelper.AddFlashLines( obj, skin, MaterialType.Tracer, 10, 0.2f );
            var flashLinesMain = flashLines.main;
            flashLinesMain.scalingMode = ParticleSystemScalingMode.Shape;
            var flashLineShape = flashLines.shape;
            flashLineShape.enabled = true;
            flashLineShape.shapeType = ParticleSystemShapeType.Cone;
            flashLineShape.radius = 1f;
            flashLineShape.length = 5f;
            flashLineShape.angle = 30f;
            flashLineShape.rotation = new Vector3( -90f, 0f, 0f );




            //var explosion = EffectHelper.AddExplosion( obj, skin, MaterialType.Explosion, 20, 0.3f, 5, 5f );
            //var explShape = explosion.shape;
            //explShape.enabled = true;
            //explShape.shapeType = ParticleSystemShapeType.Hemisphere;
            //explShape.radius = 0.5f;
            //explShape.rotation = new Vector3( -90f, 0f, 0f );


            var light = EffectHelper.AddLight( obj, skin, true, 20f, 100f );
            light.transform.localPosition += new Vector3( 0f, 3f, 0f );
            EffectHelper.EditLightOverTime( light, 2f, AnimationCurve.EaseInOut( 0f, 1f, 1f, 0f ), AnimationCurve.EaseInOut( 0f, 1f, 1f, 0f ) );


            var distortion = EffectHelper.AddDistortion( obj, skin, MaterialType.Distortion, 8f, 0.3f, 0f );

            AW_secondaryExplosion = obj;
            RegisterEffect( AW_secondaryExplosion );
        }

        private void AW_SecondaryPredictionEffect()
        {
            var obj = PrefabsCore.CreatePrefab( "SecondaryPredictionEffect", false );

            var effComp = obj.AddComponent<EffectComponent>();
            effComp.positionAtReferencedTransform = false;
            effComp.parentToReferencedTransform = false;
            effComp.applyScale = false;
            effComp.soundName = "Play_elite_antiHeal_turret_die";

            var sound = obj.AddComponent<StartEndSound>();
            sound.startSound = "Play_elite_antiHeal_turret_die";
            sound.mult = 2;

            var skinner = obj.AddComponent<WispSkinnedEffect>();

            var vecScale = obj.AddComponent<EffectVectorScale>();
            vecScale.effectComp = effComp;
            vecScale.applyX = true;
            vecScale.applyY = false;
            vecScale.applyZ = true;
            vecScale.scaleOverTime = false;
            vecScale.durationFrac = 0.1f;

            var vfxAtrib = obj.AddComponent<VFXAttributes>();
            vfxAtrib.vfxPriority = VFXAttributes.VFXPriority.Always;
            vfxAtrib.vfxIntensity = VFXAttributes.VFXIntensity.Low;

            var timer = obj.AddComponent<DestroyOnEffectTimer>();
            timer.effectComp = effComp;

            var indicator = EffectHelper.AddMeshIndicator( obj, skinner, MaterialType.BossAreaIndicator, MeshIndex.Cylinder, false, false, 0.5f, 1f, true, false, true );
            indicator.transform.localScale = new Vector3( 1f, 1000f, 1f );
            indicator.transform.localPosition = new Vector3( 0f, 995f, 0f );
            //var indPS = indicator.GetComponent<ParticleSystem>();
            //timer.AddLifetimeParticle( indPS );
            //var indPSMain = indPS.main;
            //indPSMain.maxParticles = 100;
            //var indPSCOL = indPS.colorOverLifetime;
            //indPSCOL.enabled = false;
            //indPSCOL.color = new ParticleSystem.MinMaxGradient( new Gradient
            //{
            //    mode = GradientMode.Blend,
            //    alphaKeys = new[]
            //    {
            //        new GradientAlphaKey( 0f, 0f ),
            //        new GradientAlphaKey( 1f, 0.1f ),
            //        new GradientAlphaKey( 1f, 1f ),
            //    },
            //    colorKeys = new[]
            //    {
            //        new GradientColorKey( Color.white, 0f ),
            //        new GradientColorKey( Color.white, 1f ),
            //    },
            //} );






            AW_secondaryPrediction = obj;
            RegisterEffect( AW_secondaryPrediction );
        }

        private void AW_PrimaryExplosionEffect()
        {
            var obj = PrefabsCore.CreatePrefab( "PrimaryExplosionEffect", false );
            var effComp = obj.AddComponent<EffectComponent>();
            effComp.positionAtReferencedTransform = false;
            effComp.parentToReferencedTransform = false;
            effComp.applyScale = true;
            effComp.soundName = "Play_item_use_meteor_impact";

            var vfxAttrib = obj.AddComponent<VFXAttributes>();
            vfxAttrib.vfxPriority = VFXAttributes.VFXPriority.Always;
            vfxAttrib.vfxIntensity = VFXAttributes.VFXIntensity.Low;

            var skinner = obj.AddComponent<WispSkinnedEffect>();

            var flash = EffectHelper.AddFlash( obj, skinner, MaterialType.Tracer, 2f );

            var flashLines = EffectHelper.AddFlashLines( obj, skinner, MaterialType.Tracer, 30, 0.05f, 0.05f, 1f );
            var flashLineShape = flashLines.shape;
            flashLineShape.enabled = true;
            flashLineShape.shapeType = ParticleSystemShapeType.Cone;
            flashLineShape.radius = 0.01f;
            flashLineShape.length = 1f;
            flashLineShape.angle = 60f;
            flashLineShape.rotation = new Vector3( -90f, 0f, 0f );
            flashLineShape.radiusThickness = 0.5f;

            var distortion = EffectHelper.AddDistortion( obj, skinner, MaterialType.DistortionHeavy, 2f, 0.25f, 0f );
            var distortionEmis = distortion.emission;
            distortionEmis.enabled = true;
            distortionEmis.burstCount = 1;
            distortionEmis.SetBurst( 0, new ParticleSystem.Burst( 0f, 1f, 1, 0.1f ) );
            var distortionSOL = distortion.sizeOverLifetime;
            distortionSOL.enabled = true;
            distortionSOL.size = new ParticleSystem.MinMaxCurve( 1f, AnimationCurve.Linear( 0f, 0f, 1f, 1f ) );

            var explosion =  EffectHelper.AddExplosion( obj, skinner, MaterialType.Explosion, 15, 0.25f, 5f, 1f );
            var explMain = explosion.main;
            explMain.gravityModifier = 0.5f;
            var explShape = explosion.shape;
            explShape.enabled = true;
            explShape.shapeType = ParticleSystemShapeType.Cone;
            explShape.angle = 60f;
            explShape.radius = 0.25f;
            explShape.rotation = new Vector3( -90f, 0f, 0f );


            var light = EffectHelper.AddLight( obj, skinner, true, 10f, 10f );
            light.transform.localPosition += new Vector3( 0f, 0.5f, 0f );
            EffectHelper.EditLightOverTime( light, 0.5f, AnimationCurve.EaseInOut( 0f, 1f, 1f, 0f ), AnimationCurve.EaseInOut( 0f, 1f, 1f, 0f ) );




            AW_primaryExplosionEffect = obj;
            RegisterEffect( AW_primaryExplosionEffect );
        }

        private void AW_PrimaryChargeEffect()
        {
            var obj = PrefabsCore.CreatePrefab( "PrimaryChargeEffect", false );

            var skinner = obj.AddComponent<WispSkinnedEffect>();

            var light = EffectHelper.AddLight( obj, skinner, true, 4f, 2f );

            var chargeLines = EffectHelper.AddChargeSphereLines( obj, skinner, MaterialType.Tracer, 0.75f, 0.15f, 0.05f, 30f );

            var arcCircle = EffectHelper.AddArcaneCircle( obj, skinner, MaterialType.ArcaneCircle, 5f, 2f );
            var arcMain = arcCircle.main;
            arcMain.simulationSpace = ParticleSystemSimulationSpace.Local;
            arcMain.emitterVelocityMode = ParticleSystemEmitterVelocityMode.Transform;
            var arcEmis = arcCircle.emission;
            arcEmis.burstCount = 1;
            arcEmis.SetBurst( 0, new ParticleSystem.Burst( 0f, 1f, 1, 0.01f ) );
            var arcShape = arcCircle.shape;
            arcShape.enabled = false;
            var arcCOL = arcCircle.colorOverLifetime;
            arcCOL.enabled = true;
            arcCOL.color = new ParticleSystem.MinMaxGradient( new Gradient
            {
                mode = GradientMode.Blend,
                alphaKeys = new[]
                {
                    new GradientAlphaKey(0f, 0f ),
                    new GradientAlphaKey(1f, 1f ),
                },
                colorKeys = new[]
                {
                    new GradientColorKey(Color.white, 0f ),
                    new GradientColorKey(Color.white, 0f ),
                },
            } );
            var arcSOL = arcCircle.sizeOverLifetime;
            arcSOL.enabled = true;
            arcSOL.separateAxes = false;
            arcSOL.size = new ParticleSystem.MinMaxCurve( 1f, AnimationCurve.Linear( 0f, 1f, 1f, 0f ) );
            var arcROL = arcCircle.rotationOverLifetime;
            arcROL.enabled = true;
            arcROL.z = 2f;




            AW_primaryChargeEffect = obj;
        }

        private void AW_PrimaryProjGhost()
        {
            var obj = PrefabsCore.CreatePrefab( "PrimaryProjGhost", false );

            var ghostControl = obj.AddComponent<ProjectileGhostController>();
            var skinner = obj.AddComponent<WispSkinnedEffect>();

            var light = EffectHelper.AddLight( obj, skinner, true, 8f, 4f );

            var trail1 = EffectHelper.AddTrail( obj, skinner, MaterialType.Tracer, 0.5f, 1f, 0f, 0.5f, false );
            var trail2 = EffectHelper.AddTrail( obj, skinner, MaterialType.Tracer, 0.5f, 1f, 0f, 0.5f, false );

            var rotator = EffectHelper.AddRotator( obj, new Vector3( 0f, 0f, 360f ), Vector3.forward, 1f, trail1.transform, trail2.transform );

            var flame = EffectHelper.AddFire( obj, skinner, MaterialType.Flames, 4f, 0.3f, 5f, 1f, 0f, true );

            AW_primaryProjGhost = obj;
        }

    }
}
#endif
