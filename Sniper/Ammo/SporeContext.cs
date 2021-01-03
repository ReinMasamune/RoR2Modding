namespace Rein.Sniper.Ammo
{
    using System;

    using RoR2;
    using Rein.Sniper.Expansions;
    using Rein.Sniper.States.Bases;

    using UnityEngine;
    using Rein.Sniper.Modules;
    using Rein.Sniper.Orbs;
    using ReinCore;

    internal class SporeContext : OnHitContextBase<NoData>
    {
        internal const Single healPercentBase = 0.0015f;
        internal const Single zoneProcCoef = 0.2f;
        internal const Single zoneDamageMult = 0.1f;

        private const Single baseDuration = 10f;



        private static readonly GameObject _tracer = VFXModule.GetSporeAmmoTracer();
        public override GameObject tracerEffectPrefab => _tracer;
        protected override Single baseDamageMultiplier => 0.5f;
        protected override Single procCoefficient => 1f;
        protected override Single bulletRadius => 0.5f;
        protected override OnBulletDelegate<NoData> onHit => (bullet, hit) =>
        {
            new SporeOrb()
            {
                origin = hit.point,
                attacker = bullet.attackerBody,
                crit = bullet.isCrit,
                duration = baseDuration,
                power = bullet.baseStateMult * bullet.reloadBoost,
                boost = Mathf.Sqrt(1f + bullet.chargeBoost)
            }.Create();
        };

        public override void InitBullet<T>(ExpandableBulletAttack<NoData> bullet, SnipeState<T> state)
        {
            base.InitBullet(bullet, state);
            bullet.damage *= state.reloadBoost;
            bullet.damage *= (1f + Mathf.Sqrt(state.chargeBoost));
        }
    }

}
