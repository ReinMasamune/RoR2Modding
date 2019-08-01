using BepInEx;
using System;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Reflection;
using ReinSniperRework;
using RoR2.UI;
using R2API;

namespace ReinSniperRework
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.ReinThings.ReinSurvivorMod", "ReinSurvivorMod", "0.0.3")]

    public class ReinSurvivorMod : BaseUnityPlugin
    {
        public void Awake()
        {
            R2API.SurvivorAPI.SurvivorCatalogReady += delegate (object s, EventArgs e)
            {
                GameObject body = BodyCatalog.FindBodyPrefab("SniperBody");

                ReinDataLibrary data = body.AddComponent<ReinDataLibrary>();
                data.g_reload = body.AddComponent<SniperReloadTracker>();
                data.g_charge = body.AddComponent<SniperChargeTracker>();

                SkillLocator SL = body.GetComponent<SkillLocator>();
                CharacterBody charbody = body.GetComponent<CharacterBody>();

                GenericSkill Sniper1 = SL.primary;
                GenericSkill Sniper2 = SL.secondary;
                GenericSkill Sniper3 = SL.utility;
                GenericSkill Sniper4 = SL.special;

                charbody.baseDamage = 16f;
                charbody.baseMaxHealth = 110f;
                charbody.baseRegen = 0.6f;
                charbody.crosshairPrefab = Resources.Load<GameObject>("prefabs/crosshair/banditcrosshairrevolver");

                //Config skill1
                Sniper1.baseRechargeInterval = 0.001f;
                Sniper1.baseMaxStock = 1;
                Sniper1.rechargeStock = 1;
                Sniper1.isBullets = false;
                Sniper1.shootDelay = 0f;
                Sniper1.beginSkillCooldownOnSkillEnd = false;
                Sniper1.isCombatSkill = true;
                Sniper1.noSprint = true;
                Sniper1.mustKeyPress = true;
                Sniper1.requiredStock = 1;
                Sniper1.stockToConsume = 1;
                Sniper1.activationState = new EntityStates.SerializableEntityStateType(typeof(EntityStates.ReinSniperRework.SniperWeapon.SniperPrimary));
                object box = Sniper1.activationState;
                var field = typeof(EntityStates.SerializableEntityStateType)?.GetField("_typeName", BindingFlags.NonPublic | BindingFlags.Instance);
                field?.SetValue(box, typeof(EntityStates.ReinSniperRework.SniperWeapon.SniperPrimary)?.AssemblyQualifiedName);
                Sniper1.activationState = (EntityStates.SerializableEntityStateType)box;

                Sniper2.baseRechargeInterval = 30f;
                Sniper2.baseMaxStock = 1;
                Sniper2.rechargeStock = 1;
                Sniper2.isBullets = false;
                Sniper2.shootDelay = 0.1f;
                Sniper2.beginSkillCooldownOnSkillEnd = false;
                Sniper2.isCombatSkill = false;
                Sniper2.noSprint = true;
                Sniper2.mustKeyPress = false;
                Sniper2.requiredStock = 0;
                Sniper2.stockToConsume = 0;
                Sniper2.activationState = new EntityStates.SerializableEntityStateType(typeof(EntityStates.ReinSniperRework.SniperWeapon.SniperSecondary));
                object box2 = Sniper2.activationState;
                var field2 = typeof(EntityStates.SerializableEntityStateType)?.GetField("_typeName", BindingFlags.NonPublic | BindingFlags.Instance);
                field2?.SetValue(box2, typeof(EntityStates.ReinSniperRework.SniperWeapon.SniperSecondary)?.AssemblyQualifiedName);
                Sniper2.activationState = (EntityStates.SerializableEntityStateType)box2;

                Sniper3.baseRechargeInterval = 8f;
                Sniper3.baseMaxStock = 1;
                Sniper3.rechargeStock = 1;
                Sniper3.isBullets = false;
                Sniper3.shootDelay = 0.25f;
                Sniper3.beginSkillCooldownOnSkillEnd = true;
                Sniper3.isCombatSkill = false;
                Sniper3.noSprint = false;
                Sniper3.mustKeyPress = true;
                Sniper3.requiredStock = 1;
                Sniper3.stockToConsume = 1;
                Sniper3.activationState = new EntityStates.SerializableEntityStateType(typeof(EntityStates.ReinSniperRework.SniperWeapon.SniperUtility));
                object box3 = Sniper3.activationState;
                var field3 = typeof(EntityStates.SerializableEntityStateType)?.GetField("_typeName", BindingFlags.NonPublic | BindingFlags.Instance);
                field3?.SetValue(box3, typeof(EntityStates.ReinSniperRework.SniperWeapon.SniperUtility)?.AssemblyQualifiedName);
                Sniper3.activationState = (EntityStates.SerializableEntityStateType)box3;

                Sniper4.baseRechargeInterval = 30f;
                Sniper4.baseMaxStock = 1;
                Sniper4.rechargeStock = 1;
                Sniper4.isBullets = false;
                Sniper4.shootDelay = 0.1f;
                Sniper4.beginSkillCooldownOnSkillEnd = false;
                Sniper4.isCombatSkill = true;
                Sniper4.noSprint = true;
                Sniper4.mustKeyPress = true;
                Sniper4.requiredStock = 1;
                Sniper4.stockToConsume = 1;
                Sniper4.activationState = new EntityStates.SerializableEntityStateType(typeof(EntityStates.ReinSniperRework.SniperWeapon.SniperSpecial));
                object box4 = Sniper4.activationState;
                var field4 = typeof(EntityStates.SerializableEntityStateType)?.GetField("_typeName", BindingFlags.NonPublic | BindingFlags.Instance);
                field4?.SetValue(box4, typeof(EntityStates.ReinSniperRework.SniperWeapon.SniperSpecial)?.AssemblyQualifiedName);
                Sniper4.activationState = (EntityStates.SerializableEntityStateType)box4;

                var survivor = new SurvivorDef
                {
                    bodyPrefab = body,
                    descriptionToken = "But it is really sniper!",
                    displayPrefab = Resources.Load<GameObject>("Prefabs/Characters/SniperDisplay"),
                    primaryColor = new Color(0.8039216f, 0.482352942f, 0.843137264f),
                    unlockableName = "",
                    survivorIndex = SurvivorIndex.Count
                };
                R2API.SurvivorAPI.AddSurvivorOnReady(survivor);
            };
        }
    }
}


