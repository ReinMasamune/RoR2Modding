namespace ReinCore
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using BepInEx;
    using MonoMod.RuntimeDetour;
    using RoR2;
    using UnityEngine;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static class SkinsCore
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static Boolean loaded { get; internal set; } = false;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static void AddValidSkinOverride( GameObject body, ValidSkinOverrideDelegate validityOverride ) => validSkinOverrides[body] = validityOverride ?? throw new ArgumentNullException( nameof( validityOverride ) );
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public delegate Boolean ValidSkinOverrideDelegate( UInt32 skinIndex );
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static void AddLockedSkinOverride( GameObject body, LockedSkinOverrideDelegate lockedOverride ) => lockedSkinOverrides[body] = lockedOverride ?? throw new ArgumentNullException( nameof( lockedOverride ) );

        /// <summary>
        /// A delegate to override the default check for if a skin is locked
        /// </summary>
        /// <param name="skinIndex">The index of the skin being checked</param>
        /// <returns></returns>
        public delegate Boolean LockedSkinOverrideDelegate( UInt32 skinIndex );




        static SkinsCore()
        {
            BindingFlags allFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
            Type parentType = typeof(Loadout.BodyLoadoutManager);
            Type bodyLoadoutType = parentType.GetNestedType( "BodyLoadout", allFlags );
            bodyIndex = new Accessor<Int32>( bodyLoadoutType, "bodyIndex" );
            skinPreference = new Accessor<UInt32>( bodyLoadoutType, "skinPreference" );
            skillPreferences = new Accessor<UInt32[]>( bodyLoadoutType, "skillPreferences" );

            MethodInfo isSkinValidMethod = bodyLoadoutType.GetMethod("IsSkinValid", allFlags );
            MethodInfo localIsSkinValidMethod = typeof(SkinsCore).GetMethod( "IsSkinValid", allFlags );
            isSkinValidHook = new Hook( isSkinValidMethod, localIsSkinValidMethod );
            IsSkinValidOrig = isSkinValidHook.GenerateTrampoline<OrigIsSkinValid>();

            MethodInfo isSkinLockedMethod = bodyLoadoutType.GetMethod("IsSkinLocked", allFlags );
            MethodInfo localIsSkinLockedMethod = typeof(SkinsCore).GetMethod( "IsSkinLocked", allFlags );
            isSkinLockedHook = new Hook( isSkinLockedMethod, localIsSkinLockedMethod );
            IsSkinLockedOrig = isSkinLockedHook.GenerateTrampoline<OrigIsSkinLocked>();

            if( !ReinCore.r2apiExists )
            {
                _ = bodyLoadoutType.GetMethod( "EnforceValidity", allFlags );
                //EnforceValidityOrig = Invoker.CreateDelegate<OrigEnforceValidity>( enforceValidityMethod );

                MethodInfo enforceUnlockablesMethod = bodyLoadoutType.GetMethod( "EnforceUnlockables", allFlags );
                MethodInfo localEnforceUnlockablesMethod = typeof(SkinsCore).GetMethod( "EnforceUnlockables", allFlags );
                enforceUnlockablesHook = new Hook( enforceUnlockablesMethod, localEnforceUnlockablesMethod );
                EnforceUnlockablesOrig = enforceUnlockablesHook.GenerateTrampoline<OrigEnforceUnlockables>();
            }
        }

        private delegate void OrigEnforceValidity( System.Object self );
        private delegate void OrigEnforceUnlockables( System.Object self, UserProfile userProfile );
        private delegate Boolean OrigIsSkinValid( System.Object self );
        private delegate Boolean OrigIsSkinLocked( System.Object self, UserProfile userProfile );

        private static readonly Accessor<Int32> bodyIndex;
        private static readonly Accessor<UInt32> skinPreference;
        private static readonly Accessor<UInt32[]> skillPreferences;

        private static readonly Hook isSkinValidHook;
        private static readonly Hook isSkinLockedHook;
        private static readonly Hook enforceUnlockablesHook;

#pragma warning disable IDE1006 // Naming Styles
        private static readonly OrigIsSkinValid IsSkinValidOrig;
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
        private static readonly OrigIsSkinLocked IsSkinLockedOrig;
#pragma warning restore IDE1006 // Naming Styles
        //private static OrigEnforceValidity EnforceValidityOrig;
#pragma warning disable IDE1006 // Naming Styles
        private static readonly OrigEnforceUnlockables EnforceUnlockablesOrig;
#pragma warning restore IDE1006 // Naming Styles

        private static readonly Dictionary<GameObject,ValidSkinOverrideDelegate> validSkinOverrides = new Dictionary<GameObject, ValidSkinOverrideDelegate>();
        private static readonly Dictionary<GameObject,LockedSkinOverrideDelegate> lockedSkinOverrides = new Dictionary<GameObject, LockedSkinOverrideDelegate>();

        private static Boolean IsSkinValid(System.Object self )
        {
            Int32 bInd = bodyIndex.Get( self );
            UInt32 skinPref = skinPreference.Get( self );
            GameObject body = BodyCatalog.GetBodyPrefab(bInd);
            return validSkinOverrides.TryGetValue( body, out ValidSkinOverrideDelegate Override )
                ? Override( skinPref )
                : IsSkinValidOrig( self );
        }

        private static Boolean IsSkinLocked(System.Object self, UserProfile userProfile)
        {
            Int32 bInd = bodyIndex.Get( self );
            UInt32 skinPref = skinPreference.Get( self );
            GameObject body = BodyCatalog.GetBodyPrefab(bInd);
            return lockedSkinOverrides.TryGetValue( body, out LockedSkinOverrideDelegate Override )
                ? Override( skinPref )
                : IsSkinLockedOrig( self, userProfile );
        }

        private static void EnforceUnlockables(System.Object self, UserProfile userProfile)
        {
            _ = IsSkinValid( self );
            EnforceUnlockablesOrig( self, userProfile );
        }
    }
}
