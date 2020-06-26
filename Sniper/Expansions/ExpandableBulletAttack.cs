namespace Sniper.Expansions
{
    using System;

    using RoR2;

    internal delegate void OnBulletDelegate( ExpandableBulletAttack bullet, BulletAttack.BulletHit hitInfo );
    internal class ExpandableBulletAttack : BulletAttack, ICloneable
    {
        internal OnBulletDelegate onHit;
        internal OnBulletDelegate onStop;
        internal CharacterBody attackerBody;


        internal Single chargeLevel = 0f;

        internal ExpandableBulletData data = null;

        internal TeamIndex team
        {
            get => this.attackerBody.teamComponent.teamIndex;
        }

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

        public System.Object Clone()
        {
            var res = new ExpandableBulletAttack
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
            };

            res.onHit = this.onHit;

            return res;
        }

        internal abstract class ExpandableBulletData { }
    }
}
