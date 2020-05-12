namespace ReinGeneralFixes
{
    using BepInEx;
    using RoR2;
    using UnityEngine;
    using System.Collections.Generic;
    using RoR2.Navigation;
    using Mono.Cecil.Cil;
    using MonoMod.Cil;
    using System;
    using System.Reflection;
    using EntityStates;
    using RoR2.Skills;
    using ReinCore;

    internal partial class Main
    {
        partial void QoLCommandoRoll()
        {
            this.Enable += this.AddCommandoRollEdits;
            this.Disable += this.RemoveCommandoRollEdits;
            this.Load += this.AdjustRollSettings;
        }

        private void AdjustRollSettings()
        {
            SkillDef def = Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponent<SkillLocator>().utility.skillFamily.variants[0].skillDef;
            def.noSprint = false;
            
        }

        private void RemoveCommandoRollEdits() => HooksCore.EntityStates.Commando.DodgeState.OnEnter.On -= OnEnter_On;

        private void AddCommandoRollEdits() => HooksCore.EntityStates.Commando.DodgeState.OnEnter.On += OnEnter_On;

        private static void OnEnter_On( HooksCore.EntityStates.Commando.DodgeState.OnEnter.Orig orig, EntityStates.Commando.DodgeState self )
        {
            self.outer.commonComponents.characterBody.isSprinting = true;
            self.finalSpeedCoefficient *= 0.8f;
            self.initialSpeedCoefficient *= 0.8f;
            orig( self );
        }
    }
}
/*
Changelist:
Commando Phase round 2s cooldown, phase blast 3s cooldown



















*/