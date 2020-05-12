namespace AlternateArtificer.SelectablePassive
{
    using System;
    using System.Collections.Generic;

    using JetBrains.Annotations;

    using RoR2;
    using RoR2.Skills;

    public class PassiveSkillDef : SkillDef
    {
        public struct StateMachineDefaults
        {
            public String machineName;
            public EntityStates.SerializableEntityStateType initalState;
            public EntityStates.SerializableEntityStateType mainState;
            public EntityStates.SerializableEntityStateType defaultInitalState;
            public EntityStates.SerializableEntityStateType defaultMainState;
        }

        public StateMachineDefaults[] stateMachineDefaults;

        public override BaseSkillInstanceData OnAssigned( [NotNull] GenericSkill skillSlot )
        {
            this.applyVisuals?.Invoke( skillSlot.characterBody.modelLocator.modelTransform.GetComponent<CharacterModel>() );

            EntityStateMachine[] stateMachines = skillSlot.GetComponents<EntityStateMachine>();
            foreach( StateMachineDefaults def in this.stateMachineDefaults )
            {
                foreach( EntityStateMachine mach in stateMachines )
                {
                    if( mach.customName == def.machineName )
                    {
                        mach.initialStateType = def.initalState;
                        mach.mainStateType = def.mainState;

                        if( mach.state.GetType() == def.defaultMainState.stateType )
                        {
                            //mach.SetNextState( EntityStates.EntityState.Instantiate( def.mainState ) );
                        }

                        break;
                    }
                }
            }

            return base.OnAssigned( skillSlot );
        }

        public override void OnUnassigned( [NotNull] GenericSkill skillSlot )
        {
            if( skillSlot && skillSlot.characterBody && skillSlot.characterBody.modelLocator && skillSlot.characterBody.modelLocator.modelTransform )
            {
                this.applyVisuals?.Invoke( skillSlot.characterBody.modelLocator.modelTransform.GetComponent<CharacterModel>() );
            }

            EntityStateMachine[] stateMachines = skillSlot.GetComponents<EntityStateMachine>();
            foreach( StateMachineDefaults def in this.stateMachineDefaults )
            {
                foreach( EntityStateMachine mach in stateMachines )
                {
                    if( mach.customName == def.machineName )
                    {
                        mach.initialStateType = def.defaultInitalState;
                        mach.mainStateType = def.defaultMainState;

                        if( mach.state.GetType() == def.mainState.stateType )
                        {
                            //mach.SetNextState( EntityStates.EntityState.Instantiate( def.defaultMainState ) );
                        }

                        break;
                    }
                }
            }

            base.OnUnassigned( skillSlot );
        }


        public Action<CharacterModel> applyVisuals;
        public Action<CharacterModel> removeVisuals;
        private readonly Dictionary<CharacterModel, Boolean> isDisplayed = new Dictionary<CharacterModel, Boolean>();

        public void OnAssignDisplay( CharacterModel model )
        {
            if( !this.isDisplayed.ContainsKey( model ) || !this.isDisplayed[model] )
            {
                this.isDisplayed[model] = true;
                this.applyVisuals?.Invoke( model );
            }
        }

        public void OnUnassignDisplay( CharacterModel model )
        {
            if( this.isDisplayed.ContainsKey( model ) && this.isDisplayed[model] )
            {
                this.isDisplayed[model] = false;
                this.removeVisuals?.Invoke( model );
            }
        }




    }
}
