﻿namespace Rein.Sniper.SkillDefs
{
    using System;

    using Rein.Sniper.SkillDefTypes.Bases;

    internal class DecoySkillData : SkillData
    {
        internal override String targetStateMachineName { get; } = null;

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
