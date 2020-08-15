namespace ILHelpers
{
    using System;
    using System.Reflection;

    using Mono.Cecil;
    using Mono.Cecil.Cil;

    using MonoMod.Cil;

    using Object = System.Object;

    public static class UnstackedCursorWriteExtensions
    {
        public static TCursor Add<TCursor>( this TCursor cursor, Boolean overflow = false, Boolean unsigned = false )
            where TCursor : ICursorWrite, ICursor
        {
            cursor._Emit( overflow ? unsigned ? OpCodes.Add_Ovf_Un : OpCodes.Add_Ovf : OpCodes.Add );
            return cursor;
        }
        public static TCursor And<TCursor>( this TCursor cursor )
            where TCursor : ICursorWrite, ICursor
        {
            cursor._Emit( OpCodes.And );
            return cursor;
        }
        public static TCursor ArgList<TCursor>( this TCursor cursor )
            where TCursor : ICursorWrite, ICursor
        {
            cursor._Emit( OpCodes.Arglist );
            return cursor;
        }
        public static TCursor Branch<TCursor>( this TCursor cursor, ILLabel to )
            where TCursor : ICursorWrite, ICursor
        {
            cursor._Emit( OpCodes.Br, to );
            return cursor;
        }
        public static TCursor Branch<TBranch, TCursor>( this TCursor cursor, ILLabel to, TBranch branchType )
            where TCursor : ICursorWrite, ICursor
            where TBranch : struct, IBranch
        {
            cursor._Emit( branchType.opcode, to );
            return cursor;
        }
        public static TCursor Box<TCursor>( this TCursor cursor, Type type )
            where TCursor : ICursorWrite, ICursor
        {
            cursor._Emit( OpCodes.Box, type );
            return cursor;
        }
        public static TCursor TripBreakpoint<TCursor>( this TCursor cursor )
            where TCursor : ICursorWrite, ICursor
        {
            cursor._Emit( OpCodes.Break );
            return cursor;
        }
        public static TCursor Call<TCursor>( this TCursor cursor, MethodInfo method, Boolean doTailcall = false )
            where TCursor : ICursorWrite, ICursor
        {
            if( doTailcall ) cursor._Emit( OpCodes.Tail );
            cursor._Emit( OpCodes.Call, method );
            if( doTailcall ) cursor._Emit( OpCodes.Ret );
            return cursor;
        }
        public static TCursor CallIndirect<TCursor>( this TCursor cursor, CallSite callsite, Boolean doTailcall = false )
            where TCursor : ICursorWrite, ICursor
        {
            if( doTailcall ) cursor._Emit( OpCodes.Tail );
            cursor._Emit( OpCodes.Calli, callsite );
            if( doTailcall ) cursor._Emit( OpCodes.Ret );
            return cursor;
        }
        public static TCursor CallVirtual<TCursor>( this TCursor cursor, MethodInfo method, Type constrainedType = null, Boolean doTailcall = false )
            where TCursor : ICursorWrite, ICursor
        {
            if( doTailcall ) cursor._Emit( OpCodes.Tail );
            if( constrainedType != null ) cursor._Emit( OpCodes.Constrained, constrainedType );
            cursor._Emit( OpCodes.Callvirt, method );
            if( doTailcall ) cursor._Emit( OpCodes.Ret );
            return cursor;
        }
        public static TCursor CallInline<TCursor>( this TCursor cursor, MethodInfo method )
            where TCursor : ICursorWrite, ICursor
        {
            // TODO: Implement CallInline
            return cursor;
        }
        public static TCursor Cast<TCursor>( this TCursor cursor, Type type )
            where TCursor : ICursorWrite, ICursor
        {
            cursor._Emit( OpCodes.Castclass, type );
            return cursor;
        }
        public static TCursor Compare<TComparison, TCursor>( this TCursor cursor, TComparison comparison )
            where TCursor : ICursorWrite, ICursor
            where TComparison : struct, IComparison
        {
            cursor._Emit( comparison.opcode );
            return cursor;
        }
        public static TCursor Convert<TConversion, TCursor>( this TCursor cursor, TConversion conversion )
            where TCursor : ICursorWrite, ICursor
            where TConversion : struct, IConversion
        {
            cursor._Emit( conversion.opcode );
            return cursor;
        }
        public static TCursor CopyBlock<TCursor>( this TCursor cursor )
            where TCursor : ICursorWrite, ICursor
        {
            cursor._Emit( OpCodes.Cpblk );
            return cursor;
        }
        public static TCursor CopyObject<TCursor>( this TCursor cursor, Type type )
            where TCursor : ICursorWrite, ICursor
        {
            cursor._Emit( OpCodes.Cpobj, type );
            return cursor;
        }
        public static TCursor Divide<TCursor>( this TCursor cursor, Boolean unsigned = false )
            where TCursor : ICursorWrite, ICursor
        {
            cursor._Emit( unsigned ? OpCodes.Div_Un : OpCodes.Div );
            return cursor;
        }
        public static TCursor Dupe<TCursor>( this TCursor cursor, UInt32 number = 1u )
            where TCursor : ICursorWrite, ICursor
        {
            for( UInt32 i = 1; i <= number; i = checked(i+1) ) cursor._Emit( OpCodes.Dup );
            return cursor;
        }
        public static TCursor EndFilter<TCursor>( this TCursor cursor )
            where TCursor : ICursorWrite, ICursor
        {
            cursor._Emit( OpCodes.Endfilter );
            return cursor;
        }
        public static TCursor EndFinally<TCursor>( this TCursor cursor )
            where TCursor : ICursorWrite, ICursor
        {
            cursor._Emit( OpCodes.Endfinally );
            return cursor;
        }
        public static TCursor InitBlock<TCursor>( this TCursor cursor )
            where TCursor : ICursorWrite, ICursor
        {
            cursor._Emit( OpCodes.Initblk );
            return cursor;
        }
        public static TCursor InitObject<TCursor>( this TCursor cursor, Type type )
            where TCursor : ICursorWrite, ICursor
        {
            cursor._Emit( OpCodes.Initobj, type );
            return cursor;
        }
        public static TCursor As<TCursor>( this TCursor cursor, Type type )
            where TCursor : ICursorWrite, ICursor
        {
            cursor._Emit( OpCodes.Isinst, type );
            return cursor;
        }
        public static TCursor Jump<TCursor>( this TCursor cursor, MethodInfo method )
            where TCursor : ICursorWrite, ICursor
        {
            cursor._Emit( OpCodes.Jmp, method );
            return cursor;
        }
        public static TCursor LoadArg<TCursor>( this TCursor cursor, UInt16 index )
            where TCursor : ICursorWrite, ICursor
        {
            switch( index )
            {
                case 0:
                cursor._Emit( OpCodes.Ldarg_0 );
                return cursor;
                case 1:
                cursor._Emit( OpCodes.Ldarg_1 );
                return cursor;
                case 2:
                cursor._Emit( OpCodes.Ldarg_2 );
                return cursor;
                case 3:
                cursor._Emit( OpCodes.Ldarg_3 );
                return cursor;
                case UInt16 ind when ind <= Byte.MaxValue:
                cursor._Emit( OpCodes.Ldarg_S, (Byte)ind );
                return cursor;
                default:
                cursor._Emit( OpCodes.Ldarg, index );
                return cursor;
            }
        }
        public static TCursor LoadByrefArg<TCursor>( this TCursor cursor, UInt16 index )
            where TCursor : ICursorWrite, ICursor
        {
            switch( index )
            {
                case UInt16 ind when ind <= Byte.MaxValue:
                cursor._Emit( OpCodes.Ldarga_S, (Byte)ind );
                return cursor;
                default:
                cursor._Emit( OpCodes.Ldarga, index );
                return cursor;
            }
        }
        public static TCursor LoadConst<TCursor>( this TCursor cursor, Int32 value )
            where TCursor : ICursorWrite, ICursor
        {
            switch( value )
            {
                case -1:
                cursor._Emit( OpCodes.Ldc_I4_M1 );
                return cursor;
                case 0:
                cursor._Emit( OpCodes.Ldc_I4_0 );
                return cursor;
                case 1:
                cursor._Emit( OpCodes.Ldc_I4_1 );
                return cursor;
                case 2:
                cursor._Emit( OpCodes.Ldc_I4_2 );
                return cursor;
                case 3:
                cursor._Emit( OpCodes.Ldc_I4_3 );
                return cursor;
                case 4:
                cursor._Emit( OpCodes.Ldc_I4_4 );
                return cursor;
                case 5:
                cursor._Emit( OpCodes.Ldc_I4_5 );
                return cursor;
                case 6:
                cursor._Emit( OpCodes.Ldc_I4_6 );
                return cursor;
                case 7:
                cursor._Emit( OpCodes.Ldc_I4_7 );
                return cursor;
                case 8:
                cursor._Emit( OpCodes.Ldc_I4_8 );
                return cursor;
                case Int32 i when i >= SByte.MinValue && i <= SByte.MaxValue:
                cursor._Emit( OpCodes.Ldc_I4_S, (SByte)i );
                return cursor;
                default:
                cursor._Emit( OpCodes.Ldc_I4, value );
                return cursor;
            }
        }
        public static TCursor LoadElement<TCursor>( this TCursor cursor, Type elementType )
            where TCursor : ICursorWrite, ICursor
        {
            switch( Type.GetTypeCode( elementType ) )
            {
                case TypeCode.SByte:
                cursor._Emit( OpCodes.Ldelem_I1 );
                return cursor;
                case TypeCode.Int16:
                cursor._Emit( OpCodes.Ldelem_I2 );
                return cursor;
                case TypeCode.Int32:
                cursor._Emit( OpCodes.Ldelem_I4 );
                return cursor;
                case TypeCode.Int64:
                cursor._Emit( OpCodes.Ldelem_I8 );
                return cursor;
                case TypeCode.Single:
                cursor._Emit( OpCodes.Ldelem_R4 );
                return cursor;
                case TypeCode.Double:
                cursor._Emit( OpCodes.Ldelem_R8 );
                return cursor;
                case TypeCode.Byte:
                cursor._Emit( OpCodes.Ldelem_U1 );
                return cursor;
                case TypeCode.UInt16:
                cursor._Emit( OpCodes.Ldelem_U2 );
                return cursor;
                case TypeCode.UInt32:
                cursor._Emit( OpCodes.Ldelem_U4 );
                return cursor;
                case TypeCode.Empty:
                cursor._Emit( OpCodes.Ldelem_Ref );
                return cursor;
                default:
                if( elementType == typeof( IntPtr ) )
                {
                    cursor._Emit( OpCodes.Ldelem_I );
                    return cursor;
                }
                cursor._Emit( OpCodes.Ldelem_Any, elementType );
                return cursor;
            }
        }
        public static TCursor LoadElementByref<TCursor>( this TCursor cursor, Type elementType, Boolean readonlyPrefix = false )
            where TCursor : ICursorWrite, ICursor
        {
            if( readonlyPrefix ) cursor._Emit( OpCodes.Readonly );
            cursor._Emit( OpCodes.Ldelema, elementType );
            return cursor;
        }
        public static TCursor LoadField<TCursor>( this TCursor cursor, FieldInfo field )
            where TCursor : ICursorWrite, ICursor
        {
            if( field is null ) throw new ArgumentNullException( nameof( field ) );
            cursor._Emit( field.IsStatic ? OpCodes.Ldsfld : OpCodes.Ldfld, field );
            return cursor;
        }
        public static TCursor LoadFieldRef<TCursor>( this TCursor cursor, FieldInfo field )
            where TCursor : ICursorWrite, ICursor
        {
            if( field is null ) throw new ArgumentNullException( nameof( field ) );
            cursor._Emit( field.IsStatic ? OpCodes.Ldsflda : OpCodes.Ldflda, field );
            return cursor;
        }
        public static TCursor LoadFunctionPointer<TCursor>( this TCursor cursor, MethodInfo method )
            where TCursor : ICursorWrite, ICursor
        {
            if( method is null ) throw new ArgumentNullException( nameof( method ) );
            cursor._Emit( method.IsVirtual ? OpCodes.Ldvirtftn : OpCodes.Ldftn, method );
            return cursor;
        }
        public static TCursor LoadLength<TCursor>( this TCursor cursor )
            where TCursor : ICursorWrite, ICursor
        {
            cursor._Emit( OpCodes.Ldlen );
            return cursor;
        }
        public static TCursor LoadLocal<TCursor>( this TCursor cursor, UInt16 index )
            where TCursor : ICursorWrite, ICursor
        {
            switch( index )
            {
                case 0:
                cursor._Emit( OpCodes.Ldloc_0 );
                return cursor;
                case 1:
                cursor._Emit( OpCodes.Ldloc_1 );
                return cursor;
                case 2:
                cursor._Emit( OpCodes.Ldloc_2 );
                return cursor;
                case 3:
                cursor._Emit( OpCodes.Ldloc_3 );
                return cursor;
                case UInt16 ind when ind <= Byte.MaxValue:
                cursor._Emit( OpCodes.Ldloc_S, (Byte)ind );
                return cursor;
                default:
                cursor._Emit( OpCodes.Ldloc, index );
                return cursor;
            }
        }
        public static TCursor LoadLocalByref<TCursor>( this TCursor cursor, UInt16 index )
            where TCursor : ICursorWrite, ICursor
        {
            switch( index )
            {
                case UInt16 ind when ind <= Byte.MaxValue:
                cursor._Emit( OpCodes.Ldloca_S, (Byte)ind );
                return cursor;
                default:
                cursor._Emit( OpCodes.Ldloca, index );
                return cursor;
            }
        }
        public static TCursor Dereference<TCursor>( this TCursor cursor, Type type )
            where TCursor : ICursorWrite, ICursor
        {
            // TODO: Need to use ldind for some types
            cursor._Emit( OpCodes.Ldobj, type );
            return cursor;
        }
        public static TCursor TypeOf<TCursor>( this TCursor cursor, Type type )
            where TCursor : ICursorWrite, ICursor
        {
            cursor._Emit( OpCodes.Ldtoken, type );
            return cursor;
        }
        public static TCursor FieldOf<TCursor>( this TCursor cursor, FieldInfo field )
            where TCursor : ICursorWrite, ICursor
        {
            cursor._Emit( OpCodes.Ldtoken, field );
            return cursor;
        }
        public static TCursor MethodOf<TCursor>( this TCursor cursor, MethodInfo method )
            where TCursor : ICursorWrite, ICursor
        {
            cursor._Emit( OpCodes.Ldtoken, method );
            return cursor;
        }
        public static TCursor Leave<TCursor>( this TCursor cursor, ILLabel target )
            where TCursor : ICursorWrite, ICursor
        {
            cursor._Emit( OpCodes.Leave, target );
            return cursor;
        }
        public static TCursor StackAlloc<TCursor>( this TCursor cursor )
            where TCursor : ICursorWrite, ICursor
        {
            cursor._Emit( OpCodes.Localloc );
            return cursor;
        }
        public static TCursor MakeRefAny<TCursor>( this TCursor cursor, Type type )
            where TCursor : ICursorWrite, ICursor
        {
            cursor._Emit( OpCodes.Mkrefany, type );
            return cursor;
        }
        public static TCursor Multiply<TCursor>( this TCursor cursor, Boolean overflow = false, Boolean unsigned = false )
            where TCursor : ICursorWrite, ICursor
        {
            cursor._Emit( overflow ? unsigned ? OpCodes.Mul_Ovf_Un : OpCodes.Mul_Ovf : OpCodes.Mul );
            return cursor;
        }
        public static TCursor Negate<TCursor>( this TCursor cursor )
            where TCursor : ICursorWrite, ICursor
        {
            cursor._Emit( OpCodes.Neg );
            return cursor;
        }
        public static TCursor NewArray<TCursor>( this TCursor cursor, Type elementType )
            where TCursor : ICursorWrite, ICursor
        {
            cursor._Emit( OpCodes.Newarr, elementType );
            return cursor;
        }
        public static TCursor NewObject<TCursor>( this TCursor cursor, ConstructorInfo constructor )
            where TCursor : ICursorWrite, ICursor
        {
            cursor._Emit( OpCodes.Newobj, constructor );
            return cursor;
        }
        public static TCursor NoOp<TCursor>( this TCursor cursor )
            where TCursor : ICursorWrite, ICursor
        {
            cursor._Emit( OpCodes.Nop );
            return cursor;
        }
        public static TCursor Not<TCursor>( this TCursor cursor )
            where TCursor : ICursorWrite, ICursor
        {
            cursor._Emit( OpCodes.Not );
            return cursor;
        }
        public static TCursor Or<TCursor>( this TCursor cursor )
            where TCursor : ICursorWrite, ICursor
        {
            cursor._Emit( OpCodes.Or );
            return cursor;
        }
        public static TCursor Pop<TCursor>( this TCursor cursor, UInt32 number = 1u )
            where TCursor : ICursorWrite, ICursor
        {
            for( UInt32 i = 1; i <= number; i = checked(i+1) ) cursor._Emit( OpCodes.Pop );
            return cursor;
        }
        public static TCursor RefAnyType<TCursor>( this TCursor cursor )
            where TCursor : ICursorWrite, ICursor
        {
            cursor._Emit( OpCodes.Refanytype );
            return cursor;
        }
        public static TCursor RefAnyVal<TCursor>( this TCursor cursor, Type type )
            where TCursor : ICursorWrite, ICursor
        {
            cursor._Emit( OpCodes.Refanyval, type );
            return cursor;
        }
        public static TCursor Modulus<TCursor>( this TCursor cursor, Boolean unsigned = false )
            where TCursor : ICursorWrite, ICursor
        {
            cursor._Emit( unsigned ? OpCodes.Rem_Un : OpCodes.Rem );
            return cursor;
        }
        public static TCursor Return<TCursor>( this TCursor cursor )
            where TCursor : ICursorWrite, ICursor
        {
            cursor._Emit( OpCodes.Ret );
            return cursor;
        }
        public static TCursor ReThrow<TCursor>( this TCursor cursor )
            where TCursor : ICursorWrite, ICursor
        {
            cursor._Emit( OpCodes.Rethrow );
            return cursor;
        }
        public static TCursor ShiftL<TCursor>( this TCursor cursor )
            where TCursor : ICursorWrite, ICursor
        {
            cursor._Emit( OpCodes.Shl );
            return cursor;
        }
        public static TCursor ShiftR<TCursor>( this TCursor cursor, Boolean unsigned = false )
            where TCursor : ICursorWrite, ICursor
        {
            cursor._Emit( unsigned ? OpCodes.Shr_Un : OpCodes.Shr );
            return cursor;
        }
        public static TCursor SizeOf<TCursor>( this TCursor cursor, Type type )
            where TCursor : ICursorWrite, ICursor
        {
            cursor._Emit( OpCodes.Sizeof, type );
            return cursor;
        }
        public static TCursor StoreArg<TCursor>( this TCursor cursor, UInt16 index )
            where TCursor : ICursorWrite, ICursor
        {
            switch( index )
            {
                case UInt16 ind when ind <= Byte.MaxValue:
                cursor._Emit( OpCodes.Starg_S, (Byte)ind );
                return cursor;
                default:
                cursor._Emit( OpCodes.Starg, index );
                return cursor;
            }
        }
        public static TCursor StoreElement<TCursor>( this TCursor cursor, Type elementType )
            where TCursor : ICursorWrite, ICursor
        {
            switch( Type.GetTypeCode( elementType ) )
            {
                case TypeCode.SByte:
                cursor._Emit( OpCodes.Stelem_I1 );
                return cursor;
                case TypeCode.Int16:
                cursor._Emit( OpCodes.Stelem_I2 );
                return cursor;
                case TypeCode.Int32:
                cursor._Emit( OpCodes.Stelem_I4 );
                return cursor;
                case TypeCode.Int64:
                cursor._Emit( OpCodes.Stelem_I8 );
                return cursor;
                case TypeCode.Single:
                cursor._Emit( OpCodes.Stelem_R4 );
                return cursor;
                case TypeCode.Double:
                cursor._Emit( OpCodes.Stelem_R8 );
                return cursor;
                case TypeCode.Empty:
                cursor._Emit( OpCodes.Stelem_Ref );
                return cursor;
                default:
                if( elementType == typeof( IntPtr ) )
                {
                    cursor._Emit( OpCodes.Stelem_I );
                    return cursor;
                }
                cursor._Emit( OpCodes.Stelem_Any, elementType );
                return cursor;
            }
        }
        public static TCursor StoreField<TCursor>( this TCursor cursor, FieldInfo field )
            where TCursor : ICursorWrite, ICursor
        {
            if( field is null ) throw new ArgumentNullException( nameof( field ) );
            cursor._Emit( field.IsStatic ? OpCodes.Stsfld : OpCodes.Stfld, field );
            return cursor;
        }
        public static TCursor StoreObject<TCursor>( this TCursor cursor, Type type )
            where TCursor : ICursorWrite, ICursor
        {
            // TODO: StInd versions
            cursor._Emit( OpCodes.Stobj, type );
            return cursor;
        }
        public static TCursor StoreLocal<TCursor>( this TCursor cursor, UInt16 index )
            where TCursor : ICursorWrite, ICursor
        {
            switch( index )
            {
                case 0:
                cursor._Emit( OpCodes.Stloc_0 );
                return cursor;
                case 1:
                cursor._Emit( OpCodes.Stloc_1 );
                return cursor;
                case 2:
                cursor._Emit( OpCodes.Stloc_2 );
                return cursor;
                case 3:
                cursor._Emit( OpCodes.Stloc_3 );
                return cursor;
                case UInt16 ind when ind <= Byte.MaxValue:
                cursor._Emit( OpCodes.Stloc_S, (Byte)ind );
                return cursor;
                default:
                cursor._Emit( OpCodes.Stloc, index );
                return cursor;
            }
        }
        public static TCursor Subtract<TCursor>( this TCursor cursor, Boolean overflow = false, Boolean unsigned = false )
            where TCursor : ICursorWrite, ICursor
        {
            cursor._Emit( overflow ? unsigned ? OpCodes.Sub_Ovf_Un : OpCodes.Sub_Ovf : OpCodes.Sub );
            return cursor;
        }
        public static TCursor Switch<TCursor>( this TCursor cursor, ILLabel[] targets )
            where TCursor : ICursorWrite, ICursor
        {
            cursor._Emit( OpCodes.Switch, targets );
            return cursor;
        }
        public static TCursor Throw<TCursor>( this TCursor cursor )
            where TCursor : ICursorWrite, ICursor
        {
            cursor._Emit( OpCodes.Throw );
            return cursor;
        }
        public static TCursor Unbox<TCursor>( this TCursor cursor, Type type )
            where TCursor : ICursorWrite, ICursor
        {
            cursor._Emit(OpCodes.Unbox, type);
            return cursor;
        }
        public static TCursor UnboxAny<TCursor>( this TCursor cursor, Type type )
            where TCursor : ICursorWrite, ICursor
        {

            cursor._Emit(OpCodes.Unbox_Any, type);
            return cursor;
        }
        // TODO: Unbox
        // TODO: Unbox_Any
        // TODO: Unaligned
        // TODO: Volatile
        public static TCursor XOr<TCursor>( this TCursor cursor )
            where TCursor : ICursorWrite, ICursor
        {
            cursor._Emit( OpCodes.Xor );
            return cursor;
        }
    }
}