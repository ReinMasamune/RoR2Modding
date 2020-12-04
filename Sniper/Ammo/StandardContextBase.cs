namespace Rein.Sniper.Ammo
{
    using System;

    using RoR2;
    using Rein.Sniper.Expansions;
    using Rein.Sniper.SkillDefs;
    using Rein.Sniper.States.Bases;

    using UnityEngine;
    using UnityEngine.Networking;
    using Rein.Sniper.Modules;

    internal abstract class StandardContextBase : IAmmoStateContext
        {
            public abstract GameObject tracerEffectPrefab { get; }
            protected virtual Single durationMultiplier { get => 1f; }
            protected virtual SoundModule.FireType fireSoundType { get => SoundModule.FireType.Normal; }
            protected virtual GameObject? hitEffectPrefab { get => null; }
            protected virtual Boolean hitEffectNormal { get => true; }
            protected virtual UInt32 bulletCount { get => 1; }
            protected virtual Single maxDistance { get => 1000f; }
            protected virtual Single minSpread { get => 0f; }
            protected virtual Single maxSpread { get => 0f; }
            protected virtual Single procCoefficient { get => 1f; }
            protected virtual Single bulletRadius { get => 0.2f; }
            protected virtual LayerMask hitMask { get => LayerIndex.entityPrecise.mask | LayerIndex.world.mask; }
            protected virtual LayerMask stopperMask { get => LayerIndex.entityPrecise.mask | LayerIndex.world.mask; }
            protected virtual Boolean smartCollision { get => true; }
            protected virtual Boolean headshotCapable { get => false; }
            protected virtual Single spreadPitchScale { get => 1f; }
            protected virtual Single spreadYawScale { get => 1f; }
            protected virtual DamageColorIndex damageColor { get => DamageColorIndex.Default; }
            protected virtual DamageType damageType { get => DamageType.Generic; }
            protected virtual BulletAttack.FalloffModel bulletFalloff { get => BulletAttack.FalloffModel.None; }
            protected virtual Single baseDamageMultiplier { get => 1f; }
            protected virtual Single baseForceMultiplier { get => 1f; }
            protected virtual Single recoilMultiplier { get => 1f; }
            protected virtual Single animPlayRate { get => 1f; }
            protected virtual Single aimModeLinger { get => 5f; }
            protected virtual Boolean chargeIncreasesRecoil { get => true; }


            protected Int32 bulletsFired = 0;
            protected Single duration;
            public virtual void OnEnter<T>(SnipeState<T> state) where T : struct, ISniperPrimaryDataProvider
            {
                this.duration = this.durationMultiplier * state.baseDuration / state.characterBody.attackSpeed;
                this.FireBullet(state);           
            }
            protected void FireBullet<T>(SnipeState<T> state) where T : struct, ISniperPrimaryDataProvider
            {
                this.bulletsFired++;
                var aim = state.GetAimRay();
                state.StartAimMode(aim, this.duration + this.aimModeLinger, true);
                if(state.isAuthority)
                {
                    var bullet = this.CreateBullet();
                    this.ModifyBullet(bullet, state, aim);
                    bullet.Fire();
                    var recoil = state.recoilStrength * this.recoilMultiplier * (1f + (this.chargeIncreasesRecoil ? 0.5f * state.chargeBoost : 0f));
                    state.AddRecoil(-1f * recoil, -3f * recoil, -0.2f * recoil, 0.2f * recoil);
                }
                state.PlayAnimation("Gesture, Additive", "Shoot", "rateShoot", this.duration * animPlayRate);
                SoundModule.PlayFire(state.gameObject, state.soundFrac, this.fireSoundType);
            }
            public virtual ExpandableBulletAttack CreateBullet() => new ExpandableBulletAttack<NoData>();
            public virtual void ModifyBullet<T>(ExpandableBulletAttack bullet, SnipeState<T> state, Ray aimRay)
                 where T : struct, ISniperPrimaryDataProvider
            {
                var body = state.characterBody;

                bullet.aimVector = aimRay.direction;
                bullet.origin = aimRay.origin;
                bullet.attackerBody = body;
                bullet.isCrit = body.RollCrit();
                bullet.procChainMask = default;
                bullet.owner = body.gameObject;
                bullet.chargeBoost = state.chargeBoost;
                bullet.reloadBoost = state.reloadBoost;
                bullet.muzzleName = state.muzzleName;
                bullet.queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
                bullet.weapon = null;

                bullet.hitEffectPrefab = this.hitEffectPrefab;
                bullet.tracerEffectPrefab = this.tracerEffectPrefab;
                bullet.bulletCount = this.bulletCount;
                bullet.HitEffectNormal = this.hitEffectNormal;
                bullet.maxDistance = this.maxDistance;
                bullet.maxSpread = this.maxSpread;
                bullet.minSpread = this.minSpread;
                bullet.procCoefficient = this.procCoefficient;
                bullet.radius = this.bulletRadius;
                bullet.hitMask = this.hitMask;
                bullet.stopperMask = this.stopperMask;
                bullet.sniper = this.headshotCapable;
                bullet.smartCollision = this.smartCollision;
                bullet.spreadPitchScale = this.spreadPitchScale;
                bullet.spreadYawScale = this.spreadYawScale;
                bullet.damageColorIndex = this.damageColor;
                bullet.damageType = this.damageType;
                bullet.falloffModel = this.bulletFalloff;

                bullet.damage = body.damage * state.damageMultiplier * this.baseDamageMultiplier;
                bullet.force = state.forceMultiplier * this.baseForceMultiplier;
            }
            public virtual void FixedUpdate<T>(SnipeState<T> state) where T : struct, ISniperPrimaryDataProvider
            {
                if(state.isAuthority && state.fixedAge >= this.duration)
                {
                    state.outer.SetNextStateToMain();
                }
            }

            public virtual void OnExit<T>(SnipeState<T> state) where T : struct, ISniperPrimaryDataProvider
            {
            }

            public virtual void OnDeserialize<T>(SnipeState<T> state, NetworkReader reader) where T : struct, ISniperPrimaryDataProvider { }
            public virtual void OnSerialize<T>(SnipeState<T> state, NetworkWriter writer) where T : struct, ISniperPrimaryDataProvider { }
        }

}
