using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;
using static WispSurvivor.Helpers.APIInterface;

namespace WispSurvivor.Modules
{
    public static class WispBodySetupModule
    {
        public static void DoModule( GameObject body, Dictionary<Type, Component> dic, AssetBundle bundle )
        {
            CharBodyStats( body, dic );
            CharBodyOther( body, dic, bundle );
        }

        private static void CharBodyStats( GameObject body, Dictionary<Type, Component> dic )
        {
            CharacterBody chbod = dic.C<CharacterBody>();
            chbod.baseMaxHealth = 100.0f;
            chbod.levelMaxHealth = 30.0f;
            chbod.baseRegen = 0.6f;
            chbod.levelRegen = 0.12f;
            chbod.baseMaxShield = 30f;
            chbod.levelMaxShield = 9f;
            chbod.baseMoveSpeed = 7f;
            chbod.levelMoveSpeed = 0f;
            chbod.baseJumpPower = 15.0f;
            chbod.levelJumpPower = 0f;
            chbod.baseDamage = 12f;
            chbod.levelDamage = 2.4f;
            chbod.baseAttackSpeed = 1.0f;
            chbod.levelAttackSpeed = 0f;
            chbod.baseCrit = 1f;
            chbod.levelCrit = 0f;
            chbod.baseArmor = 0f;
            chbod.levelArmor = 0f;
            chbod.baseJumpCount = 1;
            chbod.baseAcceleration = 60.0f;
            chbod.spreadBloomDecayTime = 1f;
        }

        private static void CharBodyOther( GameObject body, Dictionary<Type, Component> dic, AssetBundle bundle )
        {
            CharacterBody chbod = dic.C<CharacterBody>();
            chbod.bodyFlags = CharacterBody.BodyFlags.ImmuneToExecutes;
            chbod.hullClassification = HullClassification.Human;
            chbod.isChampion = false;
            chbod.crosshairPrefab = WispUIModule.crosshair;
            chbod.subtitleNameToken = "ReinThings.WispSurvivor";
            chbod.baseNameToken = "WISP_SURVIVOR_BODY_NAME";
            chbod.preferredPodPrefab = CreateSurvivorPod();
            chbod.portraitIcon = bundle.LoadAsset<Texture2D>( "Assets/__EXPORT/WispyIcon.png" );
        }

        private static GameObject CreateSurvivorPod()
        {
            GameObject g = Resources.Load<GameObject>("Prefabs/NetworkedObjects/SurvivorPod").InstantiateClone("WispSurvivorPod");
            Components.WispSurvivorPodController wispPodControl = g.AddComponent<Components.WispSurvivorPodController>();
            Transform podModel = g.GetComponent<ModelLocator>().modelTransform.Find("EscapePodArmature");

            InstantiatePrefabOnStart prefabSpawn = new InstantiatePrefabOnStart();

            foreach( InstantiatePrefabOnStart ipos in g.GetComponents<InstantiatePrefabOnStart>() )
            {
                if( ipos.prefab.name == "SurvivorPodBatteryPanel" ) prefabSpawn = ipos;
            }

            GameObject pref = prefabSpawn.prefab.InstantiateClone("WispPodPanel");
            pref.AddComponent<Components.WispSurvivorPodPanelController>();
            prefabSpawn.prefab = pref;

            Transform podMesh = podModel.Find("Base").Find("EscapePodMesh");
            Transform doorMesh = podModel.Find("Base").Find("Door").Find("EscapePodDoorMesh");
            Transform doorMesh2 = podModel.Find("Base").Find("ReleaseExhaustFX").Find("Door,Physics");

            wispPodControl.podMesh = podMesh;
            wispPodControl.doorMesh = doorMesh;
            wispPodControl.doorMesh2 = doorMesh2;

            return g;
        }

        private static T C<T>( this Dictionary<Type, Component> dic ) where T : Component => dic[typeof( T )] as T;
    }
}
