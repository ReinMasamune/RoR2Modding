using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BepInEx;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Rein.Properties;
using RoR2;
using RoR2.Networking;
using UnityEngine;

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

        /// <summary>
        /// 
        /// </summary>
        public static IEnumerable<PluginInfo> plugins
        {
            get => pluginsByName.Values;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static Boolean IsPluginLoaded( String guid )
        {
            return pluginsByName.ContainsKey( guid );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="doNetChecks"></param>
        /// <param name="debugLogs"></param>
        /// <param name="infoLogs"></param>
        /// <param name="messageLogs"></param>
        /// <param name="warningLogs"></param>
        /// <param name="errorLogs"></param>
        /// <param name="fatalLogs"></param>
        public static void Init( Boolean doNetChecks, Boolean debugLogs, Boolean infoLogs, Boolean messageLogs, Boolean warningLogs, Boolean errorLogs, Boolean fatalLogs )
        {
            if( !loaded ) throw new CoreNotLoadedException( nameof( ReinCore ) );
            ReinCore.execLevel = 0;
            execLevel |= debugLogs ? ExecutionLevel.Debug : 0;
            execLevel |= infoLogs ? ExecutionLevel.Info : 0;
            execLevel |= messageLogs ? ExecutionLevel.Message : 0;
            execLevel |= warningLogs ? ExecutionLevel.Warning : 0;
            execLevel |= errorLogs ? ExecutionLevel.Error : 0;
            execLevel |= fatalLogs ? ExecutionLevel.Fatal : 0;
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
            HooksCore.RoR2.RoR2Application.UnitySystemConsoleRedirector.Redirect.On += Redirect_On;
            HooksCore.RoR2.UI.QuickPlayButtonController.Start.On += Start_On;
            _ = Tools.LoadAssembly( Rein.Properties.Resources.RoR2ScriptForwarding );
            if( !Log.loaded ) throw new CoreNotLoadedException( nameof( Log ) );

            CheckPlugins();
            r2apiExists = pluginsByName.ContainsKey( "com.bepis.r2api" );

            loaded = true;
            managerObject = new GameObject( "coremanager" );
            MonoBehaviour.DontDestroyOnLoad( managerObject );
            _ = managerObject.AddComponent<CoreManager>();
        }

        private static void Start_On( HooksCore.RoR2.UI.QuickPlayButtonController.Start.Orig orig, RoR2.UI.QuickPlayButtonController self )
        {
            self.gameObject.SetActive( false );
            orig( self );
            self.gameObject.SetActive( false );
        }

        internal static ExecutionLevel execLevel;
        internal static Boolean r2apiExists;
        internal static R2APISubmodule activeSubmodules = R2APISubmodule.None;
        internal static event OnSubmoduleDataSuppliedDelegate onSubmoduleDataSupplied;
        internal delegate void OnSubmoduleDataSuppliedDelegate( R2APISubmodule activeSubmodules );

        internal static event Action awake;
        internal static event Action start;
        internal static event Action onEnable;
        internal static event Action onDisable;
        internal static event Action update;
        internal static event Action fixedUpdate;
        internal static event Action lateUpdate;
        internal static event Action destroy;


        private static GameObject managerObject;
        private static Dictionary<String,PluginInfo> pluginsByName = new Dictionary<String, PluginInfo>();

        private static void Redirect_On( HooksCore.RoR2.RoR2Application.UnitySystemConsoleRedirector.Redirect.Orig orig )
        {
            // Do Nothing
        }

        [MethodImpl(MethodImplOptions.ForwardRef)]
        private static extern Int32 Square( Int32 number );

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

        private static void CheckPlugins()
        {
            foreach( var kv in BepInEx.Bootstrap.Chainloader.PluginInfos )
            {
                var k = kv.Key;
                var v = kv.Value;
                if( String.IsNullOrEmpty( k ) || v == null ) continue;
                pluginsByName[k] = v;
            }
        }

        private class CoreManager : MonoBehaviour
        {
            private void Awake() => ReinCore.awake?.Invoke();
            private void Start() => ReinCore.start?.Invoke();
            private void OnEnable() => ReinCore.onEnable?.Invoke();
            private void OnDisable() => ReinCore.onDisable?.Invoke();
            private void Update() => ReinCore.update?.Invoke();
            private void FixedUpdate() => ReinCore.fixedUpdate?.Invoke();
            private void LateUpdate() => ReinCore.lateUpdate?.Invoke();
            private void Destroy() => ReinCore.destroy?.Invoke();
        }
    }
}
