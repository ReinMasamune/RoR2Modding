namespace ReinCore
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using BepInEx;
    using RoR2;
    using UnityEngine;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable IDE1006 // Naming Styles
    public static class _NetworkStateMachine
#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
#pragma warning disable IDE1006 // Naming Styles
        private static readonly Accessor<NetworkStateMachine,EntityStateMachine[]> _stateMachines = new Accessor<NetworkStateMachine, EntityStateMachine[]>( "stateMachines" );
#pragma warning restore IDE1006 // Naming Styles

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable IDE1006 // Naming Styles
        public static EntityStateMachine[] _GetStateMachines( this NetworkStateMachine inst ) => _stateMachines.Get( inst );
#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable IDE1006 // Naming Styles
        public static void _SetStateMachines( this NetworkStateMachine inst, params EntityStateMachine[] stateMachines ) => _stateMachines.Set( inst, stateMachines );
#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
