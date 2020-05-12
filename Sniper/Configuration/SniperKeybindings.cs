namespace Sniper.Configuration
{
    using System.Collections.Generic;

    internal static class SniperKeybindings
    {
        private static readonly Dictionary<SniperAction,SniperKey> actionMap = new Dictionary<SniperAction, SniperKey>();
        private static readonly Dictionary<SniperKey,SniperAction> keyMap = new Dictionary<SniperKey, SniperAction>();
        private static readonly Dictionary<SniperKey,SniperKeyType> keyTypeMap = new Dictionary<SniperKey, SniperKeyType>();
    }
}
