namespace Rein.Sniper.Expansions
{
    using System;

    using RoR2;

    internal delegate void OnBulletDelegate<TData>( ExpandableBulletAttack<TData> bullet, BulletAttack.BulletHit hitInfo ) where TData : struct;

    internal abstract class ExpandableBulletAttack : BulletAttack
    {
        internal CharacterBody attackerBody;
        internal Single chargeLevel = 0f;
        internal Single reloadBoost;
        internal Single chargeBoost;

        internal TeamIndex team
        {
            get => this.attackerBody.teamComponent.teamIndex;
        }
    }
    internal class ExpandableBulletAttack<TData> : ExpandableBulletAttack
        where TData : struct
    {
        internal OnBulletDelegate<TData> onHit;
        internal OnBulletDelegate<TData> onStop;
        internal TData data;
        
        internal ExpandableBulletAttack() : base()
        {
            base.hitCallback = this.ExpandableHitCallback;
        }

        private Boolean ExpandableHitCallback( ref BulletHit hitInfo )
        {
            Boolean result = base.DefaultHitCallback( ref hitInfo );
            this.onHit?.Invoke( this, hitInfo );
            if( !result )
            {
                this.onStop?.Invoke( this, hitInfo );
            }
            return result;
        }

        public ExpandableBulletAttack<TData> Clone()
        {
            var res = new ExpandableBulletAttack<TData>
            {
                aimVector = this.aimVector,
                attackerBody = this.attackerBody,
                bulletCount = this.bulletCount,
                damage = this.damage,
                damageColorIndex = this.damageColorIndex,
                damageType = this.damageType,
                falloffModel = this.falloffModel,
                filterCallback = this.filterCallback,
                force = this.force,
                HitEffectNormal = this.HitEffectNormal,
                hitEffectPrefab = this.hitEffectPrefab,
                hitMask = this.hitMask,
                isCrit = this.isCrit,
                maxSpread = this.maxSpread,
                maxDistance = this.maxDistance,
                minSpread = this.minSpread,
                muzzleName = this.muzzleName,
                origin = this.origin,
                owner = this.owner,
                procChainMask = this.procChainMask,
                procCoefficient = this.procCoefficient,
                queryTriggerInteraction = this.queryTriggerInteraction,
                radius = this.radius,
                smartCollision = this.smartCollision,
                sniper = this.sniper,
                spreadPitchScale = this.spreadPitchScale,
                spreadYawScale = this.spreadYawScale,
                stopperMask = this.stopperMask,
                tracerEffectPrefab = this.tracerEffectPrefab,
                weapon = this.weapon,
                data = this.data,
            };

            res.onHit = this.onHit;
            res.onStop = this.onStop;

            return res;
        }
    }
}
