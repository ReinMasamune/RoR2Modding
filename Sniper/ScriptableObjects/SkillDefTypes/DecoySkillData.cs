namespace Sniper.SkillDefs
{
    using System;

    using Sniper.SkillDefTypes.Bases;

    internal class DecoySkillData : SkillData
    {
        internal override String targetStateMachineName { get; } = null;

        // TODO: Implement DecoySkillData



        internal override Boolean IsDataInitialized()
        {
            return true;
        }
        internal override Boolean IsDataValid()
        {
            return true;
        }
    }
}
