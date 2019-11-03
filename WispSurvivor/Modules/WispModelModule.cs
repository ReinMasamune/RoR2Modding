using RoR2;
using UnityEngine;
using System;
using System.Collections.Generic;
using static WispSurvivor.Util.PrefabUtilities;

namespace WispSurvivor.Modules
{
    public static class WispModelModule
    {
        private const uint matIndex = 0;

        public static void DoModule( GameObject body , Dictionary<Type,Component> dic)
        {
            EditModelScale(body, dic);
            EditModelStructure(body, dic);
            EditModelParticles(body, dic);
            EditModelSkins(body, dic);
        }

        private static void EditModelScale( GameObject body , Dictionary<Type,Component> dic )
        {
            dic.C<ModelLocator>().modelBaseTransform.gameObject.name = "ModelBase";
            dic.C<ModelLocator>().modelTransform.localScale = new Vector3(1f, 1f, 1f);
            dic.C<ModelLocator>().modelTransform.localPosition = new Vector3(0f, 4f, 0f);
        }

        private static void EditModelStructure( GameObject body, Dictionary<Type,Component> dic )
        {
            Transform modelTransform = dic.C<ModelLocator>().modelTransform;
            GameObject pivotPoint = new GameObject("CannonPivot");
            pivotPoint.transform.parent = modelTransform;
            pivotPoint.transform.localPosition = new Vector3(0f, 2f, 0f);
            pivotPoint.transform.localEulerAngles = new Vector3( -90f, 0f, 0f );

            Transform armatureTransform = modelTransform.Find("AncientWispArmature");
            armatureTransform.parent = pivotPoint.transform;
        }

        private static void EditModelParticles( GameObject body, Dictionary<Type,Component> dic )
        {
            Transform modelTransform = dic.C<ModelLocator>().modelTransform;
            CharacterModel bodyCharModel = modelTransform.GetComponent<CharacterModel>();
            MonoBehaviour.Destroy(bodyCharModel.baseLightInfos[0].light.gameObject);
            MonoBehaviour.Destroy(bodyCharModel.baseLightInfos[1].light.gameObject);
            MonoBehaviour.Destroy(bodyCharModel.baseParticleSystemInfos[0].particleSystem.gameObject);
            MonoBehaviour.Destroy(bodyCharModel.baseParticleSystemInfos[1].particleSystem.gameObject);
            MonoBehaviour.Destroy(bodyCharModel.gameObject.GetComponent<AncientWispFireController>());
            Array.Resize<CharacterModel.LightInfo>(ref bodyCharModel.baseLightInfos, 0);

            Components.WispFlamesController flameCont = dic.C<Components.WispFlamesController>();
            flameCont.passive = dic.C<Components.WispPassiveController>();

            string tempName;
            ParticleSystem tempPS;
            ParticleSystemRenderer tempPSR;

            Dictionary<string, FlamePSInfo> flames = CreateFlameDictionary();
            List<PSCont> tempPSList = new List<PSCont>();

            foreach( Transform t in modelTransform.GetComponentsInChildren<Transform>() )
            {
                if (!t) continue;
                tempName = t.gameObject.name;

                if( tempName == "GameObject" )
                {
                    MonoBehaviour.Destroy(t.gameObject);
                    continue;
                }

                if (flames.ContainsKey(tempName))
                {
                    tempPS = t.gameObject.AddOrGetComponent<ParticleSystem>();
                    tempPSR = t.gameObject.AddOrGetComponent<ParticleSystemRenderer>();
                    tempPS.SetupFlameParticleSystem(0, flames[tempName]);
                    tempPSList.Add(new PSCont
                    {
                        ps = tempPS,
                        psr = tempPSR,
                        info = flames[tempName]
                    });
                }
            }

            Array.Resize<CharacterModel.ParticleSystemInfo>(ref bodyCharModel.baseParticleSystemInfos, tempPSList.Count);

            for( int i = 0; i < tempPSList.Count; i++ )
            {
                bodyCharModel.baseParticleSystemInfos[i] = new CharacterModel.ParticleSystemInfo
                { 
                    particleSystem = tempPSList[i].ps,
                    renderer = tempPSList[i].psr,
                    defaultMaterial = WispMaterialModule.fireMaterials[0][0]
                };
                flameCont.flames.Add(tempPSList[i].ps);
                flameCont.flameInfos.Add(tempPSList[i].info.rate);
            }
        }

