﻿namespace Rein.Sniper.Data
{
    using System;

    using RoR2;

    using Rein.Sniper.Expansions;

    using UnityEngine;

    [Serializable]
    internal struct BulletModifier
    {
        internal Single charge;

        internal void Apply( ExpandableBulletAttack bullet )
        {
            bullet.chargeLevel = this.charge < 0f ? this.charge : Mathf.Max( bullet.chargeLevel, this.charge );
            if( this.applyCountMultiplier )
            {
                bullet.bulletCount = (UInt32)( bullet.bulletCount * this.countMultiplier );
            }

            if( this.applyDamageMultiplier )
            {
                bullet.damage *= this.damageMultiplier;
            }

            if( this.applyDamageColor )
            {
                bullet.damageColorIndex = this.damageColor;
            }

            if( this.applyDamageTypeRemove )
            {
                bullet.damageType &= ~this.damageTypeRemove;
            }

            if( this.applyDamageTypeAdd )
            {
                bullet.damageType |= this.damageTypeAdd;
            }

            if( this.applyForceMultiplier )
            {
                bullet.force *= this.forceMultiplier;
            }

            if( this.applyHitMaskRemove )
            {
                bullet.hitMask &= ~this.hitMaskRemove;
            }

            if( this.applyHitMaskAdd )
            {
                bullet.hitMask |= this.hitMaskAdd;
            }

            if( this.applyMaxSpread )
            {
                bullet.maxSpread *= this.maxSpread;
            }

            if( this.applyMinSpread )
            {
                bullet.minSpread *= this.minSpread;
            }

            if( this.applyProcMultiplier )
            {
                bullet.procCoefficient *= this.procMultiplier;
            }

            if( this.applyRadiusMultiplier )
            {
                bullet.radius = ( bullet.radius == 0f ) ? this.radiusMultiplier : bullet.radius * this.radiusMultiplier;
            }

            if( this.applySpreadPitch )
            {
                bullet.spreadPitchScale = ( bullet.spreadPitchScale == 0f ) ? this.spreadPitch : bullet.spreadPitchScale * this.spreadPitch;
            }

            if( this.applySpreadYaw )
            {
                bullet.spreadYawScale = ( bullet.spreadYawScale == 0f ) ? this.spreadYaw : bullet.spreadYawScale * this.spreadYaw;
            }

            if( this.applyStopperMaskRemove )
            {
                bullet.stopperMask &= ~this.stopperMaskRemove;
            }

            if( this.applyStopperMaskAdd )
            {
                bullet.stopperMask |= this.stopperMaskAdd;
            }
        }


        internal Single countMultiplier
        {
            get => this._countMultiplier;
            set
            {
                this.applyCountMultiplier = true;
                this._countMultiplier = value;
            }
        }
        [SerializeField]

        private Single _countMultiplier;

        [SerializeField]
        private Boolean applyCountMultiplier;


        internal Single damageMultiplier
        {
            get => this._damageMultiplier;
            set
            {
                this.applyDamageMultiplier = true;
                this._damageMultiplier = value;
            }
        }
        [SerializeField]

        private Single _damageMultiplier;

        [SerializeField]
        private Boolean applyDamageMultiplier;


        internal DamageColorIndex damageColor
        {
            get => this._damageColor;
            set
            {
                this.applyDamageColor = true;
                this._damageColor = value;
            }
        }
        [SerializeField]

        private DamageColorIndex _damageColor;

        [SerializeField]
        private Boolean applyDamageColor;


        internal DamageType damageTypeAdd
        {
            get => this._damageTypeAdd;
            set
            {
                this.applyDamageTypeAdd = true;
                this._damageTypeAdd = value;
            }
        }
        [SerializeField]

        private DamageType _damageTypeAdd;

        [SerializeField]
        private Boolean applyDamageTypeAdd;


        internal DamageType damageTypeRemove
        {
            get => this._damageTypeRemove;
            set
            {
                this.applyDamageTypeRemove = true;
                this._damageTypeRemove = value;
            }
        }
        [SerializeField]

        private DamageType _damageTypeRemove;

        [SerializeField]
        private Boolean applyDamageTypeRemove;


        internal Single forceMultiplier
        {
            get => this._forceMultiplier;
            set
            {
                this.applyForceMultiplier = true;
                this._forceMultiplier = value;
            }
        }
        [SerializeField]

        private Single _forceMultiplier;

        [SerializeField]
        private Boolean applyForceMultiplier;


        internal LayerMask hitMaskRemove
        {
            get => this._hitMaskRemove;
            set
            {
                this.applyHitMaskRemove = true;
                this._hitMaskRemove = value;
            }
        }
        [SerializeField]

        private LayerMask _hitMaskRemove;

        [SerializeField]
        private Boolean applyHitMaskRemove;


        internal LayerMask hitMaskAdd
        {
            get => this._hitMaskAdd;
            set
            {
                this.applyHitMaskAdd = true;
                this._hitMaskAdd = value;
            }
        }
        [SerializeField]

        private LayerMask _hitMaskAdd;

        [SerializeField]
        private Boolean applyHitMaskAdd;


        internal Single maxSpread
        {
            get => this._maxSpread;
            set
            {
                this.applyMaxSpread = true;
                this._maxSpread = value;
            }
        }
        [SerializeField]

        private Single _maxSpread;

        [SerializeField]
        private Boolean applyMaxSpread;


        internal Single minSpread
        {
            get => this._minSpread;
            set
            {
                this.applyMinSpread = true;
                this._minSpread = value;
            }
        }
        [SerializeField]

        private Single _minSpread;

        [SerializeField]
        private Boolean applyMinSpread;


        internal Single procMultiplier
        {
            get => this._procMultiplier;
            set
            {
                this.applyProcMultiplier = true;
                this._procMultiplier = value;
            }
        }
        [SerializeField]

        private Single _procMultiplier;

        [SerializeField]
        private Boolean applyProcMultiplier;


        internal Single radiusMultiplier
        {
            get => this._radiusMultiplier;
            set
            {
                this.applyRadiusMultiplier = true;
                this._radiusMultiplier = value;
            }
        }
        [SerializeField]

        private Single _radiusMultiplier;

        [SerializeField]
        private Boolean applyRadiusMultiplier;


        internal Single spreadPitch
        {
            get => this._spreadPitch;
            set
            {
                this.applySpreadPitch = true;
                this._spreadPitch = value;
            }
        }
        [SerializeField]

        private Single _spreadPitch;

        [SerializeField]
        private Boolean applySpreadPitch;


        internal Single spreadYaw
        {
            get => this._spreadYaw;
            set
            {
                this.applySpreadYaw = true;
                this._spreadYaw = value;
            }
        }
        [SerializeField]

        private Single _spreadYaw;

        [SerializeField]
        private Boolean applySpreadYaw;


        internal LayerMask stopperMaskRemove
        {
            get => this._stopperMaskRemove;
            set
            {
                this.applyStopperMaskRemove = true;
                this._stopperMaskRemove = value;
            }
        }
        [SerializeField]

        private LayerMask _stopperMaskRemove;

        [SerializeField]
        private Boolean applyStopperMaskRemove;


        internal LayerMask stopperMaskAdd
        {
            get => this._stopperMaskAdd;
            set
            {
                this.applyStopperMaskAdd = true;
                this._stopperMaskAdd = value;
            }
        }
        [SerializeField]

        private LayerMask _stopperMaskAdd;

        [SerializeField]
        private Boolean applyStopperMaskAdd;
    }
}
