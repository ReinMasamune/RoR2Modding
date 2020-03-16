using System;
using System.Collections.Generic;
using System.Text;
using BepInEx;
using ReinCore;

namespace Rein.AlternateArtificer
{
    [BepInDependency( AssemblyLoad.guid )]
    [BepInPlugin( Main.guid, Main.name, Main.ver )]
    internal partial class Main : CorePlugin
    {
        public const String guid = "com.Rein.AltArti";
        public const String name = "Alternate Artificer";

        internal static Main instance;
        private static BepInEx.Logging.ManualLogSource log;

        partial void GetBody();
        partial void GetComponents();
        partial void AddComponents();
        partial void RemoveComponents();
        partial void SetupHooks();



        protected override void Init()
        {
            instance = this;
            log = base.logger;
            this.GetBody();
            this.RemoveComponents();
            this.GetComponents();
            this.AddComponents();
        }

        protected override void Fail()
        {
            base.Fail();

        }

    }
}
