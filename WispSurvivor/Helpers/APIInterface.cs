using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace WispSurvivor.Helpers
{
    public static class APIInterface
    {
        public static GameObject InstantiateClone( this GameObject g, System.String nameToSet, System.Boolean registerNetwork = true, [CallerFilePath] System.String file = "", [CallerMemberName] System.String member = "", [CallerLineNumber] System.Int32 line = 0 )
        {
            return OldStuff.PrefabHelpers.InstantiateClone( g, nameToSet, registerNetwork, file, member, line );
        }

        public static System.Boolean RegisterNewBody( GameObject g )
        {
            return OldStuff.CatalogHelpers.RegisterNewBody( g );
        }

        public static System.Boolean RegisterNewEffect( GameObject prefab )
        {
            return OldStuff.CatalogHelpers.RegisterNewEffect( prefab );
        }

        public static Boolean AddOrb( Type t )
        {
            return OldStuff.OrbHelper.AddOrb( t );
        }

        public static Boolean AddSkill( Type t )
        {
            return OldStuff.SkillsHelper.AddSkill( t );
        }

        public static Boolean AddSkillDef( SkillDef s )
        {
            return OldStuff.SkillsHelper.AddSkillDef( s );
        }

        public static Boolean AddSkillFamily( SkillFamily sf )
        {
            return OldStuff.SkillsHelper.AddSkillFamily( sf );
        }


        public static SkinDef CreateNewSkinDef( SkinDefInfo skin )
        {
            return OldStuff.CatalogHelpers.CreateNewSkinDef( skin );
        }

        public static CharacterModel.RendererInfo CreateRendererInfo( Renderer r, Material m, System.Boolean ignoreOverlays, UnityEngine.Rendering.ShadowCastingMode shadow )
        {
            return OldStuff.CatalogHelpers.CreateRendererInfo( r, m, ignoreOverlays, shadow );
        }

        public struct SkinDefInfo
        {
            public SkinDef[] baseSkins;
            public Sprite icon;
            public System.String nameToken;
            public System.String unlockableName;
            public GameObject rootObject;
            public CharacterModel.RendererInfo[] rendererInfos;
            public System.String name;
        }
    }
}
