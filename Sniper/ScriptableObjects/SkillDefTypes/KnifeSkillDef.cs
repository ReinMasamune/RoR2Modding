namespace Sniper.SkillDefs
{
    using System;

    using Sniper.SkillDefTypes.Bases;
    using Sniper.States.Bases;

    using UnityEngine;

    internal class KnifeSkillDef : ReactivatedSkillDef<KnifeSkillData>
    {
        internal static KnifeSkillDef Create<TActivation, TReactivation>( String activationMachine, String reactivationMachine )
            where TActivation : ActivationBaseState<KnifeSkillData>
            where TReactivation : ReactivationBaseState<KnifeSkillData>
        {
            KnifeSkillDef def = ScriptableObject.CreateInstance<KnifeSkillDef>();
            def.InitStates<TActivation, TReactivation>( activationMachine, reactivationMachine );
            return def;
        }
    }
}
