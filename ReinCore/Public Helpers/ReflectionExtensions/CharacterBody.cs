using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using BepInEx;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace ReinCore
{
    /// <summary>
    /// 
    /// </summary>
    public static class _CharacterBody
    {
        private delegate void SetBuffCountDelegate( CharacterBody body, BuffIndex buff, Int32 count );
        private static SetBuffCountDelegate _setBuffCount;

        static _CharacterBody()
        {
            var instParam = Expression.Parameter( typeof(CharacterBody), "instance" );
            var buffParam = Expression.Parameter( typeof(BuffIndex), "buff" );
            var countParam = Expression.Parameter( typeof(Int32), "count" );

            var method = typeof(CharacterBody).GetMethod( "SetBuffCount", BindingFlags.NonPublic | BindingFlags.Instance );
            var body = Expression.Call( instParam, method, buffParam, countParam );

            _setBuffCount = Expression.Lambda<SetBuffCountDelegate>( body, instParam, buffParam, countParam ).Compile();
        }


        public static void _SetBuffCount( this CharacterBody body, BuffIndex buff, Int32 count )
        {
            if( NetworkServer.active ) _setBuffCount( body, buff, count );
        }
    }
}
