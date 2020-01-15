using EntityStates;
using JetBrains.Annotations;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ReinSniperRework
{
    internal partial class Main
    {
        internal class ReactivatableSkillDef : SkillDef
        {
            internal Single reactivationWindow;
            internal Single reactivationDelay;
            internal Int32 reactivationStockToConsume;
            internal Int32 reactivationRequiredStock;


            internal Boolean reactivationRestartsCooldown;
            internal Boolean reactivationNoSprint;

            internal SerializableEntityStateType reactivationState;
            internal InterruptPriority reactivationInterruptPriority;

            internal Sprite reactivationIcon;

            internal class InstanceData : SkillDef.BaseSkillInstanceData
            {
                internal Boolean waitingForReactivation;
                internal Single reactivationWindowTimer;
                internal Single reactivationDelayTimer;
            }

            public override SkillDef.BaseSkillInstanceData OnAssigned([NotNull]GenericSkill skillSlot )
            {
                return new InstanceData
                {
                    waitingForReactivation = false,
                    reactivationWindowTimer = 0f,
                    reactivationDelayTimer = 0f
                };
            }

            public override Boolean IsReady( [NotNull] GenericSkill skillSlot )
            {
                var data = skillSlot.skillInstanceData as InstanceData;
                if( data.waitingForReactivation )
                {
                    return data.reactivationDelayTimer <= 0f && skillSlot.stock >= this.reactivationRequiredStock;
                } else return base.IsReady( skillSlot );
            }

            public override Boolean CanExecute( [NotNull] GenericSkill skillSlot )
            {
                var data = skillSlot.skillInstanceData as InstanceData;
                if( data.waitingForReactivation )
                {
                    return data.reactivationDelayTimer <= 0f && skillSlot.stock >= this.reactivationRequiredStock && skillSlot.stateMachine && !skillSlot.stateMachine.HasPendingState() && skillSlot.stateMachine.CanInterruptState( this.reactivationInterruptPriority );
                } else return base.CanExecute( skillSlot );
            }

            public override void OnFixedUpdate( [NotNull] GenericSkill skillSlot )
            {
                base.OnFixedUpdate( skillSlot );
                var data = skillSlot.skillInstanceData as InstanceData;
                if( data.waitingForReactivation )
                {
                    data.reactivationDelayTimer -= Time.fixedDeltaTime;
                    data.reactivationWindowTimer -= Time.fixedDeltaTime;
                    if( data.reactivationWindowTimer <= 0f ) data.waitingForReactivation = false;
                }
            }

            public override void OnExecute( [NotNull] GenericSkill skillSlot )
            {
                var data = skillSlot.skillInstanceData as InstanceData;
                if( data.waitingForReactivation )
                {
                    skillSlot.stateMachine.SetInterruptState( this.InstantiateNextState( skillSlot ), this.reactivationInterruptPriority );
                    if( this.reactivationNoSprint )
                    {
                        skillSlot.characterBody.isSprinting = false;
                    }
                    skillSlot.stock -= this.reactivationStockToConsume;
                    if( this.reactivationRestartsCooldown )
                    {
                        skillSlot.rechargeStopwatch = 0f;
                    }
                    if( skillSlot.characterBody )
                    {
                        skillSlot.characterBody.OnSkillActivated( skillSlot );
                    }
                    data.waitingForReactivation = false;
                } else
                {
                    base.OnExecute( skillSlot );
                    data.waitingForReactivation = true;
                    data.reactivationDelayTimer = this.reactivationDelay;
                    data.reactivationWindowTimer = this.reactivationWindow;
                }
            }

            protected override EntityState InstantiateNextState( [NotNull] GenericSkill skillSlot )
            {
                var data = skillSlot.skillInstanceData as InstanceData;
                var entityState = EntityState.Instantiate( data.waitingForReactivation ? this.reactivationState : base.activationState );
                BaseSkillState baseSkillState;
                if( (baseSkillState = (entityState as BaseSkillState)) != null )
                {
                    baseSkillState.activatorSkillSlot = skillSlot;
                }
                return entityState;
            }

            public override Sprite GetCurrentIcon( [NotNull] GenericSkill skillSlot )
            {
                var data = skillSlot.skillInstanceData as InstanceData;
                if( data.waitingForReactivation )
                {
                    return this.reactivationIcon;
                } else
                {
                    return base.icon;
                }
            }
        }
    }
}