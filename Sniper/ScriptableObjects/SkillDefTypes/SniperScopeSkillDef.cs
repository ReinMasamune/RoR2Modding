namespace Sniper.SkillDefs
{
    using System;

    using EntityStates;

    using ReinCore;

    using RoR2;

    using Sniper.Components;
    using Sniper.Data;
    using Sniper.SkillDefTypes.Bases;
    using Sniper.States.Bases;

    using UnityEngine;

    internal class SniperScopeSkillDef : SniperSkillDef
    {
        internal static SniperScopeSkillDef Create<TSecondary>( GameObject scopeUIPrefab, ZoomParams zoomParams ) where TSecondary : ScopeBaseState
        {
            SniperScopeSkillDef def = ScriptableObject.CreateInstance<SniperScopeSkillDef>();
            def.scopeCrosshair = scopeUIPrefab;
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
        internal GameObject scopeCrosshair;
        [SerializeField]
        internal ZoomParams zoomParams;
        [SerializeField]
        internal Int32 stockToConsumeOnFire;
        [SerializeField]
        internal Int32 stockRequiredToModifyFire;
        [SerializeField]
        internal Int32 stockRequiredToKeepZoom;


        public sealed override BaseSkillInstanceData OnAssigned( GenericSkill skillSlot ) => new ScopeInstanceData( this, skillSlot );

        protected sealed override EntityState InstantiateNextState( GenericSkill skillSlot )
        {
            var data = skillSlot.skillInstanceData as ScopeInstanceData;
            var state = base.InstantiateNextState( skillSlot ) as ScopeBaseState;
            data.StateCreated( state );
            return state;
        }



        internal class ScopeInstanceData : BaseSkillInstanceData
        {
            internal ScopeInstanceData( SniperScopeSkillDef scopeSkill, GenericSkill skillSlot )
            {
                this.scopeSkill = scopeSkill;
                this.zoomParams = scopeSkill.zoomParams;
                this.skillSlot = skillSlot;
                this.defaultCrosshair = skillSlot.characterBody.crosshairPrefab;
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
                }
                return mod;
            }

            internal void Invalidate()
            {
                this.stateInstance = null;
                this.scopeUIController?.EndZoomSession();
                this.skillSlot.characterBody.crosshairPrefab = this.defaultCrosshair;
            }

            internal void UpdateCameraParams( Single zoomInput )
            {
                this.zoom = this.zoomParams.UpdateZoom( zoomInput, this.zoom );
                this.scopeUIController?.UpdateUI( this.zoom );
            }

            internal void StateCreated( ScopeBaseState stateInstance )
            {

                this.skillSlot.characterBody.crosshairPrefab = this.scopeSkill.scopeCrosshair;
                // TODO: Config for zoom resetting on scope start
                this.zoom = this.zoomParams.defaultZoom;
                stateInstance.instanceData = this;
                this.stateInstance = stateInstance;
            }

            internal void CrosshairCheckIn( ScopeUIController uiController )
            {
                this.scopeUIController = uiController;
                if( this.stateInstance != null )
                {
                    this.scopeUIController.StartZoomSession( this.stateInstance, this.zoomParams );
                }
            }

            private ScopeBaseState stateInstance;

            private ScopeUIController scopeUIController;
            private readonly SniperScopeSkillDef scopeSkill;
            private readonly GenericSkill skillSlot;
            private Single zoom;
            internal ZoomParams zoomParams { get; private set; }
            private readonly GameObject defaultCrosshair;
        }
    }
}
