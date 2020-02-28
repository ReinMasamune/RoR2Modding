#if ROGUEWISP
using R2API;
using RogueWispPlugin.Helpers;
using RoR2;
using System;
using System.Reflection;
using UnityEngine;
//using static RogueWispPlugin.Helpers.APIInterface;

namespace RogueWispPlugin
{

    internal partial class Main
    {
        partial void RW_NewSecondaryExplosionEffect()
        {
            this.Load += this.RW_CreateNewSecondaryExplosion;
        }

        private void RW_CreateNewSecondaryExplosion()
        {
            var obj = new GameObject().InstantiateClone("SecondaryExplosion", false );

            var effComp = obj.AddComponent<EffectComponent>();
            effComp.positionAtReferencedTransform = false;
            effComp.parentToReferencedTransform = false;
            effComp.applyScale = false;
            effComp.soundName = "Play_item_use_meteor_impact";

            var skin = obj.AddComponent<WispSkinnedEffect>();

            var vfxAtrib = obj.AddComponent<VFXAttributes>();
            vfxAtrib.vfxPriority = VFXAttributes.VFXPriority.Always;
            vfxAtrib.vfxIntensity = VFXAttributes.VFXIntensity.Low;

            var timer = obj.AddComponent<DestroyOnTimer>();
            timer.duration = 4f;


            var flash1 = EffectHelper.AddFlash( obj, skin, MaterialType.Tracer );


            var pillar = EffectHelper.AddFlamePillar( obj, skin, MaterialType.FlamePillar, 20f, 3.5f, 0.5f  );


            var sparks = EffectHelper.AddSparks( obj, skin, MaterialType.Tracer, 100, 0.15f, 1.2f );
            var sparkShape = sparks.shape;
            sparkShape.enabled = true;
            sparkShape.shapeType = ParticleSystemShapeType.ConeVolume;
            sparkShape.angle = 0f;
            sparkShape.radius = 3.5f;
            sparkShape.length = 20f;
            sparkShape.rotation = new Vector3( -90f, 0f, 0f );



            var flashLines = EffectHelper.AddFlashLines( obj, skin, MaterialType.Tracer, 10, 0.2f );
            var flashLineShape = flashLines.shape;
            flashLineShape.enabled = true;
            flashLineShape.shapeType = ParticleSystemShapeType.Cone;
            flashLineShape.radius = 3.5f;
            flashLineShape.length = 5f;
            flashLineShape.angle = 30f;
            flashLineShape.rotation = new Vector3( -90f, 0f, 0f );




            var explosion = EffectHelper.AddExplosion( obj, skin, MaterialType.Explosion, 20, 0.3f, 5, 5f );
            var explShape = explosion.shape;
            explShape.enabled = true;
            explShape.shapeType = ParticleSystemShapeType.Hemisphere;
            explShape.radius = 0.5f;
            explShape.rotation = new Vector3( -90f, 0f, 0f );


            var light = EffectHelper.AddLight( obj, skin, true, 10f, 100f );
            light.transform.localPosition += new Vector3( 0f, 3f, 0f );
            EffectHelper.EditLightOverTime( light, 2f, AnimationCurve.EaseInOut( 0f, 1f, 1f, 0f ), AnimationCurve.EaseInOut( 0f, 1f, 1f, 0f ) );


            var distortion = EffectHelper.AddDistortion( obj, skin, MaterialType.Distortion, 8f, 0.3f, 0f );



            secondaryExplosion = obj;
            RegisterEffect( secondaryExplosion );
        }

