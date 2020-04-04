using BepInEx;
using BepInEx.Configuration;
using RoR2;
using UnityEngine;
using System;
using UnityEngine.Scripting;
using System.Collections;
using System.IO;

namespace ReinStutterStunter
{
    [BepInPlugin("com.ReinThings.ReinStutterStunter", "ReinStutterStunter", "2.0.0")]
    [BepInDependency( Rein.AssemblyLoad.guid, BepInDependency.DependencyFlags.HardDependency )]
    public class ReinStutterStunterMain : BaseUnityPlugin
    {
        private bool lockGCOn = false;
        private long memoryWarning = 1000;
        private long memoryCap = 2000;
        private const long div = 1048576;
        private float checkDelay = 30.0f;

        private string folderPath;
        private string logPath;
        private bool log = true;

        public static ConfigWrapper<Boolean> configWrappingPaper;
        public static ConfigWrapper<Int64> memWarning;
        public static ConfigWrapper<Int64> memCap;
        public static ConfigWrapper<Boolean> disableProcessing;
        public static ConfigWrapper<Single> memCheckDelay;

        public void Awake()
        {
            configWrappingPaper = base.Config.Wrap<Boolean>("Settings", "Enable Logging", "Should a csv file of memory usage be saved?", true);
            memCheckDelay = base.Config.Wrap<Single>("Settings", "Memory check delay", "How much time should there be between memory checks?", 30.0f);
            memWarning = base.Config.Wrap<Int64>("Settings", "Memory Warning threshold", "Only change this if you know what you are doing.", 3000);
            memCap = base.Config.Wrap<Int64>("Settings", "Memory use cap", "Only change this if you know what you are doing", 4000);

            log = configWrappingPaper.Value;
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
                do
                {
                    logPath = $"\\MemLog{i++}.csv";
                }
                while (File.Exists(folderPath + logPath));
                logPath = folderPath + logPath;
            }
        }
        public void Start()
        {
            ReinCore.HooksCore.RoR2.Stage.OnDisable.On += ( orig, self ) =>
            {
                orig( self );
                GC.Collect();
                this.EnableGC( false );
            };
            ReinCore.HooksCore.RoR2.Stage.OnEnable.On += ( orig, self ) =>
            {
                orig( self );
                this.DisableGC();
                GC.Collect();
            };
            ReinCore.HooksCore.RoR2.UI.PauseScreenController.OnEnable.On += ( orig, self ) =>
            {
                this.EnableGC( false );
                GC.Collect();
                orig( self );
            };
            ReinCore.HooksCore.RoR2.UI.PauseScreenController.OnDisable.On += ( orig, self ) =>
            {
                this.DisableGC();
                orig( self );
            };
            ReinCore.HooksCore.RoR2.Run.Awake.On += ( orig, self ) =>
            {
                this.AddALine( Time.fixedUnscaledTime.ToString() + ",Run awake" );
                orig( self );
            };
            base.StartCoroutine(this.MemoryMonitor());
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
