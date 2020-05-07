using System;
using System.Linq.Expressions;
using System.Reflection;
using BepInEx;
using RoR2;
using UnityEngine;

namespace ReinCore
{
    public static class _NetworkStateMachine
    {
        private static Accessor<NetworkStateMachine,EntityStateMachine[]> _stateMachines = new Accessor<NetworkStateMachine, EntityStateMachine[]>( "stateMachines" );

        public static EntityStateMachine[] _GetStateMachines( this NetworkStateMachine inst )
        {
            return _stateMachines.Get( inst );
        }

        public static void _SetStateMachines( this NetworkStateMachine inst, params EntityStateMachine[] stateMachines )
        {
            _stateMachines.Set( inst, stateMachines );
        }
    }
}
