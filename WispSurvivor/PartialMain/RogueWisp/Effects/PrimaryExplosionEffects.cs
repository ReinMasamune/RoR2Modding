using BepInEx;
using R2API.Utils;
using RoR2;
using RoR2.Networking;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using RogueWispPlugin.Helpers;
using RogueWispPlugin.Modules;
//using static RogueWispPlugin.Helpers.APIInterface;

namespace RogueWispPlugin
{
    internal partial class Main
    {
        partial void RW_PrimaryExplosionEffects()
        {
            this.Load += this.RW_CreatePrimaryExplosionEffects;
        }

        private void RW_CreatePrimaryExplosionEffects()
        {
            GameObject baseFX = Resources.Load<GameObject>("Prefabs/Effects/ImpactEffects/ExplosionGreaterWisp");
            for( Int32 i = 0; i < 8; i++ )
            {
                primaryExplosionEffects[i] = CreatePrimaryExplosion( baseFX, i );
            }
        }

        private static GameObject CreatePrimaryExplosion( GameObject baseFX, Int32 skinIndex )
        {
            Material flamesMat = fireMaterials[skinIndex][0];
            Color flamesColor = fireColors[skinIndex];
            GameObject obj = baseFX.InstantiateClone("PrimaryExplosion"+skinIndex.ToString());
            GameObject obj2 = obj.transform.Find("Particles").gameObject;

            GameObject flamesObj = obj2.transform.Find("Flames").gameObject;
            GameObject sparksObj = obj2.transform.Find("Sparks").gameObject;
            GameObject flameSphObj = obj2.transform.Find("Flames,Sphere").gameObject;
            GameObject ringObj = obj2.transform.Find("Ring").gameObject;
            GameObject debrisObj = obj2.transform.Find("Debris").gameObject;
            GameObject flashObj = obj2.transform.Find("Flash").gameObject;
            GameObject distObj = obj2.transform.Find("Distortion").gameObject;

            VFXAttributes fx = obj.GetComponent<VFXAttributes>();
            fx.vfxPriority = VFXAttributes.VFXPriority.Always;
            fx.vfxIntensity = VFXAttributes.VFXIntensity.Low;

            ParticleSystem flamesPS = flamesObj.GetComponent<ParticleSystem>();
            ParticleSystemRenderer flamesPSR = flamesObj.GetComponent<ParticleSystemRenderer>();

            ParticleSystem.ColorOverLifetimeModule flamesCOL = flamesPS.colorOverLifetime;
            flamesCOL.color = new ParticleSystem.MinMaxGradient
            {
                mode = ParticleSystemGradientMode.Gradient,
                gradient = new Gradient
                {
                    alphaKeys = flamesCOL.color.gradient.alphaKeys,
                    colorKeys = new GradientColorKey[3]
                    {
                        new GradientColorKey
                        {
                            time = 0f,
                            color = new Color
                            {
                                r = 1f,
                                g = 1f,
                                b = 1f
                            }
                        },
                        new GradientColorKey
                        {
                            time = 0.025f,
                            color = flamesColor
                        },
                        new GradientColorKey
                        {
                            time = 1f,
                            color = new Color
                            {
                                r = 0.1f,
                                g = 0.1f,
                                b = 0.1f
                            }
                        }
                    }
                }
            };

            ParticleSystem sparksPS = sparksObj.GetComponent<ParticleSystem>();
            ParticleSystemRenderer sparksPSR = sparksObj.GetComponent<ParticleSystemRenderer>();

            sparksPSR.material = flamesMat;

            ParticleSystem.ColorOverLifetimeModule sparksCOL = sparksPS.colorOverLifetime;
            sparksCOL.color = new ParticleSystem.MinMaxGradient
            {
                mode = ParticleSystemGradientMode.Gradient,
                gradient = new Gradient
                {
                    alphaKeys = sparksCOL.color.gradient.alphaKeys,
                    colorKeys = new GradientColorKey[1]
                    {
                        new GradientColorKey
                        {
                            time = 0f,
                            color = new Color
                            {
                                r = 1f,
                                g = 1f,
                                b = 1f
                            }
                        }
                    }
                }
            };

            ParticleSystem flameSphPS = flameSphObj.GetComponent<ParticleSystem>();

            ParticleSystem.ColorOverLifetimeModule flameSphCOL = flameSphPS.colorOverLifetime;
            flameSphCOL.color = new ParticleSystem.MinMaxGradient
            {
                mode = ParticleSystemGradientMode.Gradient,
                gradient = new Gradient
                {
                    alphaKeys = flameSphCOL.color.gradient.alphaKeys,
                    colorKeys = new GradientColorKey[3]
                    {
                        new GradientColorKey
                        {
                            time = 0f,
                            color = new Color
                            {
                                r = 1f,
                                g = 1f,
                                b = 1f
                            }
                        },
                        new GradientColorKey
                        {
                            time = 0.025f,
                            color = flamesColor
                        },
                        new GradientColorKey
                        {
                            time = 1f,
                            color = new Color
                            {
                                r = 0.1f,
                                g = 0.1f,
                                b = 0.1f
                            }
                        }
                    }
                }
            };


            ParticleSystem ringPS = ringObj.GetComponent<ParticleSystem>();

            ParticleSystem.ColorOverLifetimeModule ringCOL = ringPS.colorOverLifetime;
            ringCOL.color = new ParticleSystem.MinMaxGradient
            {
                mode = ParticleSystemGradientMode.Gradient,
                gradient = new Gradient
                {
                    alphaKeys = ringCOL.color.gradient.alphaKeys,
                    colorKeys = new GradientColorKey[3]
                    {
                        new GradientColorKey
                        {
                            time = 0f,
                            color = new Color
                            {
                                r = 1f,
                                g = 1f,
                                b = 1f
                            }
                        },
                        new GradientColorKey
                        {
                            time = 0.025f,
                            color = flamesColor
                        },
                        new GradientColorKey
                        {
                            time = 1f,
                            color = new Color
                            {
                                r = 0.1f,
                                g = 0.1f,
                                b = 0.1f
                            }
                        }
                    }
                }
            };

            MonoBehaviour.Destroy( debrisObj );
            MonoBehaviour.Destroy( flashObj );
            //MonoBehaviour.Destroy(distObj);
            //MonoBehaviour.Destroy(ringObj);
            //MonoBehaviour.Destroy(flameSphObj);
            //MonoBehaviour.Destroy(sparksObj);
            //MonoBehaviour.Destroy(flamesObj);

            return obj;
        }
    }

}
