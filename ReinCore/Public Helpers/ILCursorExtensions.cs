namespace ReinCore
{
    using System;
    using MonoMod.Cil;
    using Mono.Cecil;
    using MonoMod.Utils;
    using System.Linq;
    using Mono.Cecil.Cil;
    using System.Reflection;

    public static class ILCursorExtensions
    {
        public static void EmitIndirectCall<TDelegate>( this ILCursor cursor, TDelegate del ) where TDelegate : Delegate
        {
            var proc = cursor.IL;
            var delMethod = del.Method;

            var site = new CallSite( proc.Import( delMethod.ReturnParameter.ParameterType ) )
            {
                CallingConvention = MethodCallingConvention.StdCall,
            };

            foreach( var param in delMethod.GetParameters() )
            {
                var attributes = (Mono.Cecil.ParameterAttributes) param.Attributes;
                site.Parameters.Add( new ParameterDefinition( param.Name, attributes, proc.Import( param.ParameterType ) ) );
            }
            _ = cursor.EmitReference<IntPtr>( delMethod.MethodHandle.Value );
            _ = cursor.Emit( OpCodes.Calli, site );
        }

        public static void EmitLoadArgument( this ILCursor cursor, UInt16 index )
        {
            switch( index )
            {
                case 0:
                _ = cursor.Emit( OpCodes.Ldarg_0 );
                break;

                case 1:
                _ = cursor.Emit( OpCodes.Ldarg_1 );
                break;

                case 2:
                _ = cursor.Emit( OpCodes.Ldarg_2 );
                break;

                case 3:
                _ = cursor.Emit( OpCodes.Ldarg_3 );
                break;

                default:
                if( index < Byte.MaxValue )
                {
                    _ = cursor.Emit( OpCodes.Ldarg_S, (Byte)index );
                } else
                {
                    _ = cursor.Emit( OpCodes.Ldarg, index );
                }
                break;
            }
        }

        public static void EmitString( this ILCursor cursor, String str )
        {
            cursor.Emit( OpCodes.Ldstr, str );
        }

        public static void EmitNewObj( this ILCursor cursor, ConstructorInfo constructor )
        {
            cursor.Emit( OpCodes.Newobj, constructor );
        }
    }
}