        private static void EditModelSkins( GameObject body, Dictionary<Type,Component> dic)
        {
            GameObject bodyModel = dic.C<ModelLocator>().modelTransform.gameObject;
            CharacterModel bodyCharModel = bodyModel.GetComponent<CharacterModel>();
            ModelSkinController bodySkins = bodyModel.AddOrGetComponent<ModelSkinController>();

            Renderer armorRenderer = bodyCharModel.baseRendererInfos[0].renderer;
            Material armorMaterial = bodyCharModel.baseRendererInfos[0].defaultMaterial;
            

            CharacterModel.ParticleSystemInfo[] particles = bodyCharModel.baseParticleSystemInfos;
            CharacterModel.RendererInfo[][] rendererInfos = new CharacterModel.RendererInfo[8][];
            for ( int i = 0; i < 8; i++ )
            {
                rendererInfos[i] = new CharacterModel.RendererInfo[particles.Length + 1];
                for (int j = 0; j < particles.Length; j++)
                {
                    rendererInfos[i][j] = CreateFlameRendererInfo( particles[j].renderer, WispMaterialModule.fireMaterials[i][matIndex] );
                }
                // TODO: Array of armor mats should be reffed here and used
                rendererInfos[i][particles.Length] = new CharacterModel.RendererInfo
                {
                    renderer = armorRenderer,
                    defaultMaterial = armorMaterial,
                    defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                    ignoreOverlays = false
                };
            }

            string[] skinNames = new string[8];
            skinNames[0] = "WISP_SURVIVOR_SKIN_1";
            skinNames[1] = "WISP_SURVIVOR_SKIN_2";
            skinNames[2] = "WISP_SURVIVOR_SKIN_3";
            skinNames[3] = "WISP_SURVIVOR_SKIN_4";
            skinNames[4] = "WISP_SURVIVOR_SKIN_5";
            skinNames[5] = "WISP_SURVIVOR_SKIN_6";
            skinNames[6] = "WISP_SURVIVOR_SKIN_7";
            skinNames[7] = "WISP_SURVIVOR_SKIN_8";

            SkinDef[] skins = new SkinDef[8];

            for( int i = 0; i < 8; i++ )
            {
                SkinDefInfo skinInfo = new SkinDefInfo
                {
                    baseSkins = Array.Empty<SkinDef>(),
                    icon = Resources.Load<Sprite>("NotAPath"),
                    nameToken = skinNames[i],
                    name = skinNames[i],
                    unlockableName = "",
                    rootObject = bodyModel,
                    rendererInfos = rendererInfos[i]
                };
                skins[i] = CreateNewSkinDef(skinInfo);
            }

            bodySkins.skins = skins;
        }

        private static CharacterModel.RendererInfo CreateFlameRendererInfo( Renderer r , Material m )
        {
            return CreateRendererInfo(r, m, true, UnityEngine.Rendering.ShadowCastingMode.On);
        }

