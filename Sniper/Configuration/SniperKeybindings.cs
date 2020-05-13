namespace Sniper.Configuration
{
    using System.Collections.Generic;

#pragma warning disable CA1812 // Avoid uninstantiated internal classes
    internal static class SniperKeybindings
#pragma warning restore CA1812 // Avoid uninstantiated internal classes
    {
#pragma warning disable IDE0052 // Remove unread private members
#pragma warning disable CA1823 // Avoid unused private fields
        private static readonly Dictionary<SniperAction,SniperKey> actionMap = new Dictionary<SniperAction, SniperKey>();
#pragma warning restore CA1823 // Avoid unused private fields
#pragma warning restore IDE0052 // Remove unread private members
#pragma warning disable IDE0052 // Remove unread private members
#pragma warning disable CA1823 // Avoid unused private fields
        private static readonly Dictionary<SniperKey,SniperAction> keyMap = new Dictionary<SniperKey, SniperAction>();
#pragma warning restore CA1823 // Avoid unused private fields
#pragma warning restore IDE0052 // Remove unread private members
#pragma warning disable IDE0052 // Remove unread private members
#pragma warning disable CA1823 // Avoid unused private fields
        private static readonly Dictionary<SniperKey,SniperKeyType> keyTypeMap = new Dictionary<SniperKey, SniperKeyType>();
#pragma warning restore CA1823 // Avoid unused private fields
#pragma warning restore IDE0052 // Remove unread private members
    }
}
