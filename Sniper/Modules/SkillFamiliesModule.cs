namespace Sniper.Modules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using ReinCore;

    using RoR2.Skills;
    using UnityEngine;

    internal static class SkillFamiliesModule
    {
        internal static List<SkillDef> ammoSkills;
        internal static SkillFamily GetAmmoSkillFamily()
        {
            if( ammoSkills == null )
            {
                SkillsModule.CreateAmmoSkills();
            }

            return FromList( ammoSkills, "SniperAmmoSkillFamily" );
        }

        internal static List<SkillDef> passiveSkills;
        internal static SkillFamily GetPassiveSkillFamily()
        {
            if( passiveSkills == null )
            {
                SkillsModule.CreatePassiveSkills();
            }

            return FromList( passiveSkills, "SniperPassiveSkillFamily" );
        }

        internal static List<SkillDef> primarySkills;
        internal static SkillFamily GetPrimarySkillFamily()
        {
            if( primarySkills == null )
            {
                SkillsModule.CreatePrimarySkills();
            }

            return FromList( primarySkills, "SniperPrimarySkillFamily" );
        }

        internal static List<SkillDef> secondarySkills;
        internal static SkillFamily GetSecondarySkillFamily()
        {
            if( secondarySkills == null )
            {
                SkillsModule.CreateSecondarySkills();
            }

            return FromList( secondarySkills, "SniperSecondarySkillFamily" );
        }

        internal static List<SkillDef> utilitySkills;
        internal static SkillFamily GetUtilitySkillFamily()
        {
            if( utilitySkills == null )
            {
                SkillsModule.CreateUtilitySkills();
            }

            return FromList( utilitySkills, "SniperUtilitySkillFamily" );
        }

        internal static List<SkillDef> specialSkills;
        internal static SkillFamily GetSpecialSkillFamily()
        {
            if( specialSkills == null )
            {
                SkillsModule.CreateSpecialSkills();
            }

            return FromList( specialSkills, "SniperSpecialSkillFamily" );
        }

        private static SkillFamily FromList( List<SkillDef> defs, String name )
        {
            foreach( var def in defs )
            {
                ( def as ScriptableObject ).name = def.skillName;
            }
            SkillDef defaultSkill = defs[0];
            (SkillDef def, String)[] variants = defs.GetRange(1, defs.Count - 1).Select( (def) => (def,"") ).ToArray(); // TODO: Unlockable
            var fam = SkillsCore.CreateSkillFamily( defaultSkill, variants );
            ( fam as ScriptableObject ).name = name;
            return fam;
        }
    }
}
