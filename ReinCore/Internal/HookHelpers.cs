using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using BepInEx;
using MonoMod.Cil;
using UnityEngine;

namespace ReinCore
{
    internal static class HookableHelpers
    {
        internal static MethodBase GetBase( Type self, Boolean useArgs = false, CallingConventions callInfo = CallingConventions.Standard, params Type[] arguments )
        {
            var name = self.FullName;

            var method = self.Name;
            if( method.Contains( "___" ) )
            {
                method = methodName.Match( method ).Groups[1].Value;
            }
            var typeNameBuilder = new StringBuilder();
            var curType = self.DeclaringType;
            var loop = true;
            while( loop )
            {
                var nextType = curType.DeclaringType;
                var s = "";
                if( nextType != null && !typeMask.Contains(nextType) )
                {
                    s = nextType.IsClass ? "+" : ".";
                } else
                {
                    loop = false;
                }

                typeNameBuilder.Insert( 0, String.Format( "{0}{1}", s, curType.Name ) );
                curType = nextType;
            }
            var typeName = typeNameBuilder.ToString();


            var type = ror2Assembly.GetType(typeName, true, false );
            var methodInfo = useArgs ? type.GetMethod(method, allFlags, null, callInfo, arguments, null ) : type.GetMethod( method, allFlags );
            if( methodInfo == null ) Log.Error( String.Format( "Could not find method: {0} in type {1}", method, typeName ) );
            return methodInfo;
        }

        private static BindingFlags allFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic;
        private static Assembly ror2Assembly = typeof(RoR2.RoR2Application).Assembly;
        private static Regex methodName = new Regex( @"^(.*)___", RegexOptions.Compiled );
        private static HashSet<Type> typeMask = new HashSet<Type>( new[]
        {
            typeof(HooksCore),
        });
    }


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
