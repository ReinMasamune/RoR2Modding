namespace Rein.Sniper.SkillDefTypes.Bases
{
    using System;

    using EntityStates;

    using ReinCore;

    using RoR2.Skills;

    using UnityEngine;

    internal class SniperSkillDef : SkillDef
    {
        internal static SniperSkillDef Create<TActivationState>( String stateMachineName ) where TActivationState : EntityState
        {
            SniperSkillDef def = CreateInstance<SniperSkillDef>();
            def.activationState = SkillsCore.StateType<TActivationState>();
            def.activationStateMachineName = stateMachineName;
            def.shootDelay = 0f;

            return def;
        }

        [SerializeField]
        internal Single shootDelay;
        internal Boolean noSprint { get => base.cancelSprintingOnActivation; set => base.cancelSprintingOnActivation = value; }
        internal Boolean isBullets { get => false; set { } }



    }
}
