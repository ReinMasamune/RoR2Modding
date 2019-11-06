using RoR2;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace WispSurvivor.Modules
{
    public static class WispBodySetupModule
    {
        public static void DoModule( GameObject body , Dictionary<Type,Component> dic)
        {
            CharBodyStats(body, dic);
            CharBodyOther(body, dic);
        }

        private static void CharBodyStats( GameObject body, Dictionary<Type,Component> dic )
        {
            CharacterBody chbod = dic.C<CharacterBody>();
            chbod.baseMaxHealth = 130.0f;
            chbod.levelMaxHealth = 39.0f;
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
            chbod.baseArmor = 5f;
            chbod.levelArmor = 0f;
            chbod.baseJumpCount = 1;
            chbod.baseAcceleration = 60.0f;
            chbod.spreadBloomDecayTime = 1f;
        }

        private static void CharBodyOther(GameObject body, Dictionary<Type, Component> dic)
        {
            CharacterBody chbod = dic.C<CharacterBody>();
            chbod.bodyFlags = CharacterBody.BodyFlags.ImmuneToExecutes;
            chbod.hullClassification = HullClassification.Human;
            chbod.isChampion = false;
            chbod.crosshairPrefab = WispUIModule.crosshair;
            chbod.subtitleNameToken = "ReinThings.WispSurvivor";
            chbod.baseNameToken = "WISP_SURVIVOR_BODY_NAME";
        }

        private static T C<T>( this Dictionary<Type,Component> dic ) where T : Component
        {
            return dic[typeof(T)] as T;
        }
    }
}
