using EntityStates;
using R2API.Utils;
using RoR2;
//using static RogueWispPlugin.Helpers.APIInterface;
using RoR2.Skills;
using System;
using UnityEngine;

namespace RogueWispPlugin
{
#if ROGUEWISP
    internal partial class Main
    {
        private SkillFamily[] RW_skillFamilies;
        partial void RW_Skill()
        {
            this.Load += this.RW_RegisterSkillStates;
            this.Load += this.RW_SetupGenericSkills;
            this.Load += this.RW_SetupSkillFamilies;
            this.Load += this.RW_DoStateMachines;
            this.Load += this.RW_DoPassive;
            this.Load += this.RW_DoPrimaries;
            this.Load += this.RW_DoSecondaries;
            this.Load += this.RW_DoUtilities;
            this.Load += this.RW_DoSpecials;
        }

        private void RW_DoSpecials()
        {
            SkillDef[] specials = new SkillDef[1];
            SkillDef skill = ScriptableObject.CreateInstance<SkillDef>();
            skill.activationState = new SerializableEntityStateType( typeof( IncinerationWindup ) );
            skill.activationStateMachineName = "Weapon";

            skill.baseMaxStock = 1;
            skill.baseRechargeInterval = 6f;
            skill.beginSkillCooldownOnSkillEnd = true;
            skill.canceledFromSprinting = false;
            skill.fullRestockOnAssign = true;
            skill.interruptPriority = InterruptPriority.PrioritySkill;
            skill.isBullets = false;
            skill.isCombatSkill = true;
            skill.mustKeyPress = true;
            skill.noSprint = true;
            skill.rechargeStock = 1;
            skill.requiredStock = 1;
            skill.shootDelay = 0.5f;
            skill.stockToConsume = 1;

            skill.icon = this.RW_assetBundle.LoadAsset<Sprite>( "Assets/__EXPORT/wisp4alt.png" );
            skill.skillDescriptionToken = "WISP_SURVIVOR_SPECIAL_1_DESC";
            skill.skillName = "Special1";
            skill.skillNameToken = "WISP_SURVIVOR_SPECIAL_1_NAME";


            specials[0] = skill;

            AssignVariants( this.RW_skillFamilies[3], specials );
        }
        private void RW_DoUtilities()
        {
            SkillDef[] utilities = new SkillDef[1];
            SkillDef skill = ScriptableObject.CreateInstance<SkillDef>();
            skill.activationState = new SerializableEntityStateType( typeof( PrepGaze ) );
            skill.activationStateMachineName = "Gaze";

            skill.baseMaxStock = 1;
            skill.baseRechargeInterval = 14.99999f;
            skill.beginSkillCooldownOnSkillEnd = true;
            skill.canceledFromSprinting = false;
            skill.fullRestockOnAssign = true;
            skill.interruptPriority = InterruptPriority.Skill;
            skill.isBullets = false;
            skill.isCombatSkill = true;
            skill.mustKeyPress = true;
            skill.noSprint = false;
            skill.rechargeStock = 1;
            skill.requiredStock = 1;
            skill.shootDelay = 0.5f;
            skill.stockToConsume = 1;

            skill.icon = this.RW_assetBundle.LoadAsset<Sprite>( "Assets/__EXPORT/wisp3alt.png" );
            skill.skillDescriptionToken = "WISP_SURVIVOR_UTILITY_1_DESC";
            skill.skillName = "Utility1";
            skill.skillNameToken = "WISP_SURVIVOR_UTILITY_1_NAME";


            utilities[0] = skill;

            AssignVariants( this.RW_skillFamilies[2], utilities );
        }
        private void RW_DoSecondaries()
        {
            SkillDef[] secondaries = new SkillDef[1];

            SkillDef skill = ScriptableObject.CreateInstance<SkillDef>();
            skill.activationState = new SerializableEntityStateType( typeof( TestSecondary ) );
            skill.activationStateMachineName = "Weapon";

            skill.baseMaxStock = 1;
            skill.baseRechargeInterval = 6f;
            skill.beginSkillCooldownOnSkillEnd = true;
            skill.canceledFromSprinting = false;
            skill.fullRestockOnAssign = true;
            skill.interruptPriority = InterruptPriority.Skill;
            skill.isBullets = false;
            skill.isCombatSkill = true;
            skill.mustKeyPress = true;
            skill.noSprint = true;
            skill.rechargeStock = 1;
            skill.requiredStock = 1;
            skill.shootDelay = 0.5f;
            skill.stockToConsume = 1;

            skill.icon = this.RW_assetBundle.LoadAsset<Sprite>( "Assets/__EXPORT/wisp2alt.png" );
            skill.skillDescriptionToken = "WISP_SURVIVOR_SECONDARY_1_DESC";
            skill.skillName = "Secondary1";
            skill.skillNameToken = "WISP_SURVIVOR_SECONDARY_1_NAME";


            secondaries[0] = skill;

            AssignVariants( this.RW_skillFamilies[1], secondaries );
        }
        private void RW_DoPrimaries()
        {
            SkillDef[] primaries = new SkillDef[1];


            SteppedSkillDef skill = ScriptableObject.CreateInstance<SteppedSkillDef>();
            skill.activationState = new SerializableEntityStateType( typeof( Heatwave ) );
            skill.activationStateMachineName = "Weapon";

            skill.baseMaxStock = 1;
            skill.baseRechargeInterval = 0f;
            skill.beginSkillCooldownOnSkillEnd = true;
            skill.canceledFromSprinting = false;
            skill.fullRestockOnAssign = true;
            skill.interruptPriority = InterruptPriority.Any;
            skill.isBullets = true;
            skill.isCombatSkill = true;
            skill.mustKeyPress = false;
            skill.noSprint = true;
            skill.rechargeStock = 1;
            skill.requiredStock = 1;
            skill.shootDelay = 0.5f;
            skill.stockToConsume = 1;
            skill.stepCount = 2;

            skill.icon = this.RW_assetBundle.LoadAsset<Sprite>( "Assets/__EXPORT/wisp1.png" );
            skill.skillDescriptionToken = "WISP_SURVIVOR_PRIMARY_1_DESC";
            skill.skillName = "Primry1";
            skill.skillNameToken = "WISP_SURVIVOR_PRIMARY_1_NAME";

            primaries[0] = skill;

            AssignVariants( this.RW_skillFamilies[0], primaries );
        }
        private void RW_DoPassive()
        {
            SkillLocator sl = this.RW_body.GetComponent<SkillLocator>();
            sl.passiveSkill.enabled = true;
            sl.passiveSkill.icon = this.RW_assetBundle.LoadAsset<Sprite>( "Assets/__EXPORT/WispyPassiveIcon2.png" );
            sl.passiveSkill.skillNameToken = "WISP_SURVIVOR_PASSIVE_NAME";
            sl.passiveSkill.skillDescriptionToken = "WISP_SURVIVOR_PASSIVE_DESC";
        }
        private void RW_DoStateMachines()
        {
            NetworkStateMachine net = this.RW_body.GetComponent<NetworkStateMachine>();
            CharacterDeathBehavior death = this.RW_body.GetComponent<CharacterDeathBehavior>();
            death.idleStateMachine = new EntityStateMachine[2];
            death.deathState = new EntityStates.SerializableEntityStateType( typeof( EntityStates.Commando.DeathState ) );

            EntityStateMachine[] netStates = net.GetFieldValue<EntityStateMachine[]>("stateMachines");
            Array.Resize<EntityStateMachine>( ref netStates, 3 );

            SetStateOnHurt hurtState = this.RW_body.GetComponent<SetStateOnHurt>();
            hurtState.canBeFrozen = true;
            hurtState.canBeHitStunned = false;
            hurtState.canBeStunned = true;
            hurtState.hitThreshold = 5f;
            hurtState.hurtState = new SerializableEntityStateType( typeof( EntityStates.FrozenState ) );

            foreach( EntityStateMachine esm in this.RW_body.GetComponents<EntityStateMachine>() )
            {
                switch( esm.customName )
                {
                    case "Body":
                        esm.initialStateType = new SerializableEntityStateType( typeof( SpawnTeleporterState ) );
                        esm.mainStateType = new SerializableEntityStateType( typeof( GenericCharacterMain ) );
                        netStates[0] = esm;
                        hurtState.targetStateMachine = esm;
                        death.deathStateMachine = esm;
                        break;

                    case "Weapon":
                        esm.initialStateType = new SerializableEntityStateType( typeof( Idle ) );
                        esm.mainStateType = new SerializableEntityStateType( typeof( Idle ) );
                        netStates[1] = esm;
                        death.idleStateMachine[0] = esm;
                        break;

                    case "Gaze":
                        esm.initialStateType = new SerializableEntityStateType( typeof( Idle ) );
                        esm.mainStateType = new SerializableEntityStateType( typeof( Idle ) );
                        netStates[2] = esm;
                        death.idleStateMachine[1] = esm;
                        break;

                    default:
                        break;
                }
            }

            net.SetFieldValue<EntityStateMachine[]>( "stateMachines", netStates );
        }
        private void RW_SetupSkillFamilies()
        {
            SkillLocator skillLocator = this.RW_body.GetComponent<SkillLocator>();
            this.RW_skillFamilies = new SkillFamily[4];
            this.RW_skillFamilies[0] = GetNewSkillFamily( skillLocator.primary );
            this.RW_skillFamilies[1] = GetNewSkillFamily( skillLocator.secondary );
            this.RW_skillFamilies[2] = GetNewSkillFamily( skillLocator.utility );
            this.RW_skillFamilies[3] = GetNewSkillFamily( skillLocator.special );
        }
        private void RW_SetupGenericSkills()
        {
            foreach( GenericSkill g in this.RW_body.GetComponents<GenericSkill>() )
            {
                MonoBehaviour.DestroyImmediate( g );
            }

            SkillLocator SL = this.RW_body.AddOrGetComponent<SkillLocator>();
            if( !SL.primary )
            {
                SL.primary = this.RW_body.AddComponent<GenericSkill>();
            }
            if( !SL.secondary )
            {
                SL.secondary = this.RW_body.AddComponent<GenericSkill>();
            }
            if( !SL.utility )
            {
                SL.utility = this.RW_body.AddComponent<GenericSkill>();
            }
            if( !SL.special )
            {
                SL.special = this.RW_body.AddComponent<GenericSkill>();
            }
        }
        private void RW_RegisterSkillStates()
        {
            R2API.SkillAPI.AddSkill( typeof( HeatwaveWindDown ) );
            R2API.SkillAPI.AddSkill( typeof( Heatwave ) );
            R2API.SkillAPI.AddSkill( typeof( TestSecondary ) );
            R2API.SkillAPI.AddSkill( typeof( PrepGaze ) );
            R2API.SkillAPI.AddSkill( typeof( FireGaze ) );
            R2API.SkillAPI.AddSkill( typeof( IncinerationWindup ) );
            R2API.SkillAPI.AddSkill( typeof( Incineration ) );
            R2API.SkillAPI.AddSkill( typeof( IncinerationRecovery ) );
        }


        private static SkillFamily GetNewSkillFamily( GenericSkill s )
        {
            //if( !s.skillFamily )
            //{
            s.SetFieldValue<SkillFamily>( "_skillFamily", ScriptableObject.CreateInstance<SkillFamily>() );
            //}
            s.skillFamily.variants = new SkillFamily.Variant[0];

            R2API.SkillAPI.AddSkillFamily( s.skillFamily );

            return s.skillFamily;
        }

        private static void AssignVariants( SkillFamily fam, SkillDef[] skills )
        {
            SkillFamily.Variant[] variants = new SkillFamily.Variant[skills.Length];

            for( Int32 i = 0; i < skills.Length; i++ )
            {
                variants[i] = new SkillFamily.Variant
                {
                    skillDef = skills[i],
                    unlockableName = "",
                    viewableNode = new ViewablesCatalog.Node( skills[i].skillNameToken, false )
                };
                R2API.SkillAPI.AddSkillDef( skills[i] );
            }

            fam.variants = variants;
        }
    }
#endif
}
