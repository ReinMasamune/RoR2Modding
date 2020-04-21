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
using Sniper.Expansions;
using Sniper.Data;
using Sniper.Components;

namespace Sniper.Skills
{
    internal class SniperScopeSkillDef : SniperSkillDef
    {
        internal static SniperScopeSkillDef Create<TSecondary>( GameObject scopeUIPrefab, ZoomParams zoomParams ) where TSecondary : ScopeBaseState
        {
            var def = ScriptableObject.CreateInstance<SniperScopeSkillDef>();
            def.scopeUIPrefab = scopeUIPrefab;
            def.zoomParams = zoomParams;
            def.activationState = SkillsCore.StateType<TSecondary>();
            def.activationStateMachineName = "Scope";
            def.canceledFromSprinting = true;
            def.fullRestockOnAssign = true;
            def.interruptPriority = InterruptPriority.Skill;
            def.isCombatSkill = false;
            def.mustKeyPress = false;
            def.shootDelay = 0f;
            def.stockToConsume = 0;

            return def;
        }

        [SerializeField]
        internal GameObject scopeUIPrefab;
        [SerializeField]
        internal ZoomParams zoomParams;
        [SerializeField]
        internal Int32 stockToConsumeOnFire;
        [SerializeField]
        internal Int32 stockRequiredToModifyFire;
        [SerializeField]
        internal Int32 stockRequiredToKeepZoom;


        public sealed override BaseSkillInstanceData OnAssigned( GenericSkill skillSlot )
        {
            return new ScopeInstanceData( this, skillSlot );
        }

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

                var attachedBody = skillSlot.characterBody;
                if( attachedBody.localPlayerAuthority )
                {
                    this.scopeUIController = ScopeUIController.Create( scopeSkill.scopeUIPrefab, attachedBody );
                }
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
                var mod = this.stateInstance.SendFired();
                this.skillSlot.DeductStock( this.scopeSkill.stockToConsumeOnFire );
                if( this.skillSlot.stock < this.scopeSkill.stockRequiredToKeepZoom )
                {
                    this.stateInstance.ForceScopeEnd();
                }
                return mod;
            }

            internal void Invalidate()
            {
                this.stateInstance = null;
            }

            internal void UpdateCameraParams( CharacterCameraParams cameraParams, Single zoom )
            {
                this.zoomParams.UpdateCameraParams( cameraParams, zoom );
                if( this.scopeUIController != null )
                {
                    this.scopeUIController.zoom = zoom;
                }
            }

            internal void StateCreated( ScopeBaseState stateInstance )
            {
                this.stateInstance = stateInstance;
                stateInstance.instanceData = this;
            }

            internal void SetScopeActive( Boolean active )
            {
                this.scopeUIController?.SetActivity( active );
            }

            private ScopeBaseState stateInstance;

            private ScopeUIController scopeUIController;
            private SniperScopeSkillDef scopeSkill;
            private GenericSkill skillSlot;
            internal ZoomParams zoomParams { get; private set; }
        }
    }
}
