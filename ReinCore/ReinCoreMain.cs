namespace ReinCore
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;

    using BepInEx;

    using EntityStates.GrandParentBoss;

    using Mono.Cecil;

    using MonoMod.Cil;

    using Rein.Properties;
    using RoR2;
    using RoR2.Networking;

    using UnityEngine;

    using UnityObject = UnityEngine.Object;
    using Object = System.Object;
    using MonoMod.RuntimeDetour;
    using System.Reflection;
    using UnityEngine.Experimental.UIElements;

    public static partial class ReinCore
    {
        public static Boolean loaded { get; internal set; } = false;

        public static IEnumerable<PluginInfo> plugins
        {
            get => pluginsByName.Values;
        }

        public static Boolean IsPluginLoaded(String guid) => pluginsByName.ContainsKey(guid);


        private static readonly List<String> addedGuids = new List<String>();

        private static void AddModHash(String guid, Int32 major = -1, Int32 minor = -1, Int32 build = -1, Int32 rev = -1, Int32 net = -1)
        {
            var text = $"{guid};{(major >= 0 ? major : "x")}.{(minor >= 0 ? minor : "x")}.{(build >= 0 ? build : "x")}.{(rev >= 0 ? rev : "x")}{(net >= 0 ? $".{net}" : "")}";
            NetworkModCompatibilityHelper.networkModList = NetworkModCompatibilityHelper.networkModList.Append(text);
        }
        private static void AddModHash(String guid, (Int32 major, Int32 minor, Int32 build, Int32 rev, Int32 net) ver) => AddModHash(guid, ver.major, ver.minor, ver.build, ver.rev, ver.net);
        public static void AddModHash(String guid, String ver, Boolean useBuild = false, Boolean useRevision = false, Int32 networkVer = -1) => AddModHash(guid, ParseVersion(ver, useBuild, useRevision, networkVer));

        private static (Int32 major, Int32 minor, Int32 build, Int32 rev, Int32 network) ParseVersion(String text, Boolean useBuild, Boolean useRev, Int32 networkVer)
        {
            var split = text.Split('.').SelectWhere<String,Int32>(Int32.TryParse).GetEnumerator();
            Int32 a = split.MoveNext() ? split.Current : -1;
            Int32 b = split.MoveNext() ? split.Current : -1;
            Int32 c = split.MoveNext() && useBuild ? split.Current : -1;
            Int32 d = split.MoveNext() && useRev ? split.Current : -1;

            return (a, b, c, d, networkVer);
        }

        public static void Init(Boolean doNetChecks, Boolean debugLogs, Boolean infoLogs, Boolean messageLogs, Boolean warningLogs, Boolean errorLogs, Boolean fatalLogs)
        {
            if(!loaded)
            {
                throw new CoreNotLoadedException(nameof(ReinCore));
            }






            ReinCore.execLevel = 0;
            execLevel |= debugLogs ? ExecutionLevel.Debug : 0;
            execLevel |= infoLogs ? ExecutionLevel.Info : 0;
            execLevel |= messageLogs ? ExecutionLevel.Message : 0;
            execLevel |= warningLogs ? ExecutionLevel.Warning : 0;
            execLevel |= errorLogs ? ExecutionLevel.Error : 0;
            execLevel |= fatalLogs ? ExecutionLevel.Fatal : 0;

            if(doNetChecks)
            {
                if(NetworkCore.loaded)
                {
                    Log.Message(String.Format("{0} successfully loaded", nameof(NetworkCore)));
                } else
                {
                    Log.Error(String.Format("{0} failed to load, multiplayer may not work as intended.", nameof(NetworkCore)));
                }
            }

            Log.Message(String.Format("{0} successfully loaded", nameof(ReinCore)));
        }



        private static void NetworkModCompatibilityHelper_onUpdated()
        {
            foreach(var g in addedGuids)
            {
                if(!NetworkModCompatibilityHelper.networkModList.Contains(g))
                {
                    Log.Error($"Unexpected removal of mod: {g} from networked mod list.");
                    NetworkModCompatibilityHelper.networkModList = NetworkModCompatibilityHelper.networkModList.Append(g);
                }
            }
            
        }


        //private static readonly DetourModManager detourManager = new DetourModManager();
        static ReinCore()
        {
            RoR2Application.isModded = true;
            RoR2Application.onFixedUpdate += () => RoR2Application.isModded = true;
            HooksCore.RoR2.UnitySystemConsoleRedirector.Redirect.On += Redirect_On;
            ILHook.OnDetour = DelegateHelper.Combine(CheckSillyILHooks, ILHook.OnDetour);
            Detour.OnDetour = DelegateHelper.Combine(CheckSillyDetours, Detour.OnDetour);
            Hook.OnDetour = DelegateHelper.Combine(CheckSillyHooks, Hook.OnDetour);


            //HooksCore.RoR2.UI.QuickPlayButtonController.Start.On += Start_On;
            HooksCore.RoR2.DisableIfGameModded.OnEnable.On += OnEnable_On;
            //HooksCore.RoR2.Networking.ServerAuthManager.HandleSetClientAuth.Il += ServerAuthManager_HandleSetClientAuth;
            NetworkModCompatibilityHelper.onUpdated += NetworkModCompatibilityHelper_onUpdated;
            //HooksCore.RoR2.Networking.GameNetworkManager.SimpleLocalizedKickReason.GetDisplayTokenAndFormatParams.On += GetDisplayTokenAndFormatParams_On;
            RoR2Application.onNextUpdate += RoR2Application_onNextUpdate;
            _ = Tools.LoadAssembly(Rein.Properties.Resources.RoR2ScriptForwarding);
            if(!Log.loaded)
            {
                throw new CoreNotLoadedException(nameof(Log));
            }

            CheckPlugins();
            r2apiExists = pluginsByName.ContainsKey("com.bepis.r2api");

            loaded = true;
            managerObject = new GameObject("coremanager");
            MonoBehaviour.DontDestroyOnLoad(managerObject);
            _ = managerObject.AddComponent<CoreManager>();

            HooksCore.RoR2.SystemInitializerAttribute.Execute.On += Execute_On;


        }
        internal static void Execute_On(HooksCore.RoR2.SystemInitializerAttribute.Execute.Orig orig)
        {
            try { orig(); } finally { MetaCatalog.InitAllCatalogs(); }
        }

        private static Boolean CheckSillyILHooks(ILHook hook, MethodBase method, ILContext.Manipulator manip)
        {
            return method.DeclaringType != typeof(NetworkModCompatibilityHelper);
        }
        private static Boolean CheckSillyDetours(Detour hook, MethodBase method, MethodBase to)
        {
            return method.DeclaringType != typeof(NetworkModCompatibilityHelper);
        }
        private static Boolean CheckSillyHooks(Hook hook, MethodBase method, MethodBase to, Object _)
        {
            return method.DeclaringType != typeof(NetworkModCompatibilityHelper);
        }

        private static void RoR2Application_onNextUpdate()
        {
            Log.Message($"Modlist:{Environment.NewLine}{String.Join(Environment.NewLine, NetworkModCompatibilityHelper.networkModList)}");
            Log.Message($"ModHash:{NetworkModCompatibilityHelper.networkModHash}");
        }

        private static void GetDisplayTokenAndFormatParams_On(HooksCore.RoR2.Networking.GameNetworkManager.SimpleLocalizedKickReason.GetDisplayTokenAndFormatParams.Orig orig, GameNetworkManager.SimpleLocalizedKickReason self, out String token, out System.Object[] formatArgs)
        {
            var tok = self.baseToken;
            var args = self.formatArgs;
            token = tok;
            if(tok != "KICK_REASON_MOD_MISMATCH")
            {
                token = tok;
                formatArgs = args;
                return;
            }
            var mods = args[1].Split('\n');
            var myMods = NetworkModCompatibilityHelper.networkModList;

            var extraMods = String.Join("\n", myMods.Except(mods));
            var missingMods = String.Join("\n", mods.Except(myMods));

            formatArgs = new Object[] { extraMods, missingMods };
        }

        private static void ServerAuthManager_HandleSetClientAuth(ILContext il)
        {
            var cur = new ILCursor(il);
            if(cur.TryGotoNext(MoveType.AfterLabel,
                x => x.MatchNewobj(typeof(GameNetworkManager.ModMismatchKickReason).GetConstructor(new[] { typeof(IEnumerable<String>) })),
                x => x.MatchStloc(out _)))
            {
                static GameNetworkManager.SimpleLocalizedKickReason SwapToStandardMessage(GameNetworkManager.ModMismatchKickReason reason)
                {
                    reason.GetDisplayTokenAndFormatParams(out var token, out var _);
                    return new GameNetworkManager.SimpleLocalizedKickReason(token, new[] { "Coming Soon!", String.Join("\n", NetworkModCompatibilityHelper.networkModList) });
                }
                cur.Index++;
                _ = cur.EmitDelegate<Func<GameNetworkManager.ModMismatchKickReason, GameNetworkManager.SimpleLocalizedKickReason>>(SwapToStandardMessage);
            }
        }


        private static void OnEnable_On(HooksCore.RoR2.DisableIfGameModded.OnEnable.Orig orig, DisableIfGameModded self)
        {
            if(!self.name.Contains("Eclipse")) orig(self);
        }


        private static void Start_On(HooksCore.RoR2.UI.QuickPlayButtonController.Start.Orig orig, RoR2.UI.QuickPlayButtonController self)
        {
            //Log.Warning("QPButton");
            self.gameObject.SetActive(false);
            orig(self);
            self.gameObject.SetActive(false);
        }

        internal static ExecutionLevel execLevel;
        internal static Boolean r2apiExists;
        internal static R2APISubmodule activeSubmodules = R2APISubmodule.None;
        internal static event OnSubmoduleDataSuppliedDelegate onSubmoduleDataSupplied;
        internal delegate void OnSubmoduleDataSuppliedDelegate(R2APISubmodule activeSubmodules);

        internal static event Action awake;
        internal static event Action start;
        internal static event Action onEnable;
        internal static event Action onDisable;
        internal static event Action update;
        internal static event Action fixedUpdate;
        internal static event Action lateUpdate;
        internal static event Action destroy;


        private static readonly GameObject managerObject;
        private static readonly Dictionary<String,PluginInfo> pluginsByName = new Dictionary<String, PluginInfo>();

        private static void Redirect_On(HooksCore.RoR2.UnitySystemConsoleRedirector.Redirect.Orig orig)
        {
            // Do Nothing
        }

        
        private static void CheckPlugins()
        {
            foreach(KeyValuePair<String, PluginInfo> kv in BepInEx.Bootstrap.Chainloader.PluginInfos)
            {
                String k = kv.Key;
                PluginInfo v = kv.Value;
                if(String.IsNullOrEmpty(k) || v == null)
                {
                    continue;
                }

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
