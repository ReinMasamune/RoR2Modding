#if ROGUEWISP
using System;

using EntityStates;

using ReinCore;

using RoR2;
using RoR2.CharacterAI;

using UnityEngine;
using UnityEngine.Networking;
//using static RogueWispPlugin.Helpers.APIInterface;

namespace Rein.RogueWispPlugin
{

    internal partial class Main
    {
        internal GameObject RW_master;

        partial void RW_Master()
        {
            this.Load += this.Main_Load3;
        }

        private void Main_Load3()
        {
            var master = PrefabsCore.CreatePrefab( "RogueWispMonsterMaster", true );

            var netID = master.AddOrGetComponent<NetworkIdentity>();

            var charMaster = master.AddComponent<CharacterMaster>();
            charMaster.masterIndex = (MasterCatalog.MasterIndex)( -1 );
            charMaster.bodyPrefab = this.RW_body;
            charMaster.spawnOnStart = false;
            charMaster.teamIndex = TeamIndex.Monster;
            charMaster.destroyOnBodyDeath = true;
            charMaster.isBoss = false;
            charMaster.preventGameOver = true;

            var inv = master.AddComponent<Inventory>();

            var ai = master.AddComponent<BaseAI>();
            ai.fullVision = true;
            ai.neverRetaliateFriendlies = false;
            ai.minDistanceFromEnemy = 8f;
            ai.enemyAttention = 5f;
            ai.navigationType = BaseAI.NavigationType.Nodegraph;
            ai.desiredSpawnNodeGraphType = RoR2.Navigation.MapNodeGroup.GraphType.Ground;
            var esm = ai.stateMachine = master.AddComponent<EntityStateMachine>();
            ai.scanState = new EntityStates.SerializableEntityStateType( typeof( EntityStates.AI.Walker.Wander ) );
            ai.isHealer = false;
            ai.enemyAttention = 0f;
            ai.aimVectorDampTime = 0.1f;
            ai.aimVectorMaxSpeed = 360f;
            ai.desiredAimDirection = new Vector3( 0f, 0f, 1f );
            ai.drawAIPath = false;
            ai.selectedSkilldriverName = null;
            ai.debugEnemyHurtBox = null;
            ai.currentEnemy = null;
            ai.leader = null;
            ai.customTarget = null;

            esm.customName = "AI";
            esm.initialStateType = new EntityStates.SerializableEntityStateType( typeof( EntityStates.AI.Walker.Wander ) );
            esm.mainStateType = new EntityStates.SerializableEntityStateType( typeof( EntityStates.AI.Walker.Wander ) );

            var minionOwn = master.AddOrGetComponent<MinionOwnership>();

            this.AddDrivers( master );

            var wispyManager = master.AddComponent<WispyAIManager>();

            this.RW_master = master;
            MasterCatalog.getAdditionalEntries += ( list ) => list.Add( this.RW_master );
        }

