using BepInEx;
using RoR2;
using UnityEngine;
using R2API;
using R2API.Utils;
using System.Reflection;
using EntityStates;
using RoR2.Skills;
using System;

namespace ReinSniperRework
{
    [R2APISubmoduleDependency(nameof(R2API.SurvivorAPI))]
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.ReinThings.ReinSniperRework", "ReinSniperRework", "1.0.8")]
    public class ReinSniperReworkMain : BaseUnityPlugin
    {

        public void Awake()
        {
            var execAssembly = Assembly.GetExecutingAssembly();
            var stream = execAssembly.GetManifestResourceStream("ReinSniperRework.sniperassetbundle");
            var sniperBundle = AssetBundle.LoadFromStream(stream);

            GameObject body = Resources.Load<GameObject>("prefabs/characterbodies/sniperbody");

            ReinDataLibrary data = body.AddComponent<ReinDataLibrary>();
            data.g_ui = body.AddComponent<SniperUIController>();
            data.g_ui.data = data;
            data.bundle = sniperBundle;

            SkillLocator SL = body.GetComponent<SkillLocator>();
            CharacterBody charbody = body.GetComponent<CharacterBody>();
            SetStateOnHurt hurtState = body.AddComponent<SetStateOnHurt>();

            charbody.name = "Sniper";
            charbody.baseNameToken = "Sniper";
            charbody.subtitleNameToken = "The sniping guy!";

            hurtState.canBeFrozen = true;
            hurtState.canBeHitStunned = false;
            hurtState.canBeStunned = false;
            hurtState.hitThreshold = 5f;

            hurtState.hurtState = new SerializableEntityStateType(EntityState.Instantiate(219).GetType());

            int i = 0;
            EntityStateMachine[] esmr = new EntityStateMachine[2];
            foreach( EntityStateMachine esm in body.GetComponentsInChildren<EntityStateMachine>())
            {
                switch (esm.customName)
                {
                    case "Body":
                        hurtState.targetStateMachine = esm;
                        break;
                    default:
                        if (i < 2)
                        {
                            esmr[i] = esm;
                            //Debug.Log(esm.customName);
                        }
                        i++;
                        //Debug.Log(i);
                        break;
                }
            }

            hurtState.idleStateMachine = esmr;

            GenericSkill Sniper1G = SL.primary;
            GenericSkill Sniper2G = SL.secondary;
            GenericSkill Sniper3G = SL.utility;
            GenericSkill Sniper4G = SL.special;

            SkillFamily SniperFam1 = Sniper1G.skillFamily;
            SkillFamily SniperFam2 = Sniper2G.skillFamily;
            SkillFamily SniperFam3 = Sniper3G.skillFamily;
            SkillFamily SniperFam4 = Sniper4G.skillFamily;

            SkillDef Sniper1 = SniperFam1.variants[SniperFam1.defaultVariantIndex].skillDef;
            SkillDef Sniper2 = SniperFam2.variants[SniperFam2.defaultVariantIndex].skillDef;
            SkillDef Sniper3 = SniperFam3.variants[SniperFam3.defaultVariantIndex].skillDef;
            SkillDef Sniper4 = SniperFam4.variants[SniperFam4.defaultVariantIndex].skillDef;

            charbody.baseDamage = data.g_baseDamage;
            charbody.levelDamage = data.g_baseDamage * 0.2f;
            charbody.baseMaxHealth = data.g_baseHealth;
            charbody.levelMaxHealth = data.g_baseHealth * 0.3f;
            charbody.baseRegen = data.g_baseRegen;
            charbody.levelRegen = data.g_baseRegen * 0.3f;
            charbody.crosshairPrefab = Resources.Load<GameObject>(data.g_crosshairString);
            charbody.bodyFlags = CharacterBody.BodyFlags.ImmuneToExecutes;

            charbody.baseNameToken = "Sniper";

            //Config skills
            Sniper1.SetFieldValue("baseRechargeInterval",  data.p_rechargeInterval);
            Sniper1.SetFieldValue("baseMaxStock" , data.p_baseMaxStock);
            Sniper1.SetFieldValue("rechargeStock" , data.p_rechargeStock);
            Sniper1.SetFieldValue("isBullets" , data.p_isBullets);
            Sniper1.SetFieldValue("shootDelay" , data.p_shootDelay);
            Sniper1.SetFieldValue("beginSkillCooldownOnSkillEnd" , data.p_beginCDOnEnd);
            Sniper1.SetFieldValue("isCombatSkill" , data.p_isCombatSkill);
            Sniper1.SetFieldValue("noSprint" , data.p_noSprint);
            Sniper1.SetFieldValue("mustKeyPress" , data.p_mustKeyPress);
            Sniper1.SetFieldValue("requiredStock" , data.p_requiredStock);
            Sniper1.SetFieldValue("stockToConsume" , data.p_stockToConsome);
            Sniper1.SetFieldValue("activationState", new EntityStates.SerializableEntityStateType(typeof(EntityStates.ReinSniperRework.SniperWeapon.SniperPrimary)));

            Sniper2.SetFieldValue("baseRechargeInterval" , data.s_rechargeInterval);
            Sniper2.SetFieldValue("baseMaxStock" , data.s_baseMaxStock);
            Sniper2.SetFieldValue("rechargeStock" , data.s_rechargeStock);
            Sniper2.SetFieldValue("isBullets" , data.s_isBullets);
            Sniper2.SetFieldValue("shootDelay" , data.s_shootDelay);
            Sniper2.SetFieldValue("beginSkillCooldownOnSkillEnd" , data.s_beginCDOnEnd);
            Sniper2.SetFieldValue("isCombatSkill" , data.s_isCombatSkill);
            Sniper2.SetFieldValue("noSprint" , data.s_noSprint);
            Sniper2.SetFieldValue("mustKeyPress" , data.s_mustKeyPress);
            Sniper2.SetFieldValue("requiredStock" , data.s_requiredStock);
            Sniper2.SetFieldValue("stockToConsume" , data.s_stockToConsume);
            Sniper2.SetFieldValue("activationState", new EntityStates.SerializableEntityStateType(typeof(EntityStates.ReinSniperRework.SniperWeapon.SniperSecondary)));


            Sniper3.SetFieldValue("baseRechargeInterval" , data.u_rechargeInterval);
            Sniper3.SetFieldValue("baseMaxStock" , data.u_baseMaxStock);
            Sniper3.SetFieldValue("rechargeStock" , data.u_rechargeStock);
            Sniper3.SetFieldValue("isBullets" , data.u_isBullets);
            Sniper3.SetFieldValue("shootDelay" , data.u_shootDelay);
            Sniper3.SetFieldValue("beginSkillCooldownOnSkillEnd" , data.u_beginCDOnEnd);
            Sniper3.SetFieldValue("isCombatSkill" , data.u_isCombatSkill);
            Sniper3.SetFieldValue("noSprint" , data.u_noSprint);
            Sniper3.SetFieldValue("mustKeyPress" , data.u_mustKeyPress);
            Sniper3.SetFieldValue("requiredStock" , data.u_requiredStock);
            Sniper3.SetFieldValue("stockToConsume" , data.u_stockToConsume);
            Sniper3.SetFieldValue("activationState", new EntityStates.SerializableEntityStateType(typeof(EntityStates.ReinSniperRework.SniperWeapon.SniperUtility)));


            Sniper4.SetFieldValue("baseRechargeInterval" , data.r_rechargeInterval);
            Sniper4.SetFieldValue("baseMaxStock" , data.r_baseMaxStock);
            Sniper4.SetFieldValue("rechargeStock" , data.r_rechargeStock);
            Sniper4.SetFieldValue("isBullets" , data.r_isBullets);
            Sniper4.SetFieldValue("shootDelay" , data.r_shootDelay);
            Sniper4.SetFieldValue("beginSkillCooldownOnSkillEnd" , data.r_beginCDOnEnd);
            Sniper4.SetFieldValue("isCombatSkill" , data.r_isCombatSkill);
            Sniper4.SetFieldValue("noSprint" , data.r_noSprint);
            Sniper4.SetFieldValue("mustKeyPress" , data.r_mustKeyPress);
            Sniper4.SetFieldValue("requiredStock" , data.r_requiredStock);
            Sniper4.SetFieldValue("stockToConsume" , data.r_stocktoConsume);
            Sniper4.SetFieldValue("activationState", new EntityStates.SerializableEntityStateType(typeof(EntityStates.ReinSniperRework.SniperWeapon.SniperSpecial)));

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
            SL.passiveSkill.skillDescriptionToken = "Some enemies can be shot in the head for 50% bonus damage.";

            var survivor = new SurvivorDef
            {
                bodyPrefab = body,
                descriptionToken = "Sniper is a high single-target damage survivor with moderate mobility.",
                displayPrefab = Resources.Load<GameObject>("Prefabs/Characters/SniperDisplay"),
                primaryColor = new Color(0.15f, 0.15f, 0.15f),
                unlockableName = "",
            };
            R2API.SurvivorAPI.AddSurvivor(survivor);
            

        }

        public void Start()
        {
            SniperHeadshotHitboxStuff.AddHurtboxes();
        }
        
        
        
    }
}


