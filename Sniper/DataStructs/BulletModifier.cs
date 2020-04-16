using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BepInEx.Logging;
using ReinCore;
using RoR2;
using UnityEngine;

namespace Sniper.Data
{
    [Serializable]
    internal struct BulletModifier
    {
        internal static BulletModifier identity = new BulletModifier
        {
            countMultiplier = 1f,
            damageColor = DamageColorIndex.Default,
            damageMultiplier = 1f,
            damageTypeAdd = 0,
            damageTypeRemove = 0,
            forceMultiplier = 1f,
            hitMaskAdd = 0,
            hitMaskRemove = 0,
            maxSpread = 0f,
            minSpread = 0f,
            procMultiplier = 1f,
            radiusMultiplier = 1f,
            spreadPitch = 1f,
            spreadYaw = 1f,
            stopperMaskAdd = 0,
            stopperMaskRemove = 0
        };

        internal void Apply( BulletAttack bullet )
        {
            bullet.bulletCount = (UInt32)(bullet.bulletCount * this.countMultiplier);
            bullet.damage *= this.damageMultiplier;
            bullet.damageColorIndex = this.damageColor;
            bullet.damageType &= ~this.damageTypeRemove;
            bullet.damageType |= this.damageTypeAdd;
            bullet.force *= this.forceMultiplier;
            bullet.hitMask &= ~this.hitMaskRemove;
            bullet.hitMask |= this.hitMaskAdd;
            bullet.maxSpread *= this.maxSpread;
            bullet.minSpread *= this.minSpread;
            bullet.procCoefficient *= this.procMultiplier;
            bullet.radius = (bullet.radius == 0f) ? this.radiusMultiplier : bullet.radius * this.radiusMultiplier;
            bullet.spreadPitchScale = (bullet.spreadPitchScale == 0f) ? this.spreadPitch : bullet.spreadPitchScale * this.spreadPitch;
            bullet.spreadYawScale = (bullet.spreadYawScale == 0f) ? this.spreadYaw : bullet.spreadYawScale * this.spreadYaw;
            bullet.stopperMask &= ~this.stopperMaskRemove;
            bullet.stopperMask |= this.stopperMaskAdd;
        }
        [SerializeField]
        internal Single countMultiplier;
        [SerializeField]
        internal Single damageMultiplier;
        [SerializeField]
        internal DamageColorIndex damageColor;
        [SerializeField]
        internal DamageType damageTypeAdd;
        [SerializeField]
        internal DamageType damageTypeRemove;
        [SerializeField]
        internal Single forceMultiplier;
        [SerializeField]
        internal LayerMask hitMaskRemove;
        [SerializeField]
        internal LayerMask hitMaskAdd;
        [SerializeField]
        internal Single maxSpread;
        [SerializeField]
        internal Single minSpread;
        [SerializeField]
        internal Single procMultiplier;
        [SerializeField]
        internal Single radiusMultiplier;
        [SerializeField]
        internal Single spreadPitch;
        [SerializeField]
        internal Single spreadYaw;
        [SerializeField]
        internal LayerMask stopperMaskRemove;
        [SerializeField]
        internal LayerMask stopperMaskAdd;
    }
}
