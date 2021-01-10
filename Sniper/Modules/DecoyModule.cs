namespace Rein.Sniper.Modules
{
    using System;
    using System.Collections.Generic;

    using EntityStates;

    using ReinCore;

    using RoR2;
    using RoR2.CharacterAI;
    using RoR2.Networking;

    using Rein.Sniper.Components;
    using Rein.Sniper.States.Other;
    using Rein.Sniper.States.Special;

    using UnityEngine;
    using UnityEngine.Networking;

    internal static class DecoyModule
    {
        internal static readonly DeployableSlot deployableSlot;
        static DecoyModule()
        {
            deployableSlot = DeployablesCore.AddDeployableSlot( new DeployableSlotDef( 1 ) );
        }



        internal static HashSet<ItemIndex> whitelist = new HashSet<ItemIndex>
        {
            ItemIndex.ArmorPlate,
            ItemIndex.BarrierOnOverHeal,
            ItemIndex.Bear,
            ItemIndex.ExtraLife,
            ItemIndex.FallBoots,
            ItemIndex.HealWhileSafe,
            ItemIndex.IncreaseHealing,
            ItemIndex.InvadingDoppelganger,
            ItemIndex.Infusion,
            ItemIndex.Knurl,
            ItemIndex.Medkit,
            ItemIndex.Mushroom,
            ItemIndex.NovaOnHeal,
            ItemIndex.Pearl,
            ItemIndex.PersonalShield,
            ItemIndex.RepeatHeal,
            ItemIndex.ShieldOnly,
            ItemIndex.ShinyPearl,
            ItemIndex.SiphonOnLowHealth,
            ItemIndex.SlowOnHit,
            ItemIndex.SprintArmor,
            ItemIndex.TonicAffliction,
            ItemIndex.WardOnLevel
        };


        private static GameObject decoyPrefab;
        internal static GameObject GetDecoyPrefab()
        {
            if( decoyPrefab == null )
            {
                decoyPrefab = CreateDecoyPrefab();
            }

            return decoyPrefab;
        }

        private static GameObject decoyMaster;
        internal static GameObject GetDecoyMaster()
        {
            if( decoyMaster == null )
            {
                decoyMaster = CreateDecoyMaster();
            }
            return decoyMaster;
        }

        private static GameObject CreateDecoyPrefab()
        {
            GameObject obj = PrefabsCore.CreatePrefab("SniperDecoy", true );

            TeamComponent teamComp = obj.AddOrGetComponent<TeamComponent>();
            teamComp.hideAllyCardDisplay = false;
            teamComp.teamIndex = TeamIndex.None;


            NetworkIdentity netId = obj.AddOrGetComponent<NetworkIdentity>();


            Transform modelBase = new GameObject( "ModelBase" ).transform;
            modelBase.parent = obj.transform;
            modelBase.localPosition = new Vector3( 0f, -0.81f, 0f );
            modelBase.localScale = Vector3.one;
            modelBase.localRotation = Quaternion.identity;


            GameObject mdlSniper = UnityEngine.Object.Instantiate<GameObject>( ModelModule.GetModel(), modelBase );
            mdlSniper.transform.localPosition = Vector3.zero;
            mdlSniper.transform.localScale = Vector3.one;
            mdlSniper.transform.localRotation = Quaternion.identity;



            CharacterBody body = obj.AddOrGetComponent<CharacterBody>();
            body.bodyIndex = -1;
            body.baseNameToken = Properties.Tokens.SNIPER_NAME;
            body.subtitleNameToken = Properties.Tokens.SNIPER_SUBTITLE;
            body.bodyFlags = CharacterBody.BodyFlags.ResistantToAOE | CharacterBody.BodyFlags.ImmuneToExecutes;
            body.rootMotionInMainState = false;
            body.mainRootSpeed = 0f;

            // CLEANUP: Abstract out base stats for decoy and sniper
            body.baseMaxHealth = 130f;
            body.levelMaxHealth = 39f;

            body.baseRegen = 1f;
            body.levelRegen = 0.2f;

            body.baseMaxShield = 0f;
            body.levelMaxShield = 0f;

            body.baseMoveSpeed = 0f;
            body.levelMoveSpeed = 0f;

            body.baseAcceleration = 0f;

            body.baseJumpPower = 0f;
            body.levelJumpPower = 0f;

            body.baseDamage = 12f;
            body.levelDamage = 2.4f;

            body.baseAttackSpeed = 1f;
            body.levelAttackSpeed = 0f;

            body.baseCrit = 0f;
            body.levelCrit = 0f;

            body.baseArmor = 50f;
            body.levelArmor = 10f;

            body.baseJumpCount = 1;

            body.sprintingSpeedMultiplier = 0f;

            //body.killCount = 0;
            body.wasLucky = false;
            body.spreadBloomDecayTime = 0.45f;
            body.spreadBloomCurve = new AnimationCurve();
            body.crosshairPrefab = null;
            body.hideCrosshair = false;
            body.aimOriginTransform = body.transform;
            body.hullClassification = HullClassification.Human;
            body.portraitIcon = UIModule.GetPortraitIcon();
            body.isChampion = false;
            body.currentVehicle = null;
            body.preferredPodPrefab = null;
            body.preferredInitialStateType = SkillsCore.StateType<GenericCharacterMain>();
            body.skinIndex = 0u;




            CharacterModel model = mdlSniper.AddOrGetComponent<CharacterModel>();
            model.body = body;
            model.itemDisplayRuleSet = ItemDisplayModule.GetSniperItemDisplay(model.AddOrGetComponent<ChildLocator>());



            HealthComponent health = obj.AddOrGetComponent<HealthComponent>();
            health.health = 100;
            health.shield = 0;
            health.barrier = 0;
            health.magnetiCharge = 0;
            health.body = null;
            health.dontShowHealthbar = false;
            health.globalDeathEventChanceCoefficient = 1f;



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


            InputBankTest inputs = obj.AddOrGetComponent<InputBankTest>();
            inputs.moveVector = Vector3.zero;


            CameraTargetParams cameraTargetParams = obj.AddOrGetComponent<CameraTargetParams>();
            cameraTargetParams.cameraParams = MiscModule.GetCharCameraParams();
            cameraTargetParams.cameraPivotTransform = null;
            cameraTargetParams.aimMode = CameraTargetParams.AimType.Standard;
            cameraTargetParams.recoil = Vector2.zero;
            cameraTargetParams.idealLocalCameraPos = Vector3.zero;
            cameraTargetParams.dontRaycastToPivot = false;



            ModelLocator modelLocator = obj.AddOrGetComponent<ModelLocator>();
            modelLocator.modelTransform = mdlSniper.transform;
            modelLocator.modelBaseTransform = modelBase;
            modelLocator.dontReleaseModelOnDeath = false;
            modelLocator.autoUpdateModelTransform = true;
            modelLocator.dontDetatchFromParent = false;
            modelLocator.noCorpse = true;
            modelLocator.normalizeToFloor = false;
            modelLocator.preserveModel = false;


            EntityStateMachine esm = obj.AddOrGetComponent<EntityStateMachine>();
            esm.customName = "Body";
            esm.initialStateType = SkillsCore.StateType<BeingADecoy>();
            esm.mainStateType = SkillsCore.StateType<BeingADecoy>();


            SkillLocator skillLocator = obj.AddOrGetComponent<SkillLocator>();
            skillLocator.primary = null;
            skillLocator.secondary = null;
            skillLocator.utility = null;
            skillLocator.special = null;


            CharacterDeathBehavior deathBehaviour = obj.AddOrGetComponent<CharacterDeathBehavior>();
            deathBehaviour.deathStateMachine = esm;
            deathBehaviour.deathState = SkillsCore.StateType<DecoyDeathState>();
            deathBehaviour.idleStateMachine = Array.Empty<EntityStateMachine>();


            CharacterNetworkTransform netTrans = obj.AddOrGetComponent<CharacterNetworkTransform>();
            netTrans.positionTransmitInterval = 0.1f;
            netTrans.lastPositionTransmitTime = Single.NegativeInfinity;
            netTrans.interpolationFactor = 2f;
            netTrans.debugDuplicatePositions = false;
            netTrans.debugSnapshotReceived = false;


            NetworkStateMachine netStates = obj.AddOrGetComponent<NetworkStateMachine>();
            //netStates._SetStateMachines( esm );
            netStates.stateMachines = new[] { esm };


            Interactor interactor = obj.AddOrGetComponent<Interactor>();
            interactor.maxInteractionDistance = 1f;


            InteractionDriver interactionDriver = obj.AddOrGetComponent<InteractionDriver>();
            interactionDriver.highlightInteractor = false;


            CapsuleCollider cap = obj.AddOrGetComponent<CapsuleCollider>();
            cap.isTrigger = false;
            cap.material = null;
            cap.center = Vector3.zero;
            cap.radius = 0.5f;
            cap.height = 1.82f;
            cap.direction = 1;


            SetStateOnHurt hurtState = obj.AddOrGetComponent<SetStateOnHurt>();
            hurtState.hitThreshold = 5f;
            hurtState.targetStateMachine = esm;
            hurtState.idleStateMachine = Array.Empty<EntityStateMachine>();
            hurtState.hurtState = SkillsCore.StateType<Idle>();
            hurtState.canBeHitStunned = false;
            hurtState.canBeFrozen = true;
            hurtState.canBeStunned = false;


            SfxLocator sfx = obj.AddOrGetComponent<SfxLocator>();
            // FUTURE: Death sounds for decoy


            Rigidbody rb = obj.AddOrGetComponent<Rigidbody>();
            rb.mass = 1000f;
            rb.drag = 0f;
            rb.angularDrag = 0f;
            rb.useGravity = false;
            rb.isKinematic = true;
            rb.interpolation = RigidbodyInterpolation.None;
            rb.collisionDetectionMode = CollisionDetectionMode.Discrete;


            CharacterMotor charMot = obj.AddOrGetComponent<CharacterMotor>();
            charMot.walkSpeedPenaltyCoefficient = 1f;
            CharacterDirection charDir = charMot.characterDirection = obj.AddOrGetComponent<CharacterDirection>();
            charMot.muteWalkMotion = false;
            charMot.mass = 1000f;
            charMot.airControl = 0.25f;
            charMot.disableAirControlUntilCollision = false;
            charMot.generateParametersOnAwake = true;
            charMot.useGravity = true;
            charMot.isFlying = false;


            charDir.moveVector = Vector3.zero;
            charDir.targetTransform = modelBase;
            charDir.overrideAnimatorForwardTransform = null;
            charDir.rootMotionAccumulator = null;
            charDir.modelAnimator = null;
            charDir.driveFromRootRotation = false;
            charDir.driveFromRootRotation = false;
            charDir.turnSpeed = 180f;


            KinematicCharacterController.KinematicCharacterMotor kinCharMot = obj.AddOrGetComponent<KinematicCharacterController.KinematicCharacterMotor>();
            kinCharMot.CharacterController = charMot;
            kinCharMot.Capsule = cap;
            kinCharMot.Rigidbody = rb;
            kinCharMot.CapsuleRadius = 0.5f;
            kinCharMot.CapsuleHeight = 1.8f;
            kinCharMot.CapsuleYOffset = 0f;
            kinCharMot.DetectDiscreteCollisions = false;
            kinCharMot.GroundDetectionExtraDistance = 0f;
            kinCharMot.MaxStepHeight = 0.5f;
            kinCharMot.MinRequiredStepDepth = 0.1f;
            kinCharMot.MaxStableSlopeAngle = 55f;
            kinCharMot.MaxStableDistanceFromLedge = 0.5f;
            kinCharMot.PreventSnappingOnLedges = false;
            kinCharMot.MaxStableDenivelationAngle = 55f;
            kinCharMot.RigidbodyInteractionType = KinematicCharacterController.RigidbodyInteractionType.None;
            kinCharMot.PreserveAttachedRigidbodyMomentum = true;
            kinCharMot.HasPlanarConstraint = false;
            kinCharMot.PlanarConstraintAxis = Vector3.up;
            kinCharMot.StepHandling = KinematicCharacterController.StepHandlingMethod.Standard;
            kinCharMot.LedgeHandling = true;
            kinCharMot.InteractiveRigidbodyHandling = true;
            kinCharMot.SafeMovement = false;


            _ = obj.AddComponent<DecoyDeployableSync>();


            obj.layer = LayerIndex.fakeActor.intVal;


            foreach( IRuntimePrefabComponent comp in obj.GetComponents<IRuntimePrefabComponent>() )
            {
                comp.InitializePrefab();
            }


            BodyCatalog.getAdditionalEntries += ( list ) => list.Add( obj );

            return obj;
        }


        private static GameObject CreateDecoyMaster()
        {
            GameObject master = PrefabsCore.CreatePrefab("SniperDecoyMaster",true);

            NetworkIdentity netId = master.AddOrGetComponent<NetworkIdentity>();

            CharacterMaster charMaster = master.AddOrGetComponent<CharacterMaster>();
            charMaster.masterIndex = new MasterCatalog.MasterIndex( -1 );
            charMaster.bodyPrefab = GetDecoyPrefab();
            charMaster.spawnOnStart = false;
            charMaster.teamIndex = TeamIndex.Player;
            charMaster.destroyOnBodyDeath = true;
            charMaster.isBoss = false;
            charMaster.preventGameOver = true;


            _ = master.AddOrGetComponent<Inventory>();


            BaseAI ai = master.AddOrGetComponent<BaseAI>();
            ai.fullVision = true;
            ai.neverRetaliateFriendlies = true;
            ai.minDistanceFromEnemy = 0;
            ai.enemyAttentionDuration = 0;
            ai.navigationType = BaseAI.NavigationType.Nodegraph;
            ai.desiredSpawnNodeGraphType = RoR2.Navigation.MapNodeGroup.GraphType.Ground;
            EntityStateMachine esm = ai.stateMachine = master.AddOrGetComponent<EntityStateMachine>();
            ai.isHealer = false;
            ai.enemyAttention = 0f;
            ai.aimVectorDampTime = 0f;
            ai.aimVectorMaxSpeed = 0f;
            ai.desiredAimDirection = Vector3.up;
            ai.drawAIPath = false;
            ai.selectedSkilldriverName = null;
            ai.debugEnemyHurtBox = null;
            ai.currentEnemy = null;
            ai.leader = null;
            ai.customTarget = null;

            esm.customName = "AI";
            esm.initialStateType = SkillsCore.StateType<EntityStates.AI.BaseAIState>();
            esm.mainStateType = SkillsCore.StateType<EntityStates.AI.BaseAIState>();

            AISkillDriver driver = master.AddOrGetComponent<AISkillDriver>();
            driver.customName = "Sit there and look pretty";
            driver.skillSlot = SkillSlot.None;
            driver.requiredSkill = null;
            driver.requireSkillReady = false;
            driver.requireEquipmentReady = false;
            driver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            driver.minUserHealthFraction = Single.NegativeInfinity;
            driver.maxUserHealthFraction = Single.PositiveInfinity;
            driver.minTargetHealthFraction = Single.NegativeInfinity;
            driver.maxTargetHealthFraction = Single.PositiveInfinity;
            driver.minDistance = Single.NegativeInfinity;
            driver.maxDistance = Single.PositiveInfinity;
            driver.selectionRequiresTargetLoS = false;
            driver.activationRequiresTargetLoS = false;
            driver.activationRequiresAimConfirmation = false;
            driver.movementType = AISkillDriver.MovementType.Stop;
            driver.moveInputScale = 0f;
            driver.aimType = AISkillDriver.AimType.None;
            driver.ignoreNodeGraph = true;
            driver.driverUpdateTimerOverride = 1f;
            driver.resetCurrentEnemyOnNextDriverSelection = false;
            driver.noRepeat = false;
            driver.shouldSprint = true;
            driver.shouldFireEquipment = false;
            driver.shouldTapButton = false;


            _ = master.AddOrGetComponent<MinionOwnership>();


            AIOwnership aiOwnership = master.AddOrGetComponent<AIOwnership>();
            aiOwnership.ownerMaster = null;


            foreach( IRuntimePrefabComponent comp in master.GetComponents<IRuntimePrefabComponent>() )
            {
                comp.InitializePrefab();
            }


            MasterCatalog.getAdditionalEntries += ( list ) => list.Add( master );

            return master;
        }
    }
}
