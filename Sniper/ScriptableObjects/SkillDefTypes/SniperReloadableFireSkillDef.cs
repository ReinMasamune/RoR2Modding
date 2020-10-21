namespace Rein.Sniper.SkillDefs
{
    using System;

    using EntityStates;

    using ReinCore;

    using RoR2;

    using Rein.Sniper.Components;
    using Rein.Sniper.Data;
    using Rein.Sniper.Enums;
    using Rein.Sniper.Modules;
    using Rein.Sniper.SkillDefTypes.Bases;
    using Rein.Sniper.States.Bases;
    using Rein.Sniper.UI.Components;

    using UnityEngine;

    internal class SniperReloadableFireSkillDef : SniperSkillDef
    {
        internal static SniperReloadableFireSkillDef Create<TPrimaryData, TReload>(String fireStateMachineName, String reloadStateMachineName)
            where TPrimaryData : struct, ISniperPrimaryDataProvider
            where TReload : EntityState, ISniperReloadState
        {
            SniperReloadableFireSkillDef def = ScriptableObject.CreateInstance<SniperReloadableFireSkillDef>();
            def.activationState = SkillsCore.StateType<SnipeState<TPrimaryData>>();
            def.activationStateMachineName = fireStateMachineName;
            def.baseRechargeInterval = 1f;
            def.beginSkillCooldownOnSkillEnd = true;
            def.canceledFromSprinting = false;
            def.isCombatSkill = true;
            def.fullRestockOnAssign = false;
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

        public sealed override BaseSkillInstanceData OnAssigned(GenericSkill skillSlot)
        {
            EntityStateMachine reloadTargetStatemachine = null;

            EntityStateMachine[] stateMachines = skillSlot.GetComponents<EntityStateMachine>();
            for(Int32 i = 0; i < stateMachines.Length; ++i)
            {
                EntityStateMachine mach = stateMachines[i];
                if(mach.customName == this.reloadStateMachineName)
                {
                    reloadTargetStatemachine = mach;
                }
            }

#if ASSERT
            if( reloadTargetStatemachine == null )
            {
                Log.Error( "No state machine found for reload" );
            }
#endif

            skillSlot.stock = this.actualMaxStock;

            return new SniperPrimaryInstanceData(this, reloadTargetStatemachine, this.reloadParams, skillSlot);
        }

        public sealed override Sprite GetCurrentIcon(GenericSkill skillSlot)
        {
            var data = skillSlot.skillInstanceData as SniperPrimaryInstanceData;
            return data.isReloading ? this.reloadIcon : base.icon;
        }

        public sealed override Boolean CanExecute(GenericSkill skillSlot)
        {
            var data = skillSlot.skillInstanceData as SniperPrimaryInstanceData;
            EntityStateMachine mach = data.isReloading ? data.reloadStatemachine : skillSlot.stateMachine;
            return this.IsReady(skillSlot) &&
                mach &&
                !mach.HasPendingState() &&
                mach.CanInterruptState(data.isReloading ? this.reloadInterruptPriority : base.interruptPriority);
        }

        public sealed override Boolean IsReady(GenericSkill skillSlot)
        {
            var data = skillSlot.skillInstanceData as SniperPrimaryInstanceData;
            return !data.NeedsReload() && (data.isReloading ? data.CanReload() : data.CanShoot());
        }

        public sealed override EntityState InstantiateNextState(GenericSkill skillSlot)
        {
            var data = skillSlot.skillInstanceData as SniperPrimaryInstanceData;
            var state = EntityState.Instantiate(data.isReloading ? this.reloadActivationState : base.activationState);
            if(state is BaseSkillState skillState)
            {
                skillState.activatorSkillSlot = skillSlot;
            }
            if(state is ISniperReloadState reloadState)
            {
                reloadState.reloadTier = data.ReadReload();
            }
            return state;
        }

        public sealed override void OnExecute(GenericSkill skillSlot)
        {
            var data = skillSlot.skillInstanceData as SniperPrimaryInstanceData;
            EntityState state = this.InstantiateNextState(skillSlot);
            if(data.isReloading)
            {
                //var reloadState = state as ISniperReloadState;
            } else
            {
                var fireState = state as ISnipeState;
                fireState.reloadBoost = this.reloadParams.GetBoost(data.currentReloadTier);
            }

            EntityStateMachine machine = data.isReloading ? data.reloadStatemachine : skillSlot.stateMachine;

            if(machine.SetInterruptState(state, data.isReloading ? this.reloadInterruptPriority : base.interruptPriority))
            {
                CharacterBody body = skillSlot.characterBody;
                if(body)
                {
                    if(base.noSprint)
                    {
                        body.isSprinting = false;
                    }
                    body.OnSkillActivated(skillSlot);
                }
                if(data.isReloading)
                {
                    data.StopReload();
                } else
                {
                    skillSlot.stock -= base.stockToConsume;
                    data.OnStockChanged();
                    data.delayTimer = 0f;
                    if(skillSlot.stock <= 0)
                    {
                        data.SetNeedsReload();
                    }
                }
            }
        }

        public sealed override void OnFixedUpdate(GenericSkill skillSlot)
        {
            var data = skillSlot.skillInstanceData as SniperPrimaryInstanceData;
            if(data._autoReloadPerfect)
            {
                if(data.ReadReload() == ReloadTier.Perfect) data.ForceReload(ReloadTier.Perfect);
            }
            if(data.NeedsReload())
            {
                var curState = skillSlot?.stateMachine?.state?.GetType();
                if(curState is null || curState != this.activationState.stateType)
                {
                    data.StartReload();
                }
                
            }
            data.delayTimer += Time.fixedDeltaTime * skillSlot.characterBody.attackSpeed;
        }


        internal class SniperPrimaryInstanceData : BaseSkillInstanceData
        {
            internal static event Action<ReloadTier> onReload;


            internal SniperPrimaryInstanceData(
                SniperReloadableFireSkillDef def,
                EntityStateMachine reloadTargetStatemachine,
                ReloadParams reloadParams,
                GenericSkill skillSlot
            )
            {
                this.def = def;
                this.reloadStatemachine = reloadTargetStatemachine;
                this.reloadParams = reloadParams;
                _ = ReloadUIController.GetReloadTexture(this.reloadParams);
                this.secondarySlot = this.reloadStatemachine.commonComponents.characterBody.skillLocator.secondary;
                this.isReloading = true;
                this.currentReloadTier = ReloadTier.Perfect;
                this.skillSlot = skillSlot;
                this.skillSlot.stock = this.def.stockToReload;
                this.body = this.skillSlot.characterBody as SniperCharacterBody;
                this.body.CheckIn(this);
            }

            internal void ForceReload(ReloadTier tier)
            {
                this._needsToStartReload = false;
                this._autoReloadPerfect = false;
                this.currentReloadTier = tier;
                SoundModule.PlayLoad(this.secondarySlot.gameObject, tier);
                this.isReloading = false;
                this.skillSlot.stock = Mathf.Max(this.skillSlot.stock, Mathf.Min(this.skillSlot.stock + this.def.stockToReload, this.def.actualMaxStock));
                this.body.ForceStopReload();
                this.UpdateCrosshair();
            }

            internal void SetNeedsReload()
            {
                this._needsToStartReload = true;
            }

            private Boolean _needsToStartReload;
            internal Boolean NeedsReload()
            {
                return this._needsToStartReload;
            }

            internal Boolean _autoReloadPerfect = false;
            internal void StartReload(Boolean autoOnPerfect = false)
            {
                this._autoReloadPerfect = autoOnPerfect;
                this._needsToStartReload = false;
                this.isReloading = true;
                this.body.StartReload(this.reloadParams);
                this.UpdateCrosshair();
            }
            internal void StopReload()
            {

                this.body.StopReload(this);
                this.skillSlot.stock += this.def.stockToReload;
                this.UpdateCrosshair();

                onReload?.Invoke(this.body.ReadReload());
            }

            internal ReloadTier ReadReload()
            {

                this.currentReloadTier = this.body.ReadReload();
                this.UpdateCrosshair();
                return this.currentReloadTier;
            }

            internal void OnStockChanged()
            {
                this.UpdateCrosshair();
            }


            internal SniperCrosshairController crosshair
            {
                private get => this._crosshair;
                set
                {
                    this._crosshair = value;
                    this.UpdateCrosshair();
                }
            }
            private SniperCrosshairController _crosshair;
            private void UpdateCrosshair()
            {
                if(!this.crosshair || this.crosshair is null) return;
                this.crosshair.reloadMax = this.def.actualMaxStock;
                this.crosshair.reloadCurrent = this.skillSlot.stock;
                this.crosshair.reloadTier = this.currentReloadTier;
            }

            internal Boolean CanReload() => this.body.CanReload();

            internal Boolean CanShoot() => this.delayTimer >= this.def.shootDelay;

            internal SniperReloadableFireSkillDef def;
            internal EntityStateMachine reloadStatemachine;
            internal GenericSkill skillSlot;
            internal GenericSkill secondarySlot;
            internal SniperCharacterBody body;
            internal ReloadParams reloadParams;
            internal ReloadTier currentReloadTier;
            internal Boolean isReloading = true;
            internal Single delayTimer;
        }
    }
}
