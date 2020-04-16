using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BepInEx.Logging;
using ReinCore;
using RoR2;
using RoR2.Networking;
using UnityEngine;
using KinematicCharacterController;
using EntityStates;
using RoR2.Skills;
using System.Reflection;
using Sniper.Expansions;
using Sniper.Skills;
using Sniper.Enums;

namespace Sniper.Skills
{
    internal abstract class SnipeBaseState : SniperSkillBaseState
    {
        internal ReloadTier reloadTier { private get; set; }

        private Boolean bulletFired = false;

        protected abstract ExpandableBulletAttack InitBullet( Ray aimRay, ReloadTier reloadTier );

        private void FireBullet()
        {
            if( this.bulletFired ) return;
            var aimRay = base.GetAimRay();
            var bullet = this.InitBullet( aimRay, this.reloadTier );
            base.characterBody.ammo.ModifyBullet( bullet );
            base.characterBody.passive.ModifyBullet( bullet );
            // TODO: Modifier from secondary


            bullet.Fire();
            this.bulletFired = true;
        }
    }
}
