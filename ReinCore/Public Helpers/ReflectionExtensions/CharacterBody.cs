namespace ReinCore
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;
    using BepInEx;
    using RoR2;
    using UnityEngine;
    using UnityEngine.Networking;

    /// <summary>
    /// 
    /// </summary>
#pragma warning disable IDE1006 // Naming Styles
    public static class _CharacterBody
#pragma warning restore IDE1006 // Naming Styles
    {
        private delegate void SetBuffCountDelegate( CharacterBody body, BuffIndex buff, Int32 count );
#pragma warning disable IDE1006 // Naming Styles
        private static readonly SetBuffCountDelegate _setBuffCount;
#pragma warning restore IDE1006 // Naming Styles

        static _CharacterBody()
        {
            ParameterExpression instParam = Expression.Parameter( typeof(CharacterBody), "instance" );
            ParameterExpression buffParam = Expression.Parameter( typeof(BuffIndex), "buff" );
            ParameterExpression countParam = Expression.Parameter( typeof(Int32), "count" );

            MethodInfo method = typeof(CharacterBody).GetMethod( "SetBuffCount", BindingFlags.NonPublic | BindingFlags.Instance );
            MethodCallExpression body = Expression.Call( instParam, method, buffParam, countParam );

            _setBuffCount = Expression.Lambda<SetBuffCountDelegate>( body, instParam, buffParam, countParam ).Compile();
        }


#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable IDE1006 // Naming Styles
        public static void _SetBuffCount( this CharacterBody body, BuffIndex buff, Int32 count )
#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            if( NetworkServer.active )
            {
                _setBuffCount( body, buff, count );
            }
        }
    }
}
