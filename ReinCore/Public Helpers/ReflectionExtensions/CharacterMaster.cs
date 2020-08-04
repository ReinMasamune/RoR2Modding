namespace ReinCore
{
    using System;
    using System.Collections.Generic;

    using RoR2;

    public static class _CharacterMaster

    {
        //private static readonly Accessor<CharacterMaster, List<DeployableInfo>> _deployablesList = new Accessor<CharacterMaster, List<DeployableInfo>>( "deployablesList" );
        [Obsolete( "unneeded", true )]
        public static List<DeployableInfo> _GetDeployablesList( this CharacterMaster inst ) => inst.deployablesList;// _deployablesList.Get( inst );
        [Obsolete( "unneeded", true )]
        public static void _SetDeployablesList( this CharacterMaster inst, List<DeployableInfo> value ) => inst.deployablesList = value;// => _deployablesList.Set( inst, value );

    }
}
