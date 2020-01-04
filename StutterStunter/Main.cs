using BepInEx;
using BepInEx.Configuration;
using R2API.Utils;
using RoR2;
using UnityEngine;
using System;
using UnityEngine.Scripting;
using UnityEngine.SceneManagement;
using System.Collections;
using System.IO;
using UnityEngine.Rendering.PostProcessing;

namespace ReinStutterStunter
{
    [BepInDependency(R2API.R2API.PluginGUID, BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin("com.ReinThings.ReinStutterStunter", "ReinStutterStunter", "1.0.4")]
    public class ReinStutterStunterMain : BaseUnityPlugin
    {
        private bool isGotoScary = false;

        private bool lockGCOn = false;
        private long memoryWarning = 1000;
        private long memoryCap = 2000;
        private const long div = 1048576;
        private float checkDelay = 30.0f;

        private string folderPath;
        private string logPath;
        private bool log = true;
        private bool disableProcess = true;

        public static ConfigWrapper<bool> configWrappingPaper;
        public static ConfigWrapper<long> memWarning;
        public static ConfigWrapper<long> memCap;
        public static ConfigWrapper<bool> disableProcessing;
        public static ConfigWrapper<float> memCheckDelay;

        public void Awake()
        {
            configWrappingPaper = Config.Wrap<bool>("Settings", "Enable Logging", "Should a csv file of memory usage be saved?", true);
            memCheckDelay = Config.Wrap<float>("Settings", "Memory check delay", "How much time should there be between memory checks?", 30.0f);
            disableProcessing = Config.Wrap<bool>("Settings", "Disable post processing", "Only use this if you know what you're doing", false);
            memWarning = Config.Wrap<long>("Settings", "Memory Warning threshold", "Only change this if you know what you are doing.", 3000);
            memCap = Config.Wrap<long>("Settings", "Memory use cap", "Only change this if you know what you are doing", 4000);

            log = configWrappingPaper.Value;
            disableProcess = disableProcessing.Value;
            memoryWarning = memWarning.Value;
            memoryCap = memCap.Value;
            checkDelay = memCheckDelay.Value;

            if (log)
            {
                folderPath = Environment.CurrentDirectory + "\\MemoryLog";
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                int i = 1;
                if (isGotoScary)
                {
                    do
                    {
                        logPath = $"\\MemLog{i++}.csv";
                    }
                    while (File.Exists(folderPath + logPath));
                }
                else
                {
                CheckAgain:
                    logPath = $"\\MemLog{i++}.csv";
                    if (File.Exists(folderPath + logPath)) goto CheckAgain;
                }

                logPath = folderPath + logPath;
            }
        }
        public void Start()
        {
            On.RoR2.Stage.OnDisable += ( orig, self ) =>
            {
                orig( self );
                GC.Collect();
                EnableGC( false );
            };
            On.RoR2.Stage.OnEnable += ( orig, self ) =>
            {
                orig( self );
                DisableGC();
                GC.Collect();
            };
            On.RoR2.UI.PauseScreenController.OnEnable += (orig, self) =>
            {
                EnableGC(false);
                GC.Collect();
                orig(self);
            };
            On.RoR2.UI.PauseScreenController.OnDisable += (orig, self) =>
            {
                this.DisableGC();
                orig(self);
            };
            On.RoR2.Run.Awake += (orig, self) =>
            {
                AddALine(Time.fixedUnscaledTime.ToString() + ",Run awake");
                orig(self);
            };
            StartCoroutine(MemoryMonitor());
        }
        private void DisableGC()
        {
            this.AddALine(Time.fixedUnscaledTime.ToString() + ",Disabling GC");
            if( this.lockGCOn )
            {
                this.AddALine(Time.fixedUnscaledTime.ToString() + ",GC is locked to on. Restart the game to change");
                Debug.Log("GC is locked on. Restart the game to change");
            }
            else
            {
                GarbageCollector.GCMode = GarbageCollector.Mode.Disabled;
            }
        }
        private void EnableGC(bool perma)
        {
            AddALine(Time.fixedUnscaledTime.ToString() + ",Enabling GC");
            GarbageCollector.GCMode = GarbageCollector.Mode.Enabled;
            if( perma )
            {
                lockGCOn = true;
            }
        }
        private IEnumerator MemoryMonitor()
        {
            long peakMem = 0;
            long curMem = 0;
            string warning = "Memory used is past the warning threshold, please pause the game for a few seconds to clean up.";
            string cleanupMessage = "Memory past cap, performing GC in 5 seconds.";
            string cleanupFinish = "GC is complete.";
            string cleanupAbort = "GC aborted because memory is no longer past cap.";
            while (true)
            {
                curMem = GC.GetTotalMemory(true);
                peakMem = Math.Max(curMem, peakMem);
                AddALine(Time.realtimeSinceStartup.ToString() + "," + curMem.ToString());

                if ((curMem / div) > memoryWarning)
                {
                    Chat.AddMessage(warning);
                }
                if( (curMem / div) > memoryCap )
                {
                    StartCoroutine(MemoryForceGC(cleanupMessage, cleanupFinish, cleanupAbort));
                }

                yield return new WaitForSecondsRealtime(checkDelay);
            }
        }
        private IEnumerator MemoryForceGC(string s, string s2, string s3)
        {
            Chat.AddMessage(s);
            yield return new WaitForSecondsRealtime(5f);
            if ((GC.GetTotalMemory(true) / div) > memoryCap)
            {
                EnableGC(false);
                GC.Collect();
                yield return new WaitForSecondsRealtime(10f);
                Chat.AddMessage(s2);
                DisableGC();
            }
            else
            {
                Chat.AddMessage(s3);
            }
        }
        private void AddALine( string s )
        {
            if (log)
            {
                File.AppendAllText(logPath, s + Environment.NewLine);
            }
        }
    }
}
