using RoR2.Projectile;
using UnityEngine;
using System;
using System.Collections.Generic;
using static WispSurvivor.Util.PrefabUtilities;

namespace WispSurvivor.Modules
{
    public static class WispProjectileModule
    {
        public static GameObject[] specialProjPrefabs = new GameObject[8];

        public static void DoModule( GameObject body , Dictionary<Type,Component> dic)
        {
            CreateSpecialProjectiles();
        }

        private static void ExFunction(GameObject body, Dictionary<Type, Component> dic)
        {

        }

        private static T C<T>( this Dictionary<Type,Component> dic ) where T : Component
        {
            return dic[typeof(T)] as T;
        }

        private static void CreateSpecialProjectiles()
        {
            for( int i = 0; i < 8; i++ )
            {
                specialProjPrefabs[i] = CreateSpecialProjectile(i);
                Util.PrefabUtilities.RegisterNewProjectile(specialProjPrefabs[i]);
            }
        }

        private static GameObject CreateSpecialProjectile( int skinIndex )
        {
            GameObject obj = Resources.Load<GameObject>("Prefabs/Projectiles/WispCannon").InstantiateClone("WispCannonProjectileThing"+skinIndex.ToString());

            obj.GetComponent<ProjectileController>().ghostPrefab = CreateSpecialProjectileGhost(skinIndex);

            ProjectileImpactExplosion impact = obj.GetComponent<ProjectileImpactExplosion>();

            impact.impactEffect = WispEffectModule.specialExplosion[skinIndex];
            impact.blastRadius = 10f;

            ProjectileSimple simp = obj.GetComponent<ProjectileSimple>();
            simp.velocity = 125f;

            SphereCollider sphere = obj.GetComponent<SphereCollider>();
            sphere.radius = 0.45f;

            return obj;
        }

        private static GameObject CreateSpecialProjectileGhost( int skinIndex )
        {
            GameObject obj = Resources.Load<GameObject>("Prefabs/projectileghosts/archwispcannonghost").InstantiateClone("WispCannonGhostThing"+skinIndex.ToString(), false);

            GameObject fireObj = obj.transform.Find("Particles").gameObject;
            GameObject sphereObj = fireObj.transform.Find("FireSphere").gameObject;

            sphereObj.GetComponent<ParticleSystemRenderer>().material = WispMaterialModule.fireMaterials[skinIndex][0];

            foreach( Transform t in obj.transform )
            {
                if( t.gameObject.HasComponent<Light>() )
                {
                    Light l = t.GetComponent<Light>();
                    l.color = WispMaterialModule.fireColors[skinIndex];
                }
            }

            ParticleSystem ps = sphereObj.GetComponent<ParticleSystem>();

            var psCOL = ps.colorOverLifetime;
            ParticleSystem.MinMaxGradient psCOLGrad = new ParticleSystem.MinMaxGradient();
            psCOLGrad.mode = ParticleSystemGradientMode.Gradient;
            Gradient psCOLGradMain = new Gradient();
            GradientColorKey[] cols = new GradientColorKey[3];
            cols[0] = new GradientColorKey( new Color(3f, 3f, 3f) , 0f );
            cols[1] = new GradientColorKey(new Color(1f, 1f, 1f), 0.2f);
            cols[2] = new GradientColorKey(new Color(0.8f, 0.8f, 0.8f), 1f);
            psCOLGradMain.SetKeys(cols, psCOL.color.gradient.alphaKeys);
            psCOLGrad.gradient = psCOLGradMain;
            psCOL.color = psCOLGrad;

            return obj;
        }
    }
}
