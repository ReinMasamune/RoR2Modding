using System;
using System.Reflection;
using BepInEx;
using RoR2;

namespace ReinCore
{
    public static class SkinsCore
    {
        public static Boolean loaded { get; internal set; } = false;



        static SkinsCore()
        {
            var allFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
            var parentType = typeof(Loadout.BodyLoadoutManager);
            var bodyLoadoutType = parentType.GetNestedType( "BodyLoadout", allFlags );

            
            

            if( !ReinCore.r2apiExists )
            {

            }
        }

        private delegate void OrigEnforceValidity( System.Object self );
        private delegate void OrigEnforceUnlockable( System.Object self, UserProfile userProfile );
        private delegate void OrigIsSkinValid( System.Object self );
        private delegate void OrigIsSkilLocked( System.Object self );
        



    }
}
