namespace ReinCore
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using MonoMod.RuntimeDetour;

    using RoR2;

    using UnityEngine;

    public static class SkinsCore
    {
        public static Boolean loaded { get; internal set; } = false;

        public static void AddValidSkinOverride( GameObject body, ValidSkinOverrideDelegate validityOverride ) => validSkinOverrides[body] = validityOverride ?? throw new ArgumentNullException( nameof( validityOverride ) );
        public delegate Boolean ValidSkinOverrideDelegate( UInt32 skinIndex );
        public static void AddLockedSkinOverride( GameObject body, LockedSkinOverrideDelegate lockedOverride ) => lockedSkinOverrides[body] = lockedOverride ?? throw new ArgumentNullException( nameof( lockedOverride ) );

        /// <summary>
        /// A delegate to override the default check for if a skin is locked
        /// </summary>
        /// <param name="skinIndex">The index of the skin being checked</param>
        /// <returns></returns>
        public delegate Boolean LockedSkinOverrideDelegate( UInt32 skinIndex );



        // NEXT: Use normal hooks here
        static SkinsCore()
        {
            //Log.Warning( "SkinsCore loaded" );
            BindingFlags allFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
            Type parentType = typeof(Loadout.BodyLoadoutManager);
            Type bodyLoadoutType = parentType.GetNestedType( "BodyLoadout", allFlags );
            //bodyIndex = new Accessor<Int32>( bodyLoadoutType, "bodyIndex" );
            //skinPreference = new Accessor<UInt32>( bodyLoadoutType, "skinPreference" );
            //skillPreferences = new Accessor<UInt32[]>( bodyLoadoutType, "skillPreferences" );

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
            //Log.Warning( "SkinsCore loaded" );
            loaded = true;
        }

        private delegate void OrigEnforceValidity( Loadout.BodyLoadoutManager.BodyLoadout self );
        private delegate void OrigEnforceUnlockables( Loadout.BodyLoadoutManager.BodyLoadout self, UserProfile userProfile );
        private delegate Boolean OrigIsSkinValid( Loadout.BodyLoadoutManager.BodyLoadout self );
        private delegate Boolean OrigIsSkinLocked( Loadout.BodyLoadoutManager.BodyLoadout self, UserProfile userProfile );

        //private static readonly Accessor<Int32> bodyIndex;
        //private static readonly Accessor<UInt32> skinPreference;
        //private static readonly Accessor<UInt32[]> skillPreferences;

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

        private static Boolean IsSkinValid( Loadout.BodyLoadoutManager.BodyLoadout self )
        {
            Int32 bInd = (Int32)self.bodyIndex;// bodyIndex.Get( self );
            UInt32 skinPref = self.skinPreference;
            GameObject body = BodyCatalog.GetBodyPrefab((BodyIndex)bInd);
            return validSkinOverrides.TryGetValue( body, out ValidSkinOverrideDelegate Override )
                ? Override( skinPref )
                : IsSkinValidOrig( self );
        }

        private static Boolean IsSkinLocked( Loadout.BodyLoadoutManager.BodyLoadout self, UserProfile userProfile )
        {
            Int32 bInd = (Int32)self.bodyIndex;
            UInt32 skinPref = self.skinPreference;//.Get( self );
            GameObject body = BodyCatalog.GetBodyPrefab((BodyIndex)bInd);
            return lockedSkinOverrides.TryGetValue( body, out LockedSkinOverrideDelegate Override )
                ? Override( skinPref )
                : IsSkinLockedOrig( self, userProfile );
        }

        private static void EnforceUnlockables( Loadout.BodyLoadoutManager.BodyLoadout self, UserProfile userProfile )
        {
            _ = IsSkinValid( self );
            EnforceUnlockablesOrig( self, userProfile );
        }
    }
}
