using BepInEx;
using BepInEx.Configuration;
using RoR2;
using UnityEngine;
using System;
using UnityEngine.Scripting;
using UnityEngine.SceneManagement;
using System.Collections;
using System.IO;

namespace ReinStutterStunter
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.ReinThings.ReinStutterStunter", "ReinStutterStunter", "1.0.2")]
    public class ReinStutterStunterMain : BaseUnityPlugin
    {
        private bool isGotoScary = false;

        private bool lockGCOn = false;
        private long memoryWarning = 3000;
        private long memoryCap = 4000;
        private const long div = 1048576;
        private const float checkDelay = 10.0f;

        private string folderPath;
        private string logPath;
        private bool log = true;

        public static ConfigWrapper<bool> configWrappingPaper;

        public void Awake()
        {
            configWrappingPaper = Config.Wrap<bool>("Settings", "Enable Logging", "Should a csv file of memory usage be saved?", true);

            log = configWrappingPaper.Value;

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
            //Action pauseStart = new Action(delegate ()
            //{
            //    EnableGC(false);
            //    GC.Collect();
            //});
            //Action pauseEnd = new Action(delegate ()
            //{
            //    DisableGC();
            //});
            GarbageCollector.GCModeChanged += (GarbageCollector.Mode mode) =>
            {
                switch( mode )
                {
                    case GarbageCollector.Mode.Enabled:
                        //Chat("Garbage Collection enabled");
                        break;
                    case GarbageCollector.Mode.Disabled:
                        //Chat.AddMessage("Garbage Collection disabled");
                        break;
                    default:
                        Chat.AddMessage("Something went very wrong with garbage collection");
                        Chat.AddMessage("I strongly recommend you exit the game immediately.");
                        break;
                }
            };
            SceneManager.sceneUnloaded += (Scene scene) =>
            {
                Chat.AddMessage("Scene unload");
                EnableGC(false);
                GC.Collect();
            };
            //RoR2Application.onPauseStartGlobal = (Action)Delegate.Combine(RoR2Application.onPauseStartGlobal, pauseStart);
            //RoR2Application.onPauseEndGlobal = (Action)Delegate.Combine(RoR2Application.onPauseEndGlobal, pauseEnd);
            On.RoR2.Stage.Start += (orig, self) =>
            {
                Chat.AddMessage("Stage Start");
                orig(self);
                GC.Collect();
                DisableGC();
            };
            On.RoR2.RoR2Application.CCQuit += (orig, self) =>
            {
                EnableGC(true);
                GC.Collect();
                orig(self);
            };
            On.RoR2.UI.PauseScreenController.OnEnable += (orig, self) =>
            {
                EnableGC(false);
                GC.Collect();
                orig(self);
            };
            On.RoR2.UI.PauseScreenController.OnDisable += (orig, self) =>
            {
                DisableGC();
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
            AddALine(Time.fixedUnscaledTime.ToString() + ",Disabling GC");
            if( lockGCOn )
            {
                AddALine(Time.fixedUnscaledTime.ToString() + ",GC is locked to on. Restart the game to change");
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
                AddALine(Time.fixedUnscaledTime.ToString() + "," + curMem.ToString());

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
