using RoR2;
using RoR2.Skills;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    public static class APIInterface
    {
        public static GameObject InstantiateClone( this GameObject g, System.String nameToSet, System.Boolean registerNetwork = true, [CallerFilePath] System.String file = "", [CallerMemberName] System.String member = "", [CallerLineNumber] System.Int32 line = 0 ) => R2API.PrefabAPI.InstantiateClone( g, nameToSet, registerNetwork, file, member, line );

        public static System.Boolean RegisterNewEffect( GameObject prefab ) => R2API.EffectAPI.AddEffect( prefab );

        public static Boolean AddOrb( Type t ) => R2API.OrbAPI.AddOrb( t );

        public static Boolean AddSkill( Type t ) => R2API.SkillAPI.AddSkill( t );

        public static Boolean AddSkillDef( SkillDef s ) => R2API.SkillAPI.AddSkillDef( s );

        public static Boolean AddSkillFamily( SkillFamily sf ) => R2API.SkillAPI.AddSkillFamily( sf );


        public static SkinDef CreateNewSkinDef( R2API.SkinAPI.SkinDefInfo skin ) => R2API.SkinAPI.CreateNewSkinDef( skin );

        public static CharacterModel.RendererInfo CreateRendererInfo( Renderer r, Material m, System.Boolean ignoreOverlays, UnityEngine.Rendering.ShadowCastingMode shadow ) => OldStuff.CatalogHelpers.CreateRendererInfo( r, m, ignoreOverlays, shadow );

    }
}
