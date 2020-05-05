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
using RoR2.Skills;
using Sniper.Data;
using Sniper.Properties;
using Sniper.Expansions;
using Sniper.Components;
using UnityEngine.Networking;
using Sniper.SkillDefs;
using Sniper.States.Primary.Fire;
using Sniper.States.Primary.Reload;
using Sniper.States.Secondary;
using Sniper.States.Special;
using Sniper.States.Utility;
using Sniper.SkillDefTypes.Bases;

namespace Sniper.Modules
{
    internal static class SkillsModule
    {
        internal static void CreateAmmoSkills()
        {
            var skills = new List<SkillDef>();

            BulletModifier standardModifier = BulletModifier.identity;
            standardModifier.stopperMaskRemove = LayerIndex.entityPrecise.mask;
            OnBulletDelegate standardStop = new OnBulletDelegate( (bullet, hit) =>
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
                        if( hit.damageModifier == HurtBox.DamageModifier.SniperTarget ) newBul.damage *= 1.5f;
                        RicochetController.QueueRicochet( newBul, (UInt32)(hit.distance / 6f) + 1u );
                    }
                }
            });
            var standardAmmo = SniperAmmoSkillDef.Create( null, standardStop, standardModifier, null, VFXModule.GetStandardAmmoTracer() );
            standardAmmo.icon = UIModule.GetStandardAmmoIcon();
            standardAmmo.skillName = "Standard Ammo";
            standardAmmo.skillNameToken = Tokens.SNIPER_AMMO_STANDARD_NAME;
            standardAmmo.skillDescriptionToken = Tokens.SNIPER_AMMO_STANDARD_DESC;
            skills.Add( standardAmmo );

            BulletModifier explosiveModifier = BulletModifier.identity;
            explosiveModifier.damageMultiplier = 0.6f;
            explosiveModifier.procMultiplier = 0.6f;
            explosiveModifier.forceMultiplier = 0.5f;
            OnBulletDelegate explosiveOnHit = new OnBulletDelegate((bullet, hit) =>
            {
                Single rad = 4f * ( 1f + (4f * bullet.chargeLevel) );
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
            var explosive = SniperAmmoSkillDef.Create( explosiveOnHit, null, explosiveModifier, null, VFXModule.GetExplosiveAmmoTracer() );
            explosive.icon = UIModule.GetExplosiveAmmoIcon();
            explosive.skillName = "Piercing Ammo";
            explosive.skillNameToken = Tokens.SNIPER_AMMO_EXPLOSIVE_NAME;
            explosive.skillDescriptionToken = Tokens.SNIPER_AMMO_EXPLOSIVE_DESC;
            skills.Add( explosive );

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
            snipe.baseMaxStock = 1;
            snipe.icon = UIModule.GetSnipeIcon();
            snipe.interruptPriority = InterruptPriority.Skill;
            snipe.isBullets = false;
            snipe.rechargeStock = 0;
            snipe.reloadIcon = UIModule.GetSnipeReloadIcon();
            snipe.reloadInterruptPriority = InterruptPriority.Skill;
            snipe.reloadParams = new ReloadParams
            {
                attackSpeedCap = 2f,
                attackSpeedDecayCoef = 10f,
                badMult = 0.8f,
                baseDuration = 1.5f,
                goodMult = 1.4f,
                perfectMult = 2f,
                reloadDelay = 0.3f,
                reloadEndDelay = 0.2f,
                perfectStart = 0.25f,
                perfectEnd = 0.35f,
                goodStart = 0.35f,
                goodEnd = 0.6f,
            };
            snipe.requiredStock = 1;
            snipe.shootDelay = 0.15f;
            snipe.skillDescriptionToken = Tokens.SNIPER_PRIMARY_SNIPE_DESC;
            snipe.skillNameToken = Tokens.SNIPER_PRIMARY_SNIPE_NAME;
            snipe.stockToConsume = 1;
            snipe.stockToReload = 1;
            skills.Add( snipe );

            var slide = SniperReloadableFireSkillDef.Create<SlideSnipe,SlideReload>("Weapon", "Body");
            slide.baseMaxStock = 1;
            slide.icon = null; // TODO: Assign
            slide.interruptPriority = InterruptPriority.Skill;
            slide.isBullets = false;
            slide.rechargeStock = 0;
            slide.reloadIcon = null; // TODO: Assign
            slide.reloadInterruptPriority = InterruptPriority.Skill;
            slide.reloadParams = new ReloadParams
            {
                attackSpeedCap = 2f,
                attackSpeedDecayCoef = 10f,
                badMult = 0.4f,
                baseDuration = 1.5f,
                goodMult = 1f,
                perfectMult = 2f,
                reloadDelay = 0.4f,
                reloadEndDelay = 0.5f,
                perfectStart = 0.25f,
                perfectEnd = 0.35f,
                goodStart = 0.35f,
                goodEnd = 0.6f,
            };
            slide.requiredStock = 1;
            slide.shootDelay = 0.15f;
            slide.skillDescriptionToken = Tokens.SNIPER_PRIMARY_DASH_DESC;
            slide.skillNameToken = Tokens.SNIPER_PRIMARY_DASH_NAME;
            slide.stockToConsume = 1;
            slide.stockToReload = 1;
            skills.Add( slide );

            SkillFamiliesModule.primarySkills = skills;
        }

        internal static void CreateSecondarySkills()
        {
            var skills = new List<SkillDef>();

            var charge = SniperScopeSkillDef.Create<DefaultScope>( UIModule.GetChargeScope(), new ZoomParams(shoulderStart: 1f, shoulderEnd: 5f,
                                                                                                             scopeStart: 3f, scopeEnd: 8f,
                                                                                                             shoulderFrac: 1f, defaultZoom: 0f,
                                                                                                             inputScale: 0.01f, baseFoV: 60f) ); // TODO: Zoom params
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

            var quick = SniperScopeSkillDef.Create<DefaultScope>( UIModule.GetQuickScope(), new ZoomParams(shoulderStart: 1f, shoulderEnd: 5f,
                                                                                                             scopeStart: 3f, scopeEnd: 8f,
                                                                                                             shoulderFrac: 1f, defaultZoom: 0f,
                                                                                                             inputScale: 0.01f, baseFoV: 60f) ); // TODO: Zoom params
            quick.baseMaxStock = 4;
            quick.baseRechargeInterval = 8f;
            quick.icon = UIModule.GetQuickScopeIcon();
            quick.isBullets = false;
            quick.rechargeStock = 1;
            quick.requiredStock = 0;
            quick.skillDescriptionToken = Tokens.SNIPER_SECONDARY_QUICK_DESC;
            quick.skillName = "Steady Aim";
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
            backflip.icon = UIModule.GetBackflipIcon(); // TODO: Assign
            backflip.interruptPriority = InterruptPriority.Skill;
            backflip.isBullets = false;
            backflip.isCombatSkill = true;
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
            decoy.icon = UIModule.GetDecoyIcon(); // TODO: Assign
            decoy.interruptPriority = InterruptPriority.Skill;
            decoy.isCombatSkill = false;
            decoy.maxReactivationTimer = 6f;
            decoy.minReactivationTimer = 2f;
            decoy.noSprint = false;
            decoy.reactivationIcon = UIModule.GetDecoyReactivationIcon(); // TODO: Assign
            decoy.reactivationInterruptPriority = InterruptPriority.Skill;
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
            knife.baseRechargeInterval = 14f;
            knife.beginSkillCooldownOnSkillEnd = true;
            knife.fullRestockOnAssign = true;
            knife.icon = UIModule.GetKnifeIcon(); // TODO: Assign
            knife.interruptPriority = InterruptPriority.Skill;
            knife.isCombatSkill = true;
            knife.maxReactivationTimer = 6f;
            knife.minReactivationTimer = 0.5f;
            knife.noSprint = true;
            knife.reactivationIcon = UIModule.GetKnifeReactivationIcon(); // TODO: Assign
            knife.reactivationInterruptPriority = InterruptPriority.Skill;
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
            
            SkillFamiliesModule.specialSkills = skills;
        }
    }
}
