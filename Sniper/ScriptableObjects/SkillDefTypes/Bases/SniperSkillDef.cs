namespace Sniper.SkillDefTypes.Bases
{
    using System;

    using EntityStates;

    using ReinCore;

    using RoR2.Skills;

    internal class SniperSkillDef : SkillDef
    {
        internal static SniperSkillDef Create<TActivationState>( String stateMachineName ) where TActivationState : EntityState
        {
            SniperSkillDef def = CreateInstance<SniperSkillDef>();
            def.activationState = SkillsCore.StateType<TActivationState>();
            def.activationStateMachineName = stateMachineName;

            return def;
        }

    }
}
