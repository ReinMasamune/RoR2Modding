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
        public static ICursor<IL<TRes,TStack>> Add<TVal1, TVal2, TRes, TStack>( this ICursor<IL<TVal2, IL<TVal1, TStack>>> stack, ILAdd<TVal1, TVal2, TRes> settings )
            where TStack : IStack            
        {
            return default;// stack._Pop()._Pop()._Push<TRes>( stack.cursor.Add( settings.overflow, settings.unsigned ) );
        }
        public static ICursor<IL<TRes, TStack>> And<TVal1, TVal2, TRes, TStack>( this ICursor<IL<TVal2, IL<TVal1, TStack>>> stack, ILAnd<TVal1, TVal2, TRes> settings )
            where TStack : IStack          
        {
            return default;// stack._Pop()._Pop()._Push<TRes>( stack.cursor.And() );
        }
        // TODO: public static TCursor ArgList<TCursor>( this TCursor cursor )
        //    where TCursor : ICursorWrite, ICursor
        //{
        //    cursor._Emit( OpCodes.Arglist );
        //    return default;// cursor;
        //}
        // TODO: public static TCursor Branch<TCursor>( this TCursor cursor, ILLabel to )
        //    where TCursor : ICursorWrite, ICursor
        //{
        //    cursor._Emit( OpCodes.Br, to );
        //    return default;// cursor;
        //}
        // TODO: public static TCursor Branch<TBranch, TCursor>( this TCursor cursor, ILLabel to, TBranch branchType )
        //    where TCursor : ICursorWrite, ICursor
        //    where TBranch : struct, IBranch
        //{
        //    cursor._Emit( branchType.opcode, to );
        //    return default;// cursor;
        //}
        public static ICursor<IL<Boxed<TBoxed>,TStack>> Box<TBoxed, TStack>( this ICursor<IL<TBoxed,TStack>> stack )
            where TBoxed : struct
            where TStack : IStack           
        {
            Type t;
            //if( new TBoxed() is IStackRep rep ) t = rep.representedType; else t = typeof( TBoxed );
            return default;// default;// stack._Pop()._Push<Boxed<TBoxed>>(stack.cursor.Box(t));
        }
        public static ICursor<TStack> TripBreakpoint<TStack>( this ICursor<TStack> stack )
            where TStack : IStack           
        {
            return default;// stack._Emit( stack.cursor.TripBreakpoint() );
        }
        public static ICursor<IL<TTo, TStack>> Cast<TTo, TFrom, TStack>( this ICursor<IL<TFrom, TStack>> stack, TRef<TTo> type )
            where TStack : IStack          
            where TFrom : class
            where TTo : class
        {
            return default;// stack._Pop()._Push<TTo>(stack.cursor.Cast(type.t));
        }
        // TODO: public static TCursor Compare<TComparison, TCursor>( this TCursor cursor, TComparison comparison )
        //    where TCursor : ICursorWrite, ICursor
        //    where TComparison : struct, IComparison
        //{
        //    cursor._Emit( comparison.opcode );
        //    return default;// cursor;
        //}
        // TODO: public static TCursor Convert<TConversion, TCursor>( this TCursor cursor, TConversion conversion )
        //    where TCursor : ICursorWrite, ICursor
        //    where TConversion : struct, IConversion
        //{
        //    cursor._Emit( conversion.opcode );
        //    return default;// cursor;
        //}
        // TODO: public static TCursor CopyBlock<TCursor>( this TCursor cursor )
        //    where TCursor : ICursorWrite, ICursor
        //{
        //    cursor._Emit( OpCodes.Cpblk );
        //    return default;// cursor;
        //}
        // TODO: public static TCursor CopyObject<TCursor>( this TCursor cursor, Type type )
        //    where TCursor : ICursorWrite, ICursor
        //{
        //    cursor._Emit( OpCodes.Cpobj, type );
        //    return default;// cursor;
        //}
        public static ICursor<IL<TRes, TStack>> Divide<TVal1, TVal2, TRes, TStack>( this ICursor<IL<TVal2, IL<TVal1, TStack>>> stack, ILDivide<TVal1, TVal2, TRes> settings )
            where TStack : IStack          
        {
            return default;// stack._Pop()._Pop()._Push<TRes>( stack.cursor.Divide(settings.unsigned) );
        }
        public static ICursor<IL<TValue, IL<TValue, TStack>>> Dupe<TValue, TStack>( this ICursor<IL<TValue,TStack>> stack )
            where TStack : IStack         
        {
            return default;// stack._Push<TValue>( stack.cursor.Dupe() );
        }
        // TODO: public static TCursor EndFilter<TCursor>( this TCursor cursor )
        //    where TCursor : ICursorWrite, ICursor
        //{
        //    cursor._Emit( OpCodes.Endfilter );
        //    return default;// cursor;
        //}
        // TODO: public static TCursor EndFinally<TCursor>( this TCursor cursor )
        //    where TCursor : ICursorWrite, ICursor
        //{
        //    cursor._Emit( OpCodes.Endfinally );
        //    return default;// cursor;
        //}
        // TODO: public static TCursor InitBlock<TCursor>( this TCursor cursor )
        //    where TCursor : ICursorWrite, ICursor
        //{
        //    cursor._Emit( OpCodes.Initblk );
        //    return default;// cursor;
        //}
        // TODO: public static TCursor InitObject<TCursor>( this TCursor cursor, Type type )
        //    where TCursor : ICursorWrite, ICursor
        //{
        //    cursor._Emit( OpCodes.Initobj, type );
        //    return default;// cursor;
        //}
        public static ICursor<IL<TTo, TStack>> As<TTo, TFrom, TStack>( this ICursor<IL<TFrom, TStack>> stack, TRef<TTo> type )
            where TStack : IStack         
        {
            return default;// stack._Pop()._Push<TTo>(stack.cursor.As(type.t));
        }
        public static ICursor<IL<TArg,TStack>> LoadArg<TArg, TStack>( this ICursor<TStack> stack, Arg<TArg> arg )
            where TStack : IStack        
        {
            return default;// default;// stack._Push<TArg>( stack.cursor.LoadArg( arg.index ) );
        }
        public static ICursor<IL<ByRef<TArg>, TStack>> LoadByrefArg<TArg, TStack>( this ICursor<TStack> stack, Arg<TArg> arg)
            where TStack : IStack       
        {
            return default;// default;// stack._Push<ByRef<TArg>>(stack.cursor.LoadByrefArg(arg.index));
        }
        // TODO: public static TCursor LoadConst<TCursor>( this TCursor cursor, Int32 value )
        //    where TCursor : ICursorWrite, ICursor
        //{
        //    switch( value )
        //    {
        //        case -1:
        //        cursor._Emit( OpCodes.Ldc_I4_M1 );
        //        return default;// cursor;
        //        case 0:
        //        cursor._Emit( OpCodes.Ldc_I4_0 );
        //        return default;// cursor;
        //        case 1:
        //        cursor._Emit( OpCodes.Ldc_I4_1 );
        //        return default;// cursor;
        //        case 2:
        //        cursor._Emit( OpCodes.Ldc_I4_2 );
        //        return default;// cursor;
        //        case 3:
        //        cursor._Emit( OpCodes.Ldc_I4_3 );
        //        return default;// cursor;
        //        case 4:
        //        cursor._Emit( OpCodes.Ldc_I4_4 );
        //        return default;// cursor;
        //        case 5:
        //        cursor._Emit( OpCodes.Ldc_I4_5 );
        //        return default;// cursor;
        //        case 6:
        //        cursor._Emit( OpCodes.Ldc_I4_6 );
        //        return default;// cursor;
        //        case 7:
        //        cursor._Emit( OpCodes.Ldc_I4_7 );
        //        return default;// cursor;
        //        case 8:
        //        cursor._Emit( OpCodes.Ldc_I4_8 );
        //        return default;// cursor;
        //        case Int32 i when i >= SByte.MinValue && i <= SByte.MaxValue:
        //        cursor._Emit( OpCodes.Ldc_I4_S, (SByte)i );
        //        return default;// cursor;
        //        default:
        //        cursor._Emit( OpCodes.Ldc_I4, value );
        //        return default;// cursor;
        //    }
        //}
        // TODO: public static TCursor LoadElement<TCursor>( this TCursor cursor, Type elementType )
        //    where TCursor : ICursorWrite, ICursor
        //{
        //    switch( Type.GetTypeCode( elementType ) )
        //    {
        //        case TypeCode.SByte:
        //        cursor._Emit( OpCodes.Ldelem_I1 );
        //        return default;// cursor;
        //        case TypeCode.Int16:
        //        cursor._Emit( OpCodes.Ldelem_I2 );
        //        return default;// cursor;
        //        case TypeCode.Int32:
        //        cursor._Emit( OpCodes.Ldelem_I4 );
        //        return default;// cursor;
        //        case TypeCode.Int64:
        //        cursor._Emit( OpCodes.Ldelem_I8 );
        //        return default;// cursor;
        //        case TypeCode.Single:
        //        cursor._Emit( OpCodes.Ldelem_R4 );
        //        return default;// cursor;
        //        case TypeCode.Double:
        //        cursor._Emit( OpCodes.Ldelem_R8 );
        //        return default;// cursor;
        //        case TypeCode.Byte:
        //        cursor._Emit( OpCodes.Ldelem_U1 );
        //        return default;// cursor;
        //        case TypeCode.UInt16:
        //        cursor._Emit( OpCodes.Ldelem_U2 );
        //        return default;// cursor;
        //        case TypeCode.UInt32:
        //        cursor._Emit( OpCodes.Ldelem_U4 );
        //        return default;// cursor;
        //        case TypeCode.Empty:
        //        cursor._Emit( OpCodes.Ldelem_Ref );
        //        return default;// cursor;
        //        default:
        //        if( elementType == typeof( IntPtr ) )
        //        {
        //            cursor._Emit( OpCodes.Ldelem_I );
        //            return default;// cursor;
        //        }
        //        cursor._Emit( OpCodes.Ldelem_Any, elementType );
        //        return default;// cursor;
        //    }
        //}
        // TODO: public static TCursor LoadElementByref<TCursor>( this TCursor cursor, Type elementType, Boolean readonlyPrefix = false )
        //    where TCursor : ICursorWrite, ICursor
        //{
        //    if( readonlyPrefix ) cursor._Emit( OpCodes.Readonly );
        //    cursor._Emit( OpCodes.Ldelema, elementType );
        //    return default;// cursor;
        //}
        // TODO: public static TCursor LoadField<TCursor>( this TCursor cursor, FieldInfo field )
        //    where TCursor : ICursorWrite, ICursor
        //{
        //    if( field is null ) throw new ArgumentNullException( nameof( field ) );
        //    cursor._Emit( field.IsStatic ? OpCodes.Ldsfld : OpCodes.Ldfld, field );
        //    return default;// cursor;
        //}
        // TODO: public static TCursor LoadFieldRef<TCursor>( this TCursor cursor, FieldInfo field )
        //    where TCursor : ICursorWrite, ICursor
        //{
        //    if( field is null ) throw new ArgumentNullException( nameof( field ) );
        //    cursor._Emit( field.IsStatic ? OpCodes.Ldsflda : OpCodes.Ldflda, field );
        //    return default;// cursor;
        //}
        // TODO: public static TCursor LoadFunctionPointer<TCursor>( this TCursor cursor, MethodInfo method )
        //    where TCursor : ICursorWrite, ICursor
        //{
        //    if( method is null ) throw new ArgumentNullException( nameof( method ) );
        //    cursor._Emit( method.IsVirtual ? OpCodes.Ldvirtftn : OpCodes.Ldftn, method );
        //    return default;// cursor;
        //}
        // TODO: public static TCursor LoadLength<TCursor>( this TCursor cursor )
        //    where TCursor : ICursorWrite, ICursor
        //{
        //    cursor._Emit( OpCodes.Ldlen );
        //    return default;// cursor;
        //}
        public static ICursor<IL<TLocal,TStack>> LoadLocal<TLocal, TStack>( this ICursor<TStack> stack, Local<TLocal> local )       
            where TStack : IStack
        {
            return default;// default;// stack._Push<TLocal>( stack.cursor.LoadLocal( local.index ) );
        }
        // TODO: public static TCursor LoadLocalByref<TCursor>( this TCursor cursor, UInt16 index )
        //    where TCursor : ICursorWrite, ICursor
        //{
        //    switch( index )
        //    {
        //        case UInt16 ind when ind <= Byte.MaxValue:
        //        cursor._Emit( OpCodes.Ldloca_S, (Byte)ind );
        //        return default;// cursor;
        //        default:
        //        cursor._Emit( OpCodes.Ldloca, index );
        //        return default;// cursor;
        //    }
        //}
        // TODO: public static TCursor Dereference<TCursor>( this TCursor cursor, Type type )
        //    where TCursor : ICursorWrite, ICursor
        //{
        //    // TODO: Need to use ldind for some types
        //    cursor._Emit( OpCodes.Ldobj, type );
        //    return default;// cursor;
        //}
        public static ICursor<IL<RuntimeTypeHandle, TStack>> TypeOf<TStack>( this ICursor<TStack> stack, Type type )
            where TStack : IStack
        {
            return default;// stack._Push<RuntimeTypeHandle>(stack.cursor.TypeOf(type));
        }
        public static ICursor<IL<RuntimeFieldHandle, TStack>> FieldOf<TStack>(this ICursor<TStack> stack, FieldInfo field)
            where TStack : IStack
        {
            return default;// stack._Push<RuntimeFieldHandle>(stack.cursor.FieldOf(field));
        }
        public static ICursor<IL<RuntimeMethodHandle, TStack>> MethodOf<TStack>(this ICursor<TStack> stack, MethodInfo method)
            where TStack : IStack
        {
            return default;// stack._Push<RuntimeMethodHandle>(stack.cursor.MethodOf(method));
        }
        // TODO: public static TCursor Leave<TCursor>( this TCursor cursor, ILLabel target )
        //    where TCursor : ICursorWrite, ICursor
        //{
        //    cursor._Emit( OpCodes.Leave, target );
        //    return default;// cursor;
        //}
        // TODO: public static TCursor StackAlloc<TCursor>( this TCursor cursor )
        //    where TCursor : ICursorWrite, ICursor
        //{
        //    cursor._Emit( OpCodes.Localloc );
        //    return default;// cursor;
        //}
        // TODO: public static TCursor MakeRefAny<TCursor>( this TCursor cursor, Type type )
        //    where TCursor : ICursorWrite, ICursor
        //{
        //    cursor._Emit( OpCodes.Mkrefany, type );
        //    return default;// cursor;
        //}
        public static ICursor<IL<TRes, TStack>> Multiply<TVal1, TVal2, TRes, TStack>( this ICursor<IL<TVal2, IL<TVal1, TStack>>> stack, ILMultiply<TVal1, TVal2, TRes> settings )
            where TStack : IStack
        {
            return default;// stack._Pop()._Pop()._Push<TRes>( stack.cursor.Multiply( settings.overflow, settings.unsigned ) );
        }
        public static ICursor<IL<TRes, TStack>> Negate<TVal1, TRes, TStack>( this ICursor<IL<TVal1, TStack>> stack, ILNegate<TVal1, TRes> settings )
            where TStack : IStack
        {
            return default;// stack._Pop()._Push<TRes>( stack.cursor.Negate() );
        }
        // TODO: public static TCursor NewArray<TCursor>( this TCursor cursor, Type elementType )
        //    where TCursor : ICursorWrite, ICursor
        //{
        //    cursor._Emit( OpCodes.Newarr, elementType );
        //    return default;// cursor;
        //}
        // TODO: public static TCursor NewObject<TCursor>( this TCursor cursor, ConstructorInfo constructor )
        //    where TCursor : ICursorWrite, ICursor
        //{
        //    cursor._Emit( OpCodes.Newobj, constructor );
        //    return default;// cursor;
        //}
        public static ICursor<TStack> NoOp<TStack>( this ICursor<TStack> stack )
            where TStack : IStack
        {
            return default;// stack._Emit( stack.cursor.NoOp() );
        }
        public static ICursor<IL<TRes, TStack>> Not<TVal1, TRes, TStack>( this ICursor<IL<TVal1, TStack>> stack, ILNot<TVal1, TRes> settings )
            where TStack : IStack
        {
            return default;// stack._Pop()._Push<TRes>( stack.cursor.Not() );
        }
        public static ICursor<IL<TRes, TStack>> Or<TVal1, TVal2, TRes, TStack>( this ICursor<IL<TVal2,IL<TVal1, TStack>>> stack, ILOr<TVal1, TVal2, TRes> settings )
            where TStack : IStack
        {
            return default;// stack._Pop()._Pop()._Push<TRes>( stack.cursor.Or() );
        }
        public static ICursor<TStack> Pop<TValue, TStack>(this ICursor<IL<TValue,TStack>> stack )
            where TStack : IStack
        {
            return default;// stack._Pop( stack.cursor.Pop() );
        }
        // TODO: public static TCursor RefAnyType<TCursor>( this TCursor cursor )
        //    where TCursor : ICursorWrite, ICursor
        //{
        //    cursor._Emit( OpCodes.Refanytype );
        //    return default;// cursor;
        //}
        // TODO: public static TCursor RefAnyVal<TCursor>( this TCursor cursor, Type type )
        //    where TCursor : ICursorWrite, ICursor
        //{
        //    cursor._Emit( OpCodes.Refanyval, type );
        //    return default;// cursor;
        //}
        public static ICursor<IL<TRes, TStack>> Modulus<TVal1, TVal2, TRes, TStack>( this ICursor<IL<TVal2, IL<TVal1, TStack>>> stack, ILModulus<TVal1, TVal2, TRes> settings )
            where TStack : IStack
        {
            return default;// stack._Pop()._Pop()._Push<TRes>( stack.cursor.Modulus( settings.unsigned ) );
        }
        public static DMDReturn Return( this ICursor<Empty> stack )
        {
            // TODO: return default;// creation
            return default;// default;
        }
        public static DMDReturn<TReturn> Return<TReturn>(this ICursor<IL<TReturn, Empty>> stack)//<Treturn default;//> return default;//<Treturn default;//>( this ICursor<IL<Treturn default;//, Empty>> stack )
        {
            // TODO: return default;// creation
            return default;// default;
        }
        // TODO: public static TCursor ReThrow<TCursor>( this TCursor cursor )
        //    where TCursor : ICursorWrite, ICursor
        //{
        //    cursor._Emit( OpCodes.Rethrow );
        //    return default;// cursor;
        //}
        public static ICursor<IL<TRes, TStack>> ShiftL<TVal1, TVal2, TRes, TStack>( this ICursor<IL<TVal2, IL<TVal1, TStack>>> stack, ILShiftL<TVal1, TVal2, TRes> settings )
            where TStack : IStack
        {
            return default;// stack._Pop()._Pop()._Push<TRes>( stack.cursor.ShiftL() );
        }
        public static ICursor<IL<TRes, TStack>> ShiftR<TVal1, TVal2, TRes, TStack>( this ICursor<IL<TVal2, IL<TVal1, TStack>>> stack, ILShiftR<TVal1, TVal2, TRes> settings )
            where TStack : IStack
        {
            return default;// stack._Pop()._Pop()._Push<TRes>( stack.cursor.ShiftR( settings.unsigned ) );
        }
        public static ICursor<IL<Int32,TStack>> SizeOf<TStack>( this ICursor<TStack> stack, Type type )
            where TStack : IStack
        {
            return default;// stack._Push<Int32>( stack.cursor.SizeOf( type ) );
        }
        public static ICursor<TStack> StoreArg<TArg, TStack>( this ICursor<IL<TArg,TStack>> stack, Arg<TArg> arg )
            where TStack : IStack
        {
            return default;// default;// stack._Pop( stack.cursor.StoreArg( arg.index ) );
        }
        // TODO: public static TCursor StoreElement<TCursor>( this TCursor cursor, Type elementType )
        //    where TCursor : ICursorWrite, ICursor
        //{
        //    switch( Type.GetTypeCode( elementType ) )
        //    {
        //        case TypeCode.SByte:
        //        cursor._Emit( OpCodes.Stelem_I1 );
        //        return default;// cursor;
        //        case TypeCode.Int16:
        //        cursor._Emit( OpCodes.Stelem_I2 );
        //        return default;// cursor;
        //        case TypeCode.Int32:
        //        cursor._Emit( OpCodes.Stelem_I4 );
        //        return default;// cursor;
        //        case TypeCode.Int64:
        //        cursor._Emit( OpCodes.Stelem_I8 );
        //        return default;// cursor;
        //        case TypeCode.Single:
        //        cursor._Emit( OpCodes.Stelem_R4 );
        //        return default;// cursor;
        //        case TypeCode.Double:
        //        cursor._Emit( OpCodes.Stelem_R8 );
        //        return default;// cursor;
        //        case TypeCode.Empty:
        //        cursor._Emit( OpCodes.Stelem_Ref );
        //        return default;// cursor;
        //        default:
        //        if( elementType == typeof( IntPtr ) )
        //        {
        //            cursor._Emit( OpCodes.Stelem_I );
        //            return default;// cursor;
        //        }
        //        cursor._Emit( OpCodes.Stelem_Any, elementType );
        //        return default;// cursor;
        //    }
        //}
        // TODO: public static TCursor StoreField<TCursor>( this TCursor cursor, FieldInfo field )
        //    where TCursor : ICursorWrite, ICursor
        //{
        //    if( field is null ) throw new ArgumentNullException( nameof( field ) );
        //    cursor._Emit( field.IsStatic ? OpCodes.Stsfld : OpCodes.Stfld, field );
        //    return default;// cursor;
        //}
        // TODO: public static TCursor StoreObject<TCursor>( this TCursor cursor, Type type )
        //    where TCursor : ICursorWrite, ICursor
        //{
        //    // TODO: StInd versions
        //    cursor._Emit( OpCodes.Stobj, type );
        //    return default;// cursor;
        //}
        public static ICursor<TStack> StoreLocal<TLocal, TStack>( this ICursor<IL<TLocal,TStack>> stack, Local<TLocal> local )
            where TStack : IStack
        {
            return default;// default;// stack._Pop( stack.cursor.StoreLocal( local.index ) );
        }
        public static ICursor<IL<TRes, TStack>> Subtract<TVal1, TVal2, TRes, TStack>( this ICursor<IL<TVal2, IL<TVal1, TStack>>> stack, ILSubtract<TVal1, TVal2, TRes> settings )
            where TStack : IStack
        {
            return default;// stack._Pop()._Pop()._Push<TRes>( stack.cursor.Subtract(settings.overflow, settings.unsigned) );
        }
        // TODO: public static TCursor Switch<TCursor>( this TCursor cursor, ILLabel[] targets )
        //    where TCursor : ICursorWrite, ICursor
        //{
        //    cursor._Emit( OpCodes.Switch, targets );
        //    return default;// cursor;
        //}
        // TODO: public static TCursor Throw<TCursor>( this TCursor cursor )
        //    where TCursor : ICursorWrite, ICursor
        //{
        //    cursor._Emit( OpCodes.Throw );
        //    return default;// cursor;
        //}
        public static ICursor<IL<ByRef<TBoxed>, TStack>> Unbox<TBoxed, TStack>( this ICursor<IL<Boxed<TBoxed>,TStack>> stack )
            where TBoxed : struct
            where TStack : IStack
        {
            return default;// stack._Pop()._Push<ByRef<TBoxed>>(stack.cursor.Unbox(typeof(TBoxed)));
        }
        public static ICursor<IL<TBoxed, TStack>> UnboxAny<TBoxed, TStack>( this ICursor<IL<Boxed<TBoxed>, TStack>> stack )
            where TBoxed : struct
            where TStack : IStack
        {
            return default;// stack._Pop()._Push<TBoxed>(stack.cursor.UnboxAny(typeof(TBoxed)));
        }
        public static ICursor<IL<TBoxed, TStack>> UnboxAny<TBoxed, TFrom, TStack>( this ICursor<IL<TFrom,TStack>> stack, TRef<TBoxed> type )
            where TStack : IStack
        {
            return default;// stack._Pop()._Push<TBoxed>(stack.cursor.UnboxAny(typeof(TBoxed)));
        }
        //// TODO: Unaligned
        //// TODO: Volatile
        public static ICursor<IL<TRes, TStack>> XOr<TVal1, TVal2, TRes, TStack>( this ICursor<IL<TVal2, IL<TVal1, TStack>>> stack, ILXOr<TVal1, TVal2, TRes> settings )
            where TStack : IStack
        {
            return default;// stack._Pop()._Pop()._Push<TRes>( stack.cursor.XOr() );
        }
        #endregion
    }
    #pragma warning restore IDE0022 // Use expression body for methods
}