        private static void EditParticles( ParticleSystem ps )
        {

            Color[] colors = new Color[2];
            colors[0] = new Color(1f, 1f, 1f);
            colors[1] = new Color(1f, 1f, 1f);

            float[] alphas = new float[4];
            alphas[0] = 0f;
            alphas[1] = 0.9f;
            alphas[2] = 0.65f;
            alphas[3] = 0f;

            var flameMain1 = ps.main;
            flameMain1.startSize = 10f;
            flameMain1.gravityModifier = -0.35f;
            flameMain1.emitterVelocityMode = ParticleSystemEmitterVelocityMode.Rigidbody;
            flameMain1.cullingMode = ParticleSystemCullingMode.PauseAndCatchup;
            flameMain1.startLifetime = 0.95f;

            var flameShape1 = ps.shape;
            flameShape1.scale = new Vector3(0.4f, 0.4f, 0.4f);
            flameShape1.position = new Vector3(0f, -0.1f, -0.35f);

            var flameSOL1 = ps.sizeOverLifetime;
            flameSOL1.sizeMultiplier = 0.3f;

            var flameCOL1 = ps.colorOverLifetime;
            var newColorKeys = new GradientColorKey[flameCOL1.color.gradient.colorKeys.Length];
            var newAlphaKeys = new GradientAlphaKey[flameCOL1.color.gradient.alphaKeys.Length];
            var newGrad = new Gradient();
            var newGradP2 = new ParticleSystem.MinMaxGradient();
            for (int i = 0; i < flameCOL1.color.gradient.colorKeys.Length; i++)
            {
                newColorKeys[i].time = (float)i;
                if (colors.Length == newColorKeys.Length)
                {
                    newColorKeys[i].color = colors[i];
                }
                else
                {
                    newColorKeys[i].color = flameCOL1.color.gradient.colorKeys[i].color;
                }
            }
            for (int i = 0; i < flameCOL1.color.gradient.alphaKeys.Length; i++)
            {
                newAlphaKeys[i].time = flameCOL1.color.gradient.alphaKeys[i].time;
                if (alphas.Length == newAlphaKeys.Length)
                {
                    newAlphaKeys[i].alpha = alphas[i];
                }
                else
                {
                    newAlphaKeys[i].alpha = flameCOL1.color.gradient.alphaKeys[i].alpha;
                }
            }
            newGrad.SetKeys(newColorKeys, newAlphaKeys);

            newGradP2.gradient = newGrad;
            newGradP2.mode = ParticleSystemGradientMode.Gradient;

            flameCOL1.color = newGradP2;

            var flameEmis1 = ps.emission;
            flameEmis1.rateOverDistance = new ParticleSystem.MinMaxCurve
            {
                constant = 0f,
                mode = ParticleSystemCurveMode.Constant
            };
            flameEmis1.rateOverTime = new ParticleSystem.MinMaxCurve
            {
                constant = 100f,
                mode = ParticleSystemCurveMode.Constant
            };

        }

        public struct PSCont
        {
            public ParticleSystem ps;
            public ParticleSystemRenderer psr;
            public FlamePSInfo info;
        }

        public struct FlamePSInfo
        {
            public int matIndex;

            public float startSpeed;
            public float startSize;
            public float gravity;
            public float rate;
            public float radius;

            public Vector3 position;
            public Vector3 rotation;
            public Vector3 scale;
        }

        public static Dictionary<string,FlamePSInfo> CreateFlameDictionary()
        {
            Dictionary<string, FlamePSInfo> flames = new Dictionary<string, FlamePSInfo>();
            flames.Add("Head", new FlamePSInfo
            {
                matIndex = 0,
                startSpeed = 1f,
                startSize = 1f,
                gravity = -0.3f,
                rate = 10f,
                radius = 1f,
                position = new Vector3(0f, 0.15f, 0f),
                rotation = new Vector3(180f, 0f, 0f),
                scale = new Vector3(0.1f, 0.1f, 0.1f)
            });

            flames.Add("ChestCannon1", new FlamePSInfo
            {
                matIndex = 0,
                startSpeed = 1f,
                startSize = 1f,
                gravity = -0.2f,
                rate = 10f,
                radius = 1f,
                position = new Vector3(0f, 1f, 0f),
                rotation = new Vector3(90f, 0f, 0f),
                scale = new Vector3(0.25f, 0.2f, 0.6f)
            });
            flames.Add("ChestCannon2", new FlamePSInfo
            {
                matIndex = 0,
                startSpeed = 1f,
                startSize = 1f,
                gravity = -0.2f,
                rate = 10f,
                radius = 1f,
                position = new Vector3(0f, 1f, 0f),
                rotation = new Vector3(90f, 0f, 0f),
                scale = new Vector3(0.25f, 0.2f, 0.6f)
            });

            flames.Add("upperArm1.l", new FlamePSInfo
            {
                matIndex = 0,
                startSpeed = 1f,
                startSize = 2f,
                gravity = -0.2f,
                rate = 8f,
                radius = 1f,
                position = new Vector3(0f, 0.4f, 0f),
                rotation = new Vector3(90f, 0f, 0f),
                scale = new Vector3(0.15f, 0.15f, 0.5f)
            });
            flames.Add("upperArm1.r", new FlamePSInfo
            {
                matIndex = 0,
                startSpeed = 1f,
                startSize = 2f,
                gravity = -0.2f,
                rate = 8f,
                radius = 1f,
                position = new Vector3(0f, 0.4f, 0f),
                rotation = new Vector3(90f, 0f, 0f),
                scale = new Vector3(0.15f, 0.15f, 0.5f)
            });

            flames.Add("MuzzleLeft", new FlamePSInfo
            {
                matIndex = 0,
                startSpeed = 1f,
                startSize = 2f,
                gravity = -0.2f,
                rate = 6f,
                radius = 1f,
                position = new Vector3(0f, 0f, 0.1f),
                rotation = new Vector3(180f, 0f, 0f),
                scale = new Vector3(0.1f, 0.1f, 0.5f)
            });
            flames.Add("MuzzleRight", new FlamePSInfo
            {
                matIndex = 0,
                startSpeed = 1f,
                startSize = 2f,
                gravity = -0.2f,
                rate = 6f,
                radius = 1f,
                position = new Vector3(0f, 0f, 0f),
                rotation = new Vector3(180f, 0f, 0f),
                scale = new Vector3(0.1f, 0.1f, 0.5f)
            });

            flames.Add("calf.l", new FlamePSInfo
            {
                matIndex = 0,
                startSpeed = 1f,
                startSize = 2f,
                gravity = -0.3f,
                rate = 10f,
                radius = 1f,
                position = new Vector3(0f, 0.6f, 0f),
                rotation = new Vector3(90f, 0f, 0f),
                scale = new Vector3(0.1f, 0.1f, 0.5f)
            });
            flames.Add("calf.r", new FlamePSInfo
            {
                matIndex = 0,
                startSpeed = 1f,
                startSize = 2f,
                gravity = -0.3f,
                rate = 10f,
                radius = 1f,
                position = new Vector3(0f, 0.6f, 0f),
                rotation = new Vector3(90f, 0f, 0f),
                scale = new Vector3(0.1f, 0.1f, 0.5f)
            });

            return flames;
        }

