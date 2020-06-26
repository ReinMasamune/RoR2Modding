namespace Sniper.Data
{
    using System;

    using Sniper.Enums;
    using Sniper.Expansions;

    using UnityEngine;

    [Serializable]
    internal struct ReloadParams
    {
        [SerializeField]
        internal Single baseDuration;
        [SerializeField]
        internal Single goodStart;
        [SerializeField]
        internal Single goodEnd;
        [SerializeField]
        internal Single perfectStart;
        [SerializeField]
        internal Single perfectEnd;

        [SerializeField]
        internal Single badMult;
        [SerializeField]
        internal Single goodMult;
        [SerializeField]
        internal Single perfectMult;

        [SerializeField]
        internal Single reloadDelay;
        [SerializeField]
        internal Single reloadEndDelay;
        [SerializeField]
        internal Single attackSpeedDecayCoef;
        [SerializeField]
        internal Single attackSpeedCap;

        private Single AdjustAttackSpeed( Single rawAttackSpeed )
        {
            return rawAttackSpeed > 1f
                ? 1f + ( this.attackSpeedCap * ( 1f - ( this.attackSpeedCap / ( rawAttackSpeed + ( this.attackSpeedCap - 1f ) ) ) ) )
                : rawAttackSpeed;
        }

        public override Int32 GetHashCode()
        {
            Int32 hash = this.goodStart.GetHashCode();
            unchecked
            {
                hash += this.goodEnd.GetHashCode();
                hash += this.perfectStart.GetHashCode();
                hash += this.perfectEnd.GetHashCode();
            }
            return hash;
        }

        internal Single Update( Single delta, Single attackSpeed, Single timer )
        {
            delta = this.AdjustAttackSpeed( attackSpeed ) * delta;
            timer += delta;
            timer %= this.baseDuration;
            return timer;
        }

        internal ReloadTier GetReloadTier( Single timer )
        {
            timer /= this.baseDuration;
            return timer >= this.perfectStart && timer <= this.perfectEnd
                ? ReloadTier.Perfect
                : timer >= this.goodStart && timer <= this.goodEnd ? ReloadTier.Good : ReloadTier.Bad;
        }

        internal void ModifyBullet( ExpandableBulletAttack bullet, ReloadTier tier )
        {
            switch( tier )
            {
                default:
#if ASSERT
                Log.Error( "Unknown reload tier" );
#endif
                bullet.damage *= this.badMult;
                break;
                case ReloadTier.Bad:
                bullet.damage *= this.badMult;
                break;
                case ReloadTier.Good:
                bullet.damage *= this.goodMult;
                break;
                case ReloadTier.Perfect:
                bullet.damage *= this.perfectMult;
                break;
            }
        }
    }
}
