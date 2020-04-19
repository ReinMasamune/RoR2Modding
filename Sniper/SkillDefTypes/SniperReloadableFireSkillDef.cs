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
using Sniper.Components;

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
            def.baseRechargeInterval = 1f;
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
        internal Int32 stockToReload;
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

            return new SniperPrimaryInstanceData( reloadTargetStatemachine, this.reloadParams );
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
                return data.CanReload();
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
                    data.StopReload();
                    skillSlot.stock += this.stockToReload;
                } else
                {
                    skillSlot.stock -= base.stockToConsume;
                    if( skillSlot.stock <= 0 )
                    {
                        data.StartReload();
                    }
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

        public sealed override void OnFixedUpdate( GenericSkill skillSlot )
        {

        }


        internal class SniperPrimaryInstanceData : BaseSkillInstanceData
        {
            internal SniperPrimaryInstanceData(EntityStateMachine reloadTargetStatemachine, ReloadParams reloadParams )
            {
                this.reloadStatemachine = reloadTargetStatemachine;
                this.reloadParams = reloadParams;
                ReloadUIController.GetReloadTexture( this.reloadParams );
                //this.reloadController = ReloadUIController.FindController( this.reloadStatemachine.commonComponents.characterBody );
            }

            internal void StartReload()
            {
                if( this.reloadController == null )
                {
                    Log.Warning( "No ReloadController" );
                    this.reloadController = ReloadUIController.FindController( this.reloadStatemachine.commonComponents.characterBody );
                    if( this.reloadController == null )
                    {
                        Log.Error( "Unable to find reload controller" );
                    }
                }
                this.isReloading = true;
                this.reloadController.StartReload( this.reloadParams );

            }
            internal void StopReload()
            {
                this.currentReloadTier = this.reloadController.StopReload( this );
            }

            internal Boolean CanReload()
            {
                if( !this.reloadController ) return false;
                return this.reloadController.CanReload();
            }

            internal EntityStateMachine reloadStatemachine;
            internal ReloadUIController reloadController;
            internal ReloadParams reloadParams;
            internal ReloadTier currentReloadTier = ReloadTier.None;
            internal Boolean isReloading = false;
        }
    }
}
