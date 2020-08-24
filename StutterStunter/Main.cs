namespace ReinStutterStunter
{
    using System;
    using System.Reflection;
    using BF = System.Reflection.BindingFlags;

    using BepInEx;

    using RoR2;

    using UnityEngine;
    using UnityEngine.Scripting;

    using Object = System.Object;
    using UnityObject = UnityEngine.Object;
    using UnityEngine.SceneManagement;
    using System.Collections;
    using MonoMod.RuntimeDetour;
    using BepInEx.Configuration;

    [BepInPlugin("Rein.StutterStunter", "Stutter Stunter", "2.1.0")]
    internal class ReinStutterStunterMain : BaseUnityPlugin
    {
        private static ConfigEntry<UInt16> autoGCAt;
        private static ConfigEntry<UInt16> warnGCAt;
        private static ConfigEntry<Byte> secondsBetweenChecks;

        private static TDelegate Combine<TDelegate>(params TDelegate[] delegates)
            where TDelegate : Delegate => (TDelegate)Delegate.Combine(delegates);

        static ReinStutterStunterMain() { }

        private static ReinStutterStunterMain instance;
        ReinStutterStunterMain()
        {
            instance = this;
            autoGCAt = base.Config.Bind<UInt16>("Settings", "Max memory usage (MB)", (UInt16)3000u, "The threshold at which GC will be automatically run, in MB.");
            warnGCAt = base.Config.Bind<UInt16>("Settings", "Memory usage warning (MB)", (UInt16)2000u, "The threshold at you will get a warning about memory usage and be asked to pause the game, in MB.");
            secondsBetweenChecks = base.Config.Bind<Byte>("Settings", "Scan interval (s)", (Byte)60u, "The number of seconds between memory usage checks. Memory usage checks aren't totally free, so this shouldn't usually be changed.");
        }
        protected void OnEnable()
        {
            GarbageCollector.GCModeChanged += GarbageCollector_GCModeChanged;
            PauseManager.onPauseStartGlobal = Combine(PauseManager.onPauseStartGlobal, OnPauseStartGlobal);
            PauseManager.onPauseEndGlobal = Combine(PauseManager.onPauseEndGlobal, OnPauseEndGlobal);
            RoR2Application.onShutDown = Combine(RoR2Application.onShutDown, OnShutDown);
            SceneManager.activeSceneChanged += this.SceneManager_activeSceneChanged;
            _ = base.StartCoroutine(TrackMemory());
        }

        private static readonly MethodInfo target2 = typeof(UnitySystemConsoleRedirector).GetMethod("Redirect", BF.Public | BF.Static | BF.Instance | BF.NonPublic);
        private delegate void Sig2();
        private static readonly MethodInfo hook2 = new Sig2(No).Method;
        private static readonly Hook onRedirect = new(target2, hook2);
        private static void No() { }

        private static IEnumerator TrackMemory()
        {
            while(true)
            {
                yield return new WaitForSecondsRealtime(secondsBetweenChecks.Value);
                var mem = GC.GetTotalMemory(false) / 1048576.0;
                instance.Logger.LogMessage($"Memory used: {mem}");
                switch(mem)
                {
                    case var _ when mem >= autoGCAt.Value:
                        GC.Collect();
                        Chat.AddMessage("Garbage collection forced");
                        continue;
                    case var _ when mem >= warnGCAt.Value:
                        Chat.AddMessage("Memory usage is beyond warning threshold. Please find time to pause for a few seconds.");
                        continue;
                    default:
                        continue;
                }
            }
        }

        private static Boolean IsMenuScene(Scene scene) => SceneCatalog.GetSceneDefFromScene(scene).sceneType switch
        {
            SceneType.Cutscene => true,
            SceneType.Menu => true,
            SceneType.Invalid => true,
            SceneType.Stage => false,
            SceneType.Intermission => false,
            _ => true,
        };

        private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
        {
            //instance.Logger.LogWarning("ActiveSceneChanged");
            GC.Collect();
            gc = isInMenuScene = IsMenuScene(arg1);
        }
        private static void OnPauseStartGlobal()
        {
            //instance.Logger.LogWarning("PauseStart");
            gc = true;
            GC.Collect();
        }
        private static void OnPauseEndGlobal()
        {
            //instance.Logger.LogWarning("PauseEnd");
            GC.Collect();
            gc = isInMenuScene;
        }
        private static void OnShutDown()
        {
            //instance.Logger.LogWarning("ShutDown");
            gcLocked = gc = true;
        }

        private static void GarbageCollector_GCModeChanged(GarbageCollector.Mode obj)
        {
            switch(obj)
            {
                case GarbageCollector.Mode.Disabled:
                    instance.Logger.LogMessage("GC disabled");
                    return;
                case GarbageCollector.Mode.Enabled:
                    instance.Logger.LogMessage("GC enabled");
                    return;
                default:
                    instance.Logger.LogFatal("Out of range GC mode. What the hell are you even doing?");
                    gc = true;
                    gcLocked = true;
                    return;
            }
        }

        private static Boolean gcLocked = false;
        private static Boolean isInMenuScene = true;
        private static Boolean gc
        {
            get => GarbageCollector.GCMode == GarbageCollector.Mode.Enabled;
            set
            {
                if(value == gc) return;
                if(gcLocked) return;
                GarbageCollector.GCMode = value ? GarbageCollector.Mode.Enabled : GarbageCollector.Mode.Disabled;
            }
        }
    }
}
