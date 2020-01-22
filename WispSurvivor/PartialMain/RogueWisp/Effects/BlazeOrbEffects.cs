using R2API;
using RogueWispPlugin.Helpers;
using RoR2;
using System;
using UnityEngine;
//using static RogueWispPlugin.Helpers.APIInterface;

namespace RogueWispPlugin
{
#if ROGUEWISP
    internal partial class Main
    {
        partial void RW_BlazeOrbEffects() => this.Load += this.RW_CreateBlazeOrbEffects;

        private void RW_CreateBlazeOrbEffects()
        {
            GameObject baseFX = Resources.Load<GameObject>("Prefabs/Effects/MeteorStrikePredictionEffect");

            for( Int32 i = 0; i < 8; i++ )
            {
                utilityFlames[i] = CreateBlazeEffect( baseFX, i );
            }
        }

        private static GameObject CreateBlazeEffect( GameObject baseFX, Int32 skinIndex )
        {
            GameObject obj = baseFX.InstantiateClone("BlazeEffect"+skinIndex.ToString(), false);

            VFXAttributes fx = obj.GetComponent<VFXAttributes>();
            fx.vfxPriority = VFXAttributes.VFXPriority.Always;
            fx.vfxIntensity = VFXAttributes.VFXIntensity.Low;

            MonoBehaviour.Destroy( obj.GetComponent<DestroyOnTimer>() );

            obj.AddComponent<WispBlazeEffectController>();
            obj.transform.localScale = Vector3.one;

            obj.GetComponent<EffectComponent>().applyScale = true;

            Transform indicator = obj.transform.Find("GroundSlamIndicator");
            MonoBehaviour.Destroy( indicator.gameObject );

            GameObject range = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            range.name = "rangeInd";
            range.transform.parent = obj.transform;
            range.transform.localScale = new Vector3( 2f, 2f, 2f );
            range.transform.localPosition = Vector3.zero;
            range.transform.localRotation = Quaternion.identity;

            MonoBehaviour.Destroy( range.GetComponent<SphereCollider>() );

            GameObject range2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            range2.name = "rangeInd2";
            range2.transform.parent = obj.transform;
            range2.transform.localScale = new Vector3( 2f, 2f, 2f );
            range2.transform.localPosition = Vector3.zero;
            range2.transform.localRotation = Quaternion.identity;

            MonoBehaviour.Destroy( range2.GetComponent<SphereCollider>() );

            GameObject fireObj = new GameObject("Flames");
            fireObj.transform.parent = obj.transform;
            fireObj.transform.localPosition = Vector3.zero;
            fireObj.transform.localRotation = Quaternion.identity;
            fireObj.transform.localScale = Vector3.one;

            ParticleSystem firePS = fireObj.AddComponent<ParticleSystem>();
            ParticleSystemRenderer firePSR = fireObj.GetComponent<ParticleSystemRenderer>();


            GameObject ballObj = ParticleUtils.CreateFireBallParticle( obj, fireMaterials[skinIndex][9] );


            ParticleSystem.MainModule fireMain = firePS.main;
            fireMain.duration = 1f;
            fireMain.loop = true;
            fireMain.prewarm = false;
            fireMain.startLifetime = new ParticleSystem.MinMaxCurve
            {
                mode = ParticleSystemCurveMode.TwoConstants,
                constantMin = 1.25f,
                constantMax = 1.5f
            };
            fireMain.gravityModifier = new ParticleSystem.MinMaxCurve
            {
                mode = ParticleSystemCurveMode.Constant,
                constant = -0.5f
            };
            fireMain.scalingMode = ParticleSystemScalingMode.Shape;
            fireMain.startRotation = new ParticleSystem.MinMaxCurve
            {
                mode = ParticleSystemCurveMode.TwoConstants,
                constantMin = 0f,
                constantMax = 360f
            };
            fireMain.flipRotation = 0.5f;

            ParticleSystem.EmissionModule fireEmis = firePS.emission;
            fireEmis.rateOverTime = new ParticleSystem.MinMaxCurve
            {
                mode = ParticleSystemCurveMode.Constant,
                constant = 20f
            };

            ParticleSystem.ShapeModule fireShape = firePS.shape;
            fireShape.position = new Vector3( 0f, 0f, 0f );
            fireShape.rotation = new Vector3( -90f, 0f, 0f );
            fireShape.scale = new Vector3( 0.75f, 0.75f, 0.75f );
            fireShape.radiusThickness = 1f;
            fireShape.angle = 30f;

            ParticleSystem.ColorOverLifetimeModule fireCOL = firePS.colorOverLifetime;
            fireCOL.enabled = true;
            fireCOL.color = new ParticleSystem.MinMaxGradient
            {
                mode = ParticleSystemGradientMode.Gradient,
                gradient = new Gradient
                {
                    alphaKeys = new GradientAlphaKey[4]
                    {
                        new GradientAlphaKey( 0f, 0f ),
                        new GradientAlphaKey( 0.6f, 0.05f),
                        new GradientAlphaKey( 0.1f, 0.65f),
                        new GradientAlphaKey( 0f, 1f )
                    },
                    colorKeys = new GradientColorKey[1]
                    {
                        new GradientColorKey( new Color( 1f, 1f, 1f ) , 0f )
                    },
                    mode = GradientMode.Blend
                }
            };

            ParticleSystem.SizeOverLifetimeModule fireSOL = firePS.sizeOverLifetime;
            fireSOL.enabled = true;
            fireSOL.size = new ParticleSystem.MinMaxCurve
            {
                mode = ParticleSystemCurveMode.Curve,
                curve = new AnimationCurve
                {
                    postWrapMode = WrapMode.Clamp,
                    preWrapMode = WrapMode.Clamp,
                    keys = new Keyframe[3]
                    {
                        new Keyframe(0f,0.25f),
                        new Keyframe(0.43f,0.4f),
                        new Keyframe(1f,0.01f)
                    }
                }
            };
            fireSOL.sizeMultiplier = 7.5f;

            ParticleSystem.RotationOverLifetimeModule fireROL = firePS.rotationOverLifetime;
            fireROL.enabled = true;
            fireROL.z = new ParticleSystem.MinMaxCurve
            {
                mode = ParticleSystemCurveMode.Constant,
                constant = 3f
            };

            firePSR.material = fireMaterials[skinIndex][1];

            GameObject ringObj = new GameObject("Ring");
            ringObj.transform.parent = obj.transform;
            ringObj.transform.localPosition = Vector3.zero;
            ringObj.transform.localRotation = Quaternion.identity;
            ringObj.transform.localScale = new Vector3( 1f, 1f, 1f );

            ParticleSystem ringPS = ringObj.AddComponent<ParticleSystem>();
            ParticleSystemRenderer ringPSR = ringObj.GetComponent<ParticleSystemRenderer>();

            ParticleSystem.MainModule ringMain = ringPS.main;
            ringMain.duration = 1f;
            ringMain.loop = true;
            ringMain.prewarm = false;
            ringMain.startLifetime = new ParticleSystem.MinMaxCurve
            {
                mode = ParticleSystemCurveMode.TwoConstants,
                constantMin = 0.75f,
                constantMax = 1f
            };
            ringMain.gravityModifier = 0f;
            ringMain.scalingMode = ParticleSystemScalingMode.Shape;
            ringMain.startRotation = new ParticleSystem.MinMaxCurve
            {
                mode = ParticleSystemCurveMode.TwoConstants,
                constantMin = 0f,
                constantMax = 360f
            };
            ringMain.flipRotation = 0.5f;
            ringMain.startSpeed = 0f;

            ParticleSystem.EmissionModule ringEmis = ringPS.emission;
            ringEmis.rateOverTime = new ParticleSystem.MinMaxCurve
            {
                mode = ParticleSystemCurveMode.Constant,
                constant = 50f
            };

            ParticleSystem.ShapeModule ringShape = ringPS.shape;
            ringShape.shapeType = ParticleSystemShapeType.Sphere;
            ringShape.position = new Vector3( 0f, 0f, 0f );
            ringShape.rotation = new Vector3( -90f, 0f, 0f );
            ringShape.scale = new Vector3( 1f, 1f, 1f );
            ringShape.radiusThickness = 0f;

            ParticleSystem.ColorOverLifetimeModule ringCOL = ringPS.colorOverLifetime;
            ringCOL.enabled = true;
            ringCOL.color = new ParticleSystem.MinMaxGradient
            {
                mode = ParticleSystemGradientMode.Gradient,
                gradient = new Gradient
                {
                    alphaKeys = new GradientAlphaKey[4]
                    {
                        new GradientAlphaKey( 0f, 0f ),
                        new GradientAlphaKey( 0.6f, 0.05f),
                        new GradientAlphaKey( 0.1f, 0.65f),
                        new GradientAlphaKey( 0f, 1f )
                    },
                    colorKeys = new GradientColorKey[1]
                    {
                        new GradientColorKey( new Color( 1f, 1f, 1f ) , 0f )
                    },
                    mode = GradientMode.Blend
                }
            };

            ParticleSystem.SizeOverLifetimeModule ringSOL = ringPS.sizeOverLifetime;
            ringSOL.enabled = true;
            ringSOL.size = new ParticleSystem.MinMaxCurve
            {
                mode = ParticleSystemCurveMode.Curve,
                curve = new AnimationCurve
                {
                    postWrapMode = WrapMode.Clamp,
                    preWrapMode = WrapMode.Clamp,
                    keys = new Keyframe[3]
                    {
                        new Keyframe(0f,0.1f),
                        new Keyframe(0.5f,0.5f),
                        new Keyframe(1f,0.01f)
                    }
                }
            };
            ringSOL.sizeMultiplier = 5f;

            ParticleSystem.RotationOverLifetimeModule ringROL = ringPS.rotationOverLifetime;
            ringROL.enabled = true;
            ringROL.z = new ParticleSystem.MinMaxCurve
            {
                mode = ParticleSystemCurveMode.Constant,
                constant = 3f
            };

            ringPSR.material = fireMaterials[skinIndex][1];

            return obj;
        }
    }
#endif
}
