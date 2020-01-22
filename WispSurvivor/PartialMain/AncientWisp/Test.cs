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
            this.Load += this.AW_GetSecondaryEffectStuff;
            this.Load += this.AW_GetSpecialProjectileStuff;
            this.Load += this.AW_RegisterOrbStuff;

        }



        private void AW_RegisterOrbStuff()
        {
            OrbAPI.AddOrb( typeof( UniversalHealOrb ) );
        }

        private void AW_GetSpecialProjectileStuff()
        {
            var obj1 = Resources.Load<GameObject>("Prefabs/Projectiles/EvisProjectile" ).InstantiateClone("AWZoneLaunch" );
            var obj2 = Resources.Load<GameObject>("Prefabs/Projectiles/EvisOverlapProjectile").InstantiateClone("AWZone" );

            var impactExplosion1 = obj1.GetComponent<ProjectileImpactExplosion>();
            impactExplosion1.childrenProjectilePrefab = obj2;

            var healComp = obj2.AddComponent<ProjectileUniversalHealOrbOnDamage>();
            healComp.healType = UniversalHealOrb.HealType.PercentMax;
            healComp.healTarget = UniversalHealOrb.HealTarget.Barrier;
            healComp.value = 0.05f;
            healComp.effectPrefab = Resources.Load<GameObject>( "Prefabs/Effects/OrbEffects/HealthOrbEffect" );

            this.AW_utilProj = obj1;
            this.AW_utilZoneProj = obj2;

            ProjectileCatalog.getAdditionalEntries += ( list ) => list.Add( this.AW_utilProj );
            ProjectileCatalog.getAdditionalEntries += ( list ) => list.Add( this.AW_utilZoneProj );
        }


        private void AW_GetSecondaryEffectStuff()
        {
            var effect1 = Resources.Load<GameObject>("Prefabs/Effects/MeteorStrikePredictionEffect").InstantiateClone("AncientWispPillarPrediction", false);
            Destroy( effect1.GetComponent<DestroyOnTimer>() );
            Destroy( effect1.GetComponent<EffectComponent>() );


            var effect2 = Resources.Load<GameObject>("Prefabs/Effects/ImpactEffects/MeteorStrikeImpact").InstantiateClone("AncientWispPillar", false);
            EffectAPI.AddEffect( effect2 );

            this.AW_secDelayEffect = effect1;
            this.AW_secExplodeEffect = effect2;
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
            LoadoutAPI.AddSkill( typeof( AWSecondary ) );

            secondaryDef.activationState = new EntityStates.SerializableEntityStateType( typeof( AWSecondary ) );
            secondaryDef.activationStateMachineName = "Weapon";
            secondaryDef.baseMaxStock = 1;
            secondaryDef.baseRechargeInterval = 10f;
            secondaryDef.beginSkillCooldownOnSkillEnd = true;
            secondaryDef.canceledFromSprinting = false;
            secondaryDef.fullRestockOnAssign = true;
            secondaryDef.icon = Resources.Load<Sprite>( "NotAPath" );
            secondaryDef.interruptPriority = EntityStates.InterruptPriority.Skill;
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

            secondaryFam.variants = new SkillFamily.Variant[]
            {
                new SkillFamily.Variant
                {
                    skillDef = secondaryDef,
                    unlockableName = "",
                    viewableNode = new ViewablesCatalog.Node( "aaaa", false )
                }
            };
            skillLoc.secondary.SetFieldValue<SkillFamily>( "_skillFamily", secondaryFam );


            var utilityFam = ScriptableObject.CreateInstance<SkillFamily>();
            var utilityDef = ScriptableObject.CreateInstance<SkillDef>();
            LoadoutAPI.AddSkillDef( utilityDef );
            LoadoutAPI.AddSkillFamily( utilityFam );
            LoadoutAPI.AddSkill( typeof( AWChargeUtility ) );
            LoadoutAPI.AddSkill( typeof( AWFireUtility ) );

            utilityDef.activationState = new EntityStates.SerializableEntityStateType( typeof( AWChargeUtility ) );
            utilityDef.activationStateMachineName = "Weapon";
            utilityDef.baseMaxStock = 1;
            utilityDef.baseRechargeInterval = 10f;
            utilityDef.beginSkillCooldownOnSkillEnd = true;
            utilityDef.canceledFromSprinting = false;
            utilityDef.fullRestockOnAssign = true;
            utilityDef.icon = Resources.Load<Sprite>( "NotAPath" );
            utilityDef.interruptPriority = EntityStates.InterruptPriority.Skill;
            utilityDef.isBullets = false;
            utilityDef.isCombatSkill = true;
            utilityDef.mustKeyPress = false;
            utilityDef.noSprint = false;
            utilityDef.rechargeStock = 1;
            utilityDef.requiredStock = 1;
            utilityDef.shootDelay = 0.1f;
            utilityDef.skillDescriptionToken = "";
            utilityDef.skillName = "";
            utilityDef.skillNameToken = "";
            utilityDef.stockToConsume = 1;

            utilityFam.variants = new SkillFamily.Variant[]
            {
                new SkillFamily.Variant
                {
                    skillDef = utilityDef,
                    unlockableName = "",
                    viewableNode = new ViewablesCatalog.Node( "aaaaaa", false )
                }
            };
            skillLoc.utility.SetFieldValue<SkillFamily>( "_skillFamily", utilityFam );

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
            var secondaryDriver = this.AW_master.AddComponent<AISkillDriver>();
            var utilityDriver = this.AW_master.AddComponent<AISkillDriver>();
            //var specialDriver = this.AW_master.AddComponent<AISkillDriver>();
            var chaseDriver = this.AW_master.AddComponent<AISkillDriver>();
            var strafeDriver = this.AW_master.AddComponent<AISkillDriver>();

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

            secondaryDriver.customName = "Secondary";
            secondaryDriver.skillSlot = SkillSlot.Secondary;
            secondaryDriver.requiredSkill = null;
            secondaryDriver.requireSkillReady = true;
            secondaryDriver.requireEquipmentReady = false;
            secondaryDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            secondaryDriver.minUserHealthFraction = Single.NegativeInfinity;
            secondaryDriver.maxUserHealthFraction = Single.PositiveInfinity;
            secondaryDriver.minTargetHealthFraction = Single.NegativeInfinity;
            secondaryDriver.maxTargetHealthFraction = Single.PositiveInfinity;
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
            utilityDriver.requiredSkill = null;
            utilityDriver.requireSkillReady = true;
            utilityDriver.requireEquipmentReady = false;
            utilityDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            utilityDriver.minUserHealthFraction = Single.NegativeInfinity;
            utilityDriver.maxUserHealthFraction = Single.PositiveInfinity;
            utilityDriver.minTargetHealthFraction = Single.NegativeInfinity;
            utilityDriver.maxTargetHealthFraction = Single.PositiveInfinity;
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

            chaseDriver.customName = "Chase";
            chaseDriver.skillSlot = SkillSlot.None;
            chaseDriver.requireSkillReady = false;
            chaseDriver.requireEquipmentReady = false;
            chaseDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            chaseDriver.minUserHealthFraction = Single.NegativeInfinity;
            chaseDriver.maxUserHealthFraction = Single.PositiveInfinity;
            chaseDriver.minTargetHealthFraction = Single.NegativeInfinity;
            chaseDriver.maxTargetHealthFraction = Single.PositiveInfinity;
            chaseDriver.minDistance = 50f;
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
            chaseDriver.shouldSprint = true;
            chaseDriver.shouldFireEquipment = false;

            strafeDriver.customName = "Chase";
            strafeDriver.skillSlot = SkillSlot.None;
            strafeDriver.requireSkillReady = false;
            strafeDriver.requireEquipmentReady = false;
            strafeDriver.moveTargetType = AISkillDriver.TargetType.CurrentEnemy;
            strafeDriver.minUserHealthFraction = Single.NegativeInfinity;
            strafeDriver.maxUserHealthFraction = Single.PositiveInfinity;
            strafeDriver.minTargetHealthFraction = Single.NegativeInfinity;
            strafeDriver.maxTargetHealthFraction = Single.PositiveInfinity;
            strafeDriver.minDistance = 0f;
            strafeDriver.maxDistance = 50f;
            strafeDriver.selectionRequiresTargetLoS = false;
            strafeDriver.activationRequiresTargetLoS = false;
            strafeDriver.movementType = AISkillDriver.MovementType.StrafeMovetarget;
            strafeDriver.moveInputScale = 1f;
            strafeDriver.aimType = AISkillDriver.AimType.AtMoveTarget;
            strafeDriver.ignoreNodeGraph = false;
            strafeDriver.driverUpdateTimerOverride = -1f;
            strafeDriver.resetCurrentEnemyOnNextDriverSelection = false;
            strafeDriver.noRepeat = false;
            strafeDriver.shouldSprint = true;
            strafeDriver.shouldFireEquipment = false;
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
