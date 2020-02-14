using RogueWispPlugin.Helpers;
using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;
//using static RogueWispPlugin.Helpers.APIInterface;

namespace RogueWispPlugin
{
#if ROGUEWISP
    internal partial class Main
    {
        partial void RW_EditModel()
        {
            this.Load += this.RW_EditModelScale;
            this.Load += this.RW_EditModelStructure;
            this.Load += this.RW_EditModelFlare;
            //this.Load += this.RW_EditModelParticles;
        }

        private void RW_EditModelScale()
        {
            ModelLocator modelLocator = this.RW_body.GetComponent<ModelLocator>();
            modelLocator.modelBaseTransform.gameObject.name = "ModelBase";
            modelLocator.modelBaseTransform.localPosition = new Vector3( 0f, 0.0f, 0f );
            modelLocator.modelTransform.localScale = new Vector3( 0.7f, 0.7f, 0.7f );
            modelLocator.modelTransform.localPosition = new Vector3( 0f, 0.2f, 0f );
        }

        private void RW_EditModelStructure()
        {

            Transform modelTransform = this.RW_body.GetComponent<ModelLocator>().modelTransform;
            GameObject pivotPoint = new GameObject("CannonPivot");
            pivotPoint.transform.parent = modelTransform;
            pivotPoint.transform.localPosition = new Vector3( 0f, 1.5f, 0f );
            pivotPoint.transform.localEulerAngles = new Vector3( -90f, 0f, 0f );

            Transform armatureTransform = modelTransform.Find("AncientWispArmature");
            armatureTransform.parent = pivotPoint.transform;

            GameObject beamParent = new GameObject("BeamParent");
            beamParent.transform.parent = pivotPoint.transform;
            beamParent.transform.localPosition = new Vector3( 0f, 0.5f, -1f );
            beamParent.transform.localEulerAngles = new Vector3( 0f, 0f, 0f );
        }

        private void RW_EditModelFlare()
        {
            GameObject flareObj = this.RW_body.GetComponent<ModelLocator>().modelTransform.Find("CannonPivot/AncientWispArmature/Head/GameObject").gameObject;
            flareObj.transform.localScale = Vector3.one;

            GameObject flare2Obj = MonoBehaviour.Instantiate<GameObject>(flareObj, flareObj.transform.parent);
            flare2Obj.transform.localEulerAngles = new Vector3( 0f, 180f, 0f );
            GameObject refObj = new GameObject("FlareRef");
            refObj.transform.parent = flareObj.transform.parent;
            refObj.transform.localPosition = Vector3.zero;
            refObj.transform.localEulerAngles = new Vector3( 0f, 180f, 0f );

            flare2Obj.GetComponent<EyeFlare>().directionSource = refObj.transform;

            WispFlareController flareController = this.RW_body.GetComponent<WispFlareController>();
            flareController.flare1 = flareObj.GetComponent<SpriteRenderer>();
            flareController.flare2 = flare2Obj.GetComponent<SpriteRenderer>();

            flareObj.SetActive( false );
            flare2Obj.SetActive( false );
        }

