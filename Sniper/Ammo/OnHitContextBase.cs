namespace Rein.Sniper.Ammo
{
    using Rein.Sniper.Expansions;
    using Rein.Sniper.States.Bases;

    using UnityEngine;


        internal abstract class OnHitContextBase<TData> : StandardContextBase
            where TData : struct
        {
            public virtual TData InitData() => default;
            protected virtual OnBulletDelegate<TData> onHit { get => null; }
            protected virtual OnBulletDelegate<TData> onStop { get => null; }
            public sealed override ExpandableBulletAttack CreateBullet() => new ExpandableBulletAttack<TData>();
            public sealed override void ModifyBullet<T>(ExpandableBulletAttack bullet, SnipeState<T> state, Ray aim)
            {
                base.ModifyBullet(bullet, state, aim);
                var bul = bullet as ExpandableBulletAttack<TData>;
                bul.onHit = this.onHit;
                bul.onStop = this.onStop;
                bul.data = this.InitData();
                this.InitBullet(bul, state);
            }
            public virtual void InitBullet<T>(ExpandableBulletAttack<TData> bullet, SnipeState<T> state)
                where T : struct, ISniperPrimaryDataProvider
            {

            }
        }
    
}
