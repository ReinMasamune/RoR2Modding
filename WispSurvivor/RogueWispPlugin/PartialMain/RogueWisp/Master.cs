#if ROGUEWISP
using System;
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
        partial void RW_Master()
        {
            this.Load += this.Main_Load3;
        }

        private void Main_Load3()
        {
            var master = PrefabsCore.CreatePrefab( "RogueWispMonsterMaster", true );

            var netID = master.AddComponent<NetworkIdentity>();

            var charMaster = master.AddComponent<CharacterMaster>();
            charMaster.masterIndex = (MasterCatalog.MasterIndex)(-1);
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

            var minionOwn = master.AddComponent<MinionOwnership>();

            this.AddDrivers( master );

            var wispyManager = master.AddComponent<WispyAIManager>();

        }

        private void AddDrivers( GameObject master )
        {
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
            holdSpecial.activationRequiresTargetLoS = true;
            holdSpecial.activationRequiresAimConfirmation = false;
            holdSpecial.movementType = AISkillDriver.MovementType.Stop;
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
            utilDriver.maxTargetHealthFraction = Single.NegativeInfinity;
            utilDriver.minDistance = 0f;
            utilDriver.maxDistance = PrepGaze.maxRange;
            utilDriver.selectionRequiresTargetLoS = true;
            utilDriver.activationRequiresTargetLoS = true;
            utilDriver.activationRequiresAimConfirmation = true;
            utilDriver.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            utilDriver.moveInputScale = 1f;
            utilDriver.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            utilDriver.ignoreNodeGraph = false;
            utilDriver.driverUpdateTimerOverride = -1f;
            utilDriver.resetCurrentEnemyOnNextDriverSelection = false;
            utilDriver.noRepeat = false;
            utilDriver.shouldSprint = true;
            utilDriver.shouldFireEquipment = false;
            utilDriver.shouldTapButton = true;



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
            specialDriver.activationRequiresTargetLoS = true;
            specialDriver.activationRequiresAimConfirmation = true;
            specialDriver.movementType = AISkillDriver.MovementType.Stop;
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
            secondaryDriver.minCharge = 150.0;
            secondaryDriver.maxCharge = 300.0;
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
            secondaryDriver.activationRequiresTargetLoS = true;
            secondaryDriver.activationRequiresAimConfirmation = true;
            secondaryDriver.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            secondaryDriver.moveInputScale = 1f;
            secondaryDriver.aimType = AISkillDriver.AimType.AtCurrentEnemy;
            secondaryDriver.ignoreNodeGraph = false;
            secondaryDriver.driverUpdateTimerOverride = -1f;
            secondaryDriver.resetCurrentEnemyOnNextDriverSelection = false;
            secondaryDriver.noRepeat = false;
            secondaryDriver.shouldSprint = false;
            secondaryDriver.shouldFireEquipment = false;
            secondaryDriver.shouldTapButton = false;


            var primaryMoveToZone = master.AddComponent<WispySkillDriver>();
            primaryMoveToZone.customName = "Fire primary while moving into zone";
            primaryMoveToZone.minCharge = Double.NegativeInfinity;
            primaryMoveToZone.maxCharge = Double.PositiveInfinity;
            primaryMoveToZone.requiresCustomTarget = true;
            primaryMoveToZone.skillSlot = SkillSlot.Special;
            primaryMoveToZone.requiredSkill = null; // TODO: Assign this
            primaryMoveToZone.requireSkillReady = false;
            primaryMoveToZone.requireEquipmentReady = false;
            primaryMoveToZone.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            primaryMoveToZone.minUserHealthFraction = Single.NegativeInfinity;
            primaryMoveToZone.maxUserHealthFraction = Single.PositiveInfinity;
            primaryMoveToZone.minTargetHealthFraction = Single.NegativeInfinity;
            primaryMoveToZone.maxTargetHealthFraction = Single.PositiveInfinity;
            primaryMoveToZone.minDistance = 0f;
            primaryMoveToZone.maxDistance = Single.PositiveInfinity;
            primaryMoveToZone.selectionRequiresTargetLoS = true;
            primaryMoveToZone.activationRequiresTargetLoS = true;
            primaryMoveToZone.activationRequiresAimConfirmation = false;
            primaryMoveToZone.movementType = AISkillDriver.MovementType.Stop;
            primaryMoveToZone.moveInputScale = 0f;
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


            var primaryAdvance = master.AddComponent<AISkillDriver>();
            primaryAdvance.customName = "Fire primary while moving forwards";


            var primaryStrafe = master.AddComponent<AISkillDriver>();
            primaryStrafe.customName = "Fire primary while strafing";


            var moveToZone = master.AddComponent<WispySkillDriver>();
            moveToZone.customName = "Move into zone";
            moveToZone.minCharge = Double.NegativeInfinity;
            moveToZone.maxCharge = Double.PositiveInfinity;
            moveToZone.requiresCustomTarget = true;


            var moveToTarget = master.AddComponent<AISkillDriver>();
            moveToTarget.customName = "Move closer to target";


            var moveFromTarget = master.AddComponent<AISkillDriver>();
            moveFromTarget.customName = "Retreat from target";


            var strafeTarget = master.AddComponent<AISkillDriver>();
            strafeTarget.customName = "Strafe the target";
        }
    }


    public class WispySkillDriver : AISkillDriver
    {
        public Double minCharge;
        public Double maxCharge;

        public Boolean requiresCustomTarget;


        public Boolean useEnemyRanges { get; private set; } = false;
        public Boolean requireEnemyLos { get; private set; } = false;
        public Single minDistanceToEnemy { get; private set; }
        public Single maxDistanceToEnemy { get; private set; }
        public void SetEnemyRanges( Single min, Single max, Boolean requireLOS )
        {
            this.useEnemyRanges = true;
            this.minDistanceToEnemy = min;
            this.maxDistanceToEnemy = max;
            this.requireEnemyLos = requireLOS;
        }


        public Boolean hasRequiredState { get; private set; } = false;
        public String targetStateMachine { get; private set; }
        public Type[] requiredStates { get; private set; }

        public void SetRequiredStates( String stateMachineName, params Type[] types )
        {
            this.hasRequiredState = true;
            this.targetStateMachine = stateMachineName;
            this.requiredStates = types;
        }
    }

    public class WispyAIManager : MonoBehaviour
    {
        private WispySkillDriver[] drivers;
        private (Single minHp, Single maxHp)[] origHpConstraints;
        private EntityStateMachine[] stateMachines;
        private Main.WispPassiveController passive;
        private CharacterMaster master;
        private BaseAI ai;
        private GameObject targetObj;
        private void Awake()
        {
            this.ai = base.GetComponent<BaseAI>();
            this.ai.onBodyDiscovered += ( body ) =>
            {
                this.passive = body?.GetComponent<Main.WispPassiveController>();
                this.stateMachines = body?.GetComponents<EntityStateMachine>();
                if( this.passive != null ) this.passive.onUtilityPlaced += this.SetAICustomTarget;
            };
            this.ai.onBodyLost += (body) =>
            {
                if( this.passive != null ) this.passive.onUtilityPlaced -= this.SetAICustomTarget;
                this.passive = null;
                this.stateMachines = null;
            };

            this.drivers = base.GetComponents<WispySkillDriver>();
            this.origHpConstraints = new (Single minHp, Single maxHp)[this.drivers.Length];
            this.stateMachines = new EntityStateMachine[this.drivers.Length];
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

        private void FixedUpdate()
        {
            if( this.passive != null )
            {
                var charge = this.passive.ReadCharge();
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
                            for( Int32 j = 0; i < stateMachines.Length; ++i )
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
                            var mach = this.stateMachines[i];
                            for( Int32 j = 0; i < driver.requiredStates.Length; ++j )
                            {
                                if( mach.state.GetType() == driver.requiredStates[j] )
                                {
                                    ableToActivate = true;
                                    break;
                                }
                            }
                        }
                    }

                    if( driver.requiresCustomTarget ) ableToActivate &= this.targetObj != null && this.targetObj;

                    if( driver.useEnemyRanges )
                    {
                        var enemy = this.ai.currentEnemy;

                    }


                    ableToActivate &= !(charge < driver.minCharge || charge > driver.maxCharge);

                    if( ableToActivate)
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