namespace ReinCore
{
    using System;

    using RoR2;


    public static class _NetworkStateMachine

    {

        //private static readonly Accessor<NetworkStateMachine,EntityStateMachine[]> _stateMachines = new Accessor<NetworkStateMachine, EntityStateMachine[]>( "stateMachines" );


        [Obsolete( "unneeded", true )]
        public static EntityStateMachine[] _GetStateMachines( this NetworkStateMachine inst ) => inst.stateMachines;// _stateMachines.Get( inst );
        [Obsolete( "unneeded", true )]
        public static void _SetStateMachines( this NetworkStateMachine inst, params EntityStateMachine[] stateMachines ) => inst.stateMachines = stateMachines; // => _stateMachines.Set( inst, stateMachines );
    }
}
