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
    internal static class HookHelpers
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
}
