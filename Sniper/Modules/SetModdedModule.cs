namespace Sniper.Modules
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using BepInEx.Logging;
    using ReinCore;

    internal static class SetModdedModule
    {
        internal static void SetModded() => RoR2.RoR2Application.onUpdate += () => RoR2.RoR2Application.isModded = true;
    }
}
