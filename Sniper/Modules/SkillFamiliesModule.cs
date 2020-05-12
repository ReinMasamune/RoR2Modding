namespace Sniper.Modules
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using BepInEx.Logging;
    using ReinCore;
    using RoR2;
    using RoR2.Networking;
    using UnityEngine;
    using KinematicCharacterController;
    using EntityStates;
    using RoR2.Skills;
    using System.Linq;

    internal static class SkillFamiliesModule
    {
        internal static List<SkillDef> ammoSkills;
        internal static SkillFamily GetAmmoSkillFamily()
        {
            if( ammoSkills == null )
            {
                SkillsModule.CreateAmmoSkills();
            }

            return FromList( ammoSkills );
        }

        internal static List<SkillDef> passiveSkills;
        internal static SkillFamily GetPassiveSkillFamily()
        {
            if( passiveSkills == null )
            {
                SkillsModule.CreatePassiveSkills();
            }

            return FromList( passiveSkills );
        }

        internal static List<SkillDef> primarySkills;
        internal static SkillFamily GetPrimarySkillFamily()
        {
            if( primarySkills == null )
            {
                SkillsModule.CreatePrimarySkills();
            }

            return FromList( primarySkills );
        }

        internal static List<SkillDef> secondarySkills;
        internal static SkillFamily GetSecondarySkillFamily()
        {
            if( secondarySkills == null )
            {
                SkillsModule.CreateSecondarySkills();
            }

            return FromList( secondarySkills );
        }

        internal static List<SkillDef> utilitySkills;
        internal static SkillFamily GetUtilitySkillFamily()
        {
            if( utilitySkills == null )
            {
                SkillsModule.CreateUtilitySkills();
            }

            return FromList( utilitySkills );
        }

        internal static List<SkillDef> specialSkills;
        internal static SkillFamily GetSpecialSkillFamily()
        {
            if( specialSkills == null )
            {
                SkillsModule.CreateSpecialSkills();
            }

            return FromList( specialSkills );
        }

        private static SkillFamily FromList( List<SkillDef> defs )
        {
            SkillDef defaultSkill = defs[0];
            (SkillDef def, String)[] variants = defs.GetRange(1, defs.Count - 1).Select( (def) => (def,"") ).ToArray();
            return SkillsCore.CreateSkillFamily( defaultSkill, variants );
        }
    }
}
