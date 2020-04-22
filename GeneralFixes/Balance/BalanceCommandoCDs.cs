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

namespace ReinGeneralFixes
{
    internal partial class Main
    {
        partial void BalanceCommandoCDs() => this.FirstFrame += this.EditCommandoSecondaryCooldown;

        private void EditCommandoSecondaryCooldown()
        {
            SkillFamily family = Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponent<SkillLocator>().secondary.skillFamily;
            family.variants[0].skillDef.baseRechargeInterval = 2f;
            family.variants[1].skillDef.baseRechargeInterval = 3f;

        }
    }
}
/*
Changelist:
Commando Phase round 2s cooldown, phase blast 3s cooldown



















*/