namespace Sniper.States.Secondary
{
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
    using Sniper.Enums;
    using Sniper.Data;
    using Sniper.States.Bases;

    internal class QuickScope : ScopeBaseState
    {
        const Single baseStartDelay = 0.5f;
        const Single damageMultiplier = 2f;


        internal override Boolean usesCharge { get; } = false;
        internal override Boolean usesStock { get; } = true;
        internal override Single currentCharge { get; }
        internal override UInt32 currentStock { get; }

        private Single startDelay;

        public override void OnEnter()
        {
            base.OnEnter();

            this.startDelay = baseStartDelay / base.attackSpeedStat;
        }

        internal override Boolean OnFired() => base.fixedAge >= this.startDelay;
        internal override BulletModifier ReadModifier()
        {
            var mod = new BulletModifier
            {
                damageMultiplier = damageMultiplier,
                charge = 0.5f,
                
            };
            
            return base.fixedAge >= this.startDelay ? mod : default;
        }
    }
}
