using BepInEx;
using System;
using RoR2;
using UnityEngine;
using System.Reflection;


namespace ReinAcrid
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.ReinThings.ReinSniperRework", "ReinSniperRework", "1.0.1")]

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

                data.g_reload.data = data;
                data.g_charge.data = data;

                SkillLocator SL = body.GetComponent<SkillLocator>();
                CharacterBody charbody = body.GetComponent<CharacterBody>();

                GenericSkill Sniper1 = SL.primary;
                GenericSkill Sniper2 = SL.secondary;
                GenericSkill Sniper3 = SL.utility;
                GenericSkill Sniper4 = SL.special;

                charbody.baseDamage = data.g_baseDamage;
                charbody.baseMaxHealth = data.g_baseHealth;
                charbody.baseRegen = data.g_baseRegen;
                charbody.crosshairPrefab = Resources.Load<GameObject>(data.g_crosshairString);

                //Config skill1
                Sniper1.baseRechargeInterval = data.p_rechargeInterval;
                Sniper1.baseMaxStock = data.p_baseMaxStock;
                Sniper1.rechargeStock = data.p_rechargeStock;
                Sniper1.isBullets = data.p_isBullets;
                Sniper1.shootDelay = data.p_shootDelay;
                Sniper1.beginSkillCooldownOnSkillEnd = data.p_beginCDOnEnd;
                Sniper1.isCombatSkill = data.p_isCombatSkill;
                Sniper1.noSprint = data.p_noSprint;
                Sniper1.mustKeyPress = data.p_mustKeyPress;
                Sniper1.requiredStock = data.p_requiredStock;
                Sniper1.stockToConsume = data.p_stockToConsome;
                Sniper1.activationState = new EntityStates.SerializableEntityStateType(typeof(EntityStates.ReinSniperRework.SniperWeapon.TestSniperPrimary));
                object box = Sniper1.activationState;
                var field = typeof(EntityStates.SerializableEntityStateType)?.GetField("_typeName", BindingFlags.NonPublic | BindingFlags.Instance);
                field?.SetValue(box, typeof(EntityStates.ReinSniperRework.SniperWeapon.TestSniperPrimary)?.AssemblyQualifiedName);
                Sniper1.activationState = (EntityStates.SerializableEntityStateType)box;

                Sniper2.baseRechargeInterval = data.s_rechargeInterval;
                Sniper2.baseMaxStock = data.s_baseMaxStock;
                Sniper2.rechargeStock = data.s_rechargeStock;
                Sniper2.isBullets = data.s_isBullets;
                Sniper2.shootDelay = data.s_shootDelay;
                Sniper2.beginSkillCooldownOnSkillEnd = data.s_beginCDOnEnd;
                Sniper2.isCombatSkill = data.s_isCombatSkill;
                Sniper2.noSprint = data.s_noSprint;
                Sniper2.mustKeyPress = data.s_mustKeyPress;
                Sniper2.requiredStock = data.s_requiredStock;
                Sniper2.stockToConsume = data.s_stockToConsume;
                Sniper2.activationState = new EntityStates.SerializableEntityStateType(typeof(EntityStates.ReinSniperRework.SniperWeapon.SniperSecondary));
                object box2 = Sniper2.activationState;
                var field2 = typeof(EntityStates.SerializableEntityStateType)?.GetField("_typeName", BindingFlags.NonPublic | BindingFlags.Instance);
                field2?.SetValue(box2, typeof(EntityStates.ReinSniperRework.SniperWeapon.SniperSecondary)?.AssemblyQualifiedName);
                Sniper2.activationState = (EntityStates.SerializableEntityStateType)box2;

                Sniper3.baseRechargeInterval = data.u_rechargeInterval;
                Sniper3.baseMaxStock = data.u_baseMaxStock;
                Sniper3.rechargeStock = data.u_rechargeStock;
                Sniper3.isBullets = data.u_isBullets;
                Sniper3.shootDelay = data.u_shootDelay;
                Sniper3.beginSkillCooldownOnSkillEnd = data.u_beginCDOnEnd;
                Sniper3.isCombatSkill = data.u_isCombatSkill;
                Sniper3.noSprint = data.u_noSprint;
                Sniper3.mustKeyPress = data.u_mustKeyPress;
                Sniper3.requiredStock = data.u_requiredStock;
                Sniper3.stockToConsume = data.u_stockToConsume;
                Sniper3.activationState = new EntityStates.SerializableEntityStateType(typeof(EntityStates.ReinSniperRework.SniperWeapon.SniperUtility));
                object box3 = Sniper3.activationState;
                var field3 = typeof(EntityStates.SerializableEntityStateType)?.GetField("_typeName", BindingFlags.NonPublic | BindingFlags.Instance);
                field3?.SetValue(box3, typeof(EntityStates.ReinSniperRework.SniperWeapon.SniperUtility)?.AssemblyQualifiedName);
                Sniper3.activationState = (EntityStates.SerializableEntityStateType)box3;

                Sniper4.baseRechargeInterval = data.r_rechargeInterval;
                Sniper4.baseMaxStock = data.r_baseMaxStock;
                Sniper4.rechargeStock = data.r_rechargeStock;
                Sniper4.isBullets = data.r_isBullets;
                Sniper4.shootDelay = data.r_shootDelay;
                Sniper4.beginSkillCooldownOnSkillEnd = data.r_beginCDOnEnd;
                Sniper4.isCombatSkill = data.r_isCombatSkill;
                Sniper4.noSprint = data.r_noSprint;
                Sniper4.mustKeyPress = data.r_mustKeyPress;
                Sniper4.requiredStock = data.r_requiredStock;
                Sniper4.stockToConsume = data.r_stocktoConsume;
                Sniper4.activationState = new EntityStates.SerializableEntityStateType(typeof(EntityStates.ReinSniperRework.SniperWeapon.SniperSpecial));
                object box4 = Sniper4.activationState;
                var field4 = typeof(EntityStates.SerializableEntityStateType)?.GetField("_typeName", BindingFlags.NonPublic | BindingFlags.Instance);
                field4?.SetValue(box4, typeof(EntityStates.ReinSniperRework.SniperWeapon.SniperSpecial)?.AssemblyQualifiedName);
                Sniper4.activationState = (EntityStates.SerializableEntityStateType)box4;

                Sniper1.skillNameToken = "Snipe";
                Sniper2.skillNameToken = "Steady Aim";
                Sniper3.skillNameToken = "Military Training";
                Sniper4.skillNameToken = "Snare trap";

                Sniper1.skillDescriptionToken = "Shoot enemies for 250% damage. Must reload after shooting, good timing increases damage but bad timing decreases damage. Pushes you back slightly when fired.";
                Sniper2.skillDescriptionToken = "Aim with the scope of your rifle, reducing speed but powering up Snipe to pierce enemies and increase damage with charge." + "\n" + "When fully charged fire a powerful shot with a cooldown." + "\n" + "Scope and normal charged shots can be used while on cooldown.";
                Sniper3.skillDescriptionToken = "Launch yourself a forward along the direction you are currently moving. Gain very short duration invisibility that causes enemies to lose track of you momentarily";
                Sniper4.skillDescriptionToken = "Place a trap. When activated the trap pulls nearby enemies in, slows them, and reduces their armor.";

                SL.passiveSkill.enabled = true;
                SL.passiveSkill.skillNameToken = "Headshot";
                SL.passiveSkill.skillDescriptionToken = "Some enemies can be shot in the head for 50% bonus damage. [Currently only works on Lemurians, Beetles, and Beetle Guards, more coming hopefully soon]";


                var survivor = new SurvivorDef
                {
                    bodyPrefab = body,
                    descriptionToken = "Sniper is a high single-target damage survivor with moderate mobility." + "\n\n\n" +
                    "Snipe is your only source of damage, hitting perfect reloads is essential to keeping a fast pace. " + "\n" + "The recoil can be used to extend time in the air." + "\n" + "Attack speed can make it more difficult to time reloads, but never makes it impossible." + "\n\n\n" +
                    "Steady Aim is a high-risk high-reward option that takes a very long time to charge, but does incredible damage." + "\n\n\n" +
                    "Military Training is essential to staying alive. When it is on cooldown you are at your most vulnerable." + "\n\n\n" +
                    "Snare Trap deals no damage, but can be used for a variety of purposes: Weakening high priority targets, grouping up enemies for Steady Aim, or simply to slow enemies that are chasing you.",
                    displayPrefab = Resources.Load<GameObject>("Prefabs/Characters/SniperDisplay"),
                    primaryColor = new Color(0.25f, 0.25f, 0.25f),
                    unlockableName = "",
                    survivorIndex = SurvivorIndex.Count
                };
                R2API.SurvivorAPI.AddSurvivorOnReady(survivor);
            };
        }
    }
}