        /*
        partial void RW_SecondaryExplosionEffects() => this.Load += this.RW_CreateSecondaryExplosionEffects;

        private void RW_CreateSecondaryExplosionEffects()
        {
            GameObject baseFX = Resources.Load<GameObject>("Prefabs/Effects/ImpactEffects/MeteorStrikeImpact");
            GameObject refFX = Resources.Load<GameObject>("Prefabs/Effects/ImpactEffects/AncientWispPillar");

            //Transform refPST = Resources.Load<GameObject>("Prefabs/Effects/ImpactEffects/AncientWispPillar").transform.Find("Particles");

            for( Int32 i = 0; i < 8; i++ )
            {
                secondaryExplosions[i] = CreateSecondaryExplosion( baseFX, i, refFX );
            }
        }

        private static GameObject CreateSecondaryExplosion( GameObject baseFX, Int32 skinIndex, GameObject refFX )
        {
            GameObject obj = baseFX.InstantiateClone("SecondaryExplosion"+skinIndex.ToString(), false);

            VFXAttributes fx = obj.GetComponent<VFXAttributes>();
            fx.vfxPriority = VFXAttributes.VFXPriority.Always;
            fx.vfxIntensity = VFXAttributes.VFXIntensity.Low;

            Material flameMat = fireMaterials[skinIndex][0];
            Color flameCol = fireColors[skinIndex];

            obj.transform.localScale = new Vector3( 2f, 2f, 2f );

            obj.transform.Find( "Flash" ).gameObject.name = "Flash2";

            //MonoBehaviour.Destroy(obj.transform.Find("Debris").gameObject);
            //MonoBehaviour.Destroy(obj.transform.Find("Dust").gameObject);
            MonoBehaviour.Destroy( obj.transform.Find( "Dust, Directional" ).gameObject );
            //MonoBehaviour.Destroy(obj.transform.Find("Dust, Directional").gameObject);


            //Add a new particle system and set its properties
            GameObject tube = new GameObject("Tube");
            tube.transform.parent = obj.transform;
            tube.transform.localPosition = Vector3.zero;
            tube.transform.localRotation = Quaternion.LookRotation( Vector3.forward, Vector3.down );
            tube.transform.localScale = new Vector3( 0.6f, 0.6f, 0.5f );

            ParticleSystem tubePS = tube.AddComponent<ParticleSystem>();
            ParticleSystemRenderer tubePSR = tube.GetComponent<ParticleSystemRenderer>();

            ParticleSystem tubeRefPS = refFX.transform.Find("Particles").Find("Flames, Tube, CenterHuge").GetComponent<ParticleSystem>();
            ParticleSystemRenderer tubeRefPSR = refFX.transform.Find("Particles").Find("Flames, Tube, CenterHuge").GetComponent<ParticleSystemRenderer>();

            ParticleUtils.SetParticleStruct<ParticleSystem.MainModule>( tubePS.main, tubeRefPS.main );
            ParticleUtils.SetParticleStruct<ParticleSystem.EmissionModule>( tubePS.emission, tubeRefPS.emission );
            ParticleUtils.SetParticleStruct<ParticleSystem.ShapeModule>( tubePS.shape, tubeRefPS.shape );
            ParticleUtils.SetParticleStruct<ParticleSystem.ColorOverLifetimeModule>( tubePS.colorOverLifetime, tubeRefPS.colorOverLifetime );
            ParticleUtils.SetParticleStruct<ParticleSystem.SizeOverLifetimeModule>( tubePS.sizeOverLifetime, tubeRefPS.sizeOverLifetime );
            ParticleUtils.SetParticleStruct<ParticleSystem.RotationOverLifetimeModule>( tubePS.rotationOverLifetime, tubeRefPS.rotationOverLifetime );

            ParticleSystem.EmissionModule tubePSEmis = tubePS.emission;
            ParticleSystem.EmissionModule tubeRefPSEmis = tubeRefPS.emission;
            ParticleSystem.Burst[] tempBursts = new ParticleSystem.Burst[1];
            tempBursts[0] = new ParticleSystem.Burst
            {
                count = 10,
                cycleCount = 1,
                probability = 1f,
                time = 0f,
                repeatInterval = 10f
            };
            tubePSEmis.SetBursts( tempBursts );

            foreach( PropertyInfo p in tubePSR.GetType().GetProperties() )
            {
                if( p.CanWrite && p.CanRead ) p.SetValue( tubePSR, p.GetValue( tubeRefPSR ) );
            }

            tubePSR.material = fireMaterials[skinIndex][5];

            //Configure existing particle systems
            GameObject fire = obj.transform.Find("Fire").gameObject;
            ParticleSystemRenderer firePSR = fire.GetComponent<ParticleSystemRenderer>();
            firePSR.material = fireMaterials[skinIndex][6];


            GameObject flFire = obj.transform.Find("Flash Lines, Fire").gameObject;
            ParticleSystem flFirePS = flFire.GetComponent<ParticleSystem>();
            ParticleSystemRenderer flFirePSR = flFire.GetComponent<ParticleSystemRenderer>();
            ParticleSystem.ColorOverLifetimeModule flFirePScol = flFirePS.colorOverLifetime;
            flFirePScol.color = new ParticleSystem.MinMaxGradient
            {
                mode = ParticleSystemGradientMode.Gradient,
                gradient = fireGradients[skinIndex]
            };
            flFirePSR.material = fireMaterials[skinIndex][6];

            GameObject flBase = obj.transform.Find("Flash Lines").gameObject;
            ParticleSystem flBasePS = flBase.GetComponent<ParticleSystem>();
            ParticleSystem.MainModule flBasePSmain = flBasePS.main;
            flBasePSmain.startColor = new ParticleSystem.MinMaxGradient
            {
                mode = ParticleSystemGradientMode.Color,
                color = fireColors[skinIndex]
            };

            GameObject flash = obj.transform.Find("Flash").gameObject;
            ParticleSystem flashPS = flash.GetComponent<ParticleSystem>();
            ParticleSystem.ColorOverLifetimeModule flashPScol = flashPS.colorOverLifetime;
            flashPScol.color = new ParticleSystem.MinMaxGradient
            {
                mode = ParticleSystemGradientMode.Gradient,
                gradient = fireGradients[skinIndex]
            };

            GameObject flash2 = obj.transform.Find("Flash2").gameObject;
            ParticleSystem flash2PS = flash2.GetComponent<ParticleSystem>();
            ParticleSystem.ColorOverLifetimeModule flash2PScol = flash2PS.colorOverLifetime;
            flash2PScol.color = new ParticleSystem.MinMaxGradient
            {
                mode = ParticleSystemGradientMode.Gradient,
                gradient = fireGradients[skinIndex]
            };

            obj.transform.Find( "Point light" ).GetComponent<Light>().color = fireColors[skinIndex];

            GameObject sparks = obj.transform.Find("Sparks").gameObject;
            ParticleSystem sparksPS = sparks.GetComponent<ParticleSystem>();
            ParticleSystem.MainModule sparksPSmain = sparksPS.main;
            sparksPSmain.startColor = new ParticleSystem.MinMaxGradient
            {
                mode = ParticleSystemGradientMode.Color,
                color = fireColors[skinIndex]
            };



            return obj;
        }
        */
    }

}
#endif