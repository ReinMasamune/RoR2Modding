namespace BetterModMismatchInfo
{
    using System;
    using System.Reflection;
    using System.Linq;
    using BF = System.Reflection.BindingFlags;

    using BepInEx;

    using MonoMod.RuntimeDetour;

    using RoR2;
    using RoR2.Networking;

    using Object = System.Object;
    using UnityObject = UnityEngine.Object;
    using System.Collections.Generic;
    using Mono.Security.X509.Extensions;
    using System.Diagnostics;
    using System.Text;

    internal static class KVPX
    {
        internal static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> self, out TKey key, out TValue value)
        {
            key = self.Key;
            value = self.Value;
        }
    }


    [BepInPlugin("Rein.BetterModMismatchInfo", "Better Mod Mismatch info", "1.0.0.0")]
    internal sealed class BetterModMismatchInfoPlugin : BaseUnityPlugin
    {
        static BetterModMismatchInfoPlugin() 
        {
            static String ParseToString(Byte[] bytes)
            {
                var builder = new StringBuilder();
                for(Int32 i = 0; i < bytes.Length; i += 2)
                {
                    builder = builder.Append(BitConverter.ToChar(bytes, i));
                }
                return builder.ToString();
            }

            var r = new Random();

            LinkedList<String> test1 = new();
            LinkedList<String> test2 = new();
            for(Int32 i = 0; i < 100; ++i)
            {
                var length1 = r.Next(30);
                var length2 = r.Next(100);
                var bytes1 = new Byte[length1 * 2];
                var bytes2 = new Byte[length2 * 2];
                r.NextBytes(bytes1);
                r.NextBytes(bytes2);

                var author = ParseToString(bytes1);
                var name = ParseToString(bytes2);
                var v1 = r.Next();
                var v2 = r.Next();
                var v3 = r.Next();
                var v4 = r.Next();
            }
        }

        private static String testMods1 = "Rein.GeneralFixes;2.4.3.x\nRein.RogueWisp;2.1.10.x\nRein.Sniper;1.0.8.x";
        private static String testMods2 = "com.bepis.r2api;0.0.1\ncom.iDeathHD.BadAssEngi;1.2.8\ncom.iDeathHD.DPSMeter;0.0.6\ncom.iDeathHD.Rampage;1.1.6\ncom.ldlework.multimod;1.0\ncom.rob.Aatrox;3.5.0\ndev.wildbook.libminirpc;1.0\nRein.RogueWisp.2.1.9.x\nRein.Sniper.1.0.7.x";



        private static BetterModMismatchInfoPlugin instance;
        BetterModMismatchInfoPlugin() => instance = this;
        private static readonly MethodInfo target = typeof(GameNetworkManager.SimpleLocalizedKickReason).GetMethod("GetDisplayTokenAndFormatParams", BF.Instance | BF.Public | BF.NonPublic);
        private delegate void Sig(GameNetworkManager.SimpleLocalizedKickReason self, out String token, out Object[] formatArgs);
        private static readonly MethodInfo hook = new Sig(GetDisplayTokenAndFormatParams_On).Method;
        private static readonly Hook onTokenStuff = new(target, hook);

        private static readonly MethodInfo target2 = typeof(UnitySystemConsoleRedirector).GetMethod("Redirect", BF.Public | BF.Static | BF.Instance | BF.NonPublic);
        private delegate void Sig2();
        private static readonly MethodInfo hook2 = new Sig2(No).Method;
        private static readonly Hook onRedirect = new(target2, hook2);
        protected void Start()
        {
            DoTests();
            foreach(var (_, plugin) in BepInEx.Bootstrap.Chainloader.PluginInfos)
            {
                if(plugin is not null && plugin.Instance is not null && plugin.Metadata.GUID == "___AssemblyLoader-com.Rein.Core" && plugin.Metadata.Version >= new Version("2.0.0.18")) return;
            }

            base.Logger.LogMessage($"Modlist:{Environment.NewLine}{String.Join(Environment.NewLine, NetworkModCompatibilityHelper.networkModList)}");
            base.Logger.LogMessage($"ModHash:{NetworkModCompatibilityHelper.networkModHash}");
        }

        private static void No() { }

        private static void GetDisplayTokenAndFormatParams_On(GameNetworkManager.SimpleLocalizedKickReason self, out String token, out Object[] formatArgs)
        {
            var timer = Stopwatch.StartNew();
            var tok = self.baseToken;
            var args = self.formatArgs;
            token = tok;
            if(tok != "KICK_REASON_MOD_MISMATCH")
            {
                token = tok;
                formatArgs = args;
                return;
            }
            var mods = new HashSet<String>(args[1].Split('\n'));
            var myMods = new HashSet<String>(NetworkModCompatibilityHelper.networkModList.ToArray());
            var correctMods = new HashSet<String>(mods);
            correctMods.IntersectWith(myMods);
            _ = mods.RemoveWhere(correctMods.Contains);
            _ = myMods.RemoveWhere(correctMods.Contains);
            var extraMods = String.Join("\n", myMods);
            var missingMods = String.Join("\n", mods);

            formatArgs = new Object[] { extraMods, missingMods };

            timer.Stop();
            instance.Logger.LogWarning(timer.ElapsedTicks);
        }


        private static void DoTests()
        {
            var timer1 = new Stopwatch();
            var timer2 = new Stopwatch();
            var a = testMods1.Split('\n');
            var b = testMods2.Split('\n');
            Test1(a, b, out var res1, null);
            Test2(a, b, out var res2, null);
            foreach(var v in res1) instance.Logger.LogWarning(v);
            foreach(var v in res2) instance.Logger.LogError(v);

            for(Int32 i = 0; i < 1000000; ++i)
            {
                Test1(a, b, out var _, timer1);
                Test2(a, b, out var _, timer2);
            }

            instance.Logger.LogMessage(timer1.ElapsedTicks);
            instance.Logger.LogMessage(timer2.ElapsedTicks); 
        }

        private static void Test1(String[] a, String[] b, out Object[] args, Stopwatch timer)
        {
            timer?.Start();
            var mods = new HashSet<String>(a);
            var myMods = new HashSet<String>(b);
            //var correctMods = new HashSet<String>(mods);
            mods.ExceptWith(b);
            myMods.ExceptWith(a);
            //correctMods.IntersectWith(myMods);
            //_ = mods.RemoveWhere(correctMods.Contains);
            //_ = myMods.RemoveWhere(correctMods.Contains);
            var extraMods = String.Join("\n", myMods);
            var missingMods = String.Join("\n", mods);

            args = new Object[] { extraMods, missingMods };
            timer?.Stop();
        }
        private static void Test2(String[] a, String[] b, out Object[] args, Stopwatch timer)
        {
            timer?.Start();
            //var mods = new HashSet<String>(a);
            //var myMods = new HashSet<String>(b);
            //var correctMods = new HashSet<String>(mods);
            //correctMods.IntersectWith(myMods);
            //_ = mods.RemoveWhere(correctMods.Contains);
            //_ = myMods.RemoveWhere(correctMods.Contains);
            var extraMods = String.Join("\n", b.Except(a));
            var missingMods = String.Join("\n", a.Except(b));

            args = new Object[] { extraMods, missingMods };
            timer?.Stop();
        }
    }
}
