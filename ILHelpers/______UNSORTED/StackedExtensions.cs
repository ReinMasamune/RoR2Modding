namespace ILHelpers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Reflection;
    using System.Threading;

    using ILHelpers;

    using Object = System.Object;

    public static unsafe partial class StackedCursorExtensions
    {
#pragma warning disable IDE0022 // Use expression body for methods
        #region Meta


        #endregion
        #region Emit
        public static DMDCursor<IL<TRes,TStack>,TSig> Add<TVal1, TVal2, TRes, TStack, TSig>( this DMDCursor<IL<TVal2, IL<TVal1, TStack>>,TSig> stack, ILAdd<TVal1, TVal2, TRes> settings )
            where TStack : IStack
            where TSig : Delegate
        {
            return stack._Pop()._Pop()._Push<TRes>( stack.cursor.Add( settings.overflow, settings.unsigned ) );
        }
        public static DMDCursor<IL<TRes, TStack>, TSig> And<TVal1, TVal2, TRes, TStack, TSig>( this DMDCursor<IL<TVal2, IL<TVal1, TStack>>, TSig> stack, ILAnd<TVal1, TVal2, TRes> settings )
            where TStack : IStack
            where TSig : Delegate
        {
            return stack._Pop()._Pop()._Push<TRes>( stack.cursor.And() );
        }
        // TODO: public static TCursor ArgList<TCursor>( this TCursor cursor )
        //    where TCursor : ICursorWrite, ICursor
        //{
        //    cursor._Emit( OpCodes.Arglist );
        //    return cursor;
        //}
        // TODO: public static TCursor Branch<TCursor>( this TCursor cursor, ILLabel to )
        //    where TCursor : ICursorWrite, ICursor
        //{
        //    cursor._Emit( OpCodes.Br, to );
        //    return cursor;
        //}
        // TODO: public static TCursor Branch<TBranch, TCursor>( this TCursor cursor, ILLabel to, TBranch branchType )
        //    where TCursor : ICursorWrite, ICursor
        //    where TBranch : struct, IBranch
        //{
        //    cursor._Emit( branchType.opcode, to );
        //    return cursor;
        //}
        public static DMDCursor<IL<Boxed<TBoxed>,TStack>,TSig> Box<TBoxed, TStack, TSig>( this DMDCursor<IL<TBoxed,TStack>, TSig> stack )
            where TBoxed : struct
            where TStack : IStack
            where TSig : Delegate
        {
            Type t;
            //if( new TBoxed() is IStackRep rep ) t = rep.representedType; else t = typeof( TBoxed );
            return default;// stack._Pop()._Push<Boxed<TBoxed>>(stack.cursor.Box(t));
        }
        public static DMDCursor<TStack, TSig> TripBreakpoint<TStack, TSig>( this DMDCursor<TStack, TSig> stack )
            where TStack : IStack
            where TSig : Delegate
        {
            return stack._Emit( stack.cursor.TripBreakpoint() );
        }
        public static DMDCursor<IL<TTo, TStack>, TSig> Cast<TTo, TFrom, TStack, TSig>( this DMDCursor<IL<TFrom, TStack>, TSig> stack, TRef<TTo> type )
            where TStack : IStack
            where TSig : Delegate
            where TFrom : class
            where TTo : class
        {
            return stack._Pop()._Push<TTo>(stack.cursor.Cast(type.t));
        }
        // TODO: public static TCursor Compare<TComparison, TCursor>( this TCursor cursor, TComparison comparison )
        //    where TCursor : ICursorWrite, ICursor
        //    where TComparison : struct, IComparison
        //{
        //    cursor._Emit( comparison.opcode );
        //    return cursor;
        //}
        // TODO: public static TCursor Convert<TConversion, TCursor>( this TCursor cursor, TConversion conversion )
        //    where TCursor : ICursorWrite, ICursor
        //    where TConversion : struct, IConversion
        //{
        //    cursor._Emit( conversion.opcode );
        //    return cursor;
        //}
        // TODO: public static TCursor CopyBlock<TCursor>( this TCursor cursor )
        //    where TCursor : ICursorWrite, ICursor
        //{
        //    cursor._Emit( OpCodes.Cpblk );
        //    return cursor;
        //}
        // TODO: public static TCursor CopyObject<TCursor>( this TCursor cursor, Type type )
        //    where TCursor : ICursorWrite, ICursor
        //{
        //    cursor._Emit( OpCodes.Cpobj, type );
        //    return cursor;
        //}
        public static DMDCursor<IL<TRes, TStack>, TSig> Divide<TVal1, TVal2, TRes, TStack, TSig>( this DMDCursor<IL<TVal2, IL<TVal1, TStack>>, TSig> stack, ILDivide<TVal1, TVal2, TRes> settings )
            where TStack : IStack
            where TSig : Delegate
        {
            return stack._Pop()._Pop()._Push<TRes>( stack.cursor.Divide(settings.unsigned) );
        }
        public static DMDCursor<IL<TValue, IL<TValue, TStack>>, TSig> Dupe<TValue, TStack, TSig>( this DMDCursor<IL<TValue,TStack>, TSig> stack )
            where TStack : IStack
            where TSig : Delegate
        {
            return stack._Push<TValue>( stack.cursor.Dupe() );
        }
        // TODO: public static TCursor EndFilter<TCursor>( this TCursor cursor )
        //    where TCursor : ICursorWrite, ICursor
        //{
        //    cursor._Emit( OpCodes.Endfilter );
        //    return cursor;
        //}
        // TODO: public static TCursor EndFinally<TCursor>( this TCursor cursor )
        //    where TCursor : ICursorWrite, ICursor
        //{
        //    cursor._Emit( OpCodes.Endfinally );
        //    return cursor;
        //}
        // TODO: public static TCursor InitBlock<TCursor>( this TCursor cursor )
        //    where TCursor : ICursorWrite, ICursor
        //{
        //    cursor._Emit( OpCodes.Initblk );
        //    return cursor;
        //}
        // TODO: public static TCursor InitObject<TCursor>( this TCursor cursor, Type type )
        //    where TCursor : ICursorWrite, ICursor
        //{
        //    cursor._Emit( OpCodes.Initobj, type );
        //    return cursor;
        //}
        public static DMDCursor<IL<TTo, TStack>, TSig> As<TTo, TFrom, TStack, TSig>( this DMDCursor<IL<TFrom, TStack>, TSig> stack, TRef<TTo> type )
            where TStack : IStack
            where TSig : Delegate
        {
            return stack._Pop()._Push<TTo>(stack.cursor.As(type.t));
        }
        public static TReturn Jump<TReturn, TStack, TSig>( this DMDCursor<TStack, TSig> stack, ILJump<TReturn, TSig> jump )
            where TStack : IStack
            where TSig : Delegate
            where TReturn : IDMDReturn
        {
            _ = stack.cursor.Jump( jump.target ).Return();
            return jump.ret;
        }
        public static DMDCursor<IL<TArg,TStack>,TSig> LoadArg<TArg, TStack, TSig>( this DMDCursor<TStack, TSig> stack, Arg<TArg> arg )
            where TStack : IStack
            where TSig : Delegate
        {
            return default;// stack._Push<TArg>( stack.cursor.LoadArg( arg.index ) );
        }
        public static DMDCursor<IL<ByRef<TArg>, TStack>, TSig> LoadByrefArg<TArg, TStack, TSig>( this DMDCursor<TStack, TSig> stack, Arg<TArg> arg)
            where TStack : IStack
            where TSig : Delegate
        {
            return default;// stack._Push<ByRef<TArg>>(stack.cursor.LoadByrefArg(arg.index));
        }
        // TODO: public static TCursor LoadConst<TCursor>( this TCursor cursor, Int32 value )
        //    where TCursor : ICursorWrite, ICursor
        //{
        //    switch( value )
        //    {
        //        case -1:
        //        cursor._Emit( OpCodes.Ldc_I4_M1 );
        //        return cursor;
        //        case 0:
        //        cursor._Emit( OpCodes.Ldc_I4_0 );
        //        return cursor;
        //        case 1:
        //        cursor._Emit( OpCodes.Ldc_I4_1 );
        //        return cursor;
        //        case 2:
        //        cursor._Emit( OpCodes.Ldc_I4_2 );
        //        return cursor;
        //        case 3:
        //        cursor._Emit( OpCodes.Ldc_I4_3 );
        //        return cursor;
        //        case 4:
        //        cursor._Emit( OpCodes.Ldc_I4_4 );
        //        return cursor;
        //        case 5:
        //        cursor._Emit( OpCodes.Ldc_I4_5 );
        //        return cursor;
        //        case 6:
        //        cursor._Emit( OpCodes.Ldc_I4_6 );
        //        return cursor;
        //        case 7:
        //        cursor._Emit( OpCodes.Ldc_I4_7 );
        //        return cursor;
        //        case 8:
        //        cursor._Emit( OpCodes.Ldc_I4_8 );
        //        return cursor;
        //        case Int32 i when i >= SByte.MinValue && i <= SByte.MaxValue:
        //        cursor._Emit( OpCodes.Ldc_I4_S, (SByte)i );
        //        return cursor;
        //        default:
        //        cursor._Emit( OpCodes.Ldc_I4, value );
        //        return cursor;
        //    }
        //}
        // TODO: public static TCursor LoadElement<TCursor>( this TCursor cursor, Type elementType )
        //    where TCursor : ICursorWrite, ICursor
        //{
        //    switch( Type.GetTypeCode( elementType ) )
        //    {
        //        case TypeCode.SByte:
        //        cursor._Emit( OpCodes.Ldelem_I1 );
        //        return cursor;
        //        case TypeCode.Int16:
        //        cursor._Emit( OpCodes.Ldelem_I2 );
        //        return cursor;
        //        case TypeCode.Int32:
        //        cursor._Emit( OpCodes.Ldelem_I4 );
        //        return cursor;
        //        case TypeCode.Int64:
        //        cursor._Emit( OpCodes.Ldelem_I8 );
        //        return cursor;
        //        case TypeCode.Single:
        //        cursor._Emit( OpCodes.Ldelem_R4 );
        //        return cursor;
        //        case TypeCode.Double:
        //        cursor._Emit( OpCodes.Ldelem_R8 );
        //        return cursor;
        //        case TypeCode.Byte:
        //        cursor._Emit( OpCodes.Ldelem_U1 );
        //        return cursor;
        //        case TypeCode.UInt16:
        //        cursor._Emit( OpCodes.Ldelem_U2 );
        //        return cursor;
        //        case TypeCode.UInt32:
        //        cursor._Emit( OpCodes.Ldelem_U4 );
        //        return cursor;
        //        case TypeCode.Empty:
        //        cursor._Emit( OpCodes.Ldelem_Ref );
        //        return cursor;
        //        default:
        //        if( elementType == typeof( IntPtr ) )
        //        {
        //            cursor._Emit( OpCodes.Ldelem_I );
        //            return cursor;
        //        }
        //        cursor._Emit( OpCodes.Ldelem_Any, elementType );
        //        return cursor;
        //    }
        //}
        // TODO: public static TCursor LoadElementByref<TCursor>( this TCursor cursor, Type elementType, Boolean readonlyPrefix = false )
        //    where TCursor : ICursorWrite, ICursor
        //{
        //    if( readonlyPrefix ) cursor._Emit( OpCodes.Readonly );
        //    cursor._Emit( OpCodes.Ldelema, elementType );
        //    return cursor;
        //}
        // TODO: public static TCursor LoadField<TCursor>( this TCursor cursor, FieldInfo field )
        //    where TCursor : ICursorWrite, ICursor
        //{
        //    if( field is null ) throw new ArgumentNullException( nameof( field ) );
        //    cursor._Emit( field.IsStatic ? OpCodes.Ldsfld : OpCodes.Ldfld, field );
        //    return cursor;
        //}
        // TODO: public static TCursor LoadFieldRef<TCursor>( this TCursor cursor, FieldInfo field )
        //    where TCursor : ICursorWrite, ICursor
        //{
        //    if( field is null ) throw new ArgumentNullException( nameof( field ) );
        //    cursor._Emit( field.IsStatic ? OpCodes.Ldsflda : OpCodes.Ldflda, field );
        //    return cursor;
        //}
        // TODO: public static TCursor LoadFunctionPointer<TCursor>( this TCursor cursor, MethodInfo method )
        //    where TCursor : ICursorWrite, ICursor
        //{
        //    if( method is null ) throw new ArgumentNullException( nameof( method ) );
        //    cursor._Emit( method.IsVirtual ? OpCodes.Ldvirtftn : OpCodes.Ldftn, method );
        //    return cursor;
        //}
        // TODO: public static TCursor LoadLength<TCursor>( this TCursor cursor )
        //    where TCursor : ICursorWrite, ICursor
        //{
        //    cursor._Emit( OpCodes.Ldlen );
        //    return cursor;
        //}
        public static DMDCursor<IL<TLocal,TStack>,TSig> LoadLocal<TLocal, TSig, TStack>( this DMDCursor<TStack,TSig> stack, Local<TLocal> local )
            where TSig : Delegate
            where TStack : IStack
        {
            return default;// stack._Push<TLocal>( stack.cursor.LoadLocal( local.index ) );
        }
        // TODO: public static TCursor LoadLocalByref<TCursor>( this TCursor cursor, UInt16 index )
        //    where TCursor : ICursorWrite, ICursor
        //{
        //    switch( index )
        //    {
        //        case UInt16 ind when ind <= Byte.MaxValue:
        //        cursor._Emit( OpCodes.Ldloca_S, (Byte)ind );
        //        return cursor;
        //        default:
        //        cursor._Emit( OpCodes.Ldloca, index );
        //        return cursor;
        //    }
        //}
        // TODO: public static TCursor Dereference<TCursor>( this TCursor cursor, Type type )
        //    where TCursor : ICursorWrite, ICursor
        //{
        //    // TODO: Need to use ldind for some types
        //    cursor._Emit( OpCodes.Ldobj, type );
        //    return cursor;
        //}
        public static DMDCursor<IL<RuntimeTypeHandle, TStack>, TSig> TypeOf<TStack, TSig>( this DMDCursor<TStack, TSig> stack, Type type )
            where TStack : IStack
            where TSig : Delegate
        {
            return stack._Push<RuntimeTypeHandle>(stack.cursor.TypeOf(type));
        }
        public static DMDCursor<IL<RuntimeFieldHandle, TStack>, TSig> FieldOf<TStack, TSig>(this DMDCursor<TStack, TSig> stack, FieldInfo field)
            where TStack : IStack
            where TSig : Delegate
        {
            return stack._Push<RuntimeFieldHandle>(stack.cursor.FieldOf(field));
        }
        public static DMDCursor<IL<RuntimeMethodHandle, TStack>, TSig> MethodOf<TStack, TSig>(this DMDCursor<TStack, TSig> stack, MethodInfo method)
            where TStack : IStack
            where TSig : Delegate
        {
            return stack._Push<RuntimeMethodHandle>(stack.cursor.MethodOf(method));
        }
        // TODO: public static TCursor Leave<TCursor>( this TCursor cursor, ILLabel target )
        //    where TCursor : ICursorWrite, ICursor
        //{
        //    cursor._Emit( OpCodes.Leave, target );
        //    return cursor;
        //}
        // TODO: public static TCursor StackAlloc<TCursor>( this TCursor cursor )
        //    where TCursor : ICursorWrite, ICursor
        //{
        //    cursor._Emit( OpCodes.Localloc );
        //    return cursor;
        //}
        // TODO: public static TCursor MakeRefAny<TCursor>( this TCursor cursor, Type type )
        //    where TCursor : ICursorWrite, ICursor
        //{
        //    cursor._Emit( OpCodes.Mkrefany, type );
        //    return cursor;
        //}
        public static DMDCursor<IL<TRes, TStack>, TSig> Multiply<TVal1, TVal2, TRes, TStack, TSig>( this DMDCursor<IL<TVal2, IL<TVal1, TStack>>, TSig> stack, ILMultiply<TVal1, TVal2, TRes> settings )
            where TStack : IStack
            where TSig : Delegate
        {
            return stack._Pop()._Pop()._Push<TRes>( stack.cursor.Multiply( settings.overflow, settings.unsigned ) );
        }
        public static DMDCursor<IL<TRes, TStack>, TSig> Negate<TVal1, TRes, TStack, TSig>( this DMDCursor<IL<TVal1, TStack>, TSig> stack, ILNegate<TVal1, TRes> settings )
            where TStack : IStack
            where TSig : Delegate
        {
            return stack._Pop()._Push<TRes>( stack.cursor.Negate() );
        }
        // TODO: public static TCursor NewArray<TCursor>( this TCursor cursor, Type elementType )
        //    where TCursor : ICursorWrite, ICursor
        //{
        //    cursor._Emit( OpCodes.Newarr, elementType );
        //    return cursor;
        //}
        // TODO: public static TCursor NewObject<TCursor>( this TCursor cursor, ConstructorInfo constructor )
        //    where TCursor : ICursorWrite, ICursor
        //{
        //    cursor._Emit( OpCodes.Newobj, constructor );
        //    return cursor;
        //}
        public static DMDCursor<TStack,TSig> NoOp<TStack, TSig>( this DMDCursor<TStack, TSig> stack )
            where TSig : Delegate
            where TStack : IStack
        {
            return stack._Emit( stack.cursor.NoOp() );
        }
        public static DMDCursor<IL<TRes, TStack>, TSig> Not<TVal1, TRes, TStack, TSig>( this DMDCursor<IL<TVal1, TStack>, TSig> stack, ILNot<TVal1, TRes> settings )
            where TStack : IStack
            where TSig : Delegate
        {
            return stack._Pop()._Push<TRes>( stack.cursor.Not() );
        }
        public static DMDCursor<IL<TRes, TStack>, TSig> Or<TVal1, TVal2, TRes, TStack, TSig>( this DMDCursor<IL<TVal2,IL<TVal1, TStack>>, TSig> stack, ILOr<TVal1, TVal2, TRes> settings )
            where TStack : IStack
            where TSig : Delegate
        {
            return stack._Pop()._Pop()._Push<TRes>( stack.cursor.Or() );
        }
        public static DMDCursor<TStack,TSig> Pop<TValue, TStack, TSig>(this DMDCursor<IL<TValue,TStack>, TSig> stack )
            where TSig : Delegate
            where TStack : IStack
        {
            return stack._Pop( stack.cursor.Pop() );
        }
        // TODO: public static TCursor RefAnyType<TCursor>( this TCursor cursor )
        //    where TCursor : ICursorWrite, ICursor
        //{
        //    cursor._Emit( OpCodes.Refanytype );
        //    return cursor;
        //}
        // TODO: public static TCursor RefAnyVal<TCursor>( this TCursor cursor, Type type )
        //    where TCursor : ICursorWrite, ICursor
        //{
        //    cursor._Emit( OpCodes.Refanyval, type );
        //    return cursor;
        //}
        public static DMDCursor<IL<TRes, TStack>, TSig> Modulus<TVal1, TVal2, TRes, TStack, TSig>( this DMDCursor<IL<TVal2, IL<TVal1, TStack>>, TSig> stack, ILModulus<TVal1, TVal2, TRes> settings )
            where TStack : IStack
            where TSig : Delegate
        {
            return stack._Pop()._Pop()._Push<TRes>( stack.cursor.Modulus( settings.unsigned ) );
        }
        public static DMDReturn Return<TSig>( this DMDCursor<Empty, TSig> stack )
            where TSig : Delegate
        {
            // TODO: Return creation
            return default;
        }
        public static DMDReturn<TReturn> Return<TReturn, TSig>( this DMDCursor<IL<TReturn, Empty>, TSig> stack )
            where TSig : Delegate
        {
            // TODO: Return creation
            return default;
        }
        // TODO: public static TCursor ReThrow<TCursor>( this TCursor cursor )
        //    where TCursor : ICursorWrite, ICursor
        //{
        //    cursor._Emit( OpCodes.Rethrow );
        //    return cursor;
        //}
        public static DMDCursor<IL<TRes, TStack>, TSig> ShiftL<TVal1, TVal2, TRes, TStack, TSig>( this DMDCursor<IL<TVal2, IL<TVal1, TStack>>, TSig> stack, ILShiftL<TVal1, TVal2, TRes> settings )
            where TStack : IStack
            where TSig : Delegate
        {
            return stack._Pop()._Pop()._Push<TRes>( stack.cursor.ShiftL() );
        }
        public static DMDCursor<IL<TRes, TStack>, TSig> ShiftR<TVal1, TVal2, TRes, TStack, TSig>( this DMDCursor<IL<TVal2, IL<TVal1, TStack>>, TSig> stack, ILShiftR<TVal1, TVal2, TRes> settings )
            where TStack : IStack
            where TSig : Delegate
        {
            return stack._Pop()._Pop()._Push<TRes>( stack.cursor.ShiftR( settings.unsigned ) );
        }
        public static DMDCursor<IL<Int32,TStack>,TSig> SizeOf<TStack, TSig>( this DMDCursor<TStack,TSig> stack, Type type )
            where TStack : IStack
            where TSig : Delegate
        {
            return stack._Push<Int32>( stack.cursor.SizeOf( type ) );
        }
        public static DMDCursor<TStack, TSig> StoreArg<TArg, TStack, TSig>( this DMDCursor<IL<TArg,TStack>, TSig> stack, Arg<TArg> arg )
            where TStack : IStack
            where TSig : Delegate
        {
            return default;// stack._Pop( stack.cursor.StoreArg( arg.index ) );
        }
        // TODO: public static TCursor StoreElement<TCursor>( this TCursor cursor, Type elementType )
        //    where TCursor : ICursorWrite, ICursor
        //{
        //    switch( Type.GetTypeCode( elementType ) )
        //    {
        //        case TypeCode.SByte:
        //        cursor._Emit( OpCodes.Stelem_I1 );
        //        return cursor;
        //        case TypeCode.Int16:
        //        cursor._Emit( OpCodes.Stelem_I2 );
        //        return cursor;
        //        case TypeCode.Int32:
        //        cursor._Emit( OpCodes.Stelem_I4 );
        //        return cursor;
        //        case TypeCode.Int64:
        //        cursor._Emit( OpCodes.Stelem_I8 );
        //        return cursor;
        //        case TypeCode.Single:
        //        cursor._Emit( OpCodes.Stelem_R4 );
        //        return cursor;
        //        case TypeCode.Double:
        //        cursor._Emit( OpCodes.Stelem_R8 );
        //        return cursor;
        //        case TypeCode.Empty:
        //        cursor._Emit( OpCodes.Stelem_Ref );
        //        return cursor;
        //        default:
        //        if( elementType == typeof( IntPtr ) )
        //        {
        //            cursor._Emit( OpCodes.Stelem_I );
        //            return cursor;
        //        }
        //        cursor._Emit( OpCodes.Stelem_Any, elementType );
        //        return cursor;
        //    }
        //}
        // TODO: public static TCursor StoreField<TCursor>( this TCursor cursor, FieldInfo field )
        //    where TCursor : ICursorWrite, ICursor
        //{
        //    if( field is null ) throw new ArgumentNullException( nameof( field ) );
        //    cursor._Emit( field.IsStatic ? OpCodes.Stsfld : OpCodes.Stfld, field );
        //    return cursor;
        //}
        // TODO: public static TCursor StoreObject<TCursor>( this TCursor cursor, Type type )
        //    where TCursor : ICursorWrite, ICursor
        //{
        //    // TODO: StInd versions
        //    cursor._Emit( OpCodes.Stobj, type );
        //    return cursor;
        //}
        public static DMDCursor<TStack,TSig> StoreLocal<TLocal, TStack, TSig>( this DMDCursor<IL<TLocal,TStack>, TSig> stack, Local<TLocal> local )
            where TSig : Delegate
            where TStack : IStack
        {
            return default;// stack._Pop( stack.cursor.StoreLocal( local.index ) );
        }
        public static DMDCursor<IL<TRes, TStack>, TSig> Subtract<TVal1, TVal2, TRes, TStack, TSig>( this DMDCursor<IL<TVal2, IL<TVal1, TStack>>, TSig> stack, ILSubtract<TVal1, TVal2, TRes> settings )
            where TStack : IStack
            where TSig : Delegate
        {
            return stack._Pop()._Pop()._Push<TRes>( stack.cursor.Subtract(settings.overflow, settings.unsigned) );
        }
        // TODO: public static TCursor Switch<TCursor>( this TCursor cursor, ILLabel[] targets )
        //    where TCursor : ICursorWrite, ICursor
        //{
        //    cursor._Emit( OpCodes.Switch, targets );
        //    return cursor;
        //}
        // TODO: public static TCursor Throw<TCursor>( this TCursor cursor )
        //    where TCursor : ICursorWrite, ICursor
        //{
        //    cursor._Emit( OpCodes.Throw );
        //    return cursor;
        //}
        public static DMDCursor<IL<ByRef<TBoxed>, TStack>, TSig> Unbox<TBoxed, TStack, TSig>( this DMDCursor<IL<Boxed<TBoxed>,TStack>,TSig> stack )
            where TBoxed : struct
            where TStack : IStack
            where TSig : Delegate
        {
            return stack._Pop()._Push<ByRef<TBoxed>>(stack.cursor.Unbox(typeof(TBoxed)));
        }
        public static DMDCursor<IL<TBoxed, TStack>, TSig> UnboxAny<TBoxed, TStack, TSig>( this DMDCursor<IL<Boxed<TBoxed>, TStack>, TSig> stack )
            where TBoxed : struct
            where TStack : IStack
            where TSig : Delegate
        {
            return stack._Pop()._Push<TBoxed>(stack.cursor.UnboxAny(typeof(TBoxed)));
        }
        public static DMDCursor<IL<TBoxed, TStack>, TSig> UnboxAny<TBoxed, TFrom, TStack, TSig>( this DMDCursor<IL<TFrom,TStack>, TSig> stack, TRef<TBoxed> type )
            where TStack : IStack
            where TSig : Delegate
        {
            return stack._Pop()._Push<TBoxed>(stack.cursor.UnboxAny(typeof(TBoxed)));
        }
        //// TODO: Unaligned
        //// TODO: Volatile
        public static DMDCursor<IL<TRes, TStack>, TSig> XOr<TVal1, TVal2, TRes, TStack, TSig>( this DMDCursor<IL<TVal2, IL<TVal1, TStack>>, TSig> stack, ILXOr<TVal1, TVal2, TRes> settings )
            where TStack : IStack
            where TSig : Delegate
        {
            return stack._Pop()._Pop()._Push<TRes>( stack.cursor.XOr() );
        }
        #endregion
    }
    #pragma warning restore IDE0022 // Use expression body for methods
}
