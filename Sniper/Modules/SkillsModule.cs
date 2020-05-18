namespace Sniper.Modules
{
    using System;
    using System.Collections.Generic;

    using EntityStates;

    using ReinCore;

    using RoR2;
    using RoR2.Skills;

    using Sniper.Components;
    using Sniper.Data;
    using Sniper.Expansions;
    using Sniper.Properties;
    using Sniper.SkillDefs;
    using Sniper.SkillDefTypes.Bases;
    using Sniper.States.Primary.Fire;
    using Sniper.States.Primary.Reload;
    using Sniper.States.Secondary;
    using Sniper.States.Special;
    using Sniper.States.Utility;

    using UnityEngine;
    using UnityEngine.Networking;

    internal static class SkillsModule
    {
        internal static void CreateAmmoSkills()
        {
            var skills = new List<SkillDef>();

            #region Standard Ammo
            var standardStop = new OnBulletDelegate( (bullet, hit) =>
            {
                if( hit.collider )
                {
                    Vector3 v1 = hit.direction;
                    Vector3 v2 = hit.surfaceNormal;
                    Single dot = (v1.x * v2.x) + (v1.y * v2.y) + (v1.z * v2.z);
                    Single chance = Mathf.Clamp( Mathf.Acos( dot ) / 0.02f / Mathf.PI, 0f, 100f);
                    if( Util.CheckRoll( 100f - chance, bullet.attackerBody.master ) )
                    {
                        Vector3 newDir = (-2f * dot * v2) + v1;
                        var newBul = bullet.Clone() as ExpandableBulletAttack;
                        newBul.origin = hit.point;
                        newBul.aimVector = newDir;
                        newBul.weapon = new GameObject("temp", typeof(NetworkIdentity) );
                        if( hit.damageModifier == HurtBox.DamageModifier.SniperTarget )
                        {
                            newBul.damage *= 1.5f;
                        }

                        RicochetController.QueueRicochet( newBul, (UInt32)(hit.distance / 6f) + 1u );
                    }
                }
            });
            GameObject standardTracer = VFXModule.GetStandardAmmoTracer();
            var standardCreate = new BulletCreationDelegate( (body, reload, aim, muzzle) =>
            {
                var bullet = new ExpandableBulletAttack
                {
                    aimVector = aim.direction,
                    attackerBody = body,
                    bulletCount = 1,
                    chargeLevel = 0f,
                    damage = body.damage,
                    damageColorIndex = DamageColorIndex.Default,
                    damageType = DamageType.Generic,
                    falloffModel = BulletAttack.FalloffModel.None,
                    force = 100f,
                    HitEffectNormal = true,
                    hitEffectPrefab = null, // TODO: Standard Ammo Hit Effect
                    hitMask = LayerIndex.entityPrecise.mask,
                    isCrit = body.RollCrit(),
                    maxDistance = 1000f,
                    maxSpread = 0f,
                    minSpread = 0f,
                    muzzleName = muzzle,
                    onHit = null,
                    onStop = standardStop,
                    origin = aim.origin,
                    owner = body.gameObject,
                    procChainMask = default,
                    procCoefficient = 1f,
                    queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                    radius = 0.1f,
                    smartCollision = true,
                    sniper = false,
                    spreadPitchScale = 1f,
                    spreadYawScale = 1f,
                    stopperMask = LayerIndex.world.mask,
                    tracerEffectPrefab = standardTracer,
                    weapon = null,
                };
                return bullet;
            });
            var standardAmmo = SniperAmmoSkillDef.Create( standardCreate );
            standardAmmo.icon = UIModule.GetStandardAmmoIcon();
            standardAmmo.skillName = "Standard Ammo";
            standardAmmo.skillNameToken = Tokens.SNIPER_AMMO_STANDARD_NAME;
            standardAmmo.skillDescriptionToken = Tokens.SNIPER_AMMO_STANDARD_DESC;
            skills.Add( standardAmmo );
            #endregion


            #region Explosive Ammo
            var explosiveHit = new OnBulletDelegate((bullet, hit) =>
            {
                Single rad = 6f * ( 1f + (4f * bullet.chargeLevel) );
                EffectManager.SpawnEffect(VFXModule.GetExplosiveAmmoExplosionPrefab(), new EffectData
                {
                    origin = hit.point,
                    scale = rad,
                    rotation = Util.QuaternionSafeLookRotation(hit.direction)
                }, true);
                var blast = new BlastAttack
                {
                    attacker = bullet.owner,
                    attackerFiltering = AttackerFiltering.Default,
                    baseDamage = bullet.damage * 1f,
                    baseForce = 1f,
                    bonusForce = Vector3.zero,
                    crit = bullet.isCrit,
                    damageColorIndex = DamageColorIndex.Item,
                    damageType = bullet.damageType,
                    falloffModel = BlastAttack.FalloffModel.Linear,
                    impactEffect = EffectIndex.Invalid, // TODO: Explosive Ammo Impact Effect
                    inflictor = null,
                    losType = BlastAttack.LoSType.None,
                    position = hit.point,
                    procChainMask = bullet.procChainMask,
                    procCoefficient = bullet.procCoefficient * 0.5f,
                    radius = rad,
                    teamIndex = TeamComponent.GetObjectTeam(bullet.owner),
                };

                _ = blast.Fire();
            });
            GameObject explosiveTracer = VFXModule.GetExplosiveAmmoTracer();
            var explosiveCreate = new BulletCreationDelegate( (body, reload, aim, muzzle) =>
            {
                var bullet = new ExpandableBulletAttack
                {
                    aimVector = aim.direction,
                    attackerBody = body,
                    bulletCount = 1,
                    chargeLevel = 0f,
                    damage = body.damage * 0.65f,
                    damageColorIndex = DamageColorIndex.Default,
                    damageType = DamageType.Generic,
                    falloffModel = BulletAttack.FalloffModel.None,
                    force = 500f,
                    HitEffectNormal = true,
                    hitEffectPrefab = null, // TODO: Explosive Ammo Hit Effect
                    hitMask = LayerIndex.entityPrecise.mask,
                    isCrit = body.RollCrit(),
                    maxDistance = 1000f,
                    maxSpread = 0f,
                    minSpread = 0f,
                    muzzleName = muzzle,
                    onHit = explosiveHit,
                    onStop = null,
                    origin = aim.origin,
                    owner = body.gameObject,
                    procChainMask = default,
                    procCoefficient = 0.6f,
                    queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                    radius = 0.1f,
                    smartCollision = true,
                    sniper = false,
                    spreadPitchScale = 1f,
                    spreadYawScale = 1f,
                    stopperMask = LayerIndex.world.mask | LayerIndex.entityPrecise.mask,
                    tracerEffectPrefab = explosiveTracer,
                    weapon = null,
                };
                return bullet;
            });
            var explosive = SniperAmmoSkillDef.Create( explosiveCreate );
            explosive.icon = UIModule.GetExplosiveAmmoIcon();
            explosive.skillName = "Explosive Ammo";
            explosive.skillNameToken = Tokens.SNIPER_AMMO_EXPLOSIVE_NAME;
            explosive.skillDescriptionToken = Tokens.SNIPER_AMMO_EXPLOSIVE_DESC;
            skills.Add( explosive );
            #endregion


            #region Scatter
            GameObject scatterTracer = VFXModule.GetScatterAmmoTracer();
            BulletAttack.FalloffModel scatterFalloff = BulletFalloffCore.AddFalloffModel( (dist) => Mathf.Pow(Mathf.InverseLerp( 200f, 10f, dist ),2f) );
            var scatterCreate = new BulletCreationDelegate( (body, reload, aim, muzzle) =>
            {
                var bullet = new ExpandableBulletAttack
                {
                    aimVector = aim.direction,
                    attackerBody = body,
                    bulletCount = (UInt32)( 3 + ( 2 * (Int32)reload ) ),
                    chargeLevel = 0f,
                    damage = body.damage * 0.25f,
                    damageColorIndex = DamageColorIndex.Default,
                    damageType = DamageType.Generic,
                    falloffModel = scatterFalloff,
                    force = 25f,
                    HitEffectNormal = true,
                    hitEffectPrefab = null, // TODO: Explosive Ammo Hit Effect
                    hitMask = LayerIndex.entityPrecise.mask,
                    isCrit = body.RollCrit(),
                    maxDistance = 200f,
                    maxSpread = 2.6f,
                    minSpread = 1f,
                    muzzleName = muzzle,
                    onHit = null,
                    onStop = null,
                    origin = aim.origin,
                    owner = body.gameObject,
                    procChainMask = default,
                    procCoefficient = 0.6f,
                    queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                    radius = 0.15f,
                    smartCollision = true,
                    sniper = false,
                    spreadPitchScale = 1f,
                    spreadYawScale = 1f,
                    stopperMask = LayerIndex.world.mask | LayerIndex.entityPrecise.mask,
                    tracerEffectPrefab = scatterTracer,
                    weapon = null,
                };
                return bullet;
            });
            var scatter = SniperAmmoSkillDef.Create( scatterCreate );
            scatter.icon = UIModule.GetScatterAmmoIcon();
            scatter.skillName = "Scatter Ammo";
            scatter.skillNameToken = Tokens.SNIPER_AMMO_SCATTER_NAME;
            scatter.skillDescriptionToken = Tokens.SNIPER_AMMO_SCATTER_DESC;
            skills.Add( scatter );



            #endregion
            #region Plasma
            var plasmaHit = new OnBulletDelegate( (bullet, hit) =>
            {
                HealthComponent obj = hit.hitHurtBox?.healthComponent;
                if( obj != null && obj )
                {
                    Single dmg = bullet.damage / bullet.attackerBody.damage;
                    obj.ApplyDoT( bullet.attackerBody.gameObject, bullet.isCrit ? CatalogModule.critPlasmaBurnIndex : CatalogModule.plasmaBurnIndex, 10f, dmg );
                }
            });
            GameObject plasmaTracer = VFXModule.GetPlasmaAmmoTracer();
            var plasmaCreate = new BulletCreationDelegate( (body, reload, aim, muzzle) =>
            {
                var bullet = new ExpandableBulletAttack
                {
                    aimVector = aim.direction,
                    attackerBody = body,
                    bulletCount = 1,
                    chargeLevel = 0f,
                    damage = body.damage * 0.075f,
                    damageColorIndex = CatalogModule.plasmaDamageColor,
                    damageType = DamageType.Generic | DamageType.Silent,
                    falloffModel = BulletAttack.FalloffModel.None,
                    force = 0f,
                    HitEffectNormal = true,
                    hitEffectPrefab = null, // TODO: Plasma Ammo Hit Effect
                    hitMask = LayerIndex.entityPrecise.mask,
                    isCrit = body.RollCrit(),
                    maxDistance = 1000f,
                    maxSpread = 0f,
                    minSpread = 0f,
                    muzzleName = muzzle,
                    onHit = plasmaHit,
                    onStop = null,
                    origin = aim.origin,
                    owner = body.gameObject,
                    procChainMask = default,
                    procCoefficient = 0.5f,
                    queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                    radius = 0.15f,
                    smartCollision = true,
                    sniper = false,
                    spreadPitchScale = 1f,
                    spreadYawScale = 1f,
                    stopperMask = LayerIndex.world.mask | LayerIndex.entityPrecise.mask,
                    tracerEffectPrefab = plasmaTracer,
                    weapon = null,
                };
                return bullet;
            });
            var plasma = SniperAmmoSkillDef.Create( plasmaCreate );
            plasma.icon = UIModule.GetScatterAmmoIcon();
            plasma.skillName = "Plasma Ammo";
            plasma.skillNameToken = Tokens.SNIPER_AMMO_PLASMA_NAME;
            plasma.skillDescriptionToken = Tokens.SNIPER_AMMO_PLASMA_DESC;
            skills.Add( plasma );

            #endregion


            #region Shock


            #endregion

            SkillFamiliesModule.ammoSkills = skills;
        }

        internal static void CreatePassiveSkills()
        {
            var skills = new List<SkillDef>();

            var critPassive = SniperPassiveSkillDef.Create( BulletModifier.identity, false, 1.2f );
            critPassive.icon = UIModule.GetCritPassiveIcon();
            critPassive.skillName = "Precise Aim";
            critPassive.skillNameToken = Tokens.SNIPER_PASSIVE_CRITICAL_NAME;
            critPassive.skillDescriptionToken = Tokens.SNIPER_PASSIVE_CRITICAL_DESC;
            skills.Add( critPassive );

            var headshot = SniperPassiveSkillDef.Create( BulletModifier.identity, true, 1.0f );
            headshot.icon = UIModule.GetHeadshotPassiveIcon();
            headshot.skillName = "Headshot";
            headshot.skillNameToken = Tokens.SNIPER_PASSIVE_HEADSHOT_NAME;
            headshot.skillDescriptionToken = Tokens.SNIPER_PASSIVE_HEADSHOT_DESC;
            skills.Add( headshot );


            SkillFamiliesModule.passiveSkills = skills;
        }

        internal static void CreatePrimarySkills()
        {
            var skills = new List<SkillDef>();


            var snipe = SniperReloadableFireSkillDef.Create<DefaultSnipe,DefaultReload>("Weapon", "Weapon");
            snipe.actualMaxStock = 1;
            snipe.icon = UIModule.GetSnipeIcon();
            snipe.interruptPriority = InterruptPriority.Skill;
            snipe.isBullets = false;
            snipe.rechargeStock = 0;
            snipe.reloadIcon = UIModule.GetSnipeReloadIcon();
            snipe.reloadInterruptPriority = InterruptPriority.Skill;
            snipe.reloadParams = new ReloadParams
            {
                attackSpeedCap = 1.25f,
                attackSpeedDecayCoef = 10f,
                badMult = 0.8f,
                baseDuration = 1.5f,
                goodMult = 1.4f,
                perfectMult = 2f,
                reloadDelay = 0.4f,
                reloadEndDelay = 0.6f,
                perfectStart = 0.25f,
                perfectEnd = 0.4f,
                goodStart = 0.4f,
                goodEnd = 0.6f,
            };
            snipe.requiredStock = 1;
            snipe.shootDelay = 0.15f;
            snipe.skillDescriptionToken = Tokens.SNIPER_PRIMARY_SNIPE_DESC;
            snipe.skillNameToken = Tokens.SNIPER_PRIMARY_SNIPE_NAME;
            snipe.stockToConsume = 1;
            snipe.stockToReload = 1;
            snipe.skillName = "Snipe";
            skills.Add( snipe );

            var slide = SniperReloadableFireSkillDef.Create<SlideSnipe,SlideReload>("Weapon", "Body");
            slide.actualMaxStock = 1;
            slide.icon = null; // TODO: Slide snipe icon
            slide.interruptPriority = InterruptPriority.Skill;
            slide.isBullets = false;
            slide.rechargeStock = 0;
            slide.reloadIcon = null; // TODO: Slide Snipe Reload icon
            slide.reloadInterruptPriority = InterruptPriority.Skill;
            slide.reloadParams = new ReloadParams
            {
                attackSpeedCap = 1.25f,
                attackSpeedDecayCoef = 10f,
                badMult = 0.4f,
                baseDuration = 1.5f,
                goodMult = 1f,
                perfectMult = 2f,
                reloadDelay = 0.5f,
                reloadEndDelay = 0.75f,
                perfectStart = 0.25f,
                perfectEnd = 0.4f,
                goodStart = 0.4f,
                goodEnd = 0.6f,
            };
            slide.requiredStock = 1;
            slide.shootDelay = 0.15f;
            slide.skillDescriptionToken = Tokens.SNIPER_PRIMARY_DASH_DESC;
            slide.skillNameToken = Tokens.SNIPER_PRIMARY_DASH_NAME;
            slide.stockToConsume = 1;
            slide.stockToReload = 1;
            slide.skillName = "Slide";
            skills.Add( slide );

            SkillFamiliesModule.primarySkills = skills;
        }

        internal static void CreateSecondarySkills()
        {
            var skills = new List<SkillDef>();

            var charge = SniperScopeSkillDef.Create<DefaultScope>( UIModule.GetChargeScope(), new ZoomParams(shoulderStart: 1f, shoulderEnd: 5f,
                                                                                                             scopeStart: 3f, scopeEnd: 8f,
                                                                                                             shoulderFrac: 1f, defaultZoom: 0f,
                                                                                                             inputScale: 0.01f, baseFoV: 60f) ); // TODO: Verify and adjust zoom params
            charge.baseMaxStock = 1;
            charge.baseRechargeInterval = 0f;
            charge.icon = UIModule.GetSteadyAimIcon();
            charge.isBullets = false;
            charge.rechargeStock = 1;
            charge.requiredStock = 0;
            charge.skillDescriptionToken = Tokens.SNIPER_SECONDARY_STEADY_DESC;
            charge.skillName = "Steady Aim";
            charge.skillNameToken = Tokens.SNIPER_SECONDARY_STEADY_NAME;
            charge.stockToConsumeOnFire = 0;
            charge.stockRequiredToKeepZoom = 0;
            charge.stockRequiredToModifyFire = 0;
            charge.beginSkillCooldownOnSkillEnd = false;
            skills.Add( charge );

            var quick = SniperScopeSkillDef.Create<QuickScope>( UIModule.GetQuickScope(), new ZoomParams(shoulderStart: 1f, shoulderEnd: 5f,
                                                                                                             scopeStart: 3f, scopeEnd: 8f,
                                                                                                             shoulderFrac: 1f, defaultZoom: 0f,
                                                                                                             inputScale: 0.01f, baseFoV: 60f) ); // TODO: Verify and adjust Zoom params
            quick.baseMaxStock = 4;
            quick.baseRechargeInterval = 4f;
            quick.icon = UIModule.GetQuickScopeIcon();
            quick.isBullets = false;
            quick.rechargeStock = 1;
            quick.requiredStock = 0;
            quick.skillDescriptionToken = Tokens.SNIPER_SECONDARY_QUICK_DESC;
            quick.skillName = "Quickscope" +
                "";
            quick.skillNameToken = Tokens.SNIPER_SECONDARY_QUICK_NAME;
            quick.stockToConsumeOnFire = 1;
            quick.stockRequiredToKeepZoom = 1;
            quick.stockRequiredToModifyFire = 1;
            quick.beginSkillCooldownOnSkillEnd = true;
            skills.Add( quick );



            SkillFamiliesModule.secondarySkills = skills;
        }

        internal static void CreateUtilitySkills()
        {
            var skills = new List<SkillDef>();

            var backflip = SniperSkillDef.Create<Backflip>("Body");
            backflip.baseMaxStock = 1;
            backflip.baseRechargeInterval = 8f;
            backflip.beginSkillCooldownOnSkillEnd = true;
            backflip.canceledFromSprinting = false;
            backflip.fullRestockOnAssign = true;
            backflip.icon = UIModule.GetBackflipIcon();
            backflip.interruptPriority = InterruptPriority.PrioritySkill;
            backflip.isBullets = false;
            backflip.isCombatSkill = false;
            backflip.mustKeyPress = true;
            backflip.noSprint = true;
            backflip.rechargeStock = 1;
            backflip.requiredStock = 1;
            backflip.shootDelay = 0.1f;
            backflip.skillDescriptionToken = Tokens.SNIPER_UTILITY_BACKFLIP_DESC;
            backflip.skillName = "Military Training";
            backflip.skillNameToken = Tokens.SNIPER_UTILITY_BACKFLIP_NAME;
            backflip.stockToConsume = 1;
            skills.Add( backflip );

            SkillFamiliesModule.utilitySkills = skills;
        }

        internal static void CreateSpecialSkills()
        {
            var skills = new List<SkillDef>();

            var decoy = DecoySkillDef.Create<DecoyActivation,DecoyReactivation>( "Weapon", "Weapon" );
            decoy.baseMaxStock = 1;
            decoy.baseRechargeInterval = 18f;
            decoy.beginSkillCooldownOnSkillEnd = true;
            decoy.fullRestockOnAssign = true;
            decoy.icon = UIModule.GetDecoyIcon();
            decoy.interruptPriority = InterruptPriority.PrioritySkill;
            decoy.isCombatSkill = false;
            decoy.maxReactivationTimer = -1f;
            decoy.minReactivationTimer = 2f;
            decoy.noSprint = false;
            decoy.reactivationIcon = UIModule.GetDecoyReactivationIcon();
            decoy.reactivationInterruptPriority = InterruptPriority.PrioritySkill;
            decoy.reactivationRequiredStock = 0;
            decoy.reactivationStockToConsume = 0;
            decoy.rechargeStock = 1;
            decoy.requiredStock = 1;
            decoy.skillDescriptionToken = Tokens.SNIPER_SPECIAL_DECOY_DESC;
            decoy.skillName = "Decoy";
            decoy.skillNameToken = Tokens.SNIPER_SPECIAL_DECOY_NAME;
            decoy.startCooldownAfterReactivation = true;
            decoy.stockToConsume = 1;
            skills.Add( decoy );

            var knife = KnifeSkillDef.Create<KnifeActivation,KnifeReactivation>( "Weapon", "Body" );
            knife.baseMaxStock = 1;
            knife.baseRechargeInterval = 18f;
            knife.beginSkillCooldownOnSkillEnd = true;
            knife.fullRestockOnAssign = true;
            knife.icon = UIModule.GetKnifeIcon();
            knife.interruptPriority = InterruptPriority.PrioritySkill;
            knife.isCombatSkill = true;
            knife.maxReactivationTimer = 8f;
            knife.minReactivationTimer = 0.2f;
            knife.noSprint = true;
            knife.reactivationIcon = UIModule.GetKnifeReactivationIcon();
            knife.reactivationInterruptPriority = InterruptPriority.PrioritySkill;
            knife.reactivationRequiredStock = 0;
            knife.reactivationStockToConsume = 0;
            knife.rechargeStock = 1;
            knife.requiredStock = 1;
            knife.skillDescriptionToken = Tokens.SNIPER_SPECIAL_KNIFE_DESC;
            knife.skillName = "Blink Knife";
            knife.skillNameToken = Tokens.SNIPER_SPECIAL_KNIFE_NAME;
            knife.startCooldownAfterReactivation = true;
            knife.stockToConsume = 1;
            skills.Add( knife );
            KnifeSkillData.interruptPriority = InterruptPriority.PrioritySkill;
            KnifeSkillData.targetMachineName = "Weapon";
            KnifeSkillData.slashState = SkillsCore.StateType<KnifePickupSlash>();

            SkillFamiliesModule.specialSkills = skills;
        }
    }
}
