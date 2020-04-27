using System;
using BepInEx;
using RoR2;

namespace ReinCore
{
    /// <summary>
    /// For adding soundbanks
    /// </summary>
    public static class SoundsCore
    {
        /// <summary>
        /// Is this module loaded?
        /// </summary>
        public static Boolean loaded { get; internal set; } = false;
        





        static SoundsCore()
        {






            loaded = true;
        }
    }
}
