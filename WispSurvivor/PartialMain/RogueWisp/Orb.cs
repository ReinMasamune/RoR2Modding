using BepInEx;
using R2API.Utils;
using RoR2;
using RoR2.Networking;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using RogueWispPlugin.Helpers;
using RogueWispPlugin.Modules;
using static RogueWispPlugin.Helpers.APIInterface;

namespace RogueWispPlugin
{
    internal partial class Main
    {
        partial void RW_Orb()
        {
            this.Load += this.RW_AddOrbs;
        }

        private void RW_AddOrbs()
        {
            AddOrb( typeof( Orbs.RestoreOrb ) );
            AddOrb( typeof( Orbs.SnapOrb ) );
            AddOrb( typeof( Orbs.SparkOrb ) );
            AddOrb( typeof( Orbs.BlazeOrb ) );
            AddOrb( typeof( Orbs.IgnitionOrb ) );
        }
    }

}
