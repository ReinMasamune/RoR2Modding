namespace Sniper.SkillDefTypes.Bases
{
    using System;

    using EntityStates;

    using ReinCore;

    using RoR2;

    using Sniper.States.Bases;

    using UnityEngine;

    internal abstract class ReactivatedSkillDef<TSkillData> : SniperSkillDef
        where TSkillData : SkillData, new()
    {
        internal void InitStates<TActivation, TReactivation>( String activationMachine, String reactivationMachine )
            where TActivation : ActivationBaseState<TSkillData>
            where TReactivation : ReactivationBaseState<TSkillData>
        {
            this.activationState = SkillsCore.StateType<TActivation>();
            this.reactivationState = SkillsCore.StateType<TReactivation>();
            this.activationStateMachineName = activationMachine;
            this.reactivationStateMachineName = reactivationMachine;
            this.mustKeyPress = true;
            this.isBullets = false;
            this.canceledFromSprinting = false;
        }

        [SerializeField]
        internal SerializableEntityStateType reactivationState;
        [SerializeField]
        internal String reactivationStateMachineName;
        [SerializeField]
        internal Boolean startCooldownAfterReactivation;
        [SerializeField]
        internal Single minReactivationTimer;
        [SerializeField]
        internal Single maxReactivationTimer;
        [SerializeField]
        internal Sprite reactivationIcon;
        [SerializeField]
        internal InterruptPriority reactivationInterruptPriority;
        [SerializeField]
        internal Int32 reactivationRequiredStock;
        [SerializeField]
        internal Int32 reactivationStockToConsume;

        public sealed override BaseSkillInstanceData OnAssigned( GenericSkill skillSlot ) => new ReactivationInstanceData( this, skillSlot );
        public sealed override Sprite GetCurrentIcon( GenericSkill skillSlot )
        {
            //Log.Warning( "GetCurrentIcon" );
            var data = skillSlot.skillInstanceData as ReactivationInstanceData;
            return data.waitingOnReactivation ? this.reactivationIcon : this.icon;
        }
        public sealed override Boolean CanExecute( GenericSkill skillSlot )
        {
            var data = skillSlot.skillInstanceData as ReactivationInstanceData;

            return data.waitingOnReactivation ? data.CanReactivate() : base.CanExecute( skillSlot );
        }
        public sealed override Boolean IsReady( GenericSkill skillSlot )
        {
            var data = skillSlot.skillInstanceData as ReactivationInstanceData;
            return data.waitingOnReactivation ? data.IsReady() : base.IsReady( skillSlot );
        }
        protected sealed override EntityState InstantiateNextState( GenericSkill skillSlot )
        {

            var data = skillSlot.skillInstanceData as ReactivationInstanceData;
            if( data.waitingOnReactivation )
            {
                return data.InstantiateNextState();
            }
            var state = base.InstantiateNextState( skillSlot ) as ActivationBaseState<TSkillData>;
            data.OnActivation( state );
            return state;
        }
        public sealed override void OnExecute( GenericSkill skillSlot )
        {
            var data = skillSlot.skillInstanceData as ReactivationInstanceData;
            EntityState state = this.InstantiateNextState( skillSlot );
            EntityStateMachine mach = data.waitingOnReactivation? data.reactivationStateMachine : skillSlot.stateMachine ;
            if( mach.SetInterruptState( state, data.waitingOnReactivation ? this.reactivationInterruptPriority : this.interruptPriority ) )
            {
                skillSlot.stock -= data.waitingOnReactivation ? this.reactivationStockToConsume : this.stockToConsume;
                if( !data.waitingOnReactivation )
                {
                    data.OnExecution();
                }

                if( skillSlot.characterBody )
                {
                    skillSlot.characterBody.OnSkillActivated( skillSlot );
                }
            }
        }
        public sealed override void OnFixedUpdate( GenericSkill skillSlot )
        {
            var data = skillSlot.skillInstanceData as ReactivationInstanceData;
            Single dt = Time.fixedDeltaTime;

            if( data.waitingOnReactivation )
            {
                data.RunRecharge( dt );
                if( this.startCooldownAfterReactivation )
                {
                    return;
                }
            }
            skillSlot.RunRecharge( dt );
        }
        internal class ReactivationInstanceData : BaseSkillInstanceData
        {
            internal ReactivationInstanceData( ReactivatedSkillDef<TSkillData> def, GenericSkill skillSlot )
            {
                this.def = def;
                this.waitingOnReactivation = false;
                this.skillSlot = skillSlot;
                this.reactivationTimer = 0f;
                EntityStateMachine[] machs = skillSlot.GetComponents<EntityStateMachine>();
                for( Int32 i = 0; i < machs.Length; ++i )
                {
                    EntityStateMachine mach = machs[i];
                    if( mach.customName == def.reactivationStateMachineName )
                    {
                        this.reactivationStateMachine = mach;
                        break;
                    }
                }

                if( this.reactivationStateMachine == null )
                {
                    Log.Error( "No matching statemachine found" );
                }
            }

            internal Boolean CanReactivate()
            {
                return this.IsReady() &&
                    this.reactivationStateMachine != null &&
                    this.reactivationStateMachine &&
                    !this.reactivationStateMachine.HasPendingState() &&
                    this.reactivationStateMachine.CanInterruptState( this.def.reactivationInterruptPriority );
            }

            internal Boolean IsReady()
            {
                return this.skillSlot.stock >= this.def.reactivationRequiredStock &&
                    this.reactivationTimer >= this.def.minReactivationTimer;
            }

            internal EntityState InstantiateNextState()
            {
                var state = EntityState.Instantiate( this.def.reactivationState ) as ReactivationBaseState<TSkillData>;
                state.skillData = this.data;
                state.activatorSkillSlot = this.skillSlot;

                return state;
            }

            internal void OnActivation( ActivationBaseState<TSkillData> state ) => this.data = state.CreateSkillData();

            internal void OnExecution()
            {
                this.reactivationTimer = 0f;
                this.waitingOnReactivation = true;
            }

            internal void RunRecharge( Single dt )
            {
                this.reactivationTimer += dt;
                if( !this.data.IsDataValid() || ( this.def.maxReactivationTimer > 0f && this.reactivationTimer > this.def.maxReactivationTimer ) )
                {
                    this.InvalidateReactivation();
                }
            }

            internal void InvalidateReactivation()
            {
                if( this.waitingOnReactivation )
                {
                    this.waitingOnReactivation = false;
                    this.data = null;
                } else
                {
                    this.waitingOnReactivation = false;
                    this.data = null;
                }

            }

            internal Boolean waitingOnReactivation;
            internal Single reactivationTimer;
            private TSkillData data;
            internal EntityStateMachine reactivationStateMachine;
            private readonly ReactivatedSkillDef<TSkillData> def;
            private readonly GenericSkill skillSlot;
        }

    }
}
