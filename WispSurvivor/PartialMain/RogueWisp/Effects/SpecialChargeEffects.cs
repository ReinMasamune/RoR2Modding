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
using R2API;
//using static RogueWispPlugin.Helpers.APIInterface;

namespace RogueWispPlugin
{
#if ROGUEWISP
    internal partial class Main
    {
        partial void RW_SpecialChargeEffects()
        {
            this.Load += this.RW_CreateSpecialChargeEffects;
        }

        private void RW_CreateSpecialChargeEffects()
        {
            GameObject baseFX = Resources.Load<GameObject>("Prefabs/Effects/ChargeMageFireBomb");

            for( Int32 i = 0; i < 8; i++ )
            {
                specialCharge[i] = CreateSpecialCharge( baseFX, i );
            }
        }

        private static GameObject CreateSpecialCharge( GameObject baseFX, Int32 skinIndex )
        {
            GameObject obj = baseFX.InstantiateClone("SpecialCharge"+skinIndex.ToString(), false);

            obj.name = "WispSpecialChargeEffect";

            Transform baseChild = obj.transform.Find("Base");
            Transform orbCore = obj.transform.Find("OrbCore");
            Transform light = obj.transform.Find("Point light");

            light.GetComponent<Light>().color = fireColors[skinIndex];

            ParticleSystem basePS = baseChild.GetComponent<ParticleSystem>();
            ParticleSystemRenderer basePSR = baseChild.GetComponent<ParticleSystemRenderer>();

            ParticleSystem.MainModule basePSmain = basePS.main;
            basePSmain.startColor = new Color( 1f, 1f, 1f, 1f );

            basePSR.material = fireMaterials[skinIndex][9];

            MonoBehaviour.Destroy( orbCore.GetComponent<MeshRenderer>() );
            MonoBehaviour.Destroy( orbCore.GetComponent<MeshFilter>() );

            ParticleSystem orbPS = orbCore.AddOrGetComponent<ParticleSystem>();
            ParticleSystemRenderer orbPSR = orbCore.AddOrGetComponent<ParticleSystemRenderer>();

            orbPSR.material = fireMaterials[skinIndex][9];

            BasicSetup( orbPS );

            ParticleSystem.MainModule orbMain = orbPS.main;
            orbMain.duration = 5f;
            orbMain.loop = true;
            orbMain.startLifetime = 0.5f;
            orbMain.startSpeed = 0f;
            orbMain.startSize = 3f;
            orbMain.startColor = new Color( 1f, 1f, 1f, 1f );
            orbMain.gravityModifier = 0f;
            orbMain.cullingMode = ParticleSystemCullingMode.AlwaysSimulate;

            ParticleSystem.EmissionModule orbEmis = orbPS.emission;
            orbEmis.enabled = true;
            orbEmis.rateOverTime = 100f;
            orbEmis.rateOverDistance = 0f;

            ParticleSystem.ShapeModule orbShape = orbPS.shape;
            orbShape.enabled = true;
            orbShape.shapeType = ParticleSystemShapeType.Sphere;
            orbShape.radius = 1f;

            ParticleSystem.ColorOverLifetimeModule orbCOL = orbPS.colorOverLifetime;
            orbCOL.enabled = true;
            orbCOL.color = new ParticleSystem.MinMaxGradient
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

            ParticleSystem.SizeOverLifetimeModule orbSOL = orbPS.sizeOverLifetime;
            orbSOL.enabled = true;
            orbSOL.size = new ParticleSystem.MinMaxCurve
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




            GameObject sparks = new GameObject("Sparks");
            sparks.transform.parent = obj.transform;
            sparks.transform.localPosition = Vector3.zero;
            sparks.transform.localRotation = Quaternion.identity;

            ParticleSystem sparkPS = sparks.AddComponent<ParticleSystem>();
            ParticleSystemRenderer sparkPSR = sparks.AddOrGetComponent<ParticleSystemRenderer>();

            sparkPSR.material = fireMaterials[skinIndex][1];

            BasicSetup( sparkPS );

            ParticleSystem.MainModule sparkMain = sparkPS.main;
            sparkMain.duration = 5f;
            sparkMain.loop = true;
            sparkMain.startLifetime = 1f;
            sparkMain.startSpeed = -5f;
            sparkMain.startSize = 0.5f;
            sparkMain.cullingMode = ParticleSystemCullingMode.AlwaysSimulate;

            ParticleSystem.EmissionModule sparkEmis = sparkPS.emission;
            sparkEmis.enabled = true;
            sparkEmis.rateOverTime = 100f;
            sparkEmis.rateOverDistance = 0f;
            //sparkEmis.rateOverTimeMultiplier = 1f;
            //sparkEmis.rateOverDistanceMultiplier = 0f;

            ParticleSystem.ShapeModule sparkShape = sparkPS.shape;
            sparkShape.enabled = true;
            sparkShape.shapeType = ParticleSystemShapeType.Sphere;
            sparkShape.radius = 5f;
            sparkShape.radiusThickness = 0.01f;

            ParticleSystem.ColorOverLifetimeModule sparkCOL = sparkPS.colorOverLifetime;
            sparkCOL.enabled = true;
            sparkCOL.color = new ParticleSystem.MinMaxGradient
            {
                mode = ParticleSystemGradientMode.Gradient,
                gradient = new Gradient
                {
                    mode = GradientMode.Blend,
                    alphaKeys = new GradientAlphaKey[4]
                    {
                        new GradientAlphaKey(0f, 0f ),
                        new GradientAlphaKey(1f, 0.1f),
                        new GradientAlphaKey(0.7f, 0.7f),
                        new GradientAlphaKey(0f, 1f)
                    },
                    colorKeys = new GradientColorKey[1]
                    {
                        new GradientColorKey( new Color( 1f, 1f, 1f ) , 0f )
                    }
                }
            };

            ParticleSystem.SizeOverLifetimeModule sparkSOL = sparkPS.sizeOverLifetime;
            sparkSOL.enabled = true;
            sparkSOL.size = new ParticleSystem.MinMaxCurve
            {
                mode = ParticleSystemCurveMode.Curve,
                curve = new AnimationCurve
                {
                    preWrapMode = WrapMode.Clamp,
                    postWrapMode = WrapMode.Clamp,
                    keys = new Keyframe[2]
                    {
                        new Keyframe(0f, 0f ),
                        new Keyframe(1f, 1f )
                    }
                }
            };
            sparkSOL.sizeMultiplier = 1f;


            return obj;
        }
    }
#endif
}
