namespace AlternateArtificer.States.Main
{
    using EntityStates;
    using RoR2;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;
    using UnityEngine;

    public class AltArtiMain : GenericCharacterMain
    {
        public Components.PassiveController passive;
        private Action base_base_FixedUpdate;

        public override void OnEnter()
        {
            base.OnEnter();
            passive = base.GetComponent<Components.PassiveController>();

            var method = typeof(BaseCharacterMain).GetMethod("FixedUpdate");
            var funcPointer = method.MethodHandle.GetFunctionPointer();
            base_base_FixedUpdate = (Action)Activator.CreateInstance(typeof(Action), this, funcPointer );
        }

        public override void FixedUpdate()
        {
            base_base_FixedUpdate();
            this.GatherInputs();
            this.HandleMovements();
            this.PerformInputs();
        }

        private new void PerformInputs()
        {
            if( base.isAuthority )
            {
                if( base.hasSkillLocator )
                {
                    DoSkill( base.skillLocator.primary, ref base.skill1InputReceived, base.inputBank.skill1.justPressed );
                    DoSkill( base.skillLocator.secondary, ref base.skill2InputReceived, base.inputBank.skill2.justPressed );
                    DoSkill( base.skillLocator.utility, ref base.skill3InputReceived, base.inputBank.skill3.justPressed );
                    DoSkill( base.skillLocator.special, ref base.skill4InputReceived, base.inputBank.skill4.justPressed );
                }
                base.jumpInputReceived = false;
                base.sprintInputReceived = false;
            }
        }

        private void DoSkill(GenericSkill slot, ref Boolean inputRecieved, Boolean justPressed )
        {
            Boolean temp = inputRecieved;
            inputRecieved = false;
            if( !slot ) return;

            if( ( justPressed || ( temp && !slot.mustKeyPress )) && base.CanExecuteSkill( slot ) )
            {
                if( slot.ExecuteIfReady() )
                {
                    passive.SkillCast( slot );
                }
            }
        }
    }
}