        private void RW_EditModelParticles()
        {
            /*
            Transform modelTransform = this.RW_body.GetComponent<ModelLocator>().modelTransform;
            CharacterModel bodyCharModel = modelTransform.GetComponent<CharacterModel>();
            //MonoBehaviour.Destroy( bodyCharModel.baseLightInfos[0].light.gameObject );
            //MonoBehaviour.Destroy( bodyCharModel.baseLightInfos[1].light.gameObject );
            //MonoBehaviour.Destroy( bodyCharModel.baseParticleSystemInfos[0].particleSystem.gameObject );
            //MonoBehaviour.Destroy( bodyCharModel.baseParticleSystemInfos[1].particleSystem.gameObject );
            //MonoBehaviour.Destroy( bodyCharModel.gameObject.GetComponent<AncientWispFireController>() );
            
            //Array.Resize<CharacterModel.LightInfo>( ref bodyCharModel.baseLightInfos, 0 );
            //bodyCharModel.baseLightInfos = null;
            //bodyCharModel.baseRendererInfos = null;
            WispFlamesController flameCont =this.RW_body.GetComponent<WispFlamesController>();
            //flameCont.passive = this.RW_body.GetComponent<WispPassiveController>();

            String tempName;
            ParticleSystem tempPS;
            ParticleSystemRenderer tempPSR;

            Dictionary<String, FlamePSInfo> flames = CreateFlameDictionary();
            List<PSCont> tempPSList = new List<PSCont>();

            var bitSkinCont = modelTransform.AddOrGetComponent<WispModelBitSkinController>();

            var skinned = modelTransform.GetComponentInChildren<SkinnedMeshRenderer>();
            //skinned.material = WispBitSkin.armorMain_placeholder;
            //bitSkinCont.RegisterRenderer( skinned );


            foreach( Transform t in modelTransform.GetComponentsInChildren<Transform>() )
            {
                if( !t ) continue;
                tempName = t.gameObject.name;

                if( flames.ContainsKey( tempName ) )
                {
                    tempPS = t.gameObject.AddOrGetComponent<ParticleSystem>();
                    tempPSR = t.gameObject.AddOrGetComponent<ParticleSystemRenderer>();
                    this.SetupFlameParticleSystem( tempPS, 0, flames[tempName] );

                    tempPSList.Add( new PSCont
                    {
                        ps = tempPS,
                        psr = tempPSR,
                        info = flames[tempName]
                    } );
                }
            }

            //Array.Resize<CharacterModel.ParticleSystemInfo>( ref bodyCharModel.baseParticleSystemInfos, 0 );
            bodyCharModel.baseParticleSystemInfos = null;
            
            for( Int32 i = 0; i < tempPSList.Count; i++ )
            {
                /*bodyCharModel.baseParticleSystemInfos[i] = new CharacterModel.ParticleSystemInfo
                {
                    particleSystem = tempPSList[i].ps,
                    renderer = tempPSList[i].psr,
                    defaultMaterial = Main.fireMaterials[0][0]
                };
                //tempPSList[i].psr.material = WispBitSkin.flameMain_placeholder;
                //bitSkinCont.RegisterRenderer( tempPSList[i].psr );
                flameCont.flames.Add( tempPSList[i].ps );
                flameCont.flameInfos.Add( tempPSList[i].info.rate );
            }
            */
        }

        public struct PSCont
        {
            public ParticleSystem ps;
            public ParticleSystemRenderer psr;
            public FlamePSInfo info;
        }

        public struct FlamePSInfo
        {
            public Int32 matIndex;

            public Single startSpeed;
            public Single startSize;
            public Single gravity;
            public Single rate;
            public Single radius;

            public Vector3 position;
            public Vector3 rotation;
            public Vector3 scale;
        }

        public static Dictionary<String, FlamePSInfo> CreateFlameDictionary()
        {
            Dictionary<String, FlamePSInfo> flames = new Dictionary<String, FlamePSInfo>();
            flames.Add( "Head", new FlamePSInfo
            {
                matIndex = 0,
                startSpeed = 0.5f,
                startSize = 1f,
                gravity = -0.15f,
                rate = 10f,
                radius = 1f,
                position = new Vector3( 0f, 0.15f, 0f ),
                rotation = new Vector3( 180f, 0f, 0f ),
                scale = new Vector3( 0.1f, 0.1f, 0.1f )
            } );

            flames.Add( "ChestCannon1", new FlamePSInfo
            {
                matIndex = 0,
                startSpeed = 1f,
                startSize = 1f,
                gravity = -0.2f,
                rate = 10f,
                radius = 1f,
                position = new Vector3( 0f, 1f, 0f ),
                rotation = new Vector3( 90f, 0f, 0f ),
                scale = new Vector3( 0.25f, 0.2f, 0.6f )
            } );
            flames.Add( "ChestCannon2", new FlamePSInfo
            {
                matIndex = 0,
                startSpeed = 1f,
                startSize = 1f,
                gravity = -0.2f,
                rate = 10f,
                radius = 1f,
                position = new Vector3( 0f, 1f, 0f ),
                rotation = new Vector3( 90f, 0f, 0f ),
                scale = new Vector3( 0.25f, 0.2f, 0.6f )
            } );

            flames.Add( "upperArm1.l", new FlamePSInfo
            {
                matIndex = 0,
                startSpeed = 1f,
                startSize = 1.5f,
                gravity = -0.2f,
                rate = 8f,
                radius = 1f,
                position = new Vector3( 0f, 0.4f, 0f ),
                rotation = new Vector3( 90f, 0f, 0f ),
                scale = new Vector3( 0.15f, 0.15f, 0.5f )
            } );
            flames.Add( "upperArm1.r", new FlamePSInfo
            {
                matIndex = 0,
                startSpeed = 1f,
                startSize = 1.5f,
                gravity = -0.2f,
                rate = 8f,
                radius = 1f,
                position = new Vector3( 0f, 0.4f, 0f ),
                rotation = new Vector3( 90f, 0f, 0f ),
                scale = new Vector3( 0.15f, 0.15f, 0.5f )
            } );

            flames.Add( "MuzzleLeft", new FlamePSInfo
            {
                matIndex = 0,
                startSpeed = 1f,
                startSize = 1.5f,
                gravity = -0.2f,
                rate = 6f,
                radius = 1f,
                position = new Vector3( 0f, 0f, 0.1f ),
                rotation = new Vector3( 180f, 0f, 0f ),
                scale = new Vector3( 0.1f, 0.1f, 0.5f )
            } );
            flames.Add( "MuzzleRight", new FlamePSInfo
            {
                matIndex = 0,
                startSpeed = 1f,
                startSize = 1.5f,
                gravity = -0.2f,
                rate = 6f,
                radius = 1f,
                position = new Vector3( 0f, 0f, 0f ),
                rotation = new Vector3( 180f, 0f, 0f ),
                scale = new Vector3( 0.1f, 0.1f, 0.5f )
            } );

            flames.Add( "calf.l", new FlamePSInfo
            {
                matIndex = 0,
                startSpeed = 1f,
                startSize = 1.5f,
                gravity = -0.3f,
                rate = 10f,
                radius = 1f,
                position = new Vector3( 0f, 0.6f, 0f ),
                rotation = new Vector3( 90f, 0f, 0f ),
                scale = new Vector3( 0.1f, 0.1f, 0.5f )
            } );
            flames.Add( "calf.r", new FlamePSInfo
            {
                matIndex = 0,
                startSpeed = 1f,
                startSize = 1.5f,
                gravity = -0.3f,
                rate = 10f,
                radius = 1f,
                position = new Vector3( 0f, 0.6f, 0f ),
                rotation = new Vector3( 90f, 0f, 0f ),
                scale = new Vector3( 0.1f, 0.1f, 0.5f )
            } );

            return flames;
        }

