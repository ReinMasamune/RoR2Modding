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
//using static RogueWispPlugin.Helpers.APIInterface;

namespace RogueWispPlugin
{
#if ROGUEWISP
    internal partial class Main
    {
        partial void RW_Orb()
        {
            this.Load += this.RW_AddOrbs;
        }

        private void RW_AddOrbs()
        {
            R2API.OrbAPI.AddOrb( typeof( RestoreOrb ) );
            R2API.OrbAPI.AddOrb( typeof( SnapOrb ) );
            R2API.OrbAPI.AddOrb( typeof( SparkOrb ) );
            R2API.OrbAPI.AddOrb( typeof( BlazeOrb ) );
            R2API.OrbAPI.AddOrb( typeof( IgnitionOrb ) );
        }
    }
#endif
}
