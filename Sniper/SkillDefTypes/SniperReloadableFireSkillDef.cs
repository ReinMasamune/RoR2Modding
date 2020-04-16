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
using Sniper.Data;
using Sniper.Enums;

namespace Sniper.Skills
{
    internal class SniperReloadableFireSkillDef : SniperSkillDef
    {
        internal static SniperReloadableFireSkillDef Create<TFire,TReload>(String fireStateMachineName, String reloadStateMachineName)
            where TFire : SnipeBaseState
            where TReload : EntityState,ISniperReloadState
        {
            var def = ScriptableObject.CreateInstance<SniperReloadableFireSkillDef>();
            def.activationState = SkillsCore.StateType<TFire>();
            def.activationStateMachineName = fireStateMachineName;
            def.beginSkillCooldownOnSkillEnd = true;
            def.canceledFromSprinting = false;
            def.isCombatSkill = true;
            def.fullRestockOnAssign = true;
            def.mustKeyPress = true;
            def.noSprint = true;
            def.reloadActivationState = SkillsCore.StateType<TReload>();
            def.reloadStateMachineName = reloadStateMachineName;
            return def;
        }


        [SerializeField]
        internal SerializableEntityStateType reloadActivationState;
        [SerializeField]
        internal InterruptPriority reloadInterruptPriority;
        [SerializeField]
        internal String reloadStateMachineName;
        [SerializeField]
        internal Sprite reloadIcon;
        [SerializeField]
        internal ReloadParams reloadParams;
        
        internal void StartReload(SniperPrimaryInstanceData data)
        {
            data.isReloading = true;
            data.reloadTimer = 0f;
            data.delayTimer = 0f;

        }

        internal void StopReload(SniperPrimaryInstanceData data)
        {
            data.isReloading = false;
            data.currentReloadTier = this.reloadParams.GetReloadTier( data.reloadTimer );
        }

        public sealed override BaseSkillInstanceData OnAssigned( GenericSkill skillSlot )
        {
            EntityStateMachine reloadTargetStatemachine = null;

            var stateMachines = skillSlot.GetComponents<EntityStateMachine>();
            for( Int32 i = 0; i < stateMachines.Length; ++i )
            {
                var mach = stateMachines[i];
                if( mach.customName == this.reloadStateMachineName )
                {
                    reloadTargetStatemachine = mach;
                }
            }

            if( reloadTargetStatemachine == null )
            {
                Log.Fatal( "No state machine found for reload" );
            }

            return new SniperPrimaryInstanceData( reloadTargetStatemachine );
        }


        public sealed override void OnFixedUpdate( GenericSkill skillSlot )
        {
            base.OnFixedUpdate(skillSlot);
            var data = skillSlot.skillInstanceData as SniperPrimaryInstanceData;


            if( data.isReloading && data.delayTimer >= this.reloadParams.reloadDelay )
            {
                data.reloadTimer = this.reloadParams.Update( Time.fixedDeltaTime, skillSlot.characterBody.attackSpeed, data.reloadTimer );
            } else
            {
                data.delayTimer += Time.fixedDeltaTime;
            }
        }

        public sealed override Sprite GetCurrentIcon( GenericSkill skillSlot )
        {
            var data = skillSlot.skillInstanceData as SniperPrimaryInstanceData;
            if( data.isReloading )
            {
                return this.reloadIcon;
            }
            return base.icon;
        }

        public sealed override Boolean CanExecute( GenericSkill skillSlot )
        {
            var data = skillSlot.skillInstanceData as SniperPrimaryInstanceData;
            var mach = data.isReloading ? data.reloadStatemachine : skillSlot.stateMachine;
            return this.IsReady( skillSlot ) && 
                mach && 
                !mach.HasPendingState() && 
                mach.CanInterruptState( data.isReloading ? this.reloadInterruptPriority : base.interruptPriority );
        }

        public sealed override Boolean IsReady( GenericSkill skillSlot )
        {
            var data = skillSlot.skillInstanceData as SniperPrimaryInstanceData;
            if( data.isReloading )
            {
                return data.reloadTimer >= this.reloadParams.reloadDelay;
            }

            return true;
        }

        protected sealed override EntityState InstantiateNextState( GenericSkill skillSlot )
        {
            var data = skillSlot.skillInstanceData as SniperPrimaryInstanceData;
            var state = EntityState.Instantiate(data.isReloading ? this.reloadActivationState : base.activationState);
            var skillState = (state as BaseSkillState);
            if( skillState != null )
            {
                skillState.activatorSkillSlot = skillSlot;
            }
            return state;
        }

        public sealed override void OnExecute( GenericSkill skillSlot )
        {
            var data = skillSlot.skillInstanceData as SniperPrimaryInstanceData;
            var state = this.InstantiateNextState(skillSlot);
            if( data.isReloading )
            {
                var reloadState = state as ISniperReloadState;
            } else
            {
                var fireState = state as SnipeBaseState;
                fireState.reloadTier = data.currentReloadTier;
            }

            var machine = data.isReloading ? data.reloadStatemachine : skillSlot.stateMachine;

            if( machine.SetInterruptState( state, data.isReloading ? this.reloadInterruptPriority : base.interruptPriority ) )
            {
                if( data.isReloading )
                {
                    skillSlot.stock -= base.stockToConsume;
                    this.StopReload(data);
                } else
                {
                    this.StartReload(data);
                }
                var body = skillSlot.characterBody;
                if( body )
                {
                    if( base.noSprint )
                    {
                        body.isSprinting = false;
                    }
                    body.OnSkillActivated( skillSlot );
                }
            }
        }


        internal class SniperPrimaryInstanceData : BaseSkillInstanceData
        {
            internal SniperPrimaryInstanceData(EntityStateMachine reloadTargetStatemachine)
            {
                this.reloadStatemachine = reloadTargetStatemachine;
            }

            internal EntityStateMachine reloadStatemachine;
            internal ReloadTier currentReloadTier = ReloadTier.None;
            internal Boolean isReloading = true;
            internal Single reloadTimer = 0f;
            internal Single delayTimer = 0f;
        }
    }
}
