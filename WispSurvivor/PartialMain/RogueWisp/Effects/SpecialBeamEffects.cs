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
        partial void RW_SpecialBeamEffects()
        {
            this.FirstFrame += this.RW_CreateSpecialBeamEffects;
        }

        private void RW_CreateSpecialBeamEffects()
        {
            GameObject baseFX = ( EntityStates.EntityState.Instantiate(new EntityStates.SerializableEntityStateType(typeof(EntityStates.TitanMonster.FireMegaLaser))) as EntityStates.TitanMonster.FireMegaLaser ).GetFieldValue<GameObject>("laserPrefab");

            for( Int32 i = 0; i < 8; i++ )
            {
                specialBeam[i] = CreateSpecialBeam( baseFX, i );
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

            mainLaser.widthMultiplier = 2f;

            foreach( LineRenderer line in subLasers )
            {
                //Sub laser stuff
                line.material = beamMat;
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
            */


            return g;
        }
    }
#endif
}
