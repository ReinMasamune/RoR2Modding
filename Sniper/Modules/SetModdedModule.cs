namespace Sniper.Modules
{
    internal static class SetModdedModule
    {
        internal static void SetModded() => RoR2.RoR2Application.onUpdate += () => RoR2.RoR2Application.isModded = true;
    }
}
