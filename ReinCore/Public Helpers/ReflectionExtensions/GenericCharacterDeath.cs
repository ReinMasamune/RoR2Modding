namespace ReinCore
{
    using System;

    using EntityStates;

    public static class _GenericCharacterDeath
    {
        //private static readonly Accessor<GenericCharacterDeath,Single> _restStopwatch = new Accessor<GenericCharacterDeath, Single>( "restStopwatch" );
        //private static readonly Accessor<GenericCharacterDeath,Single> _fallingStopwatch = new Accessor<GenericCharacterDeath, Single>( "fallingStopwatch" );


        public static Single _GetRestStopwatch<TInstance>( this TInstance inst ) where TInstance : GenericCharacterDeath => inst.restStopwatch;// _restStopwatch.Get( inst );

        public static void _SetRestStopwatch<TInstance>( this TInstance inst, Single value ) where TInstance : GenericCharacterDeath => inst.restStopwatch = value;// => _restStopwatch.Set( inst, value );

        public static Single _GetFallingStopwatch<TInstance>( this TInstance inst ) where TInstance : GenericCharacterDeath => inst.fallingStopwatch;// _fallingStopwatch.Get( inst );

        public static void _SetFallingStopwatch<TInstance>( this TInstance inst, Single value ) where TInstance : GenericCharacterDeath => inst.fallingStopwatch = value;// => _fallingStopwatch.Set( inst, value );
    }
}
