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
using Sniper.Data;

namespace Sniper.Skills
{
    internal abstract class ScopeBaseState : SniperSkillBaseState
    {
        internal SniperScopeSkillDef.ScopeInstanceData instanceData;



        internal abstract Boolean usesCharge { get; }
        internal abstract Single currentCharge { get; }
        internal abstract Boolean usesStock { get; }
        internal abstract UInt32 currentStock { get; }

        internal abstract BulletModifier ReadModifier();
        internal abstract void OnFired();

        internal BulletModifier SendFired()
        {
            var mod = this.ReadModifier();
            this.OnFired();
            return mod;
        }

        internal void ForceScopeEnd()
        {
            if( base.isAuthority )
            {
                base.outer.SetNextStateToMain();
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if( base.isAuthority && ( !base.IsKeyDownAuthority() || base.characterBody.isSprinting ) )
            {
                base.outer.SetNextStateToMain();
            }
        }

        public override void Update()
        {
            base.Update();
        }

        public override void OnExit()
        {
            this.instanceData.Invalidate();
            base.OnExit();
        }
    }
}
