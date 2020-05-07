using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using BepInEx;
using RoR2;
using UnityEngine;

namespace ReinCore
{
    /// <summary>
    /// 
    /// </summary>
    public static class _CharacterMaster
    {
        private static Accessor<CharacterMaster, List<DeployableInfo>> _deployablesList = new Accessor<CharacterMaster, List<DeployableInfo>>( "deployablesList" );

        public static List<DeployableInfo> _GetDeployablesList( this CharacterMaster inst )
        {
            return _deployablesList.Get( inst );
        }
        public static void _SetDeployablesList( this CharacterMaster inst, List<DeployableInfo> value )
        {
            _deployablesList.Set( inst, value );
        }
    }
}
