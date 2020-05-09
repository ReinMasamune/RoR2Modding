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
using Sniper.SkillDefTypes.Bases;
using Sniper.States.Bases;

namespace Sniper.SkillDefs
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
            def.baseMaxStock = 0;
            return def;
        }

        [SerializeField]
        internal Int32 actualMaxStock;
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

            EntityStateMachine[] stateMachines = skillSlot.GetComponents<EntityStateMachine>();
            for( Int32 i = 0; i < stateMachines.Length; ++i )
            {
                EntityStateMachine mach = stateMachines[i];
                if( mach.customName == this.reloadStateMachineName )
                {
                    reloadTargetStatemachine = mach;
                }
            }

            if( reloadTargetStatemachine == null )
            {
                Log.Fatal( "No state machine found for reload" );
            }

            skillSlot.stock = this.actualMaxStock;

            return new SniperPrimaryInstanceData( this, reloadTargetStatemachine, this.reloadParams );
        }

        public sealed override Sprite GetCurrentIcon( GenericSkill skillSlot )
        {
            var data = skillSlot.skillInstanceData as SniperPrimaryInstanceData;
            return data.isReloading ? this.reloadIcon : base.icon;
        }

        public sealed override Boolean CanExecute( GenericSkill skillSlot )
        {
            var data = skillSlot.skillInstanceData as SniperPrimaryInstanceData;
            EntityStateMachine mach = data.isReloading ? data.reloadStatemachine : skillSlot.stateMachine;
            return this.IsReady( skillSlot ) && 
                mach && 
                !mach.HasPendingState() && 
                mach.CanInterruptState( data.isReloading ? this.reloadInterruptPriority : base.interruptPriority );
        }

        public sealed override Boolean IsReady( GenericSkill skillSlot )
        {
            var data = skillSlot.skillInstanceData as SniperPrimaryInstanceData;
            return data.isReloading ? data.CanReload() : data.CanShoot();
        }

        protected sealed override EntityState InstantiateNextState( GenericSkill skillSlot )
        {
            var data = skillSlot.skillInstanceData as SniperPrimaryInstanceData;
            var state = EntityState.Instantiate(data.isReloading ? this.reloadActivationState : base.activationState);
            if( state is BaseSkillState skillState )
            {
                skillState.activatorSkillSlot = skillSlot;
            }
            if( state is SnipeBaseState snipeState )
            {
                snipeState.reloadParams = this.reloadParams;
            }
            if( state is ISniperReloadState reloadState )
            {
                
                reloadState.reloadTier = data.ReadReload();
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
                    data.delayTimer = 0f;
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
            var data = skillSlot.skillInstanceData as SniperPrimaryInstanceData;
            data.delayTimer += Time.fixedDeltaTime * skillSlot.characterBody.attackSpeed;
        }


        internal class SniperPrimaryInstanceData : BaseSkillInstanceData
        {
            internal SniperPrimaryInstanceData(SniperReloadableFireSkillDef def, EntityStateMachine reloadTargetStatemachine, ReloadParams reloadParams )
            {
                this.def = def;
                this.reloadStatemachine = reloadTargetStatemachine;
                this.reloadParams = reloadParams;
                _ = ReloadUIController.GetReloadTexture( this.reloadParams );
                this.secondarySlot = this.reloadStatemachine.commonComponents.characterBody.skillLocator.secondary;
            }

            internal void StartReload()
            {
                if( this.reloadController == null )
                {
                    this.reloadController = ReloadUIController.FindController( this.reloadStatemachine.commonComponents.characterBody );
                }
                this.isReloading = true;
                this.reloadController.StartReload( this.reloadParams );

            }
            internal void StopReload()
            {
                this.reloadController.StopReload( this );
            }

            internal ReloadTier ReadReload()
            {
                this.currentReloadTier = this.reloadController.ReadReload();
                return this.currentReloadTier;
            }

            internal Boolean CanReload()
            {
                if( !this.reloadController ) return false;
                return this.reloadController.CanReload();
            }

            internal Boolean CanShoot()
            {
                return this.delayTimer >= this.def.shootDelay;
            }

            internal SniperReloadableFireSkillDef def;
            internal EntityStateMachine reloadStatemachine;
            internal ReloadUIController reloadController;
            internal GenericSkill secondarySlot;
            internal ReloadParams reloadParams;
            internal ReloadTier currentReloadTier = ReloadTier.None;
            internal Boolean isReloading = false;
            internal Single delayTimer;
        }
    }
}