        private static void SetupFlameParticleSystem(this ParticleSystem ps, int skinIndex, FlamePSInfo psi )
        {
            var main = ps.main;
            main.duration = 1f;
            main.loop = true;
            main.prewarm = false;
            main.startDelay = 0f;
            main.startLifetime = new ParticleSystem.MinMaxCurve
            {
                mode = ParticleSystemCurveMode.Constant,
                constant = 0.8f
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
                constant = psi.startSize
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
                color = new Color(1f, 1f, 1f, 1f)
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

            var emission = ps.emission;
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

            var shape = ps.shape;
            shape.enabled = true;
            shape.shapeType = ParticleSystemShapeType.Cone;
            shape.angle = 38.26f;
            shape.radius = psi.radius;
            shape.radiusThickness = 1f;
            shape.arc = 360f;
            shape.arcMode = ParticleSystemShapeMultiModeValue.Random;
            shape.arcSpread = 0f;
            shape.position = psi.position;
            shape.rotation = psi.rotation;
            shape.scale = psi.scale;
            shape.alignToDirection = false;

            var velOverLife = ps.velocityOverLifetime;
            velOverLife.enabled = false;

            var limVelOverLife = ps.limitVelocityOverLifetime;
            limVelOverLife.enabled = false;

            var inheritVel = ps.inheritVelocity;
            inheritVel.enabled = false;

            var forceOverLife = ps.forceOverLifetime;
            forceOverLife.enabled = false;

            var colorOverLife = ps.colorOverLifetime;
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

            var colorBySpeed = ps.colorBySpeed;
            colorBySpeed.enabled = false;

            var sizeOverLife = ps.sizeOverLifetime;
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

            var sizeBySpeed = ps.sizeBySpeed;
            sizeBySpeed.enabled = false;

            var rotOverLife = ps.rotationOverLifetime;
            rotOverLife.enabled = true;
            rotOverLife.separateAxes = false;
            rotOverLife.z = new ParticleSystem.MinMaxCurve
            {
                mode = ParticleSystemCurveMode.Constant,
                constant = 3f
            };

            var rotBySpeed = ps.rotationBySpeed;
            rotBySpeed.enabled = false;

            var extForce = ps.externalForces;
            extForce.enabled = false;

            var noise = ps.noise;
            noise.enabled = false;

            var col = ps.collision;
            col.enabled = false;

            var trig = ps.trigger;
            trig.enabled = false;

            var subEmit = ps.subEmitters;
            subEmit.enabled = false;

            var texSheet = ps.textureSheetAnimation;
            texSheet.enabled = false;

            var light = ps.lights;
            light.enabled = false;

            var trails = ps.trails;
            trails.enabled = false;

            var custData = ps.customData;
            custData.enabled = false;
        }

        private static T C<T>( this Dictionary<Type,Component> dic ) where T : Component
        {
            return dic[typeof(T)] as T;
        }
    }
}
