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
        public static ILCursor Dup_( this ILCursor cursor ) => cursor.Emit( OpCodes.Dup );
        public static ILCursor Pop_( this ILCursor cursor ) => cursor.Emit( OpCodes.Pop );
        public static ILCursor Call_( this ILCursor cursor, MethodInfo target ) => cursor.Emit( OpCodes.Call, target );
        public static ILCursor Calli_<TDelegate>( this ILCursor cursor, TDelegate target )
            where TDelegate : Delegate
            => cursor.EmitIndirectCall<TDelegate>( target );
        public static ILCursor CallDel_<TDelegate>( this ILCursor cursor, TDelegate target, out Int32 index )
            where TDelegate : Delegate
        {
            index = cursor.EmitDelegate<TDelegate>( target );
            return cursor;
        }
        public static ILCursor LdArg_( this ILCursor cursor, UInt16 index ) => cursor.EmitLoadArgument( index );
        public static ILCursor LdC_( this ILCursor cursor, String value ) => cursor.Emit( OpCodes.Ldstr, value );
        public static ILCursor LdC_( this ILCursor cursor, Int32 value ) => cursor.Emit( OpCodes.Ldc_I4, value );
        public static ILCursor LdC_( this ILCursor cursor, Int64 value ) => cursor.Emit( OpCodes.Ldc_I8, value );
        public static ILCursor LdC_( this ILCursor cursor, Single value ) => cursor.Emit( OpCodes.Ldc_R4, value );
        public static ILCursor LdC_( this ILCursor cursor, Double value ) => cursor.Emit( OpCodes.Ldc_R8, value );
        public static ILCursor StLoc_( this ILCursor cursor, Int32 index ) => cursor.Emit( OpCodes.Stloc, index );
        public static ILCursor LdLoc_( this ILCursor cursor, Int32 index ) => cursor.Emit( OpCodes.Ldloc, index );
        public static ILCursor LdFld_( this ILCursor cursor, FieldInfo field ) => cursor.Emit( OpCodes.Ldfld, field );

        public static ILCursor Move( this ILCursor cursor, Int32 offset )
        {
            cursor.Index += offset;
            return cursor;
        }

        public static ILCursor AddRef<T>( this ILCursor cursor, T value, out Int32 index )
        {
            index = cursor.AddReference<T>( value );
            return cursor;
        }
        public static ILCursor GetRef<T>( this ILCursor cursor, Int32 index )
        {
            cursor.EmitGetReference<T>( index );
            return cursor;
        }

        public static ILCursor EmitRef<T>( this ILCursor cursor, T value, out Int32 index )
        {
            index = cursor.EmitReference<T>( value );
            return cursor;
        }

        public static ILCursor EmitCall( this ILCursor cursor, MethodInfo target )
        {
            cursor.Emit( OpCodes.Call, target );
            return cursor;
        }

        public static ILCursor EmitIndirectCall<TDelegate>( this ILCursor cursor, TDelegate del ) where TDelegate : Delegate
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
            _ = cursor.EmitReference<IntPtr>( delMethod.MethodHandle.GetFunctionPointer() );
            _ = cursor.Emit( OpCodes.Calli, site );
            return cursor;
        }

        public static ILCursor EmitLoadArgument( this ILCursor cursor, UInt16 index )
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

            return cursor;
        }

        public static ILCursor EmitString( this ILCursor cursor, String str )
        {
            cursor.Emit( OpCodes.Ldstr, str );
            return cursor;
        }

        public static ILCursor EmitNewObj( this ILCursor cursor, ConstructorInfo constructor )
        {
            cursor.Emit( OpCodes.Newobj, constructor );
            return cursor;
        }
    }
}