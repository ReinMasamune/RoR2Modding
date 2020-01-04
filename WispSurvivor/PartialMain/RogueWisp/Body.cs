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
        partial void RW_Body()
        {
            this.Load += this.RW_CharBodyStats;
            this.Load += this.RW_CharBodyOther;
        }

        private void RW_CharBodyOther()
        {
            CharacterBody chbod = this.RW_body.GetComponent<CharacterBody>();
            chbod.bodyFlags = CharacterBody.BodyFlags.ImmuneToExecutes;
            chbod.hullClassification = HullClassification.Human;
            chbod.isChampion = false;
            chbod.crosshairPrefab = this.RW_crosshair;
            chbod.subtitleNameToken = "ReinThings.WispSurvivor";
            chbod.baseNameToken = "WISP_SURVIVOR_BODY_NAME";
            chbod.preferredPodPrefab = CreateSurvivorPod();
            chbod.portraitIcon = this.RW_assetBundle.LoadAsset<Texture2D>( "Assets/__EXPORT/WispyIcon.png" );
        }
        private void RW_CharBodyStats()
        {
            CharacterBody chbod = this.RW_body.GetComponent<CharacterBody>();
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
    }

}
