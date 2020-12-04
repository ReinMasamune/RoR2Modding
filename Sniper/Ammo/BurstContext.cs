namespace Rein.Sniper.Ammo
{
    using System;

    using Rein.Sniper.Expansions;
    using Rein.Sniper.Modules;
    using Rein.Sniper.States.Bases;

    using UnityEngine;


        internal class BurstContext : StandardContextBase
        {
            private static readonly GameObject _tracer = VFXModule.GetBurstAmmoTracer();

            public override GameObject tracerEffectPrefab => _tracer;
            protected override Single durationMultiplier => base.durationMultiplier * 4f;
            protected override Single baseDamageMultiplier => 0.4f;
            protected override Single procCoefficient => 1f;
            protected override Single bulletRadius => 0.5f;
            protected override Single recoilMultiplier => base.recoilMultiplier * 0.5f * this._recoilMultiplier;
            protected override Single animPlayRate => this._animPlayRate;
            protected override SoundModule.FireType fireSoundType => SoundModule.FireType.Burst;
            protected override Boolean chargeIncreasesRecoil => false;



            private Int32 shotsToFire;
            private Single fireInterval;
            private Single _recoilMultiplier = 0.75f;
            private Single _animPlayRate;
            private Single timer = 0f;

            const Single firingFrac = 0.75f;
            

            public override void OnEnter<T>(SnipeState<T> state)
            {
                this.shotsToFire = (Int32)(3 * (1 + state.chargeBoost));
                this._animPlayRate = this.shotsToFire;
                //this._recoilMultiplier = base.recoilMultiplier * 0.;

                base.OnEnter(state);

                this.fireInterval = (this.duration * firingFrac) / (this.shotsToFire + 1);
            }

            public override void FixedUpdate<T>(SnipeState<T> state)
            {
                while(this.bulletsFired < this.shotsToFire && (state.fixedAge / this.fireInterval) >= this.bulletsFired)
                {
                    this._recoilMultiplier *= this._recoilMultiplier * this._recoilMultiplier;
                    this.FireBullet(state);
                }

                base.FixedUpdate(state);
            }

            public override void ModifyBullet<T>(ExpandableBulletAttack bullet, SnipeState<T> state, Ray aimRay)
            {
                base.ModifyBullet(bullet, state, aimRay);
                bullet.damage *= state.reloadBoost;
            }
        }
    
}
