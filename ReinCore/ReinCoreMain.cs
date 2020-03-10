using System;
using System.Collections.Generic;
using BepInEx;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;

namespace ReinCore
{
    /// <summary>
    /// 
    /// </summary>
    public static partial class ReinCore
    {
        /// <summary>
        /// 
        /// </summary>
        public static Boolean loaded { get; internal set; } = false;


        public static void Init( Boolean r2apiExists, Boolean debugLogs, Boolean infoLogs, Boolean messageLogs, Boolean warningLogs, Boolean errorLogs, Boolean fatalLogs )
        {
            if( !loaded ) throw new CoreNotLoadedException( nameof( ReinCore ) );
            ReinCore.execLevel = 0;
            execLevel |= debugLogs ? ExecutionLevel.Debug : 0;
            execLevel |= infoLogs ? ExecutionLevel.Info : 0;
            execLevel |= messageLogs ? ExecutionLevel.Message : 0;
            execLevel |= warningLogs ? ExecutionLevel.Warning : 0;
            execLevel |= errorLogs ? ExecutionLevel.Error : 0;
            execLevel |= fatalLogs ? ExecutionLevel.Fatal : 0;
            ReinCore.r2apiExists = r2apiExists;
            Log.Message( String.Format( "{0} successfully loaded", nameof( ReinCore ) ) );
        }
        public static void SupplySubmoduleData( HashSet<String> submoduleNames )
        {
            if( submoduleNames == null ) return;
            ParseSubmodules( submoduleNames );
            onSubmoduleDataSupplied?.Invoke( activeSubmodules );
        }

        static ReinCore()
        {
            HooksCore.on_RoR2_RoR2Application_UnitySystemConsoleRedirector_Redirect += HooksCore_on_RoR2_RoR2Application_UnitySystemConsoleRedirector_Redirect;
            if( !Log.loaded ) throw new CoreNotLoadedException( nameof( Log ) );
            loaded = true;
        }
        internal static ExecutionLevel execLevel;
        internal static Boolean r2apiExists;
        internal static R2APISubmodule activeSubmodules = R2APISubmodule.None;
        internal static event OnSubmoduleDataSuppliedDelegate onSubmoduleDataSupplied;
        internal delegate void OnSubmoduleDataSuppliedDelegate( R2APISubmodule activeSubmodules );

        private static void ParseSubmodules( HashSet<String> loadedSubmodules )
        {
            foreach( var sub in loadedSubmodules )
            {
                switch( sub )
                {
                    default:
                        Log.Warning( String.Format( "Unknown submodule: {0}", sub ) );
                        break;
                    case "AssetAPI":
                        activeSubmodules |= R2APISubmodule.AssetAPI;
                        break;
                    case "DifficultyAPI":
                        activeSubmodules |= R2APISubmodule.DifficultyAPI;
                        break;
                    case "DirectorAPI":
                        activeSubmodules |= R2APISubmodule.DirectorAPI;
                        break;
                    case "EffectAPI":
                        activeSubmodules |= R2APISubmodule.EffectAPI;
                        break;
                    case "EntityAPI":
                        activeSubmodules |= R2APISubmodule.EntityAPI;
                        break;
                    case "InventoryAPI":
                        activeSubmodules |= R2APISubmodule.InventoryAPI;
                        break;
                    case "ItemAPI":
                        activeSubmodules |= R2APISubmodule.ItemAPI;
                        break;
                    case "ItemDropAPI":
                        activeSubmodules |= R2APISubmodule.ItemDropAPI;
                        break;
                    case "LoadoutAPI":
                        activeSubmodules |= R2APISubmodule.LoadoutAPI;
                        break;
                    case "LobbyConfigAPI":
                        activeSubmodules |= R2APISubmodule.LobbyConfigAPI;
                        break;
                    case "ModListAPI":
                        activeSubmodules |= R2APISubmodule.ModListAPI;
                        break;
                    case "OrbAPI":
                        activeSubmodules |= R2APISubmodule.OrbAPI;
                        break;
                    case "PlayerAPI":
                        activeSubmodules |= R2APISubmodule.PlayerAPI;
                        break;
                    case "PrefabAPI":
                        activeSubmodules |= R2APISubmodule.PrefabAPI;
                        break;
                    case "ResourcesAPI":
                        activeSubmodules |= R2APISubmodule.ResourcesAPI;
                        break;
                    case "SkillAPI":
                        activeSubmodules |= R2APISubmodule.SkillAPI;
                        break;
                    case "SkinAPI":
                        activeSubmodules |= R2APISubmodule.SkinAPI;
                        break;
                    case "SurvivorAPI":
                        activeSubmodules |= R2APISubmodule.SurvivorAPI;
                        break;
                    case "AssetPlus":
                        activeSubmodules |= R2APISubmodule.AssetPlus;
                        break;
                }
            }
        }



        private static void HooksCore_on_RoR2_RoR2Application_UnitySystemConsoleRedirector_Redirect( HooksCore.orig_RoR2_RoR2Application_UnitySystemConsoleRedirector_Redirect orig )
        {
            // Do Nothing
        }
    }
}
