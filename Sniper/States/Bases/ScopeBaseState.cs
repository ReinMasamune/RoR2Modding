namespace Rein.Sniper.States.Bases
{
    using System;

    using EntityStates;

    using RoR2;

    using Rein.Sniper.Data;
    using Rein.Sniper.SkillDefs;

    using UnityEngine;

    internal abstract class ScopeBaseState : SniperSkillBaseState
    {
        internal SniperScopeSkillDef.ScopeInstanceData instanceData;

        internal abstract Single currentCharge { get; }

        internal abstract Boolean isReady { get; }
        internal abstract Single readyFrac { get; }

        internal abstract BulletModifier ReadModifier();
        internal abstract Boolean OnFired();

        internal Single startingCharge { get; set; }

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
            this.instanceData?.ScopeStart( this );
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            base.characterBody.isSprinting = false;

            if( base.isAuthority && ( !base.IsKeyDownAuthority() || base.characterBody.isSprinting ) )
            {
                base.outer.SetNextStateToMain();
            }
        }

        public override void Update()
        {
            base.Update();
            var dist = 10000f;
            if( base.inputBank.GetAimRaycast( dist, out var hit ) ) dist = hit.distance;
            this.instanceData?.Update( Input.mouseScrollDelta.y, this.currentCharge, this.readyFrac, this.isReady, dist );
        }

        public override void OnExit()
        {
            this.instanceData?.Invalidate( this.currentCharge );
            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority() => InterruptPriority.PrioritySkill;
    }
}
