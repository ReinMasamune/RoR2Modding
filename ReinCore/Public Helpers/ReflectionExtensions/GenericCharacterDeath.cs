using System;
using System.Linq.Expressions;
using System.Reflection;
using BepInEx;
using EntityStates;
using RoR2;
using UnityEngine;

namespace ReinCore
{
    public static class _GenericCharacterDeath
    {
        private static Accessor<GenericCharacterDeath,Single> _restStopwatch = new Accessor<GenericCharacterDeath, Single>( "restStopwatch" );
        private static Accessor<GenericCharacterDeath,Single> _fallingStopwatch = new Accessor<GenericCharacterDeath, Single>( "fallingStopwatch" );

        public static Single _GetRestStopwatch<TInstance>( this TInstance inst ) where TInstance : GenericCharacterDeath
        {
            return _restStopwatch.Get( inst );
        }
        public static void _SetRestStopwatch<TInstance>( this TInstance inst, Single value ) where TInstance : GenericCharacterDeath
        {
            _restStopwatch.Set( inst, value );
        }

        public static Single _GetFallingStopwatch<TInstance>( this TInstance inst ) where TInstance : GenericCharacterDeath
        {
            return _fallingStopwatch.Get( inst );
        }
        public static void _SetFallingStopwatch<TInstance>( this TInstance inst, Single value ) where TInstance : GenericCharacterDeath
        {
            _fallingStopwatch.Set( inst, value );
        }
    }
}
