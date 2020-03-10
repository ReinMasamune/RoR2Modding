using System;
using System.Reflection;
using BepInEx;
using UnityEngine;

namespace ReinCore
{
    internal static class HookHelpers
    {
        private static BindingFlags allFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic;
        internal static Type GetNestedType( Type type, String name )
        {
            var members = type.GetMember(name, MemberTypes.NestedType, allFlags );
            if( members.Length == 1 )
            {
                return (members[0] as TypeInfo).AsType();
            } else if( members.Length == 0 )
            {
                throw new MissingMemberException( type.AssemblyQualifiedName, name );
            } else
            {
                throw new Exception( "Multiple matching types found" );
            }
        }

        internal static MethodBase GetMethodBaseRoot( Type type, String name, params Type[] paramTypes )
        {
            var method = type.GetMethod(name, allFlags, default, paramTypes, default );

            if( method == null )
            {
                throw new MissingMethodException(type.AssemblyQualifiedName, name);
            }
            if( method.IsGenericMethod ) throw new ArgumentException( "Cannot use on generic types or methods" );
            return MethodBase.GetMethodFromHandle( method.MethodHandle );
        }

        internal static MethodBase GetMethodBase<TDeclaring>( String name, params Type[] paramTypes )
        {
            return GetMethodBaseRoot( typeof( TDeclaring ), name, paramTypes );
        }

        internal static MethodBase GetMethodBase<TDeclaring, TParam1>( String name )
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

        internal static MethodBase GetGenericMethodBaseRoot<TGeneric1>( Type type, String name, params Type[] paramTypes )
        {
            var method = type.GetMethod( name, allFlags, default, paramTypes, default );
            if( method == null ) throw new MissingMethodException( type.AssemblyQualifiedName, name );
            if( !method.IsGenericMethod ) throw new ArgumentException( "Only works on generic methods" );
            method = method.MakeGenericMethod( typeof( TGeneric1 ) );
            return MethodBase.GetMethodFromHandle( method.MethodHandle );
        }
    }
}
