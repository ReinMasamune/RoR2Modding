using System;
using System.Collections.Generic;
using System.Reflection;
using BepInEx;
using MonoMod.RuntimeDetour;
using RoR2;

namespace ReinCore
{
    public static class SkinsCore
    {
        public static Boolean loaded { get; internal set; } = false;

        public static void AddValidSkinOverride( Int32 bodyIndex, ValidSkinOverrideDelegate validityOverride )
        {
            if( validityOverride == null ) throw new ArgumentNullException( nameof( validityOverride ) );
            validSkinOverrides[bodyIndex] = validityOverride;
        }
        public delegate Boolean ValidSkinOverrideDelegate( Int32 skinIndex );

        public static void AddLockedSkinOverride( Int32 bodyIndex, LockedSkinOverrideDelegate lockedOverride )
        {
            if( lockedOverride == null ) throw new ArgumentNullException( nameof( lockedOverride ) );
            lockedSkinOverrides[bodyIndex] = lockedOverride;
        }

        /// <summary>
        /// A delegate to override the default check for if a skin is locked
        /// </summary>
        /// <param name="skinIndex">The index of the skin being checked</param>
        /// <returns></returns>
        public delegate Boolean LockedSkinOverrideDelegate( Int32 skinIndex );




        static SkinsCore()
        {
            var allFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
            var parentType = typeof(Loadout.BodyLoadoutManager);
            var bodyLoadoutType = parentType.GetNestedType( "BodyLoadout", allFlags );
            bodyIndex = new Accessor<Int32>( bodyLoadoutType, "bodyIndex" );
            skinPreference = new Accessor<Int32>( bodyLoadoutType, "skinPreference" );

            var isSkinValidMethod = bodyLoadoutType.GetMethod("IsSkinValid", allFlags );
            var localIsSkinValidMethod = typeof(SkinsCore).GetMethod( "IsSkinValid", allFlags );
            isSkinValidHook = new Hook( isSkinValidMethod, localIsSkinValidMethod );
            IsSkinValidOrig = isSkinValidHook.GenerateTrampoline<OrigIsSkinValid>();

            var isSkinLockedMethod = bodyLoadoutType.GetMethod("IsSkinLocked", allFlags );
            var localIsSkinLockedMethod = typeof(SkinsCore).GetMethod( "IsSkinLocked", allFlags );
            isSkinLockedHook = new Hook( isSkinLockedMethod, localIsSkinLockedMethod );
            IsSkinLockedOrig = isSkinLockedHook.GenerateTrampoline<OrigIsSkinLocked>();

            if( !ReinCore.r2apiExists )
            {
                var enforceValidityMethod = bodyLoadoutType.GetMethod( "EnforceValidity", allFlags );
                EnforceValidityOrig = Invoker.CreateDelegate<OrigEnforceValidity>( enforceValidityMethod );

                var enforceUnlockablesMethod = bodyLoadoutType.GetMethod( "EnforceUnlockables", allFlags );
                var localEnforceUnlockablesMethod = typeof(SkinsCore).GetMethod( "EnforceUnlockables", allFlags );
                enforceUnlockablesHook = new Hook( enforceUnlockablesMethod, localEnforceUnlockablesMethod );
                EnforceUnlockablesOrig = enforceUnlockablesHook.GenerateTrampoline<OrigEnforceUnlockables>();
            }
        }

        private delegate void OrigEnforceValidity( System.Object self );
        private delegate void OrigEnforceUnlockables( System.Object self, UserProfile userProfile );
        private delegate Boolean OrigIsSkinValid( System.Object self );
        private delegate Boolean OrigIsSkinLocked( System.Object self, UserProfile userProfile );

        private static Accessor<Int32> bodyIndex;
        private static Accessor<Int32> skinPreference;

        private static Hook isSkinValidHook;
        private static Hook isSkinLockedHook;
        private static Hook enforceUnlockablesHook;

        private static OrigIsSkinValid IsSkinValidOrig;
        private static OrigIsSkinLocked IsSkinLockedOrig;
        private static OrigEnforceValidity EnforceValidityOrig;
        private static OrigEnforceUnlockables EnforceUnlockablesOrig;

        private static Dictionary<Int32,ValidSkinOverrideDelegate> validSkinOverrides = new Dictionary<Int32, ValidSkinOverrideDelegate>();
        private static Dictionary<Int32,LockedSkinOverrideDelegate> lockedSkinOverrides = new Dictionary<Int32, LockedSkinOverrideDelegate>();

        private static Boolean IsSkinValid(System.Object self )
        {
            var bInd = bodyIndex.Get( self );
            var skinPref = skinPreference.Get( self );
            if( validSkinOverrides.TryGetValue( bInd, out var Override ) )
            {
                return Override( bInd );
            } else return IsSkinValidOrig( self );
        }

        private static Boolean IsSkinLocked(System.Object self, UserProfile userProfile)
        {
            var bInd = bodyIndex.Get( self );
            var skinPref = skinPreference.Get( self );
            if( lockedSkinOverrides.TryGetValue( bInd, out var Override ) )
            {
                return Override( skinPref );
            } else return IsSkinLockedOrig( self, userProfile );
        }

        private static void EnforceUnlockables(System.Object self, UserProfile userProfile)
        {
            EnforceValidityOrig( self );
            EnforceUnlockablesOrig( self, userProfile );
        }
    }
}
