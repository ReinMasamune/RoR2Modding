namespace Sniper.Modules
{
    using System;

    using EntityStates;

    using KinematicCharacterController;

    using ReinCore;

    using RoR2;
    using RoR2.Networking;

    using Sniper.Components;
    using Sniper.Properties;

    using UnityEngine;
    using UnityEngine.Networking;

    internal static class PrefabModule
    {
        //private static readonly Accessor<NetworkStateMachine,EntityStateMachine[]> stateMachines = new Accessor<NetworkStateMachine, EntityStateMachine[]>( "stateMachines" );
        //private static readonly Accessor<KinematicCharacterMotor,Single> capsuleRadius = new Accessor<KinematicCharacterMotor, Single>( "CapsuleRadius" );
        //private static readonly Accessor<KinematicCharacterMotor,Single> capsuleHeight = new Accessor<KinematicCharacterMotor, Single>( "CapsuleHeight" );
        //private static readonly Accessor<KinematicCharacterMotor,Single> capsuleYOffset = new Accessor<KinematicCharacterMotor, Single>( "CapsuleYOffset" );
        //private static readonly Accessor<KinematicCharacterMotor,PhysicMaterial> capsulePhysicsMaterial = new Accessor<KinematicCharacterMotor, PhysicMaterial>("CapsulePhysicsMaterial");

        internal static void CreatePrefab()
        {
            SniperMain.sniperBodyPrefab = PrefabsCore.CreatePrefab( "Sniper", true );
            SniperMain.sniperBodyPrefab.tag = "Finish";
            GameObject obj = SniperMain.sniperBodyPrefab;

            NetworkIdentity netId = obj.AddOrGetComponent<NetworkIdentity>();
            netId.localPlayerAuthority = true;


            var modelBase = new GameObject("ModelBase");
            modelBase.transform.parent = obj.transform;
            modelBase.transform.localPosition = new Vector3( 0f, -0.81f, 0f );
            modelBase.transform.localRotation = Quaternion.identity;
            modelBase.transform.localScale = new Vector3( 1f, 1f, 1f );

            var cameraPivot = new GameObject( "CameraPivot" );
            cameraPivot.transform.parent = modelBase.transform;
            cameraPivot.transform.localPosition = new Vector3( 0f, 1.6f, 0f );
            cameraPivot.transform.localRotation = Quaternion.identity;
            cameraPivot.transform.localScale = Vector3.one;

            var aimOrigin = new GameObject( "AimOrigin" );
            aimOrigin.transform.parent = modelBase.transform;
            aimOrigin.transform.localPosition = new Vector3( 0f, 1.4f, 0f );
            aimOrigin.transform.localRotation = Quaternion.identity;
            aimOrigin.transform.localScale = Vector3.one;

            GameObject model = ModelModule.GetModel();
            Transform modelTransform = model.transform;
            modelTransform.parent = modelBase.transform;
            modelTransform.localPosition = Vector3.zero;
            modelTransform.localScale = Vector3.one;
            modelTransform.localRotation = Quaternion.identity;




            CharacterDirection direction = obj.AddOrGetComponent<CharacterDirection>();
            direction.moveVector = Vector3.zero;
            direction.targetTransform = modelBase.transform;
            direction.overrideAnimatorForwardTransform = null;
            direction.rootMotionAccumulator = null;
            direction.modelAnimator = null;
            direction.driveFromRootRotation = false;
            direction.turnSpeed = 720f;


            SniperCharacterBody body = obj.AddOrGetComponent<SniperCharacterBody>();
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

            body.baseAcceleration = 50f;

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
            body.crosshairPrefab = UIModule.GetCrosshair();
            body.hideCrosshair = false;
            body.aimOriginTransform = aimOrigin.transform;
            body.hullClassification = HullClassification.Human;
            body.portraitIcon = UIModule.GetPortraitIcon();
            body.isChampion = false;
            body.currentVehicle = null;
            body.preferredPodPrefab = MiscModule.GetPodPrefab();
            body.preferredInitialStateType = SkillsCore.StateType<Uninitialized>();
            body.skinIndex = 0u;


            CharacterMotor motor = obj.AddOrGetComponent<CharacterMotor>();
            motor.walkSpeedPenaltyCoefficient = 1f;
            motor.characterDirection = direction;
            motor.muteWalkMotion = false;
            motor.mass = 100f;
            motor.airControl = 0.25f;
            motor.disableAirControlUntilCollision = false;
            motor.generateParametersOnAwake = true;
            motor.useGravity = true;
            motor.isFlying = false;


            InputBankTest input = obj.AddOrGetComponent<InputBankTest>();
            input.moveVector = Vector3.zero;





            CameraTargetParams ctp = obj.AddOrGetComponent<CameraTargetParams>();
            ctp.cameraParams = MiscModule.GetCharCameraParams();
            ctp.cameraPivotTransform = null;
            ctp.aimMode = CameraTargetParams.AimType.Standard;
            ctp.recoil = Vector2.zero;
            ctp.idealLocalCameraPos = Vector3.zero;
            ctp.dontRaycastToPivot = false;


            ModelLocator modelLocator = obj.AddOrGetComponent<ModelLocator>();
            modelLocator.modelTransform = modelTransform;
            modelLocator.modelBaseTransform = modelBase.transform;
            modelLocator.dontReleaseModelOnDeath = false;
            modelLocator.autoUpdateModelTransform = true;
            modelLocator.dontDetatchFromParent = false;
            modelLocator.noCorpse = false;
            modelLocator.normalizeToFloor = false;
            modelLocator.preserveModel = false;


            EntityStateMachine bodyMachine = obj.AddOrGetComponent<EntityStateMachine>();
            bodyMachine.customName = "Body";
            bodyMachine.initialStateType = SkillsCore.StateType<SpawnTeleporterState>();
            bodyMachine.mainStateType = SkillsCore.StateType<GenericCharacterMain>();


            EntityStateMachine weaponMachine = obj.AddComponent<EntityStateMachine>();
            weaponMachine.customName = "Weapon";
            weaponMachine.initialStateType = SkillsCore.StateType<Idle>();
            weaponMachine.mainStateType = SkillsCore.StateType<Idle>();


            EntityStateMachine scopeMachine = obj.AddComponent<EntityStateMachine>();
            scopeMachine.customName = "Scope";
            scopeMachine.initialStateType = SkillsCore.StateType<Idle>();
            scopeMachine.mainStateType = SkillsCore.StateType<Idle>();


            EntityStateMachine reloadMachine = obj.AddComponent<EntityStateMachine>();
            reloadMachine.customName = "Reload";
            reloadMachine.initialStateType = SkillsCore.StateType<Idle>();
            reloadMachine.mainStateType = SkillsCore.StateType<Idle>();


            EntityStateMachine[] allStateMachines = new[] { bodyMachine, weaponMachine, scopeMachine, reloadMachine };
            EntityStateMachine[] nonBodyStateMachines = new[] { weaponMachine, scopeMachine, reloadMachine };


            GenericSkill ammoSkill = obj.AddOrGetComponent<GenericSkill>();
            ammoSkill._skillFamily = SkillFamiliesModule.GetAmmoSkillFamily();
            HooksModule.AddReturnoverride( ammoSkill );


            GenericSkill passiveSkill = obj.AddComponent<GenericSkill>();
            passiveSkill._skillFamily = SkillFamiliesModule.GetPassiveSkillFamily();
            HooksModule.AddReturnoverride( passiveSkill );


            GenericSkill primarySkill = obj.AddComponent<GenericSkill>();
            primarySkill._skillFamily = SkillFamiliesModule.GetPrimarySkillFamily();


            GenericSkill secondarySkill = obj.AddComponent<GenericSkill>();
            secondarySkill._skillFamily = SkillFamiliesModule.GetSecondarySkillFamily();


            GenericSkill utilitySkill = obj.AddComponent<GenericSkill>();
            utilitySkill._skillFamily = SkillFamiliesModule.GetUtilitySkillFamily();


            GenericSkill specialSkill = obj.AddComponent<GenericSkill>();
            specialSkill._skillFamily = SkillFamiliesModule.GetSpecialSkillFamily();


            SkillLocator skillLocator = obj.AddOrGetComponent<SkillLocator>();
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


            TeamComponent team = obj.AddOrGetComponent<TeamComponent>();
            team.hideAllyCardDisplay = false;
            team.teamIndex = TeamIndex.None;


            HealthComponent health = obj.AddOrGetComponent<HealthComponent>();
            health.health = 100;
            health.shield = 0;
            health.barrier = 0;
            health.magnetiCharge = 0;
            health.body = null;
            health.dontShowHealthbar = false;
            health.globalDeathEventChanceCoefficient = 1f;


            Interactor interactor = obj.AddOrGetComponent<Interactor>();
            interactor.maxInteractionDistance = 3f;


            InteractionDriver interaction = obj.AddOrGetComponent<InteractionDriver>();
            interaction.highlightInteractor = true;


            CharacterDeathBehavior death = obj.AddOrGetComponent<CharacterDeathBehavior>();
            death.deathStateMachine = bodyMachine;
            death.deathState = SkillsCore.StateType<EntityStates.Commando.DeathState>();
            death.idleStateMachine = nonBodyStateMachines;


            CharacterNetworkTransform netTrans = obj.AddOrGetComponent<CharacterNetworkTransform>();
            netTrans.positionTransmitInterval = 0.05f;
            netTrans.lastPositionTransmitTime = Single.MinValue;
            netTrans.interpolationFactor = 3f;
            netTrans.debugDuplicatePositions = false;
            netTrans.debugSnapshotReceived = false;


            NetworkStateMachine netStates = obj.AddOrGetComponent<NetworkStateMachine>();
            netStates.stateMachines = allStateMachines;
            //stateMachines.Set( netStates, allStateMachines );


            CharacterEmoteDefinitions emotes = obj.AddOrGetComponent<CharacterEmoteDefinitions>();
            emotes.emoteDefinitions = null;


            EquipmentSlot equip = obj.AddOrGetComponent<EquipmentSlot>();


            SfxLocator sfx = obj.AddOrGetComponent<SfxLocator>();
            sfx.deathSound = "Play_ui_player_death";
            sfx.barkSound = "";
            sfx.openSound = "";
            sfx.landingSound = "Play_char_land";
            sfx.fallDamageSound = "Play_char_land_fall_damage";
            sfx.aliveLoopStart = "";
            sfx.aliveLoopStop = "";


            Rigidbody rb = obj.AddOrGetComponent<Rigidbody>();
            rb.mass = 100f;
            rb.drag = 0f;
            rb.angularDrag = 0f;
            rb.useGravity = false;
            rb.isKinematic = true;
            rb.interpolation = RigidbodyInterpolation.None;
            rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
            rb.constraints = RigidbodyConstraints.None;


            CapsuleCollider col = obj.AddOrGetComponent<CapsuleCollider>();
            col.isTrigger = false;
            col.material = null;
            col.center = new Vector3( 0f, 0f, 0f );
            col.radius = 0.5f;
            col.height = 1.82f;
            col.direction = 1;


            KinematicCharacterMotor kinCharMot = obj.AddOrGetComponent<KinematicCharacterMotor>();
            kinCharMot.CharacterController = motor;
            kinCharMot.Capsule = col;
            kinCharMot.Rigidbody = rb;
            
            kinCharMot.CapsuleRadius = 0.5f;
            kinCharMot.CapsuleHeight = 1.82f;
            kinCharMot.CapsuleYOffset = 0f;
            kinCharMot.CapsulePhysicsMaterial = null;
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


            SetStateOnHurt hurt = obj.AddOrGetComponent<SetStateOnHurt>();
            hurt.hitThreshold = 5f;
            hurt.targetStateMachine = bodyMachine;
            hurt.idleStateMachine = nonBodyStateMachines; 
            hurt.hurtState = SkillsCore.StateType<Idle>(); 
            hurt.canBeHitStunned = false;
            hurt.canBeStunned = false;
            hurt.canBeFrozen = true;




            CharacterModel charModel = model.AddOrGetComponent<CharacterModel>();
            charModel.body = body;
            charModel.itemDisplayRuleSet = ItemDisplayModule.GetSniperItemDisplay(model.AddOrGetComponent<ChildLocator>());


            HurtBoxGroup hurtBoxGroup = model.AddOrGetComponent<HurtBoxGroup>();
            HurtBox tempHb = model.GetComponentInChildren<HurtBox>();

            tempHb.gameObject.layer = LayerIndex.entityPrecise.intVal;
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


            FootstepHandler footsteps = model.AddComponent<FootstepHandler>();
            footsteps.baseFootstepString = "Play_player_footstep";
            footsteps.sprintFootstepOverrideString = "";
            footsteps.enableFootstepDust = true;
            footsteps.footstepDustPrefab = UnityEngine.Resources.Load<GameObject>( "Prefabs/GenericFootstepDust" );


            RagdollController ragdoll = model.AddOrGetComponent<RagdollController>();
            ragdoll.bones = null; // FUTURE: Setup sniper ragdoll controller
            ragdoll.componentsToDisableOnRagdoll = null;


            AimAnimator aimAnimator = model.AddOrGetComponent<AimAnimator>();
            aimAnimator.inputBank = input;
            aimAnimator.directionComponent = direction;
            aimAnimator.pitchRangeMax = 55f;
            aimAnimator.pitchRangeMin = -50f;
            aimAnimator.yawRangeMin = -44f;
            aimAnimator.yawRangeMax = 44f;

            aimAnimator.pitchGiveupRange = 30f;
            aimAnimator.yawGiveupRange = 10f;
            aimAnimator.giveupDuration = 8f;


            ModelSkinController skinController = model.AddOrGetComponent<ModelSkinController>();
            SkinDef[] skinsArray = skinController.skins;
            for( Int32 i = 0; i < skinsArray.Length; ++i )
            {
                SkinDef skin = skinsArray[i];
                skin.minionSkinReplacements = new[]
                {
                    new SkinDef.MinionSkinReplacement
                    {
                        minionBodyPrefab = DecoyModule.GetDecoyPrefab(),
                        minionSkin = skin
                    },
                };
            }

            foreach( IRuntimePrefabComponent comp in obj.GetComponents<IRuntimePrefabComponent>() )
            {
                comp.InitializePrefab();
            }
        }
    }
}
