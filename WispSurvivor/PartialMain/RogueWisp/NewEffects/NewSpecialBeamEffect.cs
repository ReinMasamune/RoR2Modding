#if ROGUEWISP
using R2API;
using R2API.Utils;
using RogueWispPlugin.Helpers;
using RoR2;
using System;
using UnityEngine;
//using static RogueWispPlugin.Helpers.APIInterface;

namespace RogueWispPlugin
{

    internal partial class Main
    {
        partial void RW_NewSpecialBeamEffect()
        {
            this.Load += this.RW_CreateNewSpecialBeam;
        }

        private void RW_CreateNewSpecialBeam()
        {
            var obj = new GameObject().InstantiateClone( "SpecialBeamEffect", false );

            var skin = obj.AddComponent<WispSkinnedEffect>();

            var end = new GameObject("End").transform;
            end.parent = obj.transform;
            end.localPosition = Vector3.zero;
            end.localScale = Vector3.one;
            end.localRotation = Quaternion.identity;

            var beam = EffectHelper.SetupBeam( obj, end, 5f, 2.6f );

            var beamSparks = beam.AddParticles( EffectHelper.AddSparks( beam.gameObject, skin, MaterialType.Tracer, -10, 0.05f, 1f ), true, true );
            var bsShape = beamSparks.shape;
            bsShape.enabled = true;
            bsShape.shapeType = ParticleSystemShapeType.ConeVolume;
            bsShape.angle = 0f;
            bsShape.radius = 0.25f;
            bsShape.length = 1f;
            bsShape.position = new Vector3( 0f, 0f, 0f );

            var bsMain = beamSparks.main;
            bsMain.loop = true;
            bsMain.scalingMode = ParticleSystemScalingMode.Shape;
            bsMain.simulationSpace = ParticleSystemSimulationSpace.World;
            bsMain.maxParticles = 10000;
            bsMain.startSpeed = 0f;

            var bsNoise = beamSparks.noise;
            bsNoise.quality = ParticleSystemNoiseQuality.High;
            bsNoise.scrollSpeed = 10f;



            var beamLine = beam.AddBeamLine( skin, MaterialType.Beam, 0.2f );



            specialBeam = obj;
        }

