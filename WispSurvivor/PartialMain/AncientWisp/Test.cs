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
//using static RogueWispPlugin.Helpers.APIInterface;

namespace RogueWispPlugin
{
    internal partial class Main
    {
        partial void AW_Test()
        {
            this.Load += this.AW_TestStuff;
            this.Load += this.AW_TestSkillDrivers;
            this.Load += this.AW_DirectorTestStuff;
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
            var secondaryDriver = this.AW_master.AddComponent<AISkillDriver>();
            var utilityDriver = this.AW_master.AddComponent<AISkillDriver>();
            var specialDriver = this.AW_master.AddComponent<AISkillDriver>();
            var chaseDriver = this.AW_master.AddComponent<AISkillDriver>();

            primaryDriver.customName = "Primary";
            primaryDriver.skillSlot = SkillSlot.Primary;
            primaryDriver.requireSkillReady = true;
            primaryDriver.requireEquipmentReady = false;
            primaryDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            primaryDriver.minUserHealthFraction = Single.NegativeInfinity;
            primaryDriver.maxUserHealthFraction = Single.PositiveInfinity;
            primaryDriver.minTargetHealthFraction = Single.NegativeInfinity;
            primaryDriver.maxTargetHealthFraction = Single.NegativeInfinity;
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

            secondaryDriver.customName = "Secondary";
            secondaryDriver.skillSlot = SkillSlot.Secondary;
            secondaryDriver.requireSkillReady = true;
            secondaryDriver.requireEquipmentReady = false;
            secondaryDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            secondaryDriver.minUserHealthFraction = Single.NegativeInfinity;
            secondaryDriver.maxUserHealthFraction = Single.PositiveInfinity;
            secondaryDriver.minTargetHealthFraction = Single.NegativeInfinity;
            secondaryDriver.maxTargetHealthFraction = Single.NegativeInfinity;
            secondaryDriver.minDistance = 0f;
            secondaryDriver.maxDistance = 100f;
            secondaryDriver.selectionRequiresTargetLoS = false;
            secondaryDriver.activationRequiresTargetLoS = false;
            secondaryDriver.movementType = AISkillDriver.MovementType.Stop;
            secondaryDriver.moveInputScale = 1f;
            secondaryDriver.aimType = AISkillDriver.AimType.AtMoveTarget;
            secondaryDriver.ignoreNodeGraph = false;
            secondaryDriver.driverUpdateTimerOverride = 0.5f;
            secondaryDriver.resetCurrentEnemyOnNextDriverSelection = false;
            secondaryDriver.noRepeat = false;
            secondaryDriver.shouldSprint = false;
            secondaryDriver.shouldFireEquipment = false;

            utilityDriver.customName = "Utility";
            utilityDriver.skillSlot = SkillSlot.Utility;
            utilityDriver.requireSkillReady = true;
            utilityDriver.requireEquipmentReady = false;
            utilityDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            utilityDriver.minUserHealthFraction = Single.NegativeInfinity;
            utilityDriver.maxUserHealthFraction = Single.PositiveInfinity;
            utilityDriver.minTargetHealthFraction = Single.NegativeInfinity;
            utilityDriver.maxTargetHealthFraction = Single.NegativeInfinity;
            utilityDriver.minDistance = 0f;
            utilityDriver.maxDistance = 100f;
            utilityDriver.selectionRequiresTargetLoS = false;
            utilityDriver.activationRequiresTargetLoS = false;
            utilityDriver.movementType = AISkillDriver.MovementType.Stop;
            utilityDriver.moveInputScale = 1f;
            utilityDriver.aimType = AISkillDriver.AimType.AtMoveTarget;
            utilityDriver.ignoreNodeGraph = false;
            utilityDriver.driverUpdateTimerOverride = 0.5f;
            utilityDriver.resetCurrentEnemyOnNextDriverSelection = false;
            utilityDriver.noRepeat = false;
            utilityDriver.shouldSprint = false;
            utilityDriver.shouldFireEquipment = false;

            specialDriver.customName = "Special";
            specialDriver.skillSlot = SkillSlot.Special;
            specialDriver.requireSkillReady = true;
            specialDriver.requireEquipmentReady = false;
            specialDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            specialDriver.minUserHealthFraction = Single.NegativeInfinity;
            specialDriver.maxUserHealthFraction = Single.PositiveInfinity;
            specialDriver.minTargetHealthFraction = Single.NegativeInfinity;
            specialDriver.maxTargetHealthFraction = Single.NegativeInfinity;
            specialDriver.minDistance = 0f;
            specialDriver.maxDistance = 100f;
            specialDriver.selectionRequiresTargetLoS = false;
            specialDriver.activationRequiresTargetLoS = false;
            specialDriver.movementType = AISkillDriver.MovementType.Stop;
            specialDriver.moveInputScale = 1f;
            specialDriver.aimType = AISkillDriver.AimType.AtMoveTarget;
            specialDriver.ignoreNodeGraph = false;
            specialDriver.driverUpdateTimerOverride = 0.5f;
            specialDriver.resetCurrentEnemyOnNextDriverSelection = false;
            specialDriver.noRepeat = false;
            specialDriver.shouldSprint = false;
            specialDriver.shouldFireEquipment = false;

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
            Debug.Log( "THINGS>>>>>" );
        }
    }

}

/*
Master diff list:
NetIdentity         -GOOD
CharacterMaster     -GOOD
Inventory           -GOOD
Base AI             -GOOD
EntityStateMachine  -GOOD
MinionOwnership     -GOOD
-----
NetIdentity
Charactermaster
Inventory
BaseAI
EntityStateMachine
AI Skill Driver
AI Skill Driver
AI Skill Driver
AI Skill Driver
Minion Ownership



















*/