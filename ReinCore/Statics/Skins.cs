using System;
using System.Collections.Generic;
using System.Reflection;
using BepInEx;
using MonoMod.RuntimeDetour;
using RoR2;
using UnityEngine;

namespace ReinCore
{
    public static class SkinsCore
    {
        public static Boolean loaded { get; internal set; } = false;

        public static void AddValidSkinOverride( GameObject body, ValidSkinOverrideDelegate validityOverride )
        {
            if( validityOverride == null ) throw new ArgumentNullException( nameof( validityOverride ) );
            validSkinOverrides[body] = validityOverride;
        }
        public delegate Boolean ValidSkinOverrideDelegate( UInt32 skinIndex );

        public static void AddLockedSkinOverride( GameObject body, LockedSkinOverrideDelegate lockedOverride )
        {
            if( lockedOverride == null ) throw new ArgumentNullException( nameof( lockedOverride ) );
            lockedSkinOverrides[body] = lockedOverride;
        }

        /// <summary>
        /// A delegate to override the default check for if a skin is locked
        /// </summary>
        /// <param name="skinIndex">The index of the skin being checked</param>
        /// <returns></returns>
        public delegate Boolean LockedSkinOverrideDelegate( UInt32 skinIndex );




        static SkinsCore()
        {
            var allFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
            var parentType = typeof(Loadout.BodyLoadoutManager);
            var bodyLoadoutType = parentType.GetNestedType( "BodyLoadout", allFlags );
            bodyIndex = new Accessor<Int32>( bodyLoadoutType, "bodyIndex" );
            skinPreference = new Accessor<UInt32>( bodyLoadoutType, "skinPreference" );

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
        private static Accessor<UInt32> skinPreference;

        private static Hook isSkinValidHook;
        private static Hook isSkinLockedHook;
        private static Hook enforceUnlockablesHook;

        private static OrigIsSkinValid IsSkinValidOrig;
        private static OrigIsSkinLocked IsSkinLockedOrig;
        private static OrigEnforceValidity EnforceValidityOrig;
        private static OrigEnforceUnlockables EnforceUnlockablesOrig;

        private static Dictionary<GameObject,ValidSkinOverrideDelegate> validSkinOverrides = new Dictionary<GameObject, ValidSkinOverrideDelegate>();
        private static Dictionary<GameObject,LockedSkinOverrideDelegate> lockedSkinOverrides = new Dictionary<GameObject, LockedSkinOverrideDelegate>();

        private static Boolean IsSkinValid(System.Object self )
        {
            var bInd = bodyIndex.Get( self );
            var skinPref = skinPreference.Get( self );
            var body = BodyCatalog.GetBodyPrefab(bInd);
            if( validSkinOverrides.TryGetValue( body, out var Override ) )
            {
                return Override( skinPref );
            } else return IsSkinValidOrig( self );
        }

        private static Boolean IsSkinLocked(System.Object self, UserProfile userProfile)
        {
            var bInd = bodyIndex.Get( self );
            var skinPref = skinPreference.Get( self );
            var body = BodyCatalog.GetBodyPrefab(bInd);
            if( lockedSkinOverrides.TryGetValue( body, out var Override ) )
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
