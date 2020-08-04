namespace ReinCore
{
	using System;
	using System.Collections.Generic;
	using System.Reflection;
    using System.Runtime.CompilerServices;

    using BepInEx;

    using EntityLogic;

    using Mono.Cecil.Cil;

	using MonoMod.Cil;
    using MonoMod.Utils;

    using RoR2;
	using RoR2.Achievements;

	using BF = System.Reflection.BindingFlags;


	public static class UnlocksCore
	{
        public static Boolean loaded { get; internal set; } = false;
        public static Boolean ableToAdd { get; private set; } = false;

        public static void AddUnlockable<TUnlockable>( Boolean serverTracked )
            where TUnlockable : BaseAchievement, IModdedUnlockableDataProvider, new()
        {
            if( !loaded ) throw new CoreNotLoadedException( nameof( UnlocksCore ) );
            if( !ableToAdd ) throw new InvalidOperationException( "Too late to add unlocks. Must be done during awake." );

            var instance = new TUnlockable();
            var unlockableIdentifier = instance.unlockableIdentifier;
            var identifier = instance.achievementIdentifier;

            if( !usedRewardIds.Add( unlockableIdentifier ) ) throw new InvalidOperationException( $"The unlockable identifier '{unlockableIdentifier}' is already used by another mod or the base game." );

            var ach = new AchievementDef
            {
                identifier = instance.achievementIdentifier,
                unlockableRewardIdentifier = instance.unlockableIdentifier,
                prerequisiteAchievementIdentifier = instance.prerequisiteUnlockableIdentifier,
                nameToken = instance.achievementNameToken,
                descriptionToken = instance.achievementDescToken,
                iconPath = instance.spritePath,
                type = serverTracked ? null : instance.GetType(),
                serverTrackerType = serverTracked ? instance.GetType() : null,
            };

            var unl = new UnlockableDef
            {
                nameToken = instance.unlockableNameToken,
                getHowToUnlockString = instance.GetHowToUnlock,
                getUnlockedString = instance.GetUnlocked,
            };

            moddedUnlocks.Add( (ach, unl) );
        }

        static UnlocksCore()
        {
            Log.Warning( "UnlocksCore loaded" );
            //var dmd = new DynamicMethodDefinition("UnlockCore<>UnlockableCatalog<>RegisterUnlockable<>", null, new[] { typeof( String ), typeof( UnlockableDef ) } );
            //var proc = dmd.GetILProcessor();
            //var method = typeof( UnlockableCatalog ).GetMethod( "RegisterUnlockable", BF.Public | BF.Static | BF.NonPublic );
            //if( method is null ) throw new NullReferenceException( "No method found in unlockablecatalog" );
            ////proc.Emit( OpCodes.Ldarg_0 );
            ////proc.Emit( OpCodes.Ldarg_1 );
            ////proc.Emit( OpCodes.Call, method );
            //proc.Emit( OpCodes.Jmp, method );
            //proc.Emit( OpCodes.Ret );
            //registerUnlockable = (Action<String, UnlockableDef>)dmd.Generate().CreateDelegate<Action<String, UnlockableDef>>();

            //HooksCore.RoR2.AchievementManager.CollectAchievementDefs.Il += CollectAchievementDefs_Il;
            //HooksCore.RoR2.UnlockableCatalog.Init.Il += Init_Il;

            //loaded = true;
            //ableToAdd = true;
            Log.Warning( "UnlocksCore loaded" );
		}

        private static readonly HashSet<String> usedRewardIds = new HashSet<String>();
        //private static readonly Action<String, UnlockableDef> registerUnlockable;

        private static void Init_Il( ILContext il )
        {
            static void EmittedDelegate()
            {
                ableToAdd = false;
                for( Int32 i = 0; i < moddedUnlocks.Count; ++i )
                {
                    var (ach, unl) = moddedUnlocks[i];
                    UnlockableCatalog.RegisterUnlockable(ach.unlockableRewardIdentifier, unl );
                }
            }

            var c = new ILCursor( il );

            String text = "";
            while( c.TryGotoNext( MoveType.After,
                x => x.MatchLdstr( out text ),
                x => x.MatchStfld( out _ ) ) )
            {
                _ = usedRewardIds.Add( text );
            }

            _ = c.GotoNext( MoveType.Before, x => x.MatchLdsfld( typeof( RoR2.UnlockableCatalog ).GetField( "nameToDefTable", BF.Public | BF.NonPublic | BF.Static ) ) );
            _ = c.EmitDelegate<Action>( EmittedDelegate );
        }

        private static void CollectAchievementDefs_Il( MonoMod.Cil.ILContext il )
		{
            var f1 = typeof(AchievementManager).GetField( "achievementIdentifiers", BF.Public | BF.Static | BF.NonPublic );
            if( f1 is null ) throw new NullReferenceException( $"Could not find field in {nameof( AchievementManager )}" );
			var cursor = new ILCursor( il );
			_ = cursor.GotoNext( MoveType.After,
                x => x.MatchEndfinally(),
                x => x.MatchLdloc( 1 )
            );

            cursor.Index++;

            void EmittedDelegate( List<AchievementDef> list, Dictionary<String,AchievementDef> map, List<String> identifiers )
            {
                ableToAdd = false;
                for( Int32 i = 0; i < moddedUnlocks.Count; ++i )
                {
                    var (ach, unl) = moddedUnlocks[i];
                    identifiers.Add( ach.identifier );
                    list.Add( ach );
                    map.Add( ach.identifier, ach );
                }
            }

            _ = cursor.Emit( OpCodes.Ldarg_0 );
            _ = cursor.Emit( OpCodes.Ldsfld, f1 );
            _ = cursor.EmitDelegate<Action<List<AchievementDef>,Dictionary<String,AchievementDef>,List<String>>>( EmittedDelegate );
            _ = cursor.Emit( OpCodes.Ldloc_1 );
        }

		private static List<(AchievementDef achDef, UnlockableDef unlockableDef)> moddedUnlocks = new List<(AchievementDef achDef, UnlockableDef unlockableDef)>();
	}
}