        private void AddDrivers( GameObject master )
        {
            //var holdSpecial = master.AddComponent<AISkillDriver>();
            var holdSpecial = master.AddComponent<WispySkillDriver>();
            holdSpecial.customName = "Hold down special until not able to be fired";
            holdSpecial.minCharge = 200.0;
            holdSpecial.maxCharge = Double.PositiveInfinity;
            holdSpecial.requiresCustomTarget = false;
            holdSpecial.SetRequiredStates( "Weapon", typeof( IncinerationWindup ), typeof( Incineration ) );
            holdSpecial.skillSlot = SkillSlot.Special;
            holdSpecial.requiredSkill = null; // TODO: Assign this
            holdSpecial.requireSkillReady = false;
            holdSpecial.requireEquipmentReady = false;
            holdSpecial.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            holdSpecial.minUserHealthFraction = Single.NegativeInfinity;
            holdSpecial.maxUserHealthFraction = Single.PositiveInfinity;
            holdSpecial.minTargetHealthFraction = Single.NegativeInfinity;
            holdSpecial.maxTargetHealthFraction = Single.PositiveInfinity;
            holdSpecial.minDistance = 0f;
            holdSpecial.maxDistance = Incineration.baseMaxRange;
            holdSpecial.selectionRequiresTargetLoS = true;
            holdSpecial.activationRequiresTargetLoS = false;
            holdSpecial.activationRequiresAimConfirmation = false;
            holdSpecial.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            holdSpecial.moveInputScale = 0f;
            holdSpecial.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            holdSpecial.ignoreNodeGraph = false;
            holdSpecial.driverUpdateTimerOverride = -1f;
            holdSpecial.resetCurrentEnemyOnNextDriverSelection = false;
            holdSpecial.noRepeat = false;
            holdSpecial.shouldSprint = false;
            holdSpecial.shouldFireEquipment = false;
            holdSpecial.shouldTapButton = false;



            var utilDriver = master.AddComponent<AISkillDriver>();
            utilDriver.customName = "DropZoneOnTarget";
            utilDriver.skillSlot = SkillSlot.Utility;
            utilDriver.requiredSkill = null;
            utilDriver.requireSkillReady = true;
            utilDriver.requireEquipmentReady = false;
            utilDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            utilDriver.minUserHealthFraction = Single.NegativeInfinity;
            utilDriver.maxUserHealthFraction = Single.PositiveInfinity;
            utilDriver.minTargetHealthFraction = Single.NegativeInfinity;
            utilDriver.maxTargetHealthFraction = Single.PositiveInfinity;
            utilDriver.minDistance = 0f;
            utilDriver.maxDistance = PrepGaze.maxRange;
            utilDriver.selectionRequiresTargetLoS = true;
            utilDriver.activationRequiresTargetLoS = false;
            utilDriver.activationRequiresAimConfirmation = false;
            utilDriver.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            utilDriver.moveInputScale = 1f;
            utilDriver.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            utilDriver.ignoreNodeGraph = false;
            utilDriver.driverUpdateTimerOverride = -1f;
            utilDriver.resetCurrentEnemyOnNextDriverSelection = false;
            utilDriver.noRepeat = true;
            utilDriver.shouldSprint = true;
            utilDriver.shouldFireEquipment = false;
            utilDriver.shouldTapButton = false;



            var specialDriver = master.AddComponent<WispySkillDriver>();
            specialDriver.customName = "Fire special at target";
            specialDriver.minCharge = 800.0;
            specialDriver.maxCharge = Double.PositiveInfinity;
            specialDriver.requiresCustomTarget = false;
            specialDriver.skillSlot = SkillSlot.Special;
            specialDriver.requiredSkill = null; // TODO: Assign this
            specialDriver.requireSkillReady = true;
            specialDriver.requireEquipmentReady = false;
            specialDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            specialDriver.minUserHealthFraction = Single.NegativeInfinity;
            specialDriver.maxUserHealthFraction = Single.PositiveInfinity;
            specialDriver.minTargetHealthFraction = Single.NegativeInfinity;
            specialDriver.maxTargetHealthFraction = Single.PositiveInfinity;
            specialDriver.minDistance = 0f;
            specialDriver.maxDistance = Incineration.baseMaxRange;
            specialDriver.selectionRequiresTargetLoS = true;
            specialDriver.activationRequiresTargetLoS = false;
            specialDriver.activationRequiresAimConfirmation = false;
            specialDriver.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            specialDriver.moveInputScale = 0f;
            specialDriver.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            specialDriver.ignoreNodeGraph = false;
            specialDriver.driverUpdateTimerOverride = -1f;
            specialDriver.resetCurrentEnemyOnNextDriverSelection = false;
            specialDriver.noRepeat = false;
            specialDriver.shouldSprint = false;
            specialDriver.shouldFireEquipment = false;
            specialDriver.shouldTapButton = false;


            var secondaryDriver = master.AddComponent<WispySkillDriver>();
            secondaryDriver.customName = "Fire secondary at target";
            secondaryDriver.minCharge = 180.0;
            secondaryDriver.maxCharge = 350.0;
            secondaryDriver.requiresCustomTarget = false;
            secondaryDriver.skillSlot = SkillSlot.Secondary;
            secondaryDriver.requiredSkill = null; // TODO: Assign this
            secondaryDriver.requireSkillReady = true;
            secondaryDriver.requireEquipmentReady = false;
            secondaryDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            secondaryDriver.minUserHealthFraction = Single.NegativeInfinity;
            secondaryDriver.maxUserHealthFraction = Single.PositiveInfinity;
            secondaryDriver.minTargetHealthFraction = Single.NegativeInfinity;
            secondaryDriver.maxTargetHealthFraction = Single.PositiveInfinity;
            secondaryDriver.minDistance = 0f;
            secondaryDriver.maxDistance = TestSecondary.radius * 8f;
            secondaryDriver.selectionRequiresTargetLoS = true;
            secondaryDriver.activationRequiresTargetLoS = false;
            secondaryDriver.activationRequiresAimConfirmation = false;
            secondaryDriver.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            secondaryDriver.moveInputScale = 1f;
            secondaryDriver.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            secondaryDriver.ignoreNodeGraph = false;
            secondaryDriver.driverUpdateTimerOverride = -1f;
            secondaryDriver.resetCurrentEnemyOnNextDriverSelection = false;
            secondaryDriver.noRepeat = true;
            secondaryDriver.shouldSprint = false;
            secondaryDriver.shouldFireEquipment = false;
            secondaryDriver.shouldTapButton = false;


            var primaryMoveToZone = master.AddComponent<WispySkillDriver>();
            primaryMoveToZone.customName = "Fire primary while moving into zone";
            primaryMoveToZone.minCharge = Double.NegativeInfinity;
            primaryMoveToZone.maxCharge = Double.PositiveInfinity;
            primaryMoveToZone.requiresCustomTarget = true;
            primaryMoveToZone.SetEnemyRanges( 0f, Heatwave.maxRange, true );
            primaryMoveToZone.useZoneRadiusForMinRange = true;
            primaryMoveToZone.skillSlot = SkillSlot.Primary;
            primaryMoveToZone.requiredSkill = null; // TODO: Assign this
            primaryMoveToZone.requireSkillReady = true;
            primaryMoveToZone.requireEquipmentReady = false;
            primaryMoveToZone.moveTargetType = AISkillDriver.TargetType.Custom;
            primaryMoveToZone.minUserHealthFraction = Single.NegativeInfinity;
            primaryMoveToZone.maxUserHealthFraction = Single.PositiveInfinity;
            primaryMoveToZone.minTargetHealthFraction = Single.NegativeInfinity;
            primaryMoveToZone.maxTargetHealthFraction = Single.PositiveInfinity;
            primaryMoveToZone.minDistance = 0f;
            primaryMoveToZone.maxDistance = Single.PositiveInfinity;
            primaryMoveToZone.selectionRequiresTargetLoS = false;
            primaryMoveToZone.activationRequiresTargetLoS = false;
            primaryMoveToZone.activationRequiresAimConfirmation = false;
            primaryMoveToZone.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            primaryMoveToZone.moveInputScale = 1f;
            primaryMoveToZone.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            primaryMoveToZone.ignoreNodeGraph = false;
            primaryMoveToZone.driverUpdateTimerOverride = -1f;
            primaryMoveToZone.resetCurrentEnemyOnNextDriverSelection = false;
            primaryMoveToZone.noRepeat = false;
            primaryMoveToZone.shouldSprint = false;
            primaryMoveToZone.shouldFireEquipment = false;
            primaryMoveToZone.shouldTapButton = false;


            var primaryRetreat = master.AddComponent<AISkillDriver>();
            primaryRetreat.customName = "Fire primary while moving backwards";
            primaryRetreat.skillSlot = SkillSlot.Primary;
            primaryRetreat.requiredSkill = null; // TODO: Assign this
            primaryRetreat.requireSkillReady = true;
            primaryRetreat.requireEquipmentReady = false;
            primaryRetreat.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            primaryRetreat.minUserHealthFraction = Single.NegativeInfinity;
            primaryRetreat.maxUserHealthFraction = Single.PositiveInfinity;
            primaryRetreat.minTargetHealthFraction = Single.NegativeInfinity;
            primaryRetreat.maxTargetHealthFraction = Single.PositiveInfinity;
            primaryRetreat.minDistance = 0f;
            primaryRetreat.maxDistance = Heatwave.maxRange * 0.05f;
            primaryRetreat.selectionRequiresTargetLoS = true;
            primaryRetreat.activationRequiresTargetLoS = false;
            primaryRetreat.activationRequiresAimConfirmation = false;
            primaryRetreat.movementType = AISkillDriver.MovementType.FleeMoveTarget;
            primaryRetreat.moveInputScale = 1f;
            primaryRetreat.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            primaryRetreat.ignoreNodeGraph = false;
            primaryRetreat.driverUpdateTimerOverride = -1f;
            primaryRetreat.resetCurrentEnemyOnNextDriverSelection = false;
            primaryRetreat.noRepeat = false;
            primaryRetreat.shouldSprint = false;
            primaryRetreat.shouldFireEquipment = false;
            primaryRetreat.shouldTapButton = false;


            var primaryAdvance = master.AddComponent<AISkillDriver>();
            primaryAdvance.customName = "Fire primary while moving forwards";
            primaryAdvance.skillSlot = SkillSlot.Primary;
            primaryAdvance.requiredSkill = null; // TODO: Assign this
            primaryAdvance.requireSkillReady = true;
            primaryAdvance.requireEquipmentReady = false;
            primaryAdvance.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            primaryAdvance.minUserHealthFraction = Single.NegativeInfinity;
            primaryAdvance.maxUserHealthFraction = Single.PositiveInfinity;
            primaryAdvance.minTargetHealthFraction = Single.NegativeInfinity;
            primaryAdvance.maxTargetHealthFraction = Single.PositiveInfinity;
            primaryAdvance.minDistance = Heatwave.maxRange * 0.35f;
            primaryAdvance.maxDistance = Heatwave.maxRange;
            primaryAdvance.selectionRequiresTargetLoS = true;
            primaryAdvance.activationRequiresTargetLoS = false;
            primaryAdvance.activationRequiresAimConfirmation = false;
            primaryAdvance.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            primaryAdvance.moveInputScale = 1f;
            primaryAdvance.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            primaryAdvance.ignoreNodeGraph = false;
            primaryAdvance.driverUpdateTimerOverride = -1f;
            primaryAdvance.resetCurrentEnemyOnNextDriverSelection = false;
            primaryAdvance.noRepeat = false;
            primaryAdvance.shouldSprint = false;
            primaryAdvance.shouldFireEquipment = false;
            primaryAdvance.shouldTapButton = false;


            var primaryStrafe = master.AddComponent<AISkillDriver>();
            primaryStrafe.customName = "Fire primary while strafing";
            primaryStrafe.skillSlot = SkillSlot.Primary;
            primaryStrafe.requiredSkill = null; // TODO: Assign this
            primaryStrafe.requireSkillReady = true;
            primaryStrafe.requireEquipmentReady = false;
            primaryStrafe.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            primaryStrafe.minUserHealthFraction = Single.NegativeInfinity;
            primaryStrafe.maxUserHealthFraction = Single.PositiveInfinity;
            primaryStrafe.minTargetHealthFraction = Single.NegativeInfinity;
            primaryStrafe.maxTargetHealthFraction = Single.PositiveInfinity;
            primaryStrafe.minDistance = 0f;
            primaryStrafe.maxDistance = Heatwave.maxRange;
            primaryStrafe.selectionRequiresTargetLoS = true;
            primaryStrafe.activationRequiresTargetLoS = false;
            primaryStrafe.activationRequiresAimConfirmation = false;
            primaryStrafe.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            primaryStrafe.moveInputScale = 1f;
            primaryStrafe.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            primaryStrafe.ignoreNodeGraph = false;
            primaryStrafe.driverUpdateTimerOverride = -1f;
            primaryStrafe.resetCurrentEnemyOnNextDriverSelection = false;
            primaryStrafe.noRepeat = false;
            primaryStrafe.shouldSprint = false;
            primaryStrafe.shouldFireEquipment = false;
            primaryStrafe.shouldTapButton = false;


            var moveToZone = master.AddComponent<WispySkillDriver>();
            moveToZone.customName = "Move into zone";
            moveToZone.minCharge = Double.NegativeInfinity;
            moveToZone.maxCharge = Double.PositiveInfinity;
            moveToZone.requiresCustomTarget = true;
            moveToZone.useZoneRadiusForMinRange = true;
            moveToZone.skillSlot = SkillSlot.None;
            moveToZone.requiredSkill = null; // TODO: Assign this
            moveToZone.requireSkillReady = false;
            moveToZone.requireEquipmentReady = false;
            moveToZone.moveTargetType = AISkillDriver.TargetType.Custom;
            moveToZone.minUserHealthFraction = Single.NegativeInfinity;
            moveToZone.maxUserHealthFraction = Single.PositiveInfinity;
            moveToZone.minTargetHealthFraction = Single.NegativeInfinity;
            moveToZone.maxTargetHealthFraction = Single.PositiveInfinity;
            moveToZone.minDistance = 0f;
            moveToZone.maxDistance = Single.PositiveInfinity;
            moveToZone.selectionRequiresTargetLoS = false;
            moveToZone.activationRequiresTargetLoS = false;
            moveToZone.activationRequiresAimConfirmation = false;
            moveToZone.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            moveToZone.moveInputScale = 1f;
            moveToZone.aimType = AISkillDriver.AimType.MoveDirection;
            moveToZone.ignoreNodeGraph = false;
            moveToZone.driverUpdateTimerOverride = -1f;
            moveToZone.resetCurrentEnemyOnNextDriverSelection = false;
            moveToZone.noRepeat = false;
            moveToZone.shouldSprint = true;
            moveToZone.shouldFireEquipment = false;
            moveToZone.shouldTapButton = false;


            var moveToTarget = master.AddComponent<AISkillDriver>();
            moveToTarget.customName = "Move closer to target";
            moveToTarget.skillSlot = SkillSlot.None;
            moveToTarget.requiredSkill = null; // TODO: Assign this
            moveToTarget.requireSkillReady = false;
            moveToTarget.requireEquipmentReady = false;
            moveToTarget.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            moveToTarget.minUserHealthFraction = Single.NegativeInfinity;
            moveToTarget.maxUserHealthFraction = Single.PositiveInfinity;
            moveToTarget.minTargetHealthFraction = Single.NegativeInfinity;
            moveToTarget.maxTargetHealthFraction = Single.PositiveInfinity;
            moveToTarget.minDistance = Heatwave.maxRange * 0.35f;
            moveToTarget.maxDistance = Single.PositiveInfinity;
            moveToTarget.selectionRequiresTargetLoS = false;
            moveToTarget.activationRequiresTargetLoS = false;
            moveToTarget.activationRequiresAimConfirmation = false;
            moveToTarget.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
            moveToTarget.moveInputScale = 1f;
            moveToTarget.aimType = AISkillDriver.AimType.MoveDirection;
            moveToTarget.ignoreNodeGraph = false;
            moveToTarget.driverUpdateTimerOverride = -1f;
            moveToTarget.resetCurrentEnemyOnNextDriverSelection = false;
            moveToTarget.noRepeat = false;
            moveToTarget.shouldSprint = true;
            moveToTarget.shouldFireEquipment = false;
            moveToTarget.shouldTapButton = false;


            var moveFromTarget = master.AddComponent<AISkillDriver>();
            moveFromTarget.customName = "Retreat from target";
            moveFromTarget.skillSlot = SkillSlot.None;
            moveFromTarget.requiredSkill = null; // TODO: Assign this
            moveFromTarget.requireSkillReady = false;
            moveFromTarget.requireEquipmentReady = false;
            moveFromTarget.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            moveFromTarget.minUserHealthFraction = Single.NegativeInfinity;
            moveFromTarget.maxUserHealthFraction = Single.PositiveInfinity;
            moveFromTarget.minTargetHealthFraction = Single.NegativeInfinity;
            moveFromTarget.maxTargetHealthFraction = Single.PositiveInfinity;
            moveFromTarget.minDistance = 0f;
            moveFromTarget.maxDistance = Heatwave.maxRange * 0.05f;
            moveFromTarget.selectionRequiresTargetLoS = false;
            moveFromTarget.activationRequiresTargetLoS = false;
            moveFromTarget.activationRequiresAimConfirmation = false;
            moveFromTarget.movementType = AISkillDriver.MovementType.FleeMoveTarget;
            moveFromTarget.moveInputScale = 1f;
            moveFromTarget.aimType = AISkillDriver.AimType.MoveDirection;
            moveFromTarget.ignoreNodeGraph = false;
            moveFromTarget.driverUpdateTimerOverride = -1f;
            moveFromTarget.resetCurrentEnemyOnNextDriverSelection = false;
            moveFromTarget.noRepeat = false;
            moveFromTarget.shouldSprint = true;
            moveFromTarget.shouldFireEquipment = false;
            moveFromTarget.shouldTapButton = false;


            var strafeTarget = master.AddComponent<AISkillDriver>();
            strafeTarget.customName = "Strafe the target";
            strafeTarget.skillSlot = SkillSlot.None;
            strafeTarget.requiredSkill = null; // TODO: Assign this
            strafeTarget.requireSkillReady = false;
            strafeTarget.requireEquipmentReady = false;
            strafeTarget.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            strafeTarget.minUserHealthFraction = Single.NegativeInfinity;
            strafeTarget.maxUserHealthFraction = Single.PositiveInfinity;
            strafeTarget.minTargetHealthFraction = Single.NegativeInfinity;
            strafeTarget.maxTargetHealthFraction = Single.PositiveInfinity;
            strafeTarget.minDistance = 0f;
            strafeTarget.maxDistance = Single.PositiveInfinity;
            strafeTarget.selectionRequiresTargetLoS = false;
            strafeTarget.activationRequiresTargetLoS = false;
            strafeTarget.activationRequiresAimConfirmation = false;
            strafeTarget.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            strafeTarget.moveInputScale = 1f;
            strafeTarget.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            strafeTarget.ignoreNodeGraph = false;
            strafeTarget.driverUpdateTimerOverride = -1f;
            strafeTarget.resetCurrentEnemyOnNextDriverSelection = false;
            strafeTarget.noRepeat = false;
            strafeTarget.shouldSprint = true;
            strafeTarget.shouldFireEquipment = false;
            strafeTarget.shouldTapButton = false;
        }
    }


