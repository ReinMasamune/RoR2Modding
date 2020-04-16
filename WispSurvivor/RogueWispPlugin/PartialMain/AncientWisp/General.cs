#if ANCIENTWISP
using System;
using EntityStates;
using Rein.RogueWispPlugin.Helpers;
using RoR2;
using UnityEngine;
using ReinCore;
using Rein;

namespace Rein.RogueWispPlugin
{
    internal partial class Main
    {
        private Accessor<NetworkStateMachine,EntityStateMachine[]> stateMachines = new Accessor<NetworkStateMachine, EntityStateMachine[]>( "stateMachines" );
        partial void AW_General()
        {
            this.Load += this.AW_GetBody;
            this.Load += this.AW_GetMaster;
            this.Load += this.AW_Register;
        }

        private void AW_Register()
        {
            BodyCatalog.getAdditionalEntries += ( list ) => list.Add( this.AW_body );
            MasterCatalog.getAdditionalEntries += ( list ) => list.Add( this.AW_master );

#if ANCIENTWISPSURVMENU
            var def = new SurvivorDef
            {
                bodyPrefab = this.AW_body,
                descriptionToken = "AAA",
                displayPrefab = this.AW_body.GetComponent<ModelLocator>().modelBaseTransform.gameObject,
                primaryColor = Color.white,
                unlockableName = ""
            };
            SurvivorAPI.AddSurvivor( def );

            this.AW_body.GetComponent<CharacterBody>().preferredInitialStateType = this.AW_body.GetComponent<EntityStateMachine>().initialStateType;
#endif


        }
        private void AW_GetMaster()
        {
            this.AW_master = Resources.Load<GameObject>( "Prefabs/CharacterMasters/AncientWispMaster" ).ClonePrefab( "WispBossMaster", true );
            var charMaster = this.AW_master.GetComponent<CharacterMaster>();
            charMaster.bodyPrefab = this.AW_body;
        }
        private void AW_GetBody()
        {
            this.AW_body = Resources.Load<GameObject>( "Prefabs/CharacterBodies/AncientWispBody" ).ClonePrefab( "WispBossBody", true );

            var sound = this.AW_body.AddComponent<StartEndSound>();
            sound.startSound = "Play_huntress_R_aim_loop";
            sound.endSound = "Stop_huntress_R_aim_loop";

            var charBody = this.AW_body.GetComponent<CharacterBody>();

            charBody.baseNameToken = Rein.Properties.Tokens.ANCIENT_WISP_BODY_NAME;
            charBody.subtitleNameToken = Rein.Properties.Tokens.ANCIENT_WISP_BODY_SUBNAME;

            //LanguageCore.AddLanguageToken( "ANCIENT_WISP_BODY_NAME", "Ancient Wisp" );
            //LanguageCore.AddLanguageToken( "ANCIENT_WISP_BODY_SUBNAME", "Banished and Chained" );
            //R2API.AssetPlus.Languages.AddToken( "ANCIENT_WISP_BODY_NAME", "Ancient Wisp" );
            //R2API.AssetPlus.Languages.AddToken( "ANCIENT_WISP_BODY_SUBNAME", "Banished and Chained" );

            charBody.baseMaxHealth = 6800;
            charBody.levelMaxHealth = 2040;

            charBody.baseRegen = 0f;
            charBody.levelRegen = 0f;

            charBody.baseMaxShield = 0f;
            charBody.levelMaxShield = 0f;

            charBody.baseMoveSpeed = 10f;
            charBody.levelMoveSpeed = 0f;

            charBody.baseAcceleration = 10f;

            charBody.baseJumpPower = 0f;
            charBody.levelJumpPower = 0f;

            charBody.baseDamage = 30f;
            charBody.levelDamage = 6f;

            charBody.baseAttackSpeed = 1f;
            charBody.levelAttackSpeed = 0f;

            charBody.baseCrit = 0f;
            charBody.levelCrit = 0f;

            charBody.baseArmor = 20f;
            charBody.levelArmor = 0f;

            charBody.baseJumpCount = 0;






            NetworkStateMachine net = this.AW_body.GetComponent<NetworkStateMachine>();
            CharacterDeathBehavior death = this.AW_body.GetComponent<CharacterDeathBehavior>();
            death.idleStateMachine = new EntityStateMachine[1];
            death.deathState = new EntityStates.SerializableEntityStateType( typeof( EntityStates.Commando.DeathState ) );

            EntityStateMachine[] netStates = this.stateMachines.Get( net );
            Array.Resize<EntityStateMachine>( ref netStates, 2 );


            SetStateOnHurt hurtState = this.AW_body.AddOrGetComponent<SetStateOnHurt>();
            hurtState.canBeFrozen = false;
            hurtState.canBeHitStunned = false;
            hurtState.canBeStunned = false;
            hurtState.hitThreshold = 5f;
            hurtState.hurtState = new SerializableEntityStateType( typeof( EntityStates.FrozenState ) );

            SkillsCore.AddSkill( typeof( AWDefaultMain ) );
            SkillsCore.AddSkill( typeof( AWEnrageTransition ) );
            SkillsCore.AddSkill( typeof( AWEnrageMainState ) );

            foreach( EntityStateMachine esm in this.AW_body.GetComponents<EntityStateMachine>() )
            {
                switch( esm.customName )
                {
                    case "Body":
                        netStates[0] = esm;
                        esm.mainStateType = new SerializableEntityStateType( typeof( AWDefaultMain ) );//typeof( AWDefaultMain ).EntityStateType();
                        esm.initialStateType = new SerializableEntityStateType( typeof( AWDefaultMain ) );//typeof( AWDefaultMain ).EntityStateType();
                        hurtState.targetStateMachine = esm;
                        death.deathStateMachine = esm;
                        break;

                    case "Weapon":
                        esm.initialStateType = new SerializableEntityStateType( typeof( Idle ) );
                        esm.mainStateType = new SerializableEntityStateType( typeof( Idle ) );
                        netStates[1] = esm;
                        death.idleStateMachine[0] = esm;
                        break;

                    default:
                        break;
                }
            }

            this.stateMachines.Set( net, netStates );
        }
    }
}
#endif