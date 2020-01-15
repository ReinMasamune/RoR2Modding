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
using RoR2.CharacterAI;
using R2API;
using RoR2.Skills;
//using static RogueWispPlugin.Helpers.APIInterface;

namespace RogueWispPlugin
{
#if ANCIENTWISP
    internal partial class Main
    {
        partial void AW_Test()
        {
            this.Load += this.AW_TestStuff;
            this.Load += this.AW_TestSkillDrivers;
            this.Load += this.AW_DirectorTestStuff;
            this.Load += this.AW_SurvivorCatDebugStuff;
            this.Load += this.AW_GenPrimaryProjectile;
            this.Load += this.AW_SetupSkillsStuff;
        }

        private void AW_SetupSkillsStuff()
        {
            var skillLoc = this.AW_body.GetComponent<SkillLocator>();


            var primaryFam = ScriptableObject.CreateInstance<SkillFamily>();
            var primaryDef = ScriptableObject.CreateInstance<SkillDef>();
            LoadoutAPI.AddSkillDef( primaryDef );
            LoadoutAPI.AddSkillFamily( primaryFam );
            LoadoutAPI.AddSkill( typeof( AWChargePrimary ) );
            LoadoutAPI.AddSkill( typeof( AWFirePrimary ) );

            primaryDef.activationState = new EntityStates.SerializableEntityStateType( typeof( AWChargePrimary ) );
            primaryDef.activationStateMachineName = "Weapon";
            primaryDef.baseMaxStock = 1;
            primaryDef.baseRechargeInterval = 3f;
            primaryDef.beginSkillCooldownOnSkillEnd = true;
            primaryDef.canceledFromSprinting = false;
            primaryDef.fullRestockOnAssign = true;
            primaryDef.icon = Resources.Load<Sprite>( "NotAPath" );
            primaryDef.interruptPriority = EntityStates.InterruptPriority.Any;
            primaryDef.isBullets = false;
            primaryDef.isCombatSkill = true;
            primaryDef.mustKeyPress = false;
            primaryDef.noSprint = false;
            primaryDef.rechargeStock = 1;
            primaryDef.requiredStock = 1;
            primaryDef.shootDelay = 0.1f;
            primaryDef.skillDescriptionToken = "";
            primaryDef.skillName = "";
            primaryDef.skillNameToken = "";
            primaryDef.stockToConsume = 1;

            primaryFam.variants = new SkillFamily.Variant[]
            {
                new SkillFamily.Variant
                {
                    skillDef = primaryDef,
                    unlockableName = "",
                    viewableNode = new ViewablesCatalog.Node("aaa", false )
                }
            };

            skillLoc.primary.SetFieldValue<SkillFamily>( "_skillFamily", primaryFam );

            var secondaryFam = ScriptableObject.CreateInstance<SkillFamily>();
            var secondaryDef = ScriptableObject.CreateInstance<SkillDef>();
            LoadoutAPI.AddSkillDef( secondaryDef );
            LoadoutAPI.AddSkillFamily( secondaryFam );
            LoadoutAPI.AddSkill( typeof(AWSecondary) );

            secondaryDef.activationState = new EntityStates.SerializableEntityStateType( typeof( AWSecondary ) );
            secondaryDef.activationStateMachineName = "Weapon";
            secondaryDef.baseMaxStock = 1;
            secondaryDef.baseRechargeInterval = 10f;
            secondaryDef.beginSkillCooldownOnSkillEnd = true;
            secondaryDef.canceledFromSprinting = false;
            secondaryDef.fullRestockOnAssign = true;
            secondaryDef.icon = Resources.Load<Sprite>( "NotAPath" );
            secondaryDef.interruptPriority = EntityStates.InterruptPriority.PrioritySkill;
            secondaryDef.isBullets = false;
            secondaryDef.isCombatSkill = true;
            secondaryDef.mustKeyPress = false;
            secondaryDef.noSprint = false;
            secondaryDef.rechargeStock = 1;
            secondaryDef.requiredStock = 1;
            secondaryDef.shootDelay = 0.1f;
            secondaryDef.skillDescriptionToken = "";
            secondaryDef.skillName = "";
            secondaryDef.skillNameToken = "";
            secondaryDef.stockToConsume = 1;


        }

        private void AW_GenPrimaryProjectile()
        {
            this.AW_primaryProj = Resources.Load<GameObject>( "Prefabs/Projectiles/WispCannon" ).InstantiateClone( "AncientWispCannon" );
            ProjectileCatalog.getAdditionalEntries += ( list ) => list.Add( this.AW_primaryProj );
        }

        private void AW_SurvivorCatDebugStuff()
        {
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
        }

