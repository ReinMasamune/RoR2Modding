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

        partial void Prefab();
        partial void Hooks();
        partial void Model();
        partial void Buffs();
        partial void Skills();
        partial void Materials();
        partial void Effects();
        partial void Projectiles();

        protected override void Init()
        {
            instance = this;
            log = base.logger;
            this.Prefab();
            this.Hooks();
            this.Model();
            this.Buffs();
            this.Skills();
            this.Materials();
            this.Effects();
            this.Projectiles();
        }

        protected override void Fail()
        {
            base.Fail();
        }

    }
}


// TODO: Passive selection