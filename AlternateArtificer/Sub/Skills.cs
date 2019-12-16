namespace AlternateArtificer
{
    using RoR2;
    using RoR2.Skills;

    public partial class Main
    {

        private void EditSkills()
        {
            RegisterSkills();
            SetupStateMachines();

            TempModSpecial();
        }




        private void RegisterSkills()
        {
            Helpers.SkillsHelper.AddSkill( typeof( States.Main.AltArtiMain ) );
            Helpers.SkillsHelper.AddSkill( typeof( States.Special.IonSurge ) );
        }

        private void SetupStateMachines()
        {
            EntityStateMachine jetState = null;
            foreach( EntityStateMachine esm in artiBody.GetComponents<EntityStateMachine>() )
            {
                if( esm.customName == "Body" ) esm.mainStateType = new EntityStates.SerializableEntityStateType( typeof( States.Main.AltArtiMain ) );
                if( esm.customName == "Jet" ) jetState = esm; 
            }

            jetState.customName = "Passive";
        }






        private void TempModSpecial()
        {
            SkillFamily specialFamily = artiSkillLocator.special.skillFamily;

            var specialDef = specialFamily.variants[1].skillDef;
            specialDef.activationState = new EntityStates.SerializableEntityStateType( typeof( States.Special.IonSurge ) );
        }
    }
}
