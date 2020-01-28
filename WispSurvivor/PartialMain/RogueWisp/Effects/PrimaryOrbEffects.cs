using R2API;
using RoR2;
using System;
using UnityEngine;
//using static RogueWispPlugin.Helpers.APIInterface;

namespace RogueWispPlugin
{
#if ROGUEWISP
    internal partial class Main
    {
        partial void RW_PrimaryOrbEffects() => this.Load += this.RW_CreatePrimaryOrbEffects;

        private void RW_CreatePrimaryOrbEffects()
        {
            GameObject baseFX = Resources.Load<GameObject>("Prefabs/Effects/OrbEffects/WispOrbEffect");

            for( Int32 i = 0; i < 8; i++ )
            {
                primaryOrbEffects[i] = CreatePrimaryOrb( baseFX, i );
            }
        }

        private static GameObject CreatePrimaryOrb( GameObject baseFX, Int32 skinIndex )
        {
            GameObject obj = baseFX.InstantiateClone("PrimaryOrb" + skinIndex.ToString(), false);

            VFXAttributes fx = obj.GetComponent<VFXAttributes>();
            fx.vfxPriority = VFXAttributes.VFXPriority.Always;
            fx.vfxIntensity = VFXAttributes.VFXIntensity.Low;

            MonoBehaviour.DestroyImmediate( obj.GetComponent<RoR2.Orbs.OrbEffect>() );

            WispOrbEffectController orbController = obj.AddComponent<WispOrbEffectController>();
            orbController.startSound = "Play_wisp_active_loop";
            orbController.endSound = "Stop_wisp_active_loop";
            orbController.explosionSound = "Play_item_use_fireballDash_explode";

            foreach( AkEvent ev in obj.GetComponents<AkEvent>() )
            {
                MonoBehaviour.Destroy( ev );
            }


            Material flameMat = fireMaterials[skinIndex][0];
            Color flameCol = fireColors[skinIndex];
            Color.RGBToHSV( flameCol, out Single h, out Single s, out Single v );
            s *= 0.75f;
            v /= 0.75f;
            var coreColor = Color.HSVToRGB( h, s, v);

            var trail = obj.AddComponent<TrailRenderer>();
            trail.material = trailMaterial;
            trail.startColor = coreColor;
            trail.endColor = flameCol;
            trail.textureMode = LineTextureMode.Stretch;
            trail.alignment = LineAlignment.View;
            //WidthCurve
            //trail.colorGradient = fireGradients[skinIndex];
            trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            trail.motionVectorGenerationMode = MotionVectorGenerationMode.Object;
            trail.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
            trail.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
            trail.sortingLayerName = "Default";
            trail.lightProbeProxyVolumeOverride = null;
            trail.probeAnchor = null;
            trail.lightmapScaleOffset = new Vector4( 1f, 1f, 0f, 0f );
            trail.realtimeLightmapScaleOffset = new Vector4( 1f, 1f, 0f, 0f );

            trail.time = 0.25f;
            trail.startWidth = 2f;
            trail.endWidth = 0.05f;
            trail.widthMultiplier = 1.0f;
            trail.minVertexDistance = 0.025f;
            trail.shadowBias = 0f;

            trail.autodestruct = false;
            trail.emitting = true;
            trail.generateLightingData = false;
            trail.receiveShadows = true;
            trail.allowOcclusionWhenDynamic = true;        

            trail.numCornerVertices = 64;
            trail.numCapVertices = 64;
            trail.renderingLayerMask = 1;
            trail.rendererPriority = 0;
            trail.sortingLayerID = 0;
            trail.sortingOrder = 0;
            trail.lightmapIndex = -1;
            trail.realtimeLightmapIndex = -1;
            


            //Material distortion = Resources.Load<GameObject>("Prefabs/Effects/ImpactEffects/LightningStrikeImpact").transform.Find("Distortion").GetComponent<ParticleSystemRenderer>().material;
            Material distortion = Resources.Load<GameObject>("Prefabs/Effects/ArchWispDeath").transform.Find("InitialBurst").Find("Distortion").GetComponent<ParticleSystemRenderer>().material;

            GameObject parts1 = obj.transform.Find("Mesh").gameObject;
            GameObject parts2 = obj.transform.Find("Flames").gameObject;
            GameObject parts3 = new GameObject("Ball");
            parts3.transform.parent = obj.transform;
            parts3.transform.localPosition = Vector3.zero;
            parts3.transform.localScale = Vector3.one;


            parts1.name = "FlameParticles";
            parts2.name = "DistortionParticles";

            Strip( parts1 );
            Strip( parts2 );

            ParticleSystem ps1 = parts1.AddComponent<ParticleSystem>();
            ParticleSystemRenderer psr1 = parts1.AddOrGetComponent<ParticleSystemRenderer>();
            #region Particle System definitions 1
            BasicSetup( ps1 );

            ParticleSystem.MainModule ps1Main = ps1.main;
            ps1Main.duration = 1f;
            ps1Main.loop = true;
            ps1Main.prewarm = false;
            ps1Main.startDelay = 0f;
            ps1Main.startLifetime = 1f;
            ps1Main.startSpeed = 10f;
            ps1Main.startSize = 1.25f;
            ps1Main.startRotation = 0f;
            ps1Main.flipRotation = 0f;
            ps1Main.gravityModifier = 0f;
            ps1Main.simulationSpace = ParticleSystemSimulationSpace.World;
            ps1Main.simulationSpeed = 1f;
            ps1Main.useUnscaledTime = false;
            ps1Main.scalingMode = ParticleSystemScalingMode.Hierarchy;
            ps1Main.playOnAwake = true;
            ps1Main.emitterVelocityMode = ParticleSystemEmitterVelocityMode.Rigidbody;
            ps1Main.maxParticles = 1000;
            ps1Main.stopAction = ParticleSystemStopAction.None;
            ps1Main.cullingMode = ParticleSystemCullingMode.AlwaysSimulate;
            ps1Main.ringBufferMode = ParticleSystemRingBufferMode.Disabled;

            ParticleSystem.EmissionModule ps1Emis = ps1.emission;
            ps1Emis.enabled = true;
            ps1Emis.rateOverTime = new ParticleSystem.MinMaxCurve
            {
                mode = ParticleSystemCurveMode.Constant,
                constant = 0f
            };
            ps1Emis.rateOverDistance = new ParticleSystem.MinMaxCurve
            {
                mode = ParticleSystemCurveMode.Constant,
                constant = 1f
            };

            ParticleSystem.ShapeModule ps1Shape = ps1.shape;
            ps1Shape.enabled = true;
            ps1Shape.shapeType = ParticleSystemShapeType.Donut;
            ps1Shape.radius = 5f;
            ps1Shape.donutRadius = 3f;
            ps1Shape.radiusThickness = 1f;
            ps1Shape.arc = 360f;
            ps1Shape.arcMode = ParticleSystemShapeMultiModeValue.Random;
            ps1Shape.arcSpread = 0f;
            ps1Shape.position = Vector3.zero;
            ps1Shape.rotation = new Vector3( 0f, 90f, 0f );
            ps1Shape.scale = new Vector3( 1f, 1f, 1f );
            ps1Shape.alignToDirection = false;
            ps1Shape.randomDirectionAmount = 0f;
            ps1Shape.sphericalDirectionAmount = 0f;
            ps1Shape.randomPositionAmount = 0f;

            ParticleSystem.ColorOverLifetimeModule ps1COL = ps1.colorOverLifetime;
            ps1COL.enabled = true;
            ps1COL.color = new ParticleSystem.MinMaxGradient
            {
                mode = ParticleSystemGradientMode.Gradient,
                gradient = new Gradient
                {
                    alphaKeys = new GradientAlphaKey[3]
                    {
                        new GradientAlphaKey
                        {
                            time = 0f,
                            alpha = 0f
                        },
                        new GradientAlphaKey
                        {
                            time = 0.075f,
                            alpha = 1f
                        },
                        new GradientAlphaKey
                        {
                            time = 1f,
                            alpha = 0f
                        }
                    },
                    colorKeys = new GradientColorKey[3]
                    {
                        new GradientColorKey
                        {
                            time = 0f,
                            color = new Color
                            {
                                r = 5f,
                                g = 5f,
                                b = 5f
                            }
                        },
                        new GradientColorKey
                        {
                            time = 0.08f,
                            color = new Color
                            {
                                r = 1f,
                                g = 1f,
                                b = 1f
                            }
                        },
                        new GradientColorKey
                        {
                            time = 1f,
                            color = new Color
                            {
                                r = 0.7f,
                                g = 0.7f,
                                b = 0.7f
                            }
                        }
                    }
                }
            };

            ParticleSystem.SizeOverLifetimeModule ps1SOL = ps1.sizeOverLifetime;
            ps1SOL.enabled = true;
            ps1SOL.size = new ParticleSystem.MinMaxCurve
            {
                mode = ParticleSystemCurveMode.Curve,
                curve = new AnimationCurve
                {
                    postWrapMode = WrapMode.Clamp,
                    preWrapMode = WrapMode.Clamp,
                    keys = new Keyframe[3]
                    {
                        new Keyframe
                        {
                            time = 0f,
                            value = 0.42f,
                            outTangent = 0.5f,
                            outWeight = 0.5f
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
            ps1SOL.separateAxes = false;
            ps1SOL.sizeMultiplier = 1f;

            ParticleSystem.RotationOverLifetimeModule ps1ROL = ps1.rotationOverLifetime;
            ps1ROL.enabled = true;
            ps1ROL.x = new ParticleSystem.MinMaxCurve
            {
                mode = ParticleSystemCurveMode.Constant,
                constant = 89.99f
            };
            ps1ROL.y = new ParticleSystem.MinMaxCurve
            {
                mode = ParticleSystemCurveMode.Constant,
                constant = 89.99f
            };
            ps1ROL.z = new ParticleSystem.MinMaxCurve
            {
                mode = ParticleSystemCurveMode.Constant,
                constant = 89.99f
            };

            psr1.renderMode = ParticleSystemRenderMode.Billboard;
            psr1.normalDirection = 1f;
            psr1.material = flameMat;
            psr1.sortMode = ParticleSystemSortMode.None;
            psr1.sortingFudge = 0f;
            psr1.minParticleSize = 0f;
            psr1.maxParticleSize = 0.5f;
            psr1.alignment = ParticleSystemRenderSpace.View;
            psr1.allowRoll = true;
            psr1.maskInteraction = SpriteMaskInteraction.None;
            psr1.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            psr1.receiveShadows = false;
            psr1.shadowBias = 0f;
            psr1.motionVectorGenerationMode = MotionVectorGenerationMode.Object;
            psr1.sortingLayerID = LayerIndex.defaultLayer.intVal;
            psr1.sortingOrder = 0;
            psr1.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.BlendProbes;
            psr1.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
            #endregion


            ParticleSystem ps2 = parts2.AddComponent<ParticleSystem>();
            ParticleSystemRenderer psr2 = parts2.AddOrGetComponent<ParticleSystemRenderer>();

            #region Particle System definitions 2
            BasicSetup( ps2 );

            ParticleSystem.MainModule ps2Main = ps2.main;
            ps2Main.duration = 1f;
            ps2Main.loop = true;
            ps2Main.prewarm = false;
            ps2Main.startDelay = 0f;
            ps2Main.startLifetime = 0.25f;
            ps2Main.startSpeed = 0f;
            ps2Main.startSize = 2f;
            ps2Main.startRotation = 0f;
            ps2Main.flipRotation = 0.5f;
            ps2Main.gravityModifier = 0f;
            ps2Main.simulationSpace = ParticleSystemSimulationSpace.World;
            ps2Main.simulationSpeed = 1f;
            ps2Main.useUnscaledTime = false;
            ps2Main.scalingMode = ParticleSystemScalingMode.Hierarchy;
            ps2Main.playOnAwake = true;
            ps2Main.emitterVelocityMode = ParticleSystemEmitterVelocityMode.Rigidbody;
            ps2Main.maxParticles = 1000;
            ps2Main.stopAction = ParticleSystemStopAction.None;
            ps2Main.cullingMode = ParticleSystemCullingMode.AlwaysSimulate;
            ps2Main.ringBufferMode = ParticleSystemRingBufferMode.Disabled;

            ParticleSystem.EmissionModule ps2Emis = ps2.emission;
            ps2Emis.enabled = true;
            ps2Emis.rateOverTime = new ParticleSystem.MinMaxCurve
            {
                mode = ParticleSystemCurveMode.Constant,
                constant = 0f
            };
            ps2Emis.rateOverDistance = new ParticleSystem.MinMaxCurve
            {
                mode = ParticleSystemCurveMode.Constant,
                constant = 0.5f
            };

            ParticleSystem.ShapeModule ps2Shape = ps2.shape;
            ps2Shape.enabled = true;
            ps2Shape.shapeType = ParticleSystemShapeType.BoxEdge;
            ps2Shape.radius = 0.5f;
            ps2Shape.position = Vector3.zero;
            ps2Shape.rotation = new Vector3( 0f, 0f, 0f );
            ps2Shape.scale = new Vector3( 1f, 1f, 1f );
            ps2Shape.alignToDirection = false;
            ps2Shape.randomDirectionAmount = 0f;
            ps2Shape.sphericalDirectionAmount = 0f;
            ps2Shape.randomPositionAmount = 0f;

            ParticleSystem.ColorOverLifetimeModule ps2COL = ps2.colorOverLifetime;
            ps2COL.enabled = false;
            ps2COL.color = new ParticleSystem.MinMaxGradient
            {
                mode = ParticleSystemGradientMode.Gradient,
                gradient = new Gradient
                {
                    alphaKeys = new GradientAlphaKey[3]
                    {
                        new GradientAlphaKey
                        {
                            time = 0f,
                            alpha = 0f
                        },
                        new GradientAlphaKey
                        {
                            time = 0.075f,
                            alpha = 1f
                        },
                        new GradientAlphaKey
                        {
                            time = 1f,
                            alpha = 0f
                        }
                    },
                    colorKeys = new GradientColorKey[3]
                    {
                        new GradientColorKey
                        {
                            time = 0f,
                            color = new Color
                            {
                                r = 5f,
                                g = 5f,
                                b = 5f
                            }
                        },
                        new GradientColorKey
                        {
                            time = 0.08f,
                            color = new Color
                            {
                                r = 1f,
                                g = 1f,
                                b = 1f
                            }
                        },
                        new GradientColorKey
                        {
                            time = 1f,
                            color = new Color
                            {
                                r = 0.7f,
                                g = 0.7f,
                                b = 0.7f
                            }
                        }
                    }
                }
            };

            ParticleSystem.SizeOverLifetimeModule ps2SOL = ps2.sizeOverLifetime;
            ps2SOL.enabled = true;
            ps2SOL.size = new ParticleSystem.MinMaxCurve
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
            ps2SOL.separateAxes = false;
            ps2SOL.sizeMultiplier = 2f;

            ParticleSystem.RotationOverLifetimeModule ps2ROL = ps2.rotationOverLifetime;
            ps2ROL.enabled = true;
            ps2ROL.x = new ParticleSystem.MinMaxCurve
            {
                mode = ParticleSystemCurveMode.Constant,
                constant = 2f
            };
            ps2ROL.y = new ParticleSystem.MinMaxCurve
            {
                mode = ParticleSystemCurveMode.Constant,
                constant = 2f
            };
            ps2ROL.z = new ParticleSystem.MinMaxCurve
            {
                mode = ParticleSystemCurveMode.Constant,
                constant = 2f
            };

            psr2.renderMode = ParticleSystemRenderMode.Billboard;
            psr2.normalDirection = 1f;
            psr2.material = distortion;
            psr2.sortMode = ParticleSystemSortMode.None;
            psr2.sortingFudge = 0f;
            psr2.minParticleSize = 0f;
            psr2.maxParticleSize = 0.5f;
            psr2.alignment = ParticleSystemRenderSpace.View;
            psr2.allowRoll = true;
            psr2.maskInteraction = SpriteMaskInteraction.None;
            psr2.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            psr2.receiveShadows = false;
            psr2.shadowBias = 0f;
            psr2.motionVectorGenerationMode = MotionVectorGenerationMode.Object;
            psr2.sortingLayerID = LayerIndex.defaultLayer.intVal;
            psr2.sortingOrder = 0;
            psr2.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.BlendProbes;
            psr2.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
            #endregion

            ParticleSystem ps3 = parts3.AddComponent<ParticleSystem>();
            ParticleSystemRenderer psr3 = parts3.AddOrGetComponent<ParticleSystemRenderer>();

            #region Particle System definitions 3
            BasicSetup( ps3 );

            ParticleSystem.MainModule ps3Main = ps3.main;
            ps3Main.duration = 1f;
            ps3Main.loop = true;
            ps3Main.prewarm = false;
            ps3Main.startDelay = 0f;
            ps3Main.startLifetime = 0.25f;
            ps3Main.startSpeed = 0f;
            ps3Main.startSize = 0.75f;
            ps3Main.startRotation = 0f;
            ps3Main.flipRotation = 0.5f;
            ps3Main.gravityModifier = 0f;
            ps3Main.simulationSpace = ParticleSystemSimulationSpace.World;
            ps3Main.simulationSpeed = 1f;
            ps3Main.useUnscaledTime = false;
            ps3Main.scalingMode = ParticleSystemScalingMode.Hierarchy;
            ps3Main.playOnAwake = true;
            ps3Main.emitterVelocityMode = ParticleSystemEmitterVelocityMode.Rigidbody;
            ps3Main.maxParticles = 1000;
            ps3Main.stopAction = ParticleSystemStopAction.None;
            ps3Main.cullingMode = ParticleSystemCullingMode.AlwaysSimulate;
            ps3Main.ringBufferMode = ParticleSystemRingBufferMode.Disabled;

            ParticleSystem.EmissionModule ps3Emis = ps3.emission;
            ps3Emis.enabled = true;
            ps3Emis.rateOverTime = new ParticleSystem.MinMaxCurve
            {
                mode = ParticleSystemCurveMode.Constant,
                constant = 0f
            };
            ps3Emis.rateOverDistance = new ParticleSystem.MinMaxCurve
            {
                mode = ParticleSystemCurveMode.Constant,
                constant = 1f
            };

            ParticleSystem.ShapeModule ps3Shape = ps3.shape;
            ps3Shape.enabled = true;
            ps3Shape.shapeType = ParticleSystemShapeType.BoxEdge;
            ps3Shape.radius = 0.25f;
            ps3Shape.position = Vector3.zero;
            ps3Shape.rotation = new Vector3( 0f, 0f, 0f );
            ps3Shape.scale = new Vector3( 1f, 1f, 1f );
            ps3Shape.alignToDirection = false;
            ps3Shape.randomDirectionAmount = 0f;
            ps3Shape.sphericalDirectionAmount = 0f;
            ps3Shape.randomPositionAmount = 0f;

            ParticleSystem.ColorOverLifetimeModule ps3COL = ps3.colorOverLifetime;
            ps3COL.enabled = false;
            ps3COL.color = new ParticleSystem.MinMaxGradient
            {
                mode = ParticleSystemGradientMode.Gradient,
                gradient = new Gradient
                {
                    alphaKeys = new GradientAlphaKey[3]
                    {
                        new GradientAlphaKey
                        {
                            time = 0f,
                            alpha = 0f
                        },
                        new GradientAlphaKey
                        {
                            time = 0.075f,
                            alpha = 1f
                        },
                        new GradientAlphaKey
                        {
                            time = 1f,
                            alpha = 0f
                        }
                    },
                    colorKeys = new GradientColorKey[3]
                    {
                        new GradientColorKey
                        {
                            time = 0f,
                            color = new Color
                            {
                                r = 5f,
                                g = 5f,
                                b = 5f
                            }
                        },
                        new GradientColorKey
                        {
                            time = 0.08f,
                            color = new Color
                            {
                                r = 1f,
                                g = 1f,
                                b = 1f
                            }
                        },
                        new GradientColorKey
                        {
                            time = 1f,
                            color = new Color
                            {
                                r = 0.7f,
                                g = 0.7f,
                                b = 0.7f
                            }
                        }
                    }
                }
            };

            ParticleSystem.SizeOverLifetimeModule ps3SOL = ps3.sizeOverLifetime;
            ps3SOL.enabled = true;
            ps3SOL.size = new ParticleSystem.MinMaxCurve
            {
                mode = ParticleSystemCurveMode.Curve,
                curve = new AnimationCurve
                {
                    postWrapMode = WrapMode.Clamp,
                    preWrapMode = WrapMode.Clamp,
                    keys = new Keyframe[3]
                    {
                        new Keyframe
                        {
                            time = 0f,
                            value = 0.42f,
                            outTangent = 0.5f,
                            outWeight = 0.5f
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
            ps3SOL.separateAxes = false;
            ps3SOL.sizeMultiplier = 3f;

            ParticleSystem.RotationOverLifetimeModule ps3ROL = ps3.rotationOverLifetime;
            ps3ROL.enabled = true;
            ps3ROL.x = new ParticleSystem.MinMaxCurve
            {
                mode = ParticleSystemCurveMode.Constant,
                constant = 2f
            };
            ps3ROL.y = new ParticleSystem.MinMaxCurve
            {
                mode = ParticleSystemCurveMode.Constant,
                constant = 2f
            };
            ps3ROL.z = new ParticleSystem.MinMaxCurve
            {
                mode = ParticleSystemCurveMode.Constant,
                constant = 2f
            };

            psr3.renderMode = ParticleSystemRenderMode.Billboard;
            //psr3.normalDirection = 1f;
            psr3.material = flameMat;
            //psr3.sortMode = ParticleSystemSortMode.None;
            //psr3.sortingFudge = 0f;
            //psr3.minParticleSize = 0f;
            //psr3.maxParticleSize = 0.5f;
            //psr3.alignment = ParticleSystemRenderSpace.View;
            //psr3.allowRoll = true;
            //psr3.maskInteraction = SpriteMaskInteraction.None;
            //psr3.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            //psr3.receiveShadows = false;
            //psr3.shadowBias = 0f;
            //psr3.motionVectorGenerationMode = MotionVectorGenerationMode.Object;
            //psr3.sortingLayerID = LayerIndex.defaultLayer.intVal;
            //psr3.sortingOrder = 0;
            //psr3.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.BlendProbes;
            //psr3.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
            #endregion

            return obj;
        }
    }
#endif
}
