using BepInEx;
using RoR2;
using UnityEngine;
using R2API.Utils;
using System.Runtime.CompilerServices;
using System.Reflection;
using EntityStates;
using RoR2.Projectile;

namespace ReinArtificerer
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.ReinThings.ReinArtificerer", "ReinArtificerer", "1.0.0")]

    public class ReinArtificerer : BaseUnityPlugin
    {
        public void Start()
        {
            //var execAssembly = Assembly.GetExecutingAssembly();
            //var stream = execAssembly.GetManifestResourceStream("ReinArtificerer.artificermeshes");
            //var artiBundle = AssetBundle.LoadFromStream(stream);
            GameObject body = BodyCatalog.FindBodyPrefab("MageBody");

            ReinDataLibrary data = body.AddComponent<ReinDataLibrary>();
            ReinElementTracker element = body.AddComponent<ReinElementTracker>();
            ReinLightningBuffTracker lightning = body.AddComponent<ReinLightningBuffTracker>();
            element.data = data;
            data.element = element;
            data.lightning = lightning;
            //data.bundle = artiBundle;
            //data.g_ui = body.AddComponent<SniperUIController>();
            //data.g_ui.data = data;
            //data.bundle = sniperBundle;

            SkillLocator SL = body.GetComponent<SkillLocator>();
            CharacterBody charBody = body.GetComponent<CharacterBody>();
            element.body = charBody;
            lightning.body = charBody;

            var netStates = body.GetComponent<NetworkStateMachine>();
            var states = netStates.GetFieldValue<EntityStateMachine[]>("stateMachines");
            data.bodyState = states[0];
            data.weaponState = states[1];
            Debug.Log(data.bodyState.customName);
            Debug.Log(data.weaponState.customName);

            GenericSkill mage1 = SL.primary;
            GenericSkill mage2 = SL.secondary;
            GenericSkill mage3 = SL.utility;
            GenericSkill mage4 = SL.special;

            //charbody.baseDamage = data.g_baseDamage;
            //charbody.baseMaxHealth = data.g_baseHealth;
            //charbody.baseRegen = data.g_baseRegen;
            //charbody.crosshairPrefab = Resources.Load<GameObject>(data.g_crosshairString);


            //Config skills
            //mage1.baseRechargeInterval = data.p_rechargeInterval;
            //mage1.baseMaxStock = data.p_baseMaxStock;
            //mage1.rechargeStock = data.p_rechargeStock;
            //mage1.isBullets = data.p_isBullets;
            //mage1.shootDelay = data.p_shootDelay;
            //mage1.beginSkillCooldownOnSkillEnd = data.p_beginCDOnEnd;
            //mage1.isCombatSkill = data.p_isCombatSkill;
            //mage1.noSprint = data.p_noSprint;
            //mage1.mustKeyPress = data.p_mustKeyPress;
            //mage1.requiredStock = data.p_requiredStock;
            //mage1.stockToConsume = data.p_stockToConsome;
            mage1.SetFieldValue("activationState", new EntityStates.SerializableEntityStateType(typeof(EntityStates.ReinArtificerer.Artificer.Weapon.Primary)));


            //mage2.baseRechargeInterval = data.s_rechargeInterval;
            //mage2.baseMaxStock = data.s_baseMaxStock;
            //mage2.rechargeStock = data.s_rechargeStock;
            //mage2.isBullets = data.s_isBullets;
            //mage2.shootDelay = data.s_shootDelay;
            //mage2.beginSkillCooldownOnSkillEnd = data.s_beginCDOnEnd;
            //mage2.isCombatSkill = data.s_isCombatSkill;
            //mage2.noSprint = data.s_noSprint;
            //mage2.mustKeyPress = data.s_mustKeyPress;
            //mage2.requiredStock = data.s_requiredStock;
            //mage2.stockToConsume = data.s_stockToConsume;
            mage2.SetFieldValue("activationState", new EntityStates.SerializableEntityStateType(typeof(EntityStates.ReinArtificerer.Artificer.Weapon.Secondary)));


            //mage3.baseRechargeInterval = data.u_rechargeInterval;
            //mage3.baseMaxStock = data.u_baseMaxStock;
            //mage3.rechargeStock = data.u_rechargeStock;
            //mage3.isBullets = data.u_isBullets;
            //mage3.shootDelay = data.u_shootDelay;
            //mage3.beginSkillCooldownOnSkillEnd = data.u_beginCDOnEnd;
            //mage3.isCombatSkill = data.u_isCombatSkill;
            //mage3.noSprint = data.u_noSprint;
            //mage3.mustKeyPress = data.u_mustKeyPress;
            //mage3.requiredStock = data.u_requiredStock;
            //mage3.stockToConsume = data.u_stockToConsume;
            mage3.SetFieldValue("activationState", new EntityStates.SerializableEntityStateType(typeof(EntityStates.ReinArtificerer.Artificer.Weapon.Utility)));


            //mage4.baseRechargeInterval = data.r_rechargeInterval;
            //mage4.baseMaxStock = data.r_baseMaxStock;
            //mage4.rechargeStock = data.r_rechargeStock;
            //mage4.isBullets = data.r_isBullets;
            //mage4.shootDelay = data.r_shootDelay;
            //mage4.beginSkillCooldownOnSkillEnd = data.r_beginCDOnEnd;
            //mage4.isCombatSkill = data.r_isCombatSkill;
            //mage4.noSprint = data.r_noSprint;
            //mage4.mustKeyPress = data.r_mustKeyPress;
            //mage4.requiredStock = data.r_requiredStock;
            //mage4.stockToConsume = data.r_stocktoConsume;
            mage4.SetFieldValue("activationState", new EntityStates.SerializableEntityStateType(typeof( EntityStates.ReinArtificerer.Artificer.Weapon.Special)));


            //mage1.skillNameToken = "Snipe";
            //mage2.skillNameToken = "Steady Aim";
            //mage3.skillNameToken = "Military Training";
            //mage4.skillNameToken = "Snare trap";

            //mage1.skillDescriptionToken = "Shoot enemies for 250% damage. Must reload after shooting, good timing increases damage but bad timing decreases damage. Pushes you back slightly when fired.";
            //mage2.skillDescriptionToken = "Aim with the scope of your rifle, reducing speed but powering up Snipe to pierce enemies and increase damage with charge." + "\n" + "When fully charged fire a powerful shot with a cooldown." + "\n" + "Scope and normal charged shots can be used while on cooldown.";
            //mage3.skillDescriptionToken = "Launch yourself a forward along the direction you are currently moving. Gain very short duration invisibility that causes enemies to lose track of you momentarily";
            //mage4.skillDescriptionToken = "Place a trap. When activated the trap pulls nearby enemies in, slows them, and reduces their armor.";

            //SL.passiveSkill.enabled = true;
            //SL.passiveSkill.skillNameToken = "Headshot";
            //SL.passiveSkill.skillDescriptionToken = "Some enemies can be shot in the head for 50% bonus damage. [Currently only works on Lemurians, Beetles, and Beetle Guards, more coming hopefully soon]";


            //var survivor = new SurvivorDef
            //{
            //    bodyPrefab = body,
            //    descriptionToken = "Sniper is a high single-target damage survivor with moderate mobility." + "\n\n\n" +
            //    "Snipe is your only source of damage, hitting perfect reloads is essential to keeping a fast pace. " + "\n" + "The recoil can be used to extend time in the air." + "\n" + "Attack speed can make it more difficult to time reloads, but never makes it impossible." + "\n\n\n" +
            //    "Steady Aim is a high-risk high-reward option that takes a very long time to charge, but does incredible damage." + "\n\n\n" +
            //    "Military Training is essential to staying alive. When it is on cooldown you are at your most vulnerable." + "\n\n\n" +
            //    "Snare Trap deals no damage, but can be used for a variety of purposes: Weakening high priority targets, grouping up enemies for Steady Aim, or simply to slow enemies that are chasing you.",
            //    displayPrefab = Resources.Load<GameObject>("Prefabs/Characters/SniperDisplay"),
            //    primaryColor = new Color(0.25f, 0.25f, 0.25f),
            //    unlockableName = "",
            //   survivorIndex = SurvivorIndex.Count
            //};
            //R2API.SurvivorAPI.AddSurvivorOnReady(survivor);
        }

    }
}


