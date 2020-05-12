namespace ReinCore
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;
    using BepInEx;
    using RoR2;
    using UnityEngine;

    /// <summary>
    /// 
    /// </summary>
#pragma warning disable IDE1006 // Naming Styles
    public static class _CharacterMaster
#pragma warning restore IDE1006 // Naming Styles
    {
#pragma warning disable IDE1006 // Naming Styles
        private static readonly Accessor<CharacterMaster, List<DeployableInfo>> _deployablesList = new Accessor<CharacterMaster, List<DeployableInfo>>( "deployablesList" );
#pragma warning restore IDE1006 // Naming Styles

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable IDE1006 // Naming Styles
        public static List<DeployableInfo> _GetDeployablesList( this CharacterMaster inst ) => _deployablesList.Get( inst );
#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable IDE1006 // Naming Styles
        public static void _SetDeployablesList( this CharacterMaster inst, List<DeployableInfo> value ) => _deployablesList.Set( inst, value );
#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
