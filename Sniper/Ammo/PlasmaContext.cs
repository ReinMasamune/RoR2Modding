namespace Rein.Sniper.Ammo
{
    using System;

    using RoR2;
    using Rein.Sniper.Expansions;
    using Rein.Sniper.States.Bases;

    using UnityEngine;
    using Rein.Sniper.DotTypes;
    using Rein.Sniper.Modules;

    internal class PlasmaContext : OnHitContextBase<NoData>
        {
        internal const Single plasmaTotalMult = 2f;
        internal const Single plasmaProcPerTick = 0.5f;
        internal const Single plasmaTotalDuration = 10f;
        internal const Single plasmaTickFreq = 2.5f;


        internal const Single plasmaTotalTicks = plasmaTotalDuration * plasmaTickFreq + 1f;
        internal const Single plasmaDamagePerTick = plasmaTotalMult / plasmaTotalTicks;

        private static readonly GameObject _tracer = VFXModule.GetPlasmaAmmoTracer();
            private const Single baseDotDuration = 10f;

            public override GameObject tracerEffectPrefab => _tracer;
            protected override Single baseDamageMultiplier => plasmaDamagePerTick;
            protected override Single procCoefficient => plasmaProcPerTick;
            protected override Single bulletRadius => 0.5f;
            protected override DamageColorIndex damageColor => CatalogModule.plasmaDamageColor;
            protected override SoundModule.FireType fireSoundType => SoundModule.FireType.Plasma;
            protected override OnBulletDelegate<NoData> onHit => (bullet, hit) =>
            {
                var box = hit.hitHurtBox;
                if(!box) return;
                var target = box.healthComponent?.body;
                if(!target) return;
                if(!FriendlyFireManager.ShouldDirectHitProceed(box.healthComponent, bullet.team)) return;

                PlasmaDot.Apply(target, bullet.attackerBody, bullet.damage / bullet.attackerBody.damage, plasmaTotalDuration * Mathf.Sqrt(bullet.chargeBoost + 1f), bullet.procCoefficient, bullet.isCrit, box.transform.InverseTransformPoint(hit.point), bullet.aimVector * -1f, box);
            };

            public override void InitBullet<T>(ExpandableBulletAttack<NoData> bullet, SnipeState<T> state)
            {
                base.InitBullet(bullet, state);
                bullet.damage *= state.reloadBoost;
                bullet.damage *= Mathf.Sqrt(1f + state.chargeBoost);
            }
        }
    
}
