using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using BepInEx;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using UnityEngine;

namespace ModdedTrials
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.Rein.ModdedTrials", "ModdedTrials", "1.0.0" )]
    public class Main : BaseUnityPlugin
    {
        private static Func<WeeklyRun,UInt32> getSeed;
        private static Func<String> getRunReportsFolder;
        private static Main instance;
        public Main()
        {
            instance = this;
        }
        public void Awake()
        {
            if( getSeed == null )
            {
                var instanceParameter = Expression.Parameter( typeof( WeeklyRun ), "instance" );
                var fieldAccess = Expression.Field(instanceParameter,"serverSeedCycle" );
                getSeed = Expression.Lambda<Func<WeeklyRun, UInt32>>( fieldAccess, instanceParameter ).Compile();
            }
            if( getRunReportsFolder == null )
            {
                var fieldAccess = Expression.Field( null, typeof(RunReport), "runReportsFolder" );
                getRunReportsFolder = Expression.Lambda<Func<String>>( fieldAccess ).Compile();
            }
        }

        private void OnDisable()
        {
            base.Logger.LogWarning( "Disabled" );
            IL.RoR2.WeeklyRun.ClientSubmitLeaderboardScore -= this.WeeklyRun_ClientSubmitLeaderboardScore;
            On.RoR2.DisableIfGameModded.OnEnable -= this.DisableIfGameModded_OnEnable1;
            this.HideMenuButton();
        }

        private void OnEnable()
        {
            base.Logger.LogWarning( "Enabled" );
            IL.RoR2.WeeklyRun.ClientSubmitLeaderboardScore += this.WeeklyRun_ClientSubmitLeaderboardScore;
            On.RoR2.DisableIfGameModded.OnEnable += this.DisableIfGameModded_OnEnable1;
            this.ShowMenuButton();
        }

        private static HashSet<String> names = new HashSet<String>();
        private void DisableIfGameModded_OnEnable1( On.RoR2.DisableIfGameModded.orig_OnEnable orig, DisableIfGameModded self )
        {
            if( (RoR2Application.isModded == true && !names.Contains( self.gameObject.name ) ) )
            {
                self.gameObject.SetActive( false );
            }
        }

        private void WeeklyRun_ClientSubmitLeaderboardScore( MonoMod.Cil.ILContext il )
        {
            ILCursor c = new ILCursor( il );

            c.GotoNext( MoveType.After, x => x.MatchCall<String>( nameof( String.IsNullOrEmpty ) ) );
            c.Emit( OpCodes.Ldarg_1 );
            c.Emit( OpCodes.Ldarg_0 );
            c.EmitDelegate<Func<RunReport,WeeklyRun,Boolean>>( (report, run) =>
            {
                SaveFile( report, run );
                return true;
            } );
            c.Emit( OpCodes.Or );

            c.GotoNext( MoveType.After, x => x.MatchCall<UnityEngine.Object>( "op_Implicit" ) );
            c.EmitDelegate<Func<Boolean>>( () => !RoR2Application.isModded );
            c.Emit( OpCodes.And );
        }

        private void ShowMenuButton()
        {
            names.Add( "Button, Weekly" );
        }

        private void HideMenuButton()
        {
            names.Remove( "Button, Weekly" );
        }


        private static void SaveFile( RunReport report, WeeklyRun run )
        {
            String time = ((Int32)Math.Ceiling((Double)report.runStopwatchValue / 1000.0 )).ToString();

            var cycle = WeeklyRun.GetCurrentSeedCycle();
            var cycleStart = WeeklyRun.GetSeedCycleStartDateTime( WeeklyRun.GetCurrentSeedCycle() );
            String cycleDate = cycleStart.ToShortDateString();

            var body = BodyCatalog.GetBodyPrefab(NetworkUser.readOnlyLocalPlayersList[0].bodyIndexPreference);
            if( body == null )
            {
                instance.Logger.LogError( "Invalid body selected" );
                return;
            }
            var survivorDef = SurvivorCatalog.FindSurvivorDefFromBody( body );
            if( survivorDef == null )
            {
                instance.Logger.LogError( "Selected body is not in survivorcatalog" );
                return;
            }
            String character = Language.GetString( survivorDef.displayNameToken );
            instance.Logger.LogMessage( "Your character was: " + character );
            instance.Logger.LogMessage( "Your time was: " + time );
            instance.Logger.LogMessage( "Score send aborted, saving run report to disk instead." );
            instance.Logger.LogMessage( "In the future, there may be a leaderboard set up such that you can upload the file to submit a run" );

            String directory = "\\ModdedTrials\\" + cycleStart + "\\" + character + "\\";
            String directoryGlobal = getRunReportsFolder() + directory;
            System.IO.Directory.CreateDirectory( directoryGlobal );
            String fileBaseName = time;
            Int32 i = 0;
            while( System.IO.File.Exists( directoryGlobal + fileBaseName + (i != 0 ? "(" + i + ")" : String.Empty) ) ) ++i;

            RunReport.Save( report, directory + fileBaseName );
        }
    }
}
