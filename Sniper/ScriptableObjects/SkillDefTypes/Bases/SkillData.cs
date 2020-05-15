namespace Sniper.SkillDefTypes.Bases
{
    using System;
    using RoR2;

    internal abstract class SkillData
    {
        internal EntityStateMachine targetStateMachine;

        internal virtual String targetStateMachineName { get; } = null;

        internal abstract Boolean IsDataValid();
        internal abstract Boolean IsDataInitialized();

        internal virtual void OnInvalidate()
        {

        }
    }
}