    public class WispySkillDriver : AISkillDriver
    {
        public Double minCharge;
        public Double maxCharge;

        public Boolean requiresCustomTarget;


        public Boolean useEnemyRanges = false;
        public Boolean requireEnemyLos = false;
        public Single minDistanceToEnemySq;
        public Single maxDistanceToEnemySq;
        public void SetEnemyRanges( Single min, Single max, Boolean requireLOS )
        {
            this.useEnemyRanges = true;
            this.minDistanceToEnemySq = min * min;
            this.maxDistanceToEnemySq = max * max;
            this.requireEnemyLos = requireLOS;
        }


        public Boolean useZoneRadiusForMinRange = false;


        public Boolean hasRequiredState = false;
        public String targetStateMachine;
        public SerializableEntityStateType[] requiredStates;

        public void SetRequiredStates( String stateMachineName, params Type[] types )
        {
            this.hasRequiredState = true;
            this.targetStateMachine = stateMachineName;
            this.requiredStates = new SerializableEntityStateType[types.Length];
            for( Int32 i = 0; i < types.Length; ++i )
            {
                this.requiredStates[i] = new SerializableEntityStateType( types[i] );
            }
        }
    }

    public class WispyAIManager : MonoBehaviour
    {
        private WispySkillDriver[] drivers;
        private (Single minHp, Single maxHp)[] origHpConstraints;
        private EntityStateMachine[] unsortedMachines;
        private EntityStateMachine[] stateMachines;
        private Type[] prevType;
        private Main.WispPassiveController passive;
        private CharacterMaster master;
        private BaseAI ai;
        private GameObject targetObj;
        private Single zoneRadius;
        private void Awake()
        {
            this.ai = base.GetComponent<BaseAI>();
            this.ai.onBodyDiscovered += ( body ) =>
            {
                this.passive = body?.GetComponent<Main.WispPassiveController>();
                if( this.passive != null )
                {
                    this.passive.onUtilPlaced += this.SetAICustomTarget;
                    this.passive.onUtilRangeProvided += this.SetZoneRange;
                }
            };
            this.ai.onBodyLost += ( body ) =>
            {
                if( this.passive != null )
                {
                    this.passive.onUtilPlaced -= this.SetAICustomTarget;
                    this.passive.onUtilRangeProvided -= this.SetZoneRange;
                }
                this.passive = null;
                this.stateMachines = new EntityStateMachine[this.drivers.Length];
            };

            this.drivers = base.GetComponents<WispySkillDriver>();
            this.origHpConstraints = new (Single minHp, Single maxHp)[this.drivers.Length];
            this.stateMachines = new EntityStateMachine[this.drivers.Length];
            this.prevType = new Type[this.drivers.Length];
            for( Int32 i = 0; i < this.drivers.Length; ++i )
            {
                var driver = this.drivers[i];
                this.origHpConstraints[i] = (driver.minUserHealthFraction, driver.maxUserHealthFraction);
            }

            this.master = base.GetComponent<CharacterMaster>();
        }

