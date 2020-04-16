using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BepInEx.Logging;
using ReinCore;
using RoR2;
using Sniper.Enums;
using UnityEngine;

namespace Sniper.Data
{
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
        internal Single attackSpeedDecayCoef;
        [SerializeField]
        internal Single attackSpeedCap;

        private Single AdjustAttackSpeed( Single rawAttackSpeed )
        {
            if( rawAttackSpeed > 1f )
            {
                return 1f + (this.attackSpeedCap * (1f - (this.attackSpeedCap / (rawAttackSpeed + (this.attackSpeedCap - 1f)))));
            }
            return rawAttackSpeed;
        }

        public override Int32 GetHashCode()
        {
            var hash = this.goodStart.GetHashCode();
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
            if( timer >= this.perfectStart && timer <= this.perfectEnd )
            {
                return ReloadTier.Perfect;
            }
            if( timer >= this.goodStart && timer <= this.goodEnd )
            {
                return ReloadTier.Good;
            }
            return ReloadTier.Bad;
        }
    }
}
