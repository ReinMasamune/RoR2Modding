using System;
using System.Reflection;
using BepInEx;
using MonoMod.RuntimeDetour;
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
            bodyIndex = new Accessor<Int32>( bodyLoadoutType, "bodyIndex" );

            var isSkinValidMethod = bodyLoadoutType.GetMethod("IsSkinValid", allFlags );
            IsSkinValid = Invoker.CreateDelegate<OrigIsSkinValid>( isSkinValidMethod );
            

            if( !ReinCore.r2apiExists )
            {

            }
        }

        private delegate void OrigEnforceValidity( System.Object self );
        private delegate void OrigEnforceUnlockables( System.Object self, UserProfile userProfile );
        private delegate Boolean OrigIsSkinValid( System.Object self );
        private delegate Boolean OrigIsSkilLocked( System.Object self, UserProfile userProfile );

        private static Accessor<Int32> bodyIndex;

        private static Hook isSkinValidHook;
        private static Hook isSkinLockedHook;
        private static Hook inforceValidityHook;
        private static Hook inforceUnlockablesHook;

        private static OrigIsSkinValid IsSkinValidOrig;
        private static OrigIsSkilLocked IsSkinLockedOrig;
        private static OrigEnforceValidity EnforceValidityOrig;
        private static OrigEnforceUnlockables EnforceUnlockablesOrig;

        private static Boolean IsSkinValid(System.Object self )
        {
            return IsSkinValidOrig( self );
        }

        private static Boolean IsSkinLocked(System.Object self, UserProfile userProfile)
        {
            return IsSkinLockedOrig( self, userProfile );
        }

        private static void EnforceValidity(System.Object self)
        {

        }

        private static void EnforceUnlockables(System.Object self, UserProfile userProfile)
        {

        }
    }
}