        private void AW_DirectorTestStuff()
        {
            On.RoR2.CharacterSpawnCard.Awake += this.CharacterSpawnCard_Awake;
            var spawnCard = ScriptableObject.CreateInstance<CharacterSpawnCard>();
            On.RoR2.CharacterSpawnCard.Awake -= this.CharacterSpawnCard_Awake;
            spawnCard.directorCreditCost = 600;
            spawnCard.forbiddenAsBoss = false;
            spawnCard.forbiddenFlags = RoR2.Navigation.NodeFlags.NoCharacterSpawn;
            spawnCard.hullSize = HullClassification.Golem;
            spawnCard.loadout = new SerializableLoadout();
            spawnCard.name = "cscwispboss";
            spawnCard.nodeGraphType = RoR2.Navigation.MapNodeGroup.GraphType.Air;
            spawnCard.noElites = false;
            spawnCard.occupyPosition = false;
            spawnCard.prefab = this.AW_master;
            spawnCard.requiredFlags = RoR2.Navigation.NodeFlags.None;
            spawnCard.sendOverNetwork = true;

            var dirCard = new DirectorCard();
            dirCard.allowAmbushSpawn = true;
            dirCard.forbiddenUnlockable = "";
            dirCard.minimumStageCompletions = 0;
            dirCard.preventOverhead = false;
            dirCard.requiredUnlockable = "";
            dirCard.selectionWeight = 100;
            dirCard.spawnCard = spawnCard;
            dirCard.spawnDistance = DirectorCore.MonsterSpawnDistance.Standard;

            this.AW_dirCard = new R2API.DirectorAPI.DirectorCardHolder();
            this.AW_dirCard.card = dirCard;
            this.AW_dirCard.interactableCategory = R2API.DirectorAPI.InteractableCategory.None;
            this.AW_dirCard.monsterCategory = R2API.DirectorAPI.MonsterCategory.Champions;

            R2API.DirectorAPI.MonsterActions += this.DirectorAPI_MonsterActions;
        }

        private void AW_TestSkillDrivers()
        {
            var primaryDriver = this.AW_master.AddComponent<AISkillDriver>();
            //var secondaryDriver = this.AW_master.AddComponent<AISkillDriver>();
            //var utilityDriver = this.AW_master.AddComponent<AISkillDriver>();
            //var specialDriver = this.AW_master.AddComponent<AISkillDriver>();
            var chaseDriver = this.AW_master.AddComponent<AISkillDriver>();

            primaryDriver.customName = "Primary";
            primaryDriver.skillSlot = SkillSlot.Primary;
            primaryDriver.requiredSkill = null;
            primaryDriver.requireSkillReady = true;
            primaryDriver.requireEquipmentReady = false;
            primaryDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            primaryDriver.minUserHealthFraction = Single.NegativeInfinity;
            primaryDriver.maxUserHealthFraction = Single.PositiveInfinity;
            primaryDriver.minTargetHealthFraction = Single.NegativeInfinity;
            primaryDriver.maxTargetHealthFraction = Single.PositiveInfinity;
            primaryDriver.minDistance = 0f;
            primaryDriver.maxDistance = 100f;
            primaryDriver.selectionRequiresTargetLoS = false;
            primaryDriver.activationRequiresTargetLoS = false;
            primaryDriver.movementType = AISkillDriver.MovementType.Stop;
            primaryDriver.moveInputScale = 1f;
            primaryDriver.aimType = AISkillDriver.AimType.AtMoveTarget;
            primaryDriver.ignoreNodeGraph = false;
            primaryDriver.driverUpdateTimerOverride = 0.5f;
            primaryDriver.resetCurrentEnemyOnNextDriverSelection = false;
            primaryDriver.noRepeat = false;
            primaryDriver.shouldSprint = false;
            primaryDriver.shouldFireEquipment = false;

            chaseDriver.customName = "Chase";
            chaseDriver.skillSlot = SkillSlot.None;
            chaseDriver.requireSkillReady = false;
            chaseDriver.requireEquipmentReady = false;
            chaseDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            chaseDriver.minUserHealthFraction = Single.NegativeInfinity;
            chaseDriver.maxUserHealthFraction = Single.PositiveInfinity;
            chaseDriver.minTargetHealthFraction = Single.NegativeInfinity;
            chaseDriver.maxTargetHealthFraction = Single.PositiveInfinity;
            chaseDriver.minDistance = 0f;
            chaseDriver.maxDistance = Single.PositiveInfinity;
            chaseDriver.selectionRequiresTargetLoS = false;
            chaseDriver.activationRequiresTargetLoS = false;
            chaseDriver.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            chaseDriver.moveInputScale = 1f;
            chaseDriver.aimType = AISkillDriver.AimType.AtMoveTarget;
            chaseDriver.ignoreNodeGraph = false;
            chaseDriver.driverUpdateTimerOverride = -1f;
            chaseDriver.resetCurrentEnemyOnNextDriverSelection = false;
            chaseDriver.noRepeat = false;
            chaseDriver.shouldSprint = false;
            chaseDriver.shouldFireEquipment = false;
        }

        private void AW_TestStuff()
        {
            this.AW_body = Resources.Load<GameObject>( "Prefabs/CharacterBodies/AncientWispBody" ).InstantiateClone( "WispBossBody" );
            BodyCatalog.getAdditionalEntries += ( list ) => list.Add( this.AW_body );
            this.AW_master = Resources.Load<GameObject>( "Prefabs/CharacterMasters/AncientWispMaster" ).InstantiateClone( "WispBossMaster" );
            MasterCatalog.getAdditionalEntries += ( list ) => list.Add( this.AW_master );

            var charMaster = this.AW_master.GetComponent<CharacterMaster>();
            charMaster.bodyPrefab = this.AW_body;

            



            
        }

        private void CharacterSpawnCard_Awake( On.RoR2.CharacterSpawnCard.orig_Awake orig, CharacterSpawnCard self )
        {
            self.loadout = new SerializableLoadout();
            orig( self );
        }

        private R2API.DirectorAPI.DirectorCardHolder AW_dirCard;

        private void DirectorAPI_MonsterActions( List<R2API.DirectorAPI.DirectorCardHolder> cards, R2API.DirectorAPI.StageInfo stage )
        {
            cards.Add( this.AW_dirCard );
        }
    }
#endif
}

/*
Master diff list:


















*/