        /*
        partial void RW_SpecialBeamEffects() => this.FirstFrame += this.RW_CreateSpecialBeamEffects;

        private void RW_CreateSpecialBeamEffects()
        {
            GameObject baseFX = ( EntityStates.EntityState.Instantiate(new EntityStates.SerializableEntityStateType(typeof(EntityStates.TitanMonster.FireMegaLaser))) as EntityStates.TitanMonster.FireMegaLaser ).GetFieldValue<GameObject>("laserPrefab");

            for( Int32 i = 0; i < 8; i++ )
            {
                specialBeams[i] = CreateSpecialBeam( baseFX, i );
            }
        }

        private static GameObject CreateSpecialBeam( GameObject baseFX, Int32 skinIndex )
        {
            GameObject g = baseFX.InstantiateClone("WispSpecialBeam", false);

            Transform partSysT = g.transform.Find("Particle System");
            Transform flare = g.transform.Find("Start").Find("Flare");
            Transform arcFlare = g.transform.Find("Start").Find("ArcaneFlare");
            Transform partPar = g.transform.Find("End").Find("EndEffect").Find("Particles");
            Transform debris = partPar.Find("Debris");
            Transform fire = partPar.Find("Fire");
            Transform fireEl = partPar.Find("Fire, Electric");
            Transform sparks = partPar.Find("Sparks,Wiggly");
            Transform light = partPar.Find("Point light");
            Transform glob = partPar.Find("Glob");
            Transform post = g.transform.Find("End").Find("EndEffect").Find("PostProcess");
            Transform bez = g.transform.Find("BezierHolder");
            Transform end = g.transform.Find("End");

            //GameObject bez2 = MonoBehaviour.Instantiate<GameObject>( bez.gameObject , bez.transform.parent );

            Rewired.ComponentControls.Effects.RotateAroundAxis rot1 = bez.GetComponent<Rewired.ComponentControls.Effects.RotateAroundAxis>();
            //var rot2 = bez2.GetComponent<Rewired.ComponentControls.Effects.RotateAroundAxis>();
            rot1.slowRotationSpeed = 120f;
            rot1.fastRotationSpeed = 80f;
            //rot2.slowRotationSpeed = 60f;
            //rot2.fastRotationSpeed = 40f;
            //rot2.reverse = true;


            foreach( DetachParticleOnDestroyAndEndEmission thing in g.GetComponentsInChildren<DetachParticleOnDestroyAndEndEmission>() )
            {
                //MonoBehaviour.Destroy(thing);
            }

            GameObject rings = MonoBehaviour.Instantiate<GameObject>(partSysT.gameObject,partSysT.parent);

            //None of these particles are worth the time.
            debris.gameObject.SetActive( false );
            fire.gameObject.SetActive( false );
            fireEl.gameObject.SetActive( false );
            sparks.gameObject.SetActive( false );
            glob.gameObject.SetActive( false );

            light.GetComponent<Light>().color = fireColors[skinIndex];

            LineRenderer mainLaser = g.GetComponent<LineRenderer>();
            LineRenderer[] subLasers = g.transform.Find("BezierHolder").GetComponentsInChildren<LineRenderer>();

            //MainLaser stuff
            Material beamMat = MonoBehaviour.Instantiate<Material>(mainLaser.material);
            beamMat.SetTexture( "_RemapTex", fireTextures[skinIndex] );
            mainLaser.material = beamMat;

            mainLaser.widthMultiplier = 3f;

            foreach( LineRenderer line in subLasers )
            {
                //Sub laser stuff
                line.material = beamMat;
                line.widthMultiplier *= 1.25f;
            }


            partSysT.localPosition = new Vector3( 0f, 0f, 0f );
            flare.localPosition = new Vector3( 0f, 0f, -0.2f );
            arcFlare.localPosition = new Vector3( 0f, 0f, -3f );
            rings.transform.localPosition = new Vector3( 0f, 0f, 0f );


            ParticleSystem ps1 = partSysT.GetComponent<ParticleSystem>();
            ParticleSystemRenderer psr1 = partSysT.GetComponent<ParticleSystemRenderer>();

            Material tempMat = MonoBehaviour.Instantiate<Material>(psr1.material);
            tempMat.SetTexture( "_RemapTex", electricTextures[skinIndex] );
            psr1.material = tempMat;

            ParticleSystem.MainModule ps1main = ps1.main;
            ps1main.flipRotation = 0.5f;
            ps1main.startLifetimeMultiplier = 0.5f;


            ParticleSystemRenderer psr2 = rings.GetComponent<ParticleSystemRenderer>();
            psr2.material = tempMat;

            ParticleSystem ps2 = rings.GetComponent<ParticleSystem>();

            ParticleSystem.MainModule ps2main = ps2.main;
            ps2main.flipRotation = 0.5f;
            ps2main.startLifetimeMultiplier = 1f;
            ps2main.startSpeed = 100f;


            ParticleSystem flarePs = flare.GetComponent<ParticleSystem>();
            ParticleSystemRenderer flarePsr = flare.GetComponent<ParticleSystemRenderer>();

            flarePsr.material = fireMaterials[skinIndex][9];

            ParticleSystem.MainModule flarepsmain = flarePs.main;
            flarepsmain.startLifetime = 0.5f;
            flarepsmain.startSize = 1.25f;
            flarepsmain.maxParticles = 100;

            ParticleSystem.EmissionModule flareemis = flarePs.emission;
            flareemis.rateOverTime = 100f;
            flareemis.rateOverDistance = 0f;

            ParticleSystem.ShapeModule flareshape = flarePs.shape;
            flareshape.enabled = true;
            flareshape.shapeType = ParticleSystemShapeType.Sphere;
            flareshape.radius = 0.1f;



            ParticleSystem arcFlarePs = arcFlare.GetComponent<ParticleSystem>();
            ParticleSystemRenderer arcFlarePsr = arcFlare.GetComponent<ParticleSystemRenderer>();

            arcFlarePsr.material = fireMaterials[skinIndex][9];

            ParticleSystem.MainModule arcflarepsmain = arcFlarePs.main;
            arcflarepsmain.startLifetime = 0.5f;
            arcflarepsmain.startSize = 1f;
            arcflarepsmain.maxParticles = 100;
            arcflarepsmain.startColor = new Color( 1f, 1f, 1f, 1f );

            ParticleSystem.EmissionModule arcflareemis = arcFlarePs.emission;
            arcflareemis.rateOverTime = 100f;
            arcflareemis.rateOverDistance = 0f;

            ParticleSystem.ShapeModule arcflareshape = arcFlarePs.shape;
            arcflareshape.enabled = true;
            arcflareshape.shapeType = ParticleSystemShapeType.Sphere;
            arcflareshape.radius = 0.25f;


            Transform endPS = Instantiate<GameObject>( flare.gameObject, end ).transform;
            endPS.gameObject.name = "Sub";
            endPS.parent = end;
            endPS.localPosition = Vector3.zero;
            endPS.localScale = Vector3.one * 3f;
            endPS.localRotation = Quaternion.identity;

            var endPSS = endPS.GetComponent<ParticleSystem>();
            var endPSMain = endPSS.main;
            endPSMain.simulationSpace = ParticleSystemSimulationSpace.World;
            endPSMain.startLifetime = 0.35f;
            endPSMain.gravityModifier = -0.25f;

            var endPSEmis = endPSS.emission;
            endPSEmis.rateOverTime = 300f;

            GameObject endPSDist = new GameObject( "Dist" );
            endPSDist.transform.parent = endPS;
            endPSDist.transform.localPosition = Vector3.zero;
            endPSDist.transform.localScale = Vector3.one * 0.5f;
            endPSDist.transform.localRotation = Quaternion.identity;


            /*
            Material distortion = Resources.Load<GameObject>("Prefabs/Effects/ImpactEffects/LightningStrikeImpact").transform.Find("Distortion").GetComponent<ParticleSystemRenderer>().material;
            //Material distortion = Resources.Load<GameObject>("Prefabs/Effects/ArchWispDeath").transform.Find("InitialBurst").Find("Distortion").GetComponent<ParticleSystemRenderer>().material;


            ParticleSystem distPS = endPSDist.AddComponent<ParticleSystem>();
            ParticleSystemRenderer distPSR = endPSDist.AddOrGetComponent<ParticleSystemRenderer>();

            BasicSetup( distPS );

            ParticleSystem.MainModule distPSMain = distPS.main;
            distPSMain.duration = 1f;
            distPSMain.loop = true;
            distPSMain.prewarm = true;
            distPSMain.startDelay = 0f;
            distPSMain.startLifetime = 0.25f;
            distPSMain.startSpeed = 0f;
            distPSMain.startSize = 2f;
            distPSMain.startRotation = 0f;
            distPSMain.flipRotation = 0.5f;
            distPSMain.gravityModifier = 0f;
            distPSMain.simulationSpace = ParticleSystemSimulationSpace.World;
            distPSMain.simulationSpeed = 1f;
            distPSMain.useUnscaledTime = false;
            distPSMain.scalingMode = ParticleSystemScalingMode.Hierarchy;
            distPSMain.playOnAwake = true;
            distPSMain.emitterVelocityMode = ParticleSystemEmitterVelocityMode.Rigidbody;
            distPSMain.maxParticles = 1000;
            distPSMain.stopAction = ParticleSystemStopAction.None;
            distPSMain.cullingMode = ParticleSystemCullingMode.AlwaysSimulate;
            distPSMain.ringBufferMode = ParticleSystemRingBufferMode.Disabled;

            ParticleSystem.EmissionModule distPSEmis = distPS.emission;
            distPSEmis.enabled = true;
            distPSEmis.rateOverTime = 50f;
            distPSEmis.rateOverDistance = 0f;

            ParticleSystem.ShapeModule distPSShape = distPS.shape;
            distPSShape.enabled = false;
            distPSShape.shapeType = ParticleSystemShapeType.BoxEdge;
            distPSShape.radius = 0.005f;
            distPSShape.position = Vector3.zero;
            distPSShape.rotation = new Vector3( 0f, 0f, 0f );
            distPSShape.scale = new Vector3( 1f, 1f, 1f );
            distPSShape.alignToDirection = false;
            distPSShape.randomDirectionAmount = 0f;
            distPSShape.sphericalDirectionAmount = 0f;
            distPSShape.randomPositionAmount = 0f;

            ParticleSystem.ColorOverLifetimeModule distPSCOL = distPS.colorOverLifetime;
            distPSCOL.enabled = false;

            ParticleSystem.SizeOverLifetimeModule distPSSOL = distPS.sizeOverLifetime;
            distPSSOL.enabled = true;
            distPSSOL.size = new ParticleSystem.MinMaxCurve
            {
                mode = ParticleSystemCurveMode.Curve,
                curve = new AnimationCurve
                {
                    postWrapMode = WrapMode.Clamp,
                    preWrapMode = WrapMode.Clamp,
                    keys = new Keyframe[4]
                    {
                        new Keyframe
                        {
                            time = 0f,
                            value = 0.1f,
                            outTangent = 0.5f,
                            outWeight = 0.5f
                        },
                        new Keyframe
                        {
                            time = 0.1f,
                            value = 0.4f,
                            outTangent = 0.5f,
                            outWeight = 0.5f,
                            inTangent = 0.5f,
                            inWeight = 0.5f
                        },
                        new Keyframe
                        {
                            time = 0.4f,
                            value = 0.5f,
                            inTangent = 0.5f,
                            inWeight = 0.5f,
                            outTangent = 0.5f,
                            outWeight = 0.5f
                        },
                        new Keyframe
                        {
                            time = 1f,
                            value = 0f,
                            inTangent = 0.5f,
                            inWeight = 0.5f
                        }
                    }
                }
            };
            distPSSOL.separateAxes = false;
            distPSSOL.sizeMultiplier = 1f;

            ParticleSystem.RotationOverLifetimeModule distPSROL = distPS.rotationOverLifetime;
            distPSROL.enabled = true;
            distPSROL.x = new ParticleSystem.MinMaxCurve
            {
                mode = ParticleSystemCurveMode.Constant,
                constant = 2f
            };
            distPSROL.y = new ParticleSystem.MinMaxCurve
            {
                mode = ParticleSystemCurveMode.Constant,
                constant = 2f
            };
            distPSROL.z = new ParticleSystem.MinMaxCurve
            {
                mode = ParticleSystemCurveMode.Constant,
                constant = 2f
            };

            var distPSVOL = distPS.velocityOverLifetime;
            distPSVOL.enabled = false;

            distPSR.renderMode = ParticleSystemRenderMode.Billboard;
            distPSR.normalDirection = 1f;
            distPSR.material = distortion;
            distPSR.sortMode = ParticleSystemSortMode.None;
            distPSR.sortingFudge = 0f;
            distPSR.minParticleSize = 0f;
            distPSR.maxParticleSize = 0.5f;
            distPSR.alignment = ParticleSystemRenderSpace.View;
            distPSR.allowRoll = true;
            distPSR.maskInteraction = SpriteMaskInteraction.None;
            distPSR.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            distPSR.receiveShadows = false;
            distPSR.shadowBias = 0f;
            distPSR.motionVectorGenerationMode = MotionVectorGenerationMode.Object;
            distPSR.sortingLayerID = LayerIndex.defaultLayer.intVal;
            distPSR.sortingOrder = 0;
            distPSR.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.BlendProbes;
            distPSR.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
            */
        /*
        var flarePS = arcFlare.GetComponent<ParticleSystem>();
        var flarePSR = arcFlare.GetComponent<ParticleSystemRenderer>();

        flarePSR.material = WispMaterialModule.fireMaterials[skinIndex][9];

        BasicSetup(flarePS);

        var flareMain = flarePS.main;
        flareMain.duration = 5f;
        flareMain.loop = true;
        flareMain.startLifetime = 0.5f;
        flareMain.startSpeed = 0f;
        flareMain.startSize = 3f;
        flareMain.startColor = new Color(1f, 1f, 1f, 1f);
        flareMain.gravityModifier = 0f;
        flareMain.cullingMode = ParticleSystemCullingMode.AlwaysSimulate;

        var flareEmis = flarePS.emission;
        flareEmis.enabled = true;
        flareEmis.rateOverTime = 100f;
        flareEmis.rateOverDistance = 0f;

        var flareShape = flarePS.shape;
        flareShape.enabled = true;
        flareShape.shapeType = ParticleSystemShapeType.Sphere;
        flareShape.radius = 1f;

        var flareCOL = flarePS.colorOverLifetime;
        flareCOL.enabled = true;
        flareCOL.color = new ParticleSystem.MinMaxGradient
        {
            mode = ParticleSystemGradientMode.Gradient,
            gradient = new Gradient
            {
                mode = GradientMode.Blend,
                alphaKeys = new GradientAlphaKey[2]
                {
                    new GradientAlphaKey( 1f, 0f ),
                    new GradientAlphaKey( 0f, 1f )
                },
                colorKeys = new GradientColorKey[1]
                {
                    new GradientColorKey( new Color( 1f, 1f, 1f) , 0f )
                }
            }
        };

        var flareSOL = flarePS.sizeOverLifetime;
        flareSOL.enabled = true;
        flareSOL.size = new ParticleSystem.MinMaxCurve
        {
            mode = ParticleSystemCurveMode.Curve,
            curve = new AnimationCurve
            {
                postWrapMode = WrapMode.Clamp,
                preWrapMode = WrapMode.Clamp,
                keys = new Keyframe[2]
                {
                    new Keyframe( 0f, 0f ),
                    new Keyframe( 1f, 1f )
                }
            }
        };


        endPS.gameObject.SetActive( false );
        return g;
    }
    */
    }

}
#endif