        private void SetAICustomTarget( GameObject obj )
        {
            this.ai.customTarget.gameObject = obj;
            this.targetObj = obj;
        }

        private void SetZoneRange( Single range )
        {
            this.zoneRadius = range * 0.85f;
        }

        private void FixedUpdate()
        {
            if( this.passive != null )
            {
                var charge = this.passive.ReadCharge();
                var aimOrig = this.ai.bodyInputBank.aimOrigin;
                for( Int32 i = 0; i < this.drivers.Length; ++i )
                {
                    var driver = this.drivers[i];
                    var ableToActivate = true;

                    if( driver.hasRequiredState )
                    {
                        ableToActivate = false;
                        if( this.stateMachines[i] == null )
                        {
                            var stateMachines = this.passive.GetComponents<EntityStateMachine>();
                            if( stateMachines == null || stateMachines.Length == 0 )
                            {
                                Main.LogE( "No state machines found" );
                            }
                            for( Int32 j = 0; j < stateMachines.Length; ++j )
                            {
                                var mach = stateMachines[j];
                                if( mach.customName == driver.targetStateMachine )
                                {
                                    this.stateMachines[i] = mach;
                                    break;
                                }
                            }
                        }

                        if( this.stateMachines[i] != null )
                        {
                            var curState = this.stateMachines[i].state.GetType();
                            for( Int32 j = 0; j < driver.requiredStates.Length; ++j )
                            {
                                if( curState == driver.requiredStates[j].stateType )
                                {
                                    Chat.AddMessage( "State found" );
                                    ableToActivate = true;
                                    break;
                                }
                            }
                        } else
                        {
                            Main.LogE( String.Format( "Did not find statemachine with name {0}", driver.targetStateMachine ) );
                        }
                    }

                    if( ableToActivate && driver.requiresCustomTarget ) ableToActivate = this.targetObj != null && this.targetObj;

                    if( ableToActivate && driver.useEnemyRanges )
                    {
                        var enemy = this.ai.currentEnemy;
                        if( enemy == null || ( driver.requireEnemyLos && !enemy.hasLoS ) || enemy.gameObject == null || !enemy.gameObject )
                        {
                            ableToActivate = false;
                        } else
                        {
                            var dist = (enemy.gameObject.transform.position - this.ai.bodyInputBank.aimOrigin).sqrMagnitude;
                            if( dist > driver.maxDistanceToEnemySq || dist < driver.minDistanceToEnemySq )
                            {
                                ableToActivate = false;
                            }
                        }
                    }

                    if( ableToActivate && ( charge < driver.minCharge || charge > driver.maxCharge ) )
                    {
                        ableToActivate = false;
                    }

                    if( driver.useZoneRadiusForMinRange )
                    {
                        driver.minDistance = this.zoneRadius;
                    }

                    if( ableToActivate )
                    {
                        var orig = this.origHpConstraints[i];
                        driver.minUserHealthFraction = orig.minHp;
                        driver.maxUserHealthFraction = orig.maxHp;
                    } else
                    {
                        driver.minUserHealthFraction = Single.PositiveInfinity;
                        driver.maxUserHealthFraction = Single.NegativeInfinity;
                    }
                }
            }
        }
    }
}




#endif

/*
    Skills:
        If zone off CD and in range for zone and LOS
            Cast zone

        If charge over min and special off cd and LOS and in range
            Cast special (hold until no LOS, charge under diff min, no los, out of range)

        if charge over min and secondary off cd and los and in range
            Cast secondary

        if los and in range
            cast primary
            (Needs movement stuff)

    Movement:
        If zone and outside zone:
            Go to zone
        
        If too far from enemy
            move towards enemy

        If too close to enemy
            Flee enemy

        true
            strafe


    Movement
        Enemy relative
            Towards

            Strafe

            Backwards

        Zone relative
            Towards
                Outside zone
                Zone exists

    Skills
        Primary

        Secondary
            In range
            Off Cooldown
            LOS
            Charge > %
            Special on CD
            Util on CD

        Utility
            In range
            LOS
            Off Cooldown

        Special
            In range
            LOS
            Charge > %
            Off cooldown
            Util on CD











































   */
