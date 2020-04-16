using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BepInEx.Logging;
using ReinCore;
using RoR2;
using RoR2.Networking;
using UnityEngine;
using KinematicCharacterController;
using EntityStates;
using Sniper.Properties;
using UnityEngine.Networking;
using Sniper.Components;

namespace Sniper.Modules
{
    internal static class PrefabModule
    {
        private static Accessor<NetworkStateMachine,EntityStateMachine[]> stateMachines = new Accessor<NetworkStateMachine, EntityStateMachine[]>( "stateMachines" );
        private static Accessor<KinematicCharacterMotor,Single> capsuleRadius = new Accessor<KinematicCharacterMotor, Single>( "CapsuleRadius" );
        private static Accessor<KinematicCharacterMotor,Single> capsuleHeight = new Accessor<KinematicCharacterMotor, Single>( "CapsuleHeight" );
        private static Accessor<KinematicCharacterMotor,Single> capsuleYOffset = new Accessor<KinematicCharacterMotor, Single>( "CapsuleYOffset" );
        private static Accessor<KinematicCharacterMotor,PhysicMaterial> capsulePhysicsMaterial = new Accessor<KinematicCharacterMotor, PhysicMaterial>("CapsulePhysicsMaterial");

        internal static void CreatePrefab()
        {
            SniperMain.sniperBodyPrefab = PrefabsCore.CreatePrefab( "Sniper", true );
            var obj = SniperMain.sniperBodyPrefab;

            var netId = obj.AddOrGetComponent<NetworkIdentity>();
            netId.localPlayerAuthority = true;


            var modelBase = new GameObject("ModelBase");
            modelBase.transform.parent = obj.transform;
            modelBase.transform.localPosition = Vector3.zero;
            modelBase.transform.localRotation = Quaternion.identity;
            modelBase.transform.localScale = Vector3.one;

            var cameraPivot = new GameObject( "CameraPivot" );
            cameraPivot.transform.parent = modelBase.transform;
            cameraPivot.transform.localPosition = Vector3.zero; // TODO: Camera pivot position
            cameraPivot.transform.localRotation = Quaternion.identity;
            cameraPivot.transform.localScale = Vector3.one;

            var aimOrigin = new GameObject( "AimOrigin" );
            aimOrigin.transform.parent = modelBase.transform;
            aimOrigin.transform.localPosition = Vector3.zero; // TODO: Aim origin position
            aimOrigin.transform.localRotation = Quaternion.identity;
            aimOrigin.transform.localScale = Vector3.one;

            var model = ModelModule.GetModel();
            var modelTransform = model.transform;
            modelTransform.parent = modelBase.transform;
            modelTransform.localPosition = Vector3.zero;
            modelTransform.localScale = Vector3.one;
            modelTransform.localRotation = Quaternion.identity;

            


            var direction = obj.AddOrGetComponent<CharacterDirection>();
            direction.moveVector = Vector3.zero;
            direction.targetTransform = modelBase.transform;
            direction.overrideAnimatorForwardTransform = null;
            direction.rootMotionAccumulator = null;
            direction.modelAnimator = null;
            direction.driveFromRootRotation = false;
            direction.turnSpeed = 720f;


            var body = obj.AddOrGetComponent<SniperCharacterBody>();
            body.bodyIndex = -1;
            body.baseNameToken = Tokens.SNIPER_NAME;
            body.subtitleNameToken = Tokens.SNIPER_SUBTITLE;
            body.bodyFlags = CharacterBody.BodyFlags.ImmuneToExecutes;
            body.rootMotionInMainState = false;
            body.mainRootSpeed = 0f;

            body.baseMaxHealth = 100f;
            body.levelMaxHealth = 30f;

            body.baseRegen = 1f;
            body.levelRegen = 0.2f;

            body.baseMaxShield = 0f;
            body.levelMaxShield = 0f;

            body.baseMoveSpeed = 7f;
            body.levelMoveSpeed = 0f;

            body.baseAcceleration = 80f;

            body.baseJumpPower = 15f;
            body.levelJumpPower = 0f;

            body.baseDamage = 12f;
            body.levelDamage = 2.4f;

            body.baseAttackSpeed = 1f;
            body.levelAttackSpeed = 0f;

            body.baseCrit = 1f;
            body.levelCrit = 0f;

            body.baseArmor = 0f;
            body.levelArmor = 0f;

            body.baseJumpCount = 1;

            body.sprintingSpeedMultiplier = 1.45f;

            body.wasLucky = false;
            body.spreadBloomDecayTime = 1f;
            body.spreadBloomCurve = AnimationCurve.EaseInOut( 0f, 0f, 1f, 1f );
            body.crosshairPrefab = UIModule.GetDefaultDrosshair();
            body.hideCrosshair = false;
            body.aimOriginTransform = aimOrigin.transform;
            body.hullClassification = HullClassification.Human;
            body.portraitIcon = UIModule.GetPortraitIcon();
            body.isChampion = false;
            body.currentVehicle = null;
            body.preferredPodPrefab = MiscModule.GetPodPrefab();
            body.preferredInitialStateType = SkillsCore.StateType<Uninitialized>();
            body.skinIndex = 0u;


            var motor = obj.AddOrGetComponent<CharacterMotor>();
            motor.walkSpeedPenaltyCoefficient = 1f;
            motor.characterDirection = direction;
            motor.muteWalkMotion = false;
            motor.mass = 100f;
            motor.airControl = 0.25f;
            motor.disableAirControlUntilCollision = false;
            motor.generateParametersOnAwake = true;
            motor.useGravity = true;
            motor.isFlying = false;


            var input = obj.AddOrGetComponent<InputBankTest>();
            input.moveVector = Vector3.zero;





            var ctp = obj.AddOrGetComponent<CameraTargetParams>();
            ctp.cameraParams = MiscModule.GetCharCameraParams();
            ctp.cameraPivotTransform = null;
            ctp.aimMode = CameraTargetParams.AimType.Standard;
            ctp.recoil = Vector2.zero;
            ctp.idealLocalCameraPos = Vector3.zero; // TODO: localCameraPos
            ctp.dontRaycastToPivot = false;


            var modelLocator = obj.AddOrGetComponent<ModelLocator>();
            modelLocator.modelTransform = modelTransform;
            modelLocator.modelBaseTransform = modelBase.transform;
            modelLocator.dontReleaseModelOnDeath = false;
            modelLocator.autoUpdateModelTransform = true;
            modelLocator.dontDetatchFromParent = false;
            modelLocator.noCorpse = false;
            modelLocator.normalizeToFloor = false;
            modelLocator.preserveModel = false;


            var bodyMachine = obj.AddOrGetComponent<EntityStateMachine>();
            bodyMachine.customName = "Body";
            bodyMachine.initialStateType = SkillsCore.StateType<SpawnTeleporterState>();
            bodyMachine.mainStateType = SkillsCore.StateType<GenericCharacterMain>();


            var weaponMachine = obj.AddComponent<EntityStateMachine>();
            weaponMachine.customName = "Weapon";
            weaponMachine.initialStateType = SkillsCore.StateType<Idle>();
            weaponMachine.mainStateType = SkillsCore.StateType<Idle>();


            var scopeMachine = obj.AddComponent<EntityStateMachine>();
            scopeMachine.customName = "Scope";
            scopeMachine.initialStateType = SkillsCore.StateType<Idle>();
            scopeMachine.mainStateType = SkillsCore.StateType<Idle>();


            var reloadMachine = obj.AddComponent<EntityStateMachine>();
            reloadMachine.customName = "Reload";
            reloadMachine.initialStateType = SkillsCore.StateType<Idle>(); // TODO: SkillsStuff
            reloadMachine.mainStateType = SkillsCore.StateType<Idle>(); // TODO: SkillsStuff


            var allStateMachines = new[] { bodyMachine, weaponMachine, scopeMachine, reloadMachine };
            var nonBodyStateMachines = new[] { weaponMachine, scopeMachine, reloadMachine };


            var ammoSkill = obj.AddOrGetComponent<GenericSkill>();
            ammoSkill.SetSkillFamily( SkillFamiliesModule.GetAmmoSkillFamily() );
            HooksModule.AddReturnoverride( ammoSkill );


            var passiveSkill = obj.AddComponent<GenericSkill>();
            passiveSkill.SetSkillFamily( SkillFamiliesModule.GetPassiveSkillFamily() );
            HooksModule.AddReturnoverride( passiveSkill );


            var primarySkill = obj.AddComponent<GenericSkill>();
            primarySkill.SetSkillFamily( SkillFamiliesModule.GetPrimarySkillFamily() );


            var secondarySkill = obj.AddComponent<GenericSkill>();
            secondarySkill.SetSkillFamily( SkillFamiliesModule.GetSecondarySkillFamily() );


            var utilitySkill = obj.AddComponent<GenericSkill>();
            utilitySkill.SetSkillFamily( SkillFamiliesModule.GetUtilitySkillFamily() );


            var specialSkill = obj.AddComponent<GenericSkill>();
            specialSkill.SetSkillFamily( SkillFamiliesModule.GetSpecialSkillFamily() );


            var skillLocator = obj.AddOrGetComponent<SkillLocator>();
            skillLocator.primary = primarySkill;
            skillLocator.secondary = secondarySkill;
            skillLocator.utility = utilitySkill;
            skillLocator.special = specialSkill;
            skillLocator.passiveSkill = new SkillLocator.PassiveSkill
            {
                enabled = false,
                icon = null,
                skillDescriptionToken = null,
                skillNameToken = null,
            };


            var team = obj.AddOrGetComponent<TeamComponent>();
            team.hideAllyCardDisplay = false;
            team.teamIndex = TeamIndex.None;


            var health = obj.AddOrGetComponent<HealthComponent>();
            health.health = 100;
            health.shield = 0;
            health.barrier = 0;
            health.magnetiCharge = 0;
            health.body = null;
            health.dontShowHealthbar = false;
            health.globalDeathEventChanceCoefficient = 1f;


            var interactor = obj.AddOrGetComponent<Interactor>();
            interactor.maxInteractionDistance = 3f;


            var interaction = obj.AddOrGetComponent<InteractionDriver>();
            interaction.highlightInteractor = true;


            var death = obj.AddOrGetComponent<CharacterDeathBehavior>();
            death.deathStateMachine = bodyMachine;
            death.deathState = SkillsCore.StateType<EntityStates.Commando.DeathState>(); // TODO: SkillsStuff
            death.idleStateMachine = nonBodyStateMachines;


            var netTrans = obj.AddOrGetComponent<CharacterNetworkTransform>();
            netTrans.positionTransmitInterval = 0.05f;
            netTrans.lastPositionTransmitTime = Single.MinValue;
            netTrans.interpolationFactor = 3f;
            netTrans.debugDuplicatePositions = false;
            netTrans.debugSnapshotReceived = false;


            var netStates = obj.AddOrGetComponent<NetworkStateMachine>();
            stateMachines.Set( netStates, allStateMachines );


            var emotes = obj.AddOrGetComponent<CharacterEmoteDefinitions>();
            emotes.emoteDefinitions = null; // TODO: Emotes


            var equip = obj.AddOrGetComponent<EquipmentSlot>();


            var sfx = obj.AddOrGetComponent<SfxLocator>();
            sfx.deathSound = "Play_ui_player_death";
            sfx.barkSound = "";
            sfx.openSound = "";
            sfx.landingSound = "Play_char_land";
            sfx.fallDamageSound = "Play_char_land_fall_damage";
            sfx.aliveLoopStart = "";
            sfx.aliveLoopStop = "";


            var rb = obj.AddOrGetComponent<Rigidbody>();
            rb.mass = 100f;
            rb.drag = 0f;
            rb.angularDrag = 0f;
            rb.useGravity = false;
            rb.isKinematic = true;
            rb.interpolation = RigidbodyInterpolation.None;
            rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
            rb.constraints = RigidbodyConstraints.None;


            var col = obj.AddOrGetComponent<CapsuleCollider>();
            col.isTrigger = false;
            col.material = null;
            col.center = new Vector3( 0f, 0.8f, 0f );
            col.radius = 0.5f;
            col.height = 1.82f;
            col.direction = 1;


            var kinCharMot = obj.AddOrGetComponent<KinematicCharacterMotor>();
            kinCharMot.CharacterController = motor;
            kinCharMot.Capsule = col;
            kinCharMot.Rigidbody = rb;
            capsuleRadius.Set( kinCharMot, 0.5f );
            capsuleHeight.Set( kinCharMot, 1.82f );
            capsuleYOffset.Set( kinCharMot, 0.8f );
            capsulePhysicsMaterial.Set( kinCharMot, null );
            kinCharMot.DetectDiscreteCollisions = false;
            kinCharMot.GroundDetectionExtraDistance = 0f;
            kinCharMot.MaxStepHeight = 0.2f;
            kinCharMot.MinRequiredStepDepth = 0.1f;
            kinCharMot.MaxStableSlopeAngle = 55f;
            kinCharMot.MaxStableDistanceFromLedge = 0.5f;
            kinCharMot.PreventSnappingOnLedges = false;
            kinCharMot.MaxStableDenivelationAngle = 55f;
            kinCharMot.RigidbodyInteractionType = RigidbodyInteractionType.None;
            kinCharMot.PreserveAttachedRigidbodyMomentum = true;
            kinCharMot.HasPlanarConstraint = false;
            kinCharMot.PlanarConstraintAxis = Vector3.up;
            kinCharMot.StepHandling = StepHandlingMethod.None;
            kinCharMot.LedgeHandling = true;
            kinCharMot.InteractiveRigidbodyHandling = true;
            kinCharMot.SafeMovement = false;


            var hurt = obj.AddOrGetComponent<SetStateOnHurt>();
            hurt.hitThreshold = 5f;
            hurt.targetStateMachine = bodyMachine;
            hurt.idleStateMachine = nonBodyStateMachines; // TODO: SkillsStuff
            hurt.hurtState = SkillsCore.StateType<Idle>(); // TODO: SkillsStuff
            hurt.canBeHitStunned = false;
            hurt.canBeStunned = false;
            hurt.canBeFrozen = true;




            var charModel = model.AddComponent<CharacterModel>();
            charModel.body = body;
            charModel.itemDisplayRuleSet = ItemDisplayModule.GetSniperItemDisplay();
            charModel.autoPopulateLightInfos = true;
            charModel.baseRendererInfos = null; // TODO: Assign
            charModel.baseLightInfos = null; // TODO: Assign


            var childLocator = model.AddComponent<ChildLocator>();
            // TODO: Assign...


            var hurtBoxGroup = model.AddComponent<HurtBoxGroup>();

            var tempHurtBox = new GameObject("TempHurtbox");
            tempHurtBox.layer = LayerIndex.entityPrecise.intVal;
            tempHurtBox.transform.parent = modelTransform;
            tempHurtBox.transform.localPosition = new Vector3( 0f, 0.841f, 0f );
            tempHurtBox.transform.localRotation = Quaternion.identity;
            tempHurtBox.transform.localScale = Vector3.one;

            var tempHbCol = tempHurtBox.AddComponent<CapsuleCollider>();
            tempHbCol.center = Vector3.zero;
            tempHbCol.radius = 0.32f;
            tempHbCol.height = 1.71f;
            tempHbCol.direction = 1;

            var tempHb = tempHurtBox.AddComponent<HurtBox>();
            tempHb.healthComponent = health;
            tempHb.isBullseye = true;
            tempHb.damageModifier = HurtBox.DamageModifier.Normal;
            tempHb.hurtBoxGroup = hurtBoxGroup;
            tempHb.indexInGroup = 0;

            hurtBoxGroup.hurtBoxes = new[]
            {
                tempHb,
            };
            hurtBoxGroup.mainHurtBox = tempHb;
            hurtBoxGroup.bullseyeCount = 1;


            var footsteps = model.AddComponent<FootstepHandler>();
            footsteps.baseFootstepString = "Play_player_footstep";
            footsteps.sprintFootstepOverrideString = "";
            footsteps.enableFootstepDust = false; // TODO: Enable
            footsteps.footstepDustPrefab = null; // TODO: Assign


            var ragdoll = model.AddComponent<RagdollController>();
            ragdoll.bones = null; // TODO: Assign
            ragdoll.componentsToDisableOnRagdoll = null; // Assign


            var aimAnimator = model.AddComponent<AimAnimator>();
            // TODO: Finalize values
            aimAnimator.inputBank = input;
            aimAnimator.directionComponent = direction;
            aimAnimator.pitchRangeMin = -60f;
            aimAnimator.pitchRangeMax = 60f;
            aimAnimator.yawRangeMin = -90f;
            aimAnimator.yawRangeMax = 90f;
            aimAnimator.pitchGiveupRange = 30f;
            aimAnimator.yawGiveupRange = 10f;
            aimAnimator.giveupDuration = 3f;
            aimAnimator.raisedApproachSpeed = 720f;
            aimAnimator.loweredApproachSpeed = 360f;
            aimAnimator.smoothTime = 0.1f;
            aimAnimator.fullYaw = false;
            aimAnimator.aimType = AimAnimator.AimType.Direct;
            aimAnimator.enableAimWeight = false;


            var skinController = model.AddComponent<ModelSkinController>();
            skinController.skins = Array.Empty<SkinDef>(); // TODO: Assign
        }
    }
}
