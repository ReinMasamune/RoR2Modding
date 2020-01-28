using BepInEx;
using RoR2;
using UnityEngine;
using System.Collections.Generic;
using RoR2.Navigation;
using R2API;
using R2API.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Reflection;
using EntityStates;
using RoR2.Skills;

namespace ReinGeneralFixes
{
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
            var def = Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponent<SkillLocator>().utility.skillFamily.variants[0].skillDef;
            def.noSprint = false;
            
        }

        private void RemoveCommandoRollEdits()
        {
            On.EntityStates.Commando.DodgeState.OnEnter -= this.DodgeState_OnEnter1;
        }

        private void AddCommandoRollEdits()
        {
            On.EntityStates.Commando.DodgeState.OnEnter += this.DodgeState_OnEnter1;
        }

        private void DodgeState_OnEnter1( On.EntityStates.Commando.DodgeState.orig_OnEnter orig, EntityStates.Commando.DodgeState self )
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