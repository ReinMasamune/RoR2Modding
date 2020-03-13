using System;
using System.Reflection;
using BepInEx;
using RoR2;

namespace ReinCore
{
    public static class SkinsCore
    {
        public static Boolean loaded { get; internal set; } = false;



        static SkinsCore()
        {
            var allFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
            var parentType = typeof(Loadout.BodyLoadoutManager);

            if( !ReinCore.r2apiExists )
            {

            }
        }

    }
}
