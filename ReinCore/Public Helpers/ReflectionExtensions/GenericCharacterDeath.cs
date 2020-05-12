namespace ReinCore
{
    using System;

    using EntityStates;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable IDE1006 // Naming Styles
    public static class _GenericCharacterDeath
#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
#pragma warning disable IDE1006 // Naming Styles
        private static readonly Accessor<GenericCharacterDeath,Single> _restStopwatch = new Accessor<GenericCharacterDeath, Single>( "restStopwatch" );
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
        private static readonly Accessor<GenericCharacterDeath,Single> _fallingStopwatch = new Accessor<GenericCharacterDeath, Single>( "fallingStopwatch" );
#pragma warning restore IDE1006 // Naming Styles

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable IDE1006 // Naming Styles
        public static Single _GetRestStopwatch<TInstance>( this TInstance inst ) where TInstance : GenericCharacterDeath => _restStopwatch.Get( inst );
#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable IDE1006 // Naming Styles
        public static void _SetRestStopwatch<TInstance>( this TInstance inst, Single value ) where TInstance : GenericCharacterDeath => _restStopwatch.Set( inst, value );
#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable IDE1006 // Naming Styles
        public static Single _GetFallingStopwatch<TInstance>( this TInstance inst ) where TInstance : GenericCharacterDeath => _fallingStopwatch.Get( inst );
#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable IDE1006 // Naming Styles
        public static void _SetFallingStopwatch<TInstance>( this TInstance inst, Single value ) where TInstance : GenericCharacterDeath => _fallingStopwatch.Set( inst, value );
#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
