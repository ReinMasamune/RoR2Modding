using System;
using EntityStates;
using RoR2;
using Sniper.Data;
using Sniper.SkillDefs;
using UnityEngine;

namespace Sniper.States.Bases
{
    internal abstract class ScopeBaseState : SniperSkillBaseState
    {
        internal SniperScopeSkillDef.ScopeInstanceData instanceData;

        internal abstract Boolean usesCharge { get; }
        internal abstract Single currentCharge { get; }
        internal abstract Boolean usesStock { get; }
        internal abstract UInt32 currentStock { get; }

        internal abstract BulletModifier ReadModifier();
        internal abstract Boolean OnFired();

        internal CameraTargetParams cameraTarget { get => base.cameraTargetParams; }

        internal Boolean SendFired( out BulletModifier mod )
        {
            BulletModifier temp = this.ReadModifier();
            if( this.OnFired() )
            {
                mod = temp;
                return true;
            } else
            {
                mod = default;
                return false;
            }
        }

        internal void ForceScopeEnd()
        {
            if( base.isAuthority )
            {
                base.outer.SetNextStateToMain();
            }

        }

        public override void OnEnter()
        {
            base.OnEnter();
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
            this.instanceData?.UpdateCameraParams( Input.mouseScrollDelta.y );
        }

        public override void OnExit()
        {
            base.OnExit();
            this.instanceData?.Invalidate();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
