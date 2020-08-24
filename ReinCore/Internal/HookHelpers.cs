namespace ReinCore
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.InteropServices.ComTypes;
    using System.Text;
    using System.Text.RegularExpressions;

    using MonoMod.Utils;

    using RoR2.UI;

    internal static class HookHelpers
    {
        internal static MethodBase GetBase( Type self, Boolean useArgs = false, CallingConventions callInfo = CallingConventions.Standard, params Type[] arguments )
        {
            //String name = self.FullName;

            String method = self.Name;
            if( method.Contains( "___" ) )
            {
                method = methodName.Match( method ).Groups[1].Value;
            }
            var typeNameBuilder = new StringBuilder();
            Type curType = self.DeclaringType;
            Boolean loop = true;
            while( loop )
            {
                Type nextType = curType.DeclaringType;
                String s = "";
                if( nextType != null && !typeMask.Contains( nextType ) )
                {
                    s = nextType.IsClass ? "+" : ".";
                } else
                {
                    loop = false;
                }

                _ = typeNameBuilder.Insert( 0, String.Format( "{0}{1}", s, curType.Name ) );
                curType = nextType;
            }
            String typeName = typeNameBuilder.ToString();


            Type type = ror2Assembly.GetType(typeName, true, false );
            MethodInfo methodInfo = useArgs ? type.GetMethod(method, allFlags, null, callInfo, arguments, null ) : type.GetMethod( method, allFlags );
            if( methodInfo == null )
            {
                Log.Error( String.Format( "Could not find method: {0} in type {1}", method, typeName ) );
            }

            return methodInfo;
        }

        private static readonly BindingFlags allFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic;
        private static readonly Assembly ror2Assembly = typeof(RoR2.RoR2Application).Assembly;
        private static readonly Regex methodName = new Regex( @"^(.*)___", RegexOptions.Compiled );
        private static readonly HashSet<Type> typeMask = new HashSet<Type>( new[]
        {
            typeof(HooksCore),
        });
    }
}
