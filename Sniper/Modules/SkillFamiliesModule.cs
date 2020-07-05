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
        internal static List<(SkillDef,String)> ammoSkills;
        internal static SkillFamily GetAmmoSkillFamily()
        {
            if( ammoSkills == null )
            {
                SkillsModule.CreateAmmoSkills();
            }

            return FromList( ammoSkills, "SniperAmmoSkillFamily" );
        }

        internal static List<(SkillDef,String)> passiveSkills;
        internal static SkillFamily GetPassiveSkillFamily()
        {
            if( passiveSkills == null )
            {
                SkillsModule.CreatePassiveSkills();
            }

            return FromList( passiveSkills, "SniperPassiveSkillFamily" );
        }

        internal static List<(SkillDef,String)> primarySkills;
        internal static SkillFamily GetPrimarySkillFamily()
        {
            if( primarySkills == null )
            {
                SkillsModule.CreatePrimarySkills();
            }

            return FromList( primarySkills, "SniperPrimarySkillFamily" );
        }

        internal static List<(SkillDef,String)> secondarySkills;
        internal static SkillFamily GetSecondarySkillFamily()
        {
            if( secondarySkills == null )
            {
                SkillsModule.CreateSecondarySkills();
            }

            return FromList( secondarySkills, "SniperSecondarySkillFamily" );
        }

        internal static List<(SkillDef,String)> utilitySkills;
        internal static SkillFamily GetUtilitySkillFamily()
        {
            if( utilitySkills == null )
            {
                SkillsModule.CreateUtilitySkills();
            }

            return FromList( utilitySkills, "SniperUtilitySkillFamily" );
        }

        internal static List<(SkillDef,String)> specialSkills;
        internal static SkillFamily GetSpecialSkillFamily()
        {
            if( specialSkills == null )
            {
                SkillsModule.CreateSpecialSkills();
            }

            return FromList( specialSkills, "SniperSpecialSkillFamily" );
        }

        private static SkillFamily FromList( List<(SkillDef def, String unlockable)> defs, String name )
        {
            foreach( var (def,unlock) in defs )
            {
                ( def as ScriptableObject ).name = def.skillName;
            }
            var fam = SkillsCore.CreateSkillFamily( defs[0].def, defs.Skip(1).ToArray() );
            ( fam as ScriptableObject ).name = name;
            return fam;
        }
    }
}
