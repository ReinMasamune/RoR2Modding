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
using static RogueWispPlugin.Helpers.APIInterface;

namespace RogueWispPlugin
{
    internal partial class Main
    {
        partial void RW_SpecialExplosionEffects()
        {
            this.Load += this.RW_CreateSpecialExplosionEffects;
        }

        private void RW_CreateSpecialExplosionEffects()
        {
            GameObject baseFX = Resources.Load<GameObject>("Prefabs/Effects/BeamSphereExplosion");

            for( Int32 i = 0; i < 8; i++ )
            {
                specialExplosion[i] = CreateSpecialExplosion( baseFX, i );
            }
        }

        private static GameObject CreateSpecialExplosion( GameObject baseFX, Int32 skinIndex )
        {
            GameObject obj = baseFX.InstantiateClone("SpecialExplosion"+skinIndex.ToString(), false);

            VFXAttributes fx = obj.GetComponent<VFXAttributes>();
            fx.vfxPriority = VFXAttributes.VFXPriority.Always;
            fx.vfxIntensity = VFXAttributes.VFXIntensity.Low;

            Transform burst = obj.transform.Find("InitialBurst");
            Transform ring = burst.Find("Ring");
            Transform chunks = burst.Find("Chunks, Sharp");
            Transform flames = burst.Find("Flames");
            Transform flash = burst.Find("Flash");
            Transform light = burst.Find("Point light");
            Transform zap = burst.Find("Lightning");

            ParticleSystem ringPS = ring.GetComponent<ParticleSystem>();
            ParticleSystemRenderer ringPSR = ring.GetComponent<ParticleSystemRenderer>();

            ParticleSystem.ColorOverLifetimeModule ringCol = ringPS.colorOverLifetime;
            ringCol.color = new ParticleSystem.MinMaxGradient
            {
                mode = ParticleSystemGradientMode.Gradient,
                gradient = fireGradients[skinIndex]
            };

            ringPSR.material = fireMaterials[skinIndex][8];


            ParticleSystem chunkPS = chunks.GetComponent<ParticleSystem>();

            ParticleSystem.ColorOverLifetimeModule chunkCol = chunkPS.colorOverLifetime;
            chunkCol.color = new ParticleSystem.MinMaxGradient
            {
                mode = ParticleSystemGradientMode.Gradient,
                gradient = fireGradients[skinIndex]
            };


            ParticleSystemRenderer flamesPSR = flames.GetComponent<ParticleSystemRenderer>();
            flamesPSR.material = fireMaterials[skinIndex][9];


            ParticleSystem flashPS = flash.GetComponent<ParticleSystem>();

            ParticleSystem.ColorOverLifetimeModule flashCol = flashPS.colorOverLifetime;
            flashCol.color = new ParticleSystem.MinMaxGradient
            {
                mode = ParticleSystemGradientMode.Gradient,
                gradient = new Gradient
                {
                    alphaKeys = new GradientAlphaKey[1]
                    {
                        new GradientAlphaKey( 1f, 0f )
                    },
                    colorKeys = new GradientColorKey[2]
                    {
                        new GradientColorKey( new Color( 1f, 1f, 1f ) , 0f ),
                        new GradientColorKey( fireColors[skinIndex] , 0.25f )
                    },
                    mode = GradientMode.Blend
                }
            };

            light.GetComponent<Light>().color = fireColors[skinIndex];

            ParticleSystemRenderer zapPSR = zap.GetComponent<ParticleSystemRenderer>();
            zapPSR.material = fireMaterials[skinIndex][10];

            return obj;
        }
    }

}
