using BepInEx;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using R2API;
using UnityEngine;

namespace GeneralPluginStuff
{
    internal static class HookFuncs
    {
        private static BindingFlags allFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic;
        internal static MethodBase GetMethodBaseRoot(Type type, String name, params Type[] paramTypes )
        {
            var method = type.GetMethod(name, allFlags, default, paramTypes, default );
            if( method == null )
            {
                throw new ArgumentException( "Did not find method" );
            }
            return MethodBase.GetMethodFromHandle( method.MethodHandle );
        }

        internal static MethodBase GetMethodBase<TDeclaring>(String name, params Type[] paramTypes )
        {
            return GetMethodBaseRoot( typeof(TDeclaring), name, paramTypes );
        }

        internal static MethodBase GetMethodBase<TDeclaring,TParam1>( String name, params Type[] paramTypes )
        {
            return GetMethodBase<TDeclaring>( name, typeof( TParam1 ) );
        }

        internal static MethodBase GetMethodBase<TDeclaring, TParam1, TParam2>( String name )
        {
            return GetMethodBase<TDeclaring>( name, typeof( TParam1 ), typeof( TParam2 ) );
        }

        internal static MethodBase GetMethodBase<TDeclaring, TParam1, TParam2, TParam3>( String name )
        {
            return GetMethodBase<TDeclaring>( name, typeof( TParam1 ), typeof( TParam2 ), typeof( TParam3 ) );
        }

        internal static MethodBase GetMethodBase<TDeclaring, TParam1, TParam2, TParam3, TParam4>( String name )
        {
            return GetMethodBase<TDeclaring>( name, typeof( TParam1 ), typeof( TParam2 ), typeof( TParam3 ), typeof( TParam4 ) );
        }
    }
}
