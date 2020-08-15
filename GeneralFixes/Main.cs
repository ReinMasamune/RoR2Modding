//namespace R2API.Utils
//{
//    using System;
//    using System.ComponentModel;
//    using System.Diagnostics;
//    [EditorBrowsable(EditorBrowsableState.Never)]
//    public class ManualNetworkRegistrationAttribute : Attribute { }
//}

namespace ReinGeneralFixes
{
    using System;
    using System.Runtime.CompilerServices;

    using BepInEx;

    using ReinCore;


    [BepInDependency(Rein.AssemblyLoad.guid, BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin(_guid, "General Balance + Fixes", Rein.Properties.Info.ver)]
    internal partial class Main : BaseUnityPlugin
    {
        private const Int32 networkVersion = -1;
        private const Boolean useBuild = false;
        private const Boolean useRev = false;
        private const String _guid = "Rein.GeneralBalanceFixes";
        internal static String guid => _guid;
        internal static String version => Rein.Properties.Info.ver;


        internal const Single gestureBreakChance = 0.025f;

        internal static Main instance;



#pragma warning disable IDE1006 // Naming Styles
        private event Action Load;
        private event Action Enable;
        private event Action Disable;
        private event Action FirstFrame;
        private event Action Frame;
        private event Action PostFrame;
        private event Action Tick;
        private event Action GUI;
#pragma warning restore IDE1006 // Naming Styles


        partial void BalanceCommandoCDs();
        partial void BalanceCorpsebloom();
        partial void BalanceOSP();
        partial void BalanceGesture();
        partial void BalanceConvergence();
        partial void BalanceWillOWisp();
        partial void BalanceEngiTurrets();
        partial void BalanceGame();
        partial void BalanceElites();
        partial void BalanceHealing();
        partial void BalanceOnKills();



        partial void FixBandolier();
        partial void FixSelfDamage();
        partial void FixDoTs();
        partial void FixHuntressFlurry();


        partial void QoLVisionsCrosshair();
        partial void QoLEngiTurretInheritance();
        partial void QoLHuntressTracking();
        partial void QoLEclipse();


#if PROFILER
        partial void Profiler();
#endif
#if DPSMETER
        partial void SetupDPSMeter();
#endif

        private Main()
        {
            instance = this;
            ReinCore.AddModHash(guid, version, useBuild, useRev, networkVersion);
#if PROFILER
            this.Profiler();
#endif


            this.BalanceCommandoCDs();
            this.BalanceCorpsebloom();
            this.BalanceOSP();
            this.BalanceGesture();
            this.BalanceConvergence();
            this.BalanceWillOWisp();
            //this.BalanceEngiTurrets();
            this.BalanceGame();
            this.BalanceElites();

            this.FixBandolier();
            this.FixSelfDamage();
            this.FixDoTs();
            this.FixHuntressFlurry();

            this.QoLVisionsCrosshair();
            this.QoLEngiTurretInheritance();
            this.QoLHuntressTracking();
            this.QoLEclipse();

            RoR2.RoR2Application.isModded = true;
            this.Tick += () => RoR2.RoR2Application.isModded = true;

#if DPSMETER
            this.SetupDPSMeter();
#endif
        }


#pragma warning disable IDE0051 // Remove unused private members
        private void Awake() => this.Load?.Invoke();
        private void Start() => this.FirstFrame?.Invoke();
        private void OnEnable() => this.Enable?.Invoke();
        private void OnDisable() => this.Disable?.Invoke();
        private void Update() => this.Frame?.Invoke();
        private void LateUpdate() => this.PostFrame?.Invoke();
        private void FixedUpdate() => this.Tick?.Invoke();
        private void OnGUI() => this.GUI?.Invoke();
#pragma warning restore IDE0051 // Remove unused private members



        internal static void Log( BepInEx.Logging.LogLevel level, System.Object data, String member, Int32 line )
        {
            if( data == null )
            {
                Main.instance.Logger.LogError( "Null data sent by: " + member + " line: " + line );
                return;
            }

            if( level == BepInEx.Logging.LogLevel.Fatal || level == BepInEx.Logging.LogLevel.Error || level == BepInEx.Logging.LogLevel.Warning | level == BepInEx.Logging.LogLevel.Message )
            {
                Main.instance.Logger.Log( level, data );
            } else
            {
#if LOGGING
                Main.instance.Logger.Log( level, data );
#endif
            }
#if FINDLOGS
            Main.instance.Logger.LogWarning( "Log: " + level.ToString() + " called by: " + member + " : " + line );
#endif
        }

        internal static void LogI( System.Object data, [CallerMemberName] String member = "", [CallerLineNumber] Int32 line = 0 ) => Main.Log( BepInEx.Logging.LogLevel.Info, data, member, line );
        internal static void LogM( System.Object data, [CallerMemberName] String member = "", [CallerLineNumber] Int32 line = 0 ) => Main.Log( BepInEx.Logging.LogLevel.Message, data, member, line );
        internal static void LogD( System.Object data, [CallerMemberName] String member = "", [CallerLineNumber] Int32 line = 0 ) => Main.Log( BepInEx.Logging.LogLevel.Debug, data, member, line );
        internal static void LogW( System.Object data, [CallerMemberName] String member = "", [CallerLineNumber] Int32 line = 0 ) => Main.Log( BepInEx.Logging.LogLevel.Warning, data, member, line );
        internal static void LogE( System.Object data, [CallerMemberName] String member = "", [CallerLineNumber] Int32 line = 0 ) => Main.Log( BepInEx.Logging.LogLevel.Error, data, member, line );
        internal static void LogF( System.Object data, [CallerMemberName] String member = "", [CallerLineNumber] Int32 line = 0 ) => Main.Log( BepInEx.Logging.LogLevel.Fatal, data, member, line );
        internal static Int32 logCounter = 0;
        internal static void LogC( [CallerMemberName] String member = "", [CallerLineNumber] Int32 line = 0 ) => Main.Log( BepInEx.Logging.LogLevel.Info, member + ": " + line + ":: " + logCounter++, member, line );
    }
}