        private void SetupFlameParticleSystem( ParticleSystem ps, Int32 skinIndex, FlamePSInfo psi )
        {
            ParticleSystem.MainModule main = ps.main;
            main.duration = 1f;
            main.loop = true;
            main.prewarm = false;
            main.startDelay = 0f;
            main.startLifetime = new ParticleSystem.MinMaxCurve
            {
                mode = ParticleSystemCurveMode.Constant,
                constant = 0.65f
            };
            main.startSpeed = new ParticleSystem.MinMaxCurve
            {
                mode = ParticleSystemCurveMode.Constant,
                constant = psi.startSpeed
            };
            main.startSize3D = false;
            main.startSize = new ParticleSystem.MinMaxCurve
            {
                mode = ParticleSystemCurveMode.Constant,
                constant = psi.startSize * 0.75f
            };
            main.startRotation3D = false;
            main.startRotation = new ParticleSystem.MinMaxCurve
            {
                mode = ParticleSystemCurveMode.TwoConstants,
                constantMin = 0f,
                constantMax = 360f
            };
            main.flipRotation = 0f;
            main.startColor = new ParticleSystem.MinMaxGradient
            {
                mode = ParticleSystemGradientMode.Color,
                color = new Color( 1f, 1f, 1f, 1f )
            };
            main.gravityModifier = new ParticleSystem.MinMaxCurve
            {
                mode = ParticleSystemCurveMode.Constant,
                constant = psi.gravity
            };
            main.simulationSpace = ParticleSystemSimulationSpace.Local;
            main.simulationSpeed = 1f;
            main.useUnscaledTime = false;
            main.scalingMode = ParticleSystemScalingMode.Local;
            main.playOnAwake = true;
            main.emitterVelocityMode = ParticleSystemEmitterVelocityMode.Transform;
            main.maxParticles = 1000;
            main.stopAction = ParticleSystemStopAction.None;
            main.cullingMode = ParticleSystemCullingMode.AlwaysSimulate;
            main.ringBufferMode = ParticleSystemRingBufferMode.Disabled;

            ParticleSystem.EmissionModule emission = ps.emission;
            emission.enabled = true;
            emission.rateOverTime = new ParticleSystem.MinMaxCurve
            {
                mode = ParticleSystemCurveMode.Constant,
                constant = 10f
            };
            emission.rateOverDistance = new ParticleSystem.MinMaxCurve
            {
                mode = ParticleSystemCurveMode.Constant,
                constant = 0f
            };
            emission.rateOverDistanceMultiplier = 0f;
            emission.rateOverTimeMultiplier = psi.rate;

            ParticleSystem.ShapeModule shape = ps.shape;
            shape.enabled = true;
            shape.shapeType = ParticleSystemShapeType.Cone;
            shape.angle = 38.26f;
            shape.radius = psi.radius * 0.75f;
            shape.radiusThickness = 1f;
            shape.arc = 360f;
            shape.arcMode = ParticleSystemShapeMultiModeValue.Random;
            shape.arcSpread = 0f;
            shape.position = psi.position;
            shape.rotation = psi.rotation;
            shape.scale = psi.scale;
            shape.alignToDirection = false;

            ParticleSystem.VelocityOverLifetimeModule velOverLife = ps.velocityOverLifetime;
            velOverLife.enabled = false;

            ParticleSystem.LimitVelocityOverLifetimeModule limVelOverLife = ps.limitVelocityOverLifetime;
            limVelOverLife.enabled = false;

            ParticleSystem.InheritVelocityModule inheritVel = ps.inheritVelocity;
            inheritVel.enabled = false;

            ParticleSystem.ForceOverLifetimeModule forceOverLife = ps.forceOverLifetime;
            forceOverLife.enabled = false;

            ParticleSystem.ColorOverLifetimeModule colorOverLife = ps.colorOverLifetime;
            colorOverLife.enabled = true;
            colorOverLife.color = new ParticleSystem.MinMaxGradient
            {
                mode = ParticleSystemGradientMode.Gradient,
                gradient = new Gradient
                {
                    mode = GradientMode.Blend,
                    colorKeys = new GradientColorKey[1]
                    {
                        new GradientColorKey( new Color( 1f, 1f, 1f ) , 0f )
                    },
                    alphaKeys = new GradientAlphaKey[4]
                    {
                        new GradientAlphaKey( 0f, 0f ),
                        new GradientAlphaKey( 0.9f, 0.1f ),
                        new GradientAlphaKey(0.6f, 0.6f ),
                        new GradientAlphaKey( 0f, 1f )
                    }
                }
            };

            ParticleSystem.ColorBySpeedModule colorBySpeed = ps.colorBySpeed;
            colorBySpeed.enabled = false;

            ParticleSystem.SizeOverLifetimeModule sizeOverLife = ps.sizeOverLifetime;
            sizeOverLife.enabled = true;
            sizeOverLife.size = new ParticleSystem.MinMaxCurve
            {
                mode = ParticleSystemCurveMode.Curve,
                curve = new AnimationCurve
                {
                    postWrapMode = WrapMode.Clamp,
                    preWrapMode = WrapMode.Clamp,
                    keys = new Keyframe[3]
                    {
                        new Keyframe( 0f, 0.2f),
                        new Keyframe( 0.47f, 0.71f),
                        new Keyframe( 1f, 0.025f )
                    }
                }
            };
            sizeOverLife.sizeMultiplier = 1f;

            ParticleSystem.SizeBySpeedModule sizeBySpeed = ps.sizeBySpeed;
            sizeBySpeed.enabled = false;

            ParticleSystem.RotationOverLifetimeModule rotOverLife = ps.rotationOverLifetime;
            rotOverLife.enabled = true;
            rotOverLife.separateAxes = false;
            rotOverLife.z = new ParticleSystem.MinMaxCurve
            {
                mode = ParticleSystemCurveMode.Constant,
                constant = 3f
            };

            ParticleSystem.RotationBySpeedModule rotBySpeed = ps.rotationBySpeed;
            rotBySpeed.enabled = false;

            ParticleSystem.ExternalForcesModule extForce = ps.externalForces;
            extForce.enabled = false;

            ParticleSystem.NoiseModule noise = ps.noise;
            noise.enabled = false;

            ParticleSystem.CollisionModule col = ps.collision;
            col.enabled = false;

            ParticleSystem.TriggerModule trig = ps.trigger;
            trig.enabled = false;

            ParticleSystem.SubEmittersModule subEmit = ps.subEmitters;
            subEmit.enabled = false;

            ParticleSystem.TextureSheetAnimationModule texSheet = ps.textureSheetAnimation;
            texSheet.enabled = false;

            ParticleSystem.LightsModule light = ps.lights;
            light.enabled = false;

            ParticleSystem.TrailModule trails = ps.trails;
            trails.enabled = false;

            ParticleSystem.CustomDataModule custData = ps.customData;
            custData.enabled = false;
        }
    }
#endif
}
