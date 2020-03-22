using System;
using BepInEx;
using RoR2;

namespace ReinCore
{
    public static class MeshCore
    {
        public static Boolean loaded { get; internal set; } = false;






        static MeshCore()
        {





            loaded = true;
        }
    }
}
