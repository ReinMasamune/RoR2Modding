namespace Rein.Sniper.SkillDefs
{
    using System;
    using System.Collections;

    using EntityStates;

    using ReinCore;

    using RoR2;

    using Rein.Sniper.Components;
    using Rein.Sniper.Data;
    using Rein.Sniper.Modules;
    using Rein.Sniper.SkillDefTypes.Bases;
    using Rein.Sniper.States.Bases;
    using Rein.Sniper.UI.Components;

    using UnityEngine;

    internal class SniperScopeSkillDef : SniperSkillDef
    {

        internal static SniperScopeSkillDef Create<TSecondary>( ZoomParams zoomParams ) where TSecondary : ScopeBaseState
        {
            SniperScopeSkillDef def = ScriptableObject.CreateInstance<SniperScopeSkillDef>();
            def.zoomParams = zoomParams;
            def.activationState = SkillsCore.StateType<TSecondary>();
            def.activationStateMachineName = "Scope";
            def.canceledFromSprinting = true;
            def.noSprint = true;
            def.fullRestockOnAssign = true;
            def.interruptPriority = InterruptPriority.Skill;
            def.isCombatSkill = false;
            def.mustKeyPress = false;
            def.shootDelay = 0f;
            def.stockToConsume = 0;

            return def;
        }

        [SerializeField]
        internal ZoomParams zoomParams;
        [SerializeField]
        internal Int32 stockToConsumeOnFire;
        [SerializeField]
        internal Int32 stockRequiredToModifyFire;
        [SerializeField]
        internal Int32 stockRequiredToKeepZoom;
        [SerializeField]
        internal Boolean chargeCanCarryOver;
        [SerializeField]
        internal Single decayValue;
        [SerializeField]
        internal DecayType decayType;
        [SerializeField]
        internal Single initialCarryoverLoss;

        [SerializeField]
        internal Boolean consumeChargeOnFire;


        internal enum DecayType { Linear, Exponential }


        public sealed override BaseSkillInstanceData OnAssigned( GenericSkill skillSlot ) => new ScopeInstanceData( this, skillSlot );

        public sealed override EntityState InstantiateNextState( GenericSkill skillSlot )
        {
            var data = skillSlot.skillInstanceData as ScopeInstanceData;
            var state = base.InstantiateNextState( skillSlot ) as ScopeBaseState;
            data.StateCreated( state );
            return state;
        }

        public override void OnFixedUpdate( GenericSkill skillSlot )
        {
            base.OnFixedUpdate( skillSlot );
            if( skillSlot.skillInstanceData is ScopeInstanceData data )
            {
                data.UpdateData( Time.fixedDeltaTime );
            }
        }

        internal class ScopeInstanceData : BaseSkillInstanceData 
        {
            internal ScopeInstanceData( SniperScopeSkillDef scopeSkill, GenericSkill skillSlot )
            {
                this.scopeSkill = scopeSkill;
                this.zoomParams = scopeSkill.zoomParams;
                this.zoom = this.zoomParams.defaultZoom;
                this.skillSlot = skillSlot;
            }

            internal Boolean shouldModify
            {
                get => this.stateInstance != null && this.skillSlot.stock >= this.scopeSkill.stockRequiredToModifyFire;
            }

            internal BulletModifier currentBulletModifier
            {
                get => this.stateInstance.ReadModifier();
            }

            internal BulletModifier SendFired()
            {
                if( this.stateInstance.SendFired( out BulletModifier mod ) )
                {
                    this.skillSlot.DeductStock( this.scopeSkill.stockToConsumeOnFire );
                    if( this.skillSlot.stock < this.scopeSkill.stockRequiredToKeepZoom )
                    {
                        this.stateInstance.ForceScopeEnd();
                    }
                    if(this.scopeSkill.consumeChargeOnFire)
                    {
                        this.stateInstance.ResetCharge();
                    }
                }
                return mod;
            }

            internal void Invalidate( Single charge )
            {
                this.partialActive = false;
                this.scoped = false;
                this.stateInstance.cameraTarget.aimMode = CameraTargetParams.AimType.Standard;
                this.stateInstance.cameraTarget.fovOverride = -1f;
                this.stateInstance = null;

                if( this.scopeSkill.chargeCanCarryOver && this.skillSlot.stock > 0 )
                {
                    this.valCharge = charge * ( 1f - this.scopeSkill.initialCarryoverLoss );
                } else
                {
                    this.valCharge = 0.0f;
                }
            }

            internal void Update( Single zoomInput, Single chargeFrac, Single readyFrac, Boolean ready, Single range )
            {
                this.zoom = this.zoomParams.UpdateZoom( zoomInput * ConfigModule.zoomSpeed, this.zoom );
                var fov = this.zoomParams.GetFoV( this.zoom );
                this.stateInstance.cameraTarget.fovOverride = fov;
                this.stateInstance.shouldRunDelay = this.skillSlot.stock >= this.scopeSkill.stockRequiredToModifyFire;
                this.scoped = this.zoomParams.IsInScope( this.zoom );
                this.currentStock = this.skillSlot.stock;
                this.maxStock = this.skillSlot.maxStock;
                this.range = range;
                this.currentCharge = chargeFrac;
                this.readyFrac = readyFrac;
                this.ready = ready;
            }

            internal void StateCreated( ScopeBaseState stateInstance )
            {
                if(ConfigModule.zoomNoPersist) this.zoom = this.zoomParams.defaultZoom;
                stateInstance.instanceData = this;
            }

            internal void UpdateData( Single deltaTime )
            {
                if( this.valCharge > 0f )
                {
                    var value = this.scopeSkill.decayValue * deltaTime;
                    switch( this.scopeSkill.decayType )
                    {
                        case DecayType.Linear:
                        this.valCharge -= value;
                        break;
                        case DecayType.Exponential:
                        this.valCharge *= Mathf.Exp( -value );
                        break;
                        default:
#if ASSERT
                        Log.Warning( "Unknown decay type" );
#endif
                        break;
                    }
                } else
                {
                    this.valCharge = 0f;
                }
            }

            internal void ScopeStart( ScopeBaseState state )
            {
                this.stateInstance = state;
                this.stateInstance.startingCharge = this.valCharge;
                this.stateInstance.cameraTarget.aimMode = CameraTargetParams.AimType.AimThrow;
                this.partialActive = true;
                this.ready = false;
                this.readyFrac = 0.0f;
            }

            internal Boolean partialActive
            {
                private get => this._partialActive;
                set
                {
                    if( this._partialActive == value ) return;
                    this._partialActive = value;
                    if( !this.crosshair || this.crosshair is null ) return;
                    this.crosshair.partialScopeActive = value;
                }
            }
            private Boolean _partialActive = false;

            private Single _range = 0.0f;
            internal Single range
            {
                private get => this._range;
                set
                {
                    if( !this.crosshair || this.crosshair is null ) return;
                    this.crosshair.range = value;
                }
            }

            internal Boolean scoped
            {
                get => this._scoped;
                set
                {
                    if( this._scoped == value ) return;
                    if( this.stateInstance is null ) return;
                    this._scoped = value;
                    this.stateInstance.cameraTarget.aimMode = value ? CameraTargetParams.AimType.FirstPerson : CameraTargetParams.AimType.AimThrow;
                    if( !this.crosshair || this.crosshair is null ) return;
                    this.crosshair.fullScopeActive = value;

                }
            }
            private Boolean _scoped = false;

            internal Boolean ready
            {
                private get => this._ready;
                set
                {
                    if( this._ready == value ) return;
                    this._ready = value;
                    if( !this.crosshair || this.crosshair is null ) return;
                    this.crosshair.scopeReady = value;
                }
            }
            private Boolean _ready = false;

            internal Single readyFrac
            {
                private get => this._readyFrac;
                set
                {
                    if( this._readyFrac == value ) return;
                    this._readyFrac = value;
                    if( !this.crosshair || this.crosshair is null ) return;
                    this.crosshair.scopeReadyFrac = value;
                }
            }
            private Single _readyFrac = 0.0f;

            internal Single currentCharge
            {
                private get => this._currentCharge;
                set
                {
                    if( this._currentCharge == value ) return;
                    this._currentCharge = value;
                    if( !this.crosshair || this.crosshair is null ) return;
                    this.crosshair.charge = value;
                }
            }
            private Single _currentCharge = 0.0f;

            internal Int32 currentStock
            {
                private get => this._currentStock;
                set
                {
                    if( this._currentStock == value ) return;
                    this._currentStock = value;
                    if( !this.crosshair || this.crosshair is null ) return;
                    this.crosshair.secondaryStock = value;
                }
            }
            private Int32 _currentStock;

            internal Int32 maxStock
            {
                private get => this._maxStock;
                set
                {
                    if( this._maxStock == value ) return;
                    this._maxStock = value;
                    if( !this.crosshair || this.crosshair is null ) return;
                    this.crosshair.maxSecondaryStock = value;
                }
            }
            private Int32 _maxStock;

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
                if( this.crosshair is null ) return;
                this.crosshair.partialScopeActive = this.partialActive;
                this.crosshair.range = 0f;
                this.crosshair.scopeReady = this.ready;
                this.crosshair.scopeReadyFrac = this.readyFrac;
                this.crosshair.secondaryStock = this.currentStock;
                this.crosshair.maxSecondaryStock = this.maxStock;
                this.crosshair.charge = this.currentCharge;
                this.crosshair.fullScopeActive = this.scoped;
            }

            private ScopeBaseState stateInstance;

            private readonly SniperScopeSkillDef scopeSkill;
            private readonly GenericSkill skillSlot;
            private Single zoom;
            internal ZoomParams zoomParams { get; private set; }
            internal Single valCharge = 0.0f;
        }
    }
}
