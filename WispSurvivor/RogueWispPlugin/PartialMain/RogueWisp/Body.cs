﻿#if ROGUEWISP
//using static RogueWispPlugin.Helpers.APIInterface;
using ReinCore;

using RoR2;

using UnityEngine;

namespace Rein.RogueWispPlugin
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
            chbod.subtitleNameToken = Rein.Properties.Tokens.WISP_SURVIVOR_SUBTITLE_NAME;
            chbod.baseNameToken = Rein.Properties.Tokens.WISP_SURVIVOR_BODY_NAME;
            chbod.preferredPodPrefab = CreateSurvivorPod();
            chbod.portraitIcon = this.RW_assetBundle.LoadAsset<Texture2D>( "Assets/__EXPORT/WispyIcon.png" );
        }
        private void RW_CharBodyStats()
        {
            CharacterBody chbod = this.RW_body.GetComponent<CharacterBody>();
            chbod.baseMaxHealth = 110.0f;
            chbod.levelMaxHealth = 33.0f;
            chbod.baseRegen = 1f;
            chbod.levelRegen = 0.2f;
            chbod.baseMaxShield = 0f;
            chbod.levelMaxShield = 0f;
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
            GameObject g = Resources.Load<GameObject>("Prefabs/NetworkedObjects/SurvivorPod").ClonePrefab("WispSurvivorPod", true);
            //WispSurvivorPodController wispPodControl = g.AddComponent<WispSurvivorPodController>();
            //Transform podModel = g.GetComponent<ModelLocator>().modelTransform.Find("EscapePodArmature");

            //InstantiatePrefabOnStart prefabSpawn = new InstantiatePrefabOnStart();

            //foreach( InstantiatePrefabOnStart ipos in g.GetComponents<InstantiatePrefabOnStart>() )
            //{
            //    if( ipos.prefab.name == "SurvivorPodBatteryPanel" ) prefabSpawn = ipos;
            //}

            //GameObject pref = prefabSpawn.prefab.InstantiateClone("WispPodPanel");
            //pref.AddComponent<WispSurvivorPodPanelController>();
            //prefabSpawn.prefab = pref;

            //Transform podMesh = podModel.Find("Base").Find("EscapePodMesh");
            //Transform doorMesh = podModel.Find("Base").Find("Door").Find("EscapePodDoorMesh");
            //Transform doorMesh2 = podModel.Find("Base").Find("ReleaseExhaustFX").Find("Door,Physics");

            //wispPodControl.podMesh = podMesh;
            //wispPodControl.doorMesh = doorMesh;
            //wispPodControl.doorMesh2 = doorMesh2;

            return g;
        }
    }

}
#endif