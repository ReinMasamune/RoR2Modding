namespace Rein.Sniper.Ammo
{
    using System;

    using RoR2;
    using Rein.Sniper.Expansions;
    using Rein.Sniper.States.Bases;

    using UnityEngine;
    using Rein.Sniper.Modules;

    internal class ExplosiveContext : OnHitContextBase<NoData>
        {
            private static readonly GameObject _tracer = VFXModule.GetExplosiveAmmoTracer();
            public override GameObject tracerEffectPrefab => _tracer;
            protected override Single baseDamageMultiplier => 0.2f;
            protected override Single procCoefficient => 1f;
            protected override Single bulletRadius => 0.5f;
            protected override OnBulletDelegate<NoData> onHit => (bullet, hit) =>
            {
                Single rad = 8f * (1f + bullet.chargeBoost);
                EffectManager.SpawnEffect(VFXModule.GetExplosiveAmmoExplosionIndex(), new EffectData
                {
                    origin = hit.point,
                    scale = rad,
                    rotation = Util.QuaternionSafeLookRotation(hit.direction)
                }, true);
                var blast = new BlastAttack
                {
                    attacker = bullet.owner,
                    attackerFiltering = AttackerFiltering.Default,
                    baseDamage = bullet.damage * 3.5f,
                    baseForce = 1f,
                    bonusForce = Vector3.zero,
                    crit = bullet.isCrit,
                    damageColorIndex = DamageColorIndex.Default,
                    damageType = bullet.damageType,
                    falloffModel = BlastAttack.FalloffModel.None,
                    impactEffect = EffectIndex.Invalid, // FUTURE: Explosive Ammo Impact Effect
                    inflictor = null,
                    losType = BlastAttack.LoSType.None,
                    position = hit.point,
                    procChainMask = bullet.procChainMask,
                    procCoefficient = bullet.procCoefficient * 0.5f,
                    radius = rad,
                    teamIndex = TeamComponent.GetObjectTeam(bullet.owner),
                };
                _ = blast.Fire();
            };

            public override void InitBullet<T>(ExpandableBulletAttack<NoData> bullet, SnipeState<T> state)
            {
                base.InitBullet(bullet, state);
                bullet.damage *= state.reloadBoost;
                bullet.damage *= (1f + state.chargeBoost * 0.8f);
            }
        }
    
}
