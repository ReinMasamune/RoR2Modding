namespace Sniper.Data
{
    using System;

    using Sniper.Enums;
    using Sniper.Expansions;

    using UnityEngine;

    [Serializable]
    internal struct ReloadParams : IEquatable<ReloadParams>
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

        public override Boolean Equals( System.Object obj )
        {
            if( obj is ReloadParams other )
            {
                return this.goodEnd == other.goodEnd && this.perfectStart == other.perfectStart && this.perfectEnd == other.perfectEnd && this.goodStart == other.goodStart;
            } else return false;
        }

        public Boolean Equals( ReloadParams other )
        {
            return this.goodEnd == other.goodEnd && this.perfectStart == other.perfectStart && this.perfectEnd == other.perfectEnd && this.goodStart == other.goodStart;
        }

        public static Boolean operator ==( ReloadParams p1, ReloadParams p2 )
        {
            return p1.Equals( p2 );
        }
        public static Boolean operator !=( ReloadParams p1, ReloadParams p2 )
        {
            return !p1.Equals( p2 );
        }


        internal Single Update( Single delta, Single attackSpeed, Single timer, ref Single attackSpeedDelayed, ref Single attackSpeedSpeed, Single attackSpeedTime  )
        {
            if( attackSpeedDelayed != attackSpeed )
            {
                attackSpeed = Mathf.SmoothDamp( attackSpeedDelayed, attackSpeed, ref attackSpeedSpeed, attackSpeedTime );
            }
            attackSpeedDelayed = attackSpeed;
            delta *= this.AdjustAttackSpeed( attackSpeed );
            var temp = timer + delta;
            temp %= this.baseDuration;
            return temp;
        }

        internal ReloadTier GetReloadTier( Single timer )
        {
            timer /= this.baseDuration;
            return timer >= this.perfectStart && timer <= this.perfectEnd
                ? ReloadTier.Perfect
                : timer >= this.goodStart && timer <= this.goodEnd ? ReloadTier.Good : ReloadTier.Bad;
        }

        internal Single GetFlatDelay( Single timer, Single attackSpeed )
        {
            timer /= this.baseDuration;
            return timer < this.perfectStart ? ( this.goodStart - timer ) * this.baseDuration / this.AdjustAttackSpeed( attackSpeed ) : 0.0f;
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
