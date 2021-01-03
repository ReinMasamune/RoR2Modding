namespace Rein.Sniper.Ammo
{
    using System;

    using Rein.Sniper.Expansions;
    using Rein.Sniper.Modules;
    using Rein.Sniper.Orbs;
    using Rein.Sniper.States.Bases;

    using ReinCore;

    using RoR2;
    using RoR2.Orbs;

    using UnityEngine;

    internal class ShockContext : OnHitContextBase<ShockContext.ShockData>
    {
        private static readonly GameObject _tracer = VFXModule.GetShockAmmoTracer();
        private static readonly GameObject _hit = VFXModule.GetShockImpactPrefab();
        public override GameObject tracerEffectPrefab => _tracer;
        protected override Single baseDamageMultiplier => 0.2f;
        protected override Single procCoefficient => 1f;
        protected override Single bulletRadius => 0.1f;
        protected override GameObject? hitEffectPrefab => _hit;
        protected override OnBulletDelegate<ShockData> onHit => (bullet, hit) =>
        {
            var h = hit.hitHurtBox;
            if(!h) return;
            var body = h.healthComponent.body;
            if(!body) return;
            if(!FriendlyFireManager.ShouldSeekingProceed(h.healthComponent, bullet.team)) return;

            if(body.GetBuffCount(CatalogModule.shockAmmoDebuff) is Int32 count && count > 0)
            {
                new ShockOrb()
                {
                    origin = hit.point,
                    damage = bullet.damage,
                    crit = bullet.isCrit,
                    threshold = bullet.data.stackThreshold,
                    hitEnemies = new(),
                    attacker = bullet.attackerBody,
                    procCoef = bullet.procCoefficient,
                    target = hit.hitHurtBox,
                    speed = 200f,
                    stackCount = count,
                    backupTargetPos = hit.hitHurtBox.transform.position,
                }.Create();
            } else
            {
                body.ApplyBuff(CatalogModule.shockAmmoDebuff, bullet.data.stackThreshold);
            }
        };

        public override void InitBullet<T>(ExpandableBulletAttack<ShockData> bullet, SnipeState<T> state)
        {
            base.InitBullet(bullet, state);

            var baseDamageMultiplier = state.reloadBoost * 1f + (state.chargeBoost);
            bullet.damage *= baseDamageMultiplier;
            bullet.data.stackThreshold = 1 + Mathf.RoundToInt(state.chargeBoost / 0.33f);
        }

        internal struct ShockData
        {
            internal Int32 stackThreshold;
        }
    }

}
