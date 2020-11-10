namespace ReinCore
{
    using System;
    using MonoMod.Cil;
    using Mono.Cecil;
    using MonoMod.Utils;
    using System.Linq;
    using Mono.Cecil.Cil;
    using System.Reflection;

    public static class Match
    {

    }

    public readonly struct ConvType
    {
        internal readonly OpCode opcode;

        internal ConvType(OpCode opcode) => this.opcode = opcode;

        public static readonly ConvType I = new(OpCodes.Conv_I);
        public static readonly ConvType Ovf_I = new(OpCodes.Conv_Ovf_I);
        public static readonly ConvType Ovf_I_Un = new(OpCodes.Conv_Ovf_I_Un);

        public static readonly ConvType U = new(OpCodes.Conv_U);
        public static readonly ConvType Ovf_U = new(OpCodes.Conv_Ovf_U);
        public static readonly ConvType Ovf_U_Un = new(OpCodes.Conv_Ovf_U_Un);

        public static readonly ConvType I1 = new(OpCodes.Conv_I1);
        public static readonly ConvType Ovf_I1 = new(OpCodes.Conv_Ovf_I1);
        public static readonly ConvType Ovf_I1_Un = new(OpCodes.Conv_Ovf_I1_Un);

        public static readonly ConvType U1 = new(OpCodes.Conv_U1);
        public static readonly ConvType Ovf_U1 = new(OpCodes.Conv_Ovf_U1);
        public static readonly ConvType Ovf_U1_Un = new(OpCodes.Conv_Ovf_U1_Un);

        public static readonly ConvType I2 = new(OpCodes.Conv_I2);
        public static readonly ConvType Ovf_I2 = new(OpCodes.Conv_Ovf_I2);
        public static readonly ConvType Ovf_I2_Un = new(OpCodes.Conv_Ovf_I2_Un);

        public static readonly ConvType U2 = new(OpCodes.Conv_U2);
        public static readonly ConvType Ovf_U2 = new(OpCodes.Conv_Ovf_U2);
        public static readonly ConvType Ovf_U2_Un = new(OpCodes.Conv_Ovf_U2_Un);

        public static readonly ConvType I4 = new(OpCodes.Conv_I4);
        public static readonly ConvType Ovf_I4 = new(OpCodes.Conv_Ovf_I4);
        public static readonly ConvType Ovf_I4_Un = new(OpCodes.Conv_Ovf_I4_Un);

        public static readonly ConvType U4 = new(OpCodes.Conv_U4);
        public static readonly ConvType Ovf_U4 = new(OpCodes.Conv_Ovf_U4);
        public static readonly ConvType Ovf_U4_Un = new(OpCodes.Conv_Ovf_U4_Un);

        public static readonly ConvType I8 = new(OpCodes.Conv_I8);
        public static readonly ConvType Ovf_I8 = new(OpCodes.Conv_Ovf_I8);
        public static readonly ConvType Ovf_I8_Un = new(OpCodes.Conv_Ovf_I8_Un);

        public static readonly ConvType U8 = new(OpCodes.Conv_U8);
        public static readonly ConvType Ovf_U8 = new(OpCodes.Conv_Ovf_U8);
        public static readonly ConvType Ovf_U8_Un = new(OpCodes.Conv_Ovf_U8_Un);

        public static readonly ConvType R_Un = new(OpCodes.Conv_R_Un);
        public static readonly ConvType R4 = new(OpCodes.Conv_R4);
        public static readonly ConvType R8 = new(OpCodes.Conv_R8);
    }
    public static class ILCursorExtensions
    {
        public static ILCursor Position(this ILCursor cursor, out Int32 position)
        {
            position = cursor.Index;
            return cursor;
        }

        public static ILCursor Position(this ILCursor cursor, Int32 position)
        {
            cursor.Index = position;
            return cursor;
        }
        public static ILCursor Log(this ILCursor cursor, BepInEx.Logging.ManualLogSource logger)
        {
            logger.LogWarning(cursor);
            return cursor;
        }
        public static ILCursor LogFull(this ILCursor cursor, BepInEx.Logging.ManualLogSource logger)
        {
            logger.LogWarning(cursor.Context);
            return cursor;
        }
        public static ILCursor DefLabel(this ILCursor cursor, out ILLabel label)
        {
            label = cursor.DefineLabel();
            return cursor;
        }
        public static ILCursor Mark(this ILCursor cursor, out ILLabel label)
        {
            label = cursor.MarkLabel();
            return cursor;
        }
        public static ILCursor Mark(this ILCursor cursor, ILLabel label)
        {
            cursor.MarkLabel(label);
            return cursor;
        }

        public static ILCursor Add_(this ILCursor cursor) => cursor.Emit(OpCodes.Add);
        public static ILCursor Add_Ovf_(this ILCursor cursor, Boolean unsigned = false) => cursor.Emit(unsigned ? OpCodes.Add_Ovf_Un : OpCodes.Add_Ovf);
        public static ILCursor And_(this ILCursor cursor) => cursor.Emit(OpCodes.And);
        public static ILCursor Arglist_(this ILCursor cursor) => cursor.Emit(OpCodes.Arglist);
        public static ILCursor Beq_(this ILCursor cursor, ILLabel target) => cursor.Emit(OpCodes.Beq, target);
        public static ILCursor Bge_(this ILCursor cursor, ILLabel target, Boolean unsigned = false) => cursor.Emit(unsigned ? OpCodes.Bge_Un : OpCodes.Bge_Un, target);
        public static ILCursor Bgt_(this ILCursor cursor, ILLabel target, Boolean unsigned = false) => cursor.Emit(unsigned ? OpCodes.Bgt_Un : OpCodes.Bgt_Un, target);
        public static ILCursor Ble_(this ILCursor cursor, ILLabel target, Boolean unsigned = false) => cursor.Emit(unsigned ? OpCodes.Ble_Un : OpCodes.Ble_Un, target);
        public static ILCursor Blt_(this ILCursor cursor, ILLabel target, Boolean unsigned = false) => cursor.Emit(unsigned ? OpCodes.Blt_Un : OpCodes.Blt_Un, target);
        public static ILCursor Bne_Un_(this ILCursor cursor, ILLabel target) => cursor.Emit(OpCodes.Bne_Un, target);
        public static ILCursor Box_<T>(this ILCursor cursor) where T : struct => cursor.Box_(typeof(T));
        public static ILCursor Box_(this ILCursor cursor, Type type) => cursor.Box_(type);
        public static ILCursor Br_(this ILCursor cursor, ILLabel label) => cursor.Emit(OpCodes.Br, label);
        public static ILCursor Break_(this ILCursor cursor) => cursor.Emit(OpCodes.Break);
        public static ILCursor BrFalse_(this ILCursor cursor, ILLabel target) => cursor.Emit(OpCodes.Brfalse, target);
        public static ILCursor BrTrue_(this ILCursor cursor, ILLabel target) => cursor.Emit(OpCodes.Brtrue, target);
        public static ILCursor Call_(this ILCursor cursor, MethodInfo method, Boolean tail = false) => tail ? cursor.Emit(OpCodes.Tail).Emit(OpCodes.Call).Emit(OpCodes.Ret) : cursor.Emit(OpCodes.Call, method);
        public static ILCursor CallVirt_(this ILCursor cursor, MethodInfo method, Boolean tail = false) => tail ? cursor.Emit(OpCodes.Tail).Emit(OpCodes.Callvirt).Emit(OpCodes.Ret) : cursor.Emit(OpCodes.Callvirt, method);
        public static ILCursor CallVirt_Constr_(this ILCursor cursor, MethodInfo method, Type type, Boolean tail = false) => cursor.Emit(OpCodes.Constrained, type).CallVirt_(method, tail);
        public static ILCursor CallVirt_Constr_<T>(this ILCursor cursor, MethodInfo method, Boolean tail = false) => cursor.CallVirt_Constr_(method, typeof(T), tail);      
        public static ILCursor CallDelegate_<TDelegate>(this ILCursor cursor, TDelegate method) where TDelegate : Delegate
        {
            cursor.EmitDelegate<TDelegate>(method);
            return cursor;
        }
        public static ILCursor Cast_(this ILCursor cursor, Type type) => cursor.Emit(OpCodes.Castclass, type);
        public static ILCursor Cast_<T>(this ILCursor cursor) => cursor.Cast_(typeof(T));
        public static ILCursor Ceq_(this ILCursor cursor) => cursor.Emit(OpCodes.Ceq);
        public static ILCursor Cgt_(this ILCursor cursor, Boolean unsigned = false) => cursor.Emit(unsigned ? OpCodes.Cgt_Un : OpCodes.Cgt);
        public static ILCursor CkFinite_(this ILCursor cursor) => cursor.Emit(OpCodes.Ckfinite);
        public static ILCursor Clt_(this ILCursor cursor, Boolean unsigned = false) => cursor.Emit(unsigned ? OpCodes.Clt_Un : OpCodes.Clt);
        public static ILCursor Conv_(this ILCursor cursor, ConvType conv) => cursor.Emit(conv.opcode);
        public static ILCursor CpBlk_(this ILCursor cursor, Boolean vol = false) => (vol ? cursor.Emit(OpCodes.Volatile) : cursor).Emit(OpCodes.Cpblk);
        public static ILCursor CpObj_(this ILCursor cursor, Type type) => cursor.Emit(OpCodes.Cpobj, type);
        public static ILCursor CpObj_<T>(this ILCursor cursor) where T : struct => cursor.CpObj_(typeof(T));
        public static ILCursor Div_(this ILCursor cursor, Boolean unsigned = false) => cursor.Emit(unsigned ? OpCodes.Div : OpCodes.Div_Un);
        public static ILCursor Dup_(this ILCursor cursor) => cursor.Emit(OpCodes.Dup);
        public static ILCursor EndFilter_(this ILCursor cursor) => cursor.Emit(OpCodes.Endfilter);
        public static ILCursor EndFinally_(this ILCursor cursor) => cursor.Emit(OpCodes.Endfinally);
        public static ILCursor InitBlk_(this ILCursor cursor, Boolean vol = false) => (vol ? cursor.Emit(OpCodes.Volatile) : cursor).Emit(OpCodes.Initblk);
        public static ILCursor InitObj_(this ILCursor cursor, Type type) => cursor.Emit(OpCodes.Initobj, type);
        public static ILCursor InitObj_<T>(this ILCursor cursor) => cursor.InitObj_(typeof(T));
        public static ILCursor Is_(this ILCursor cursor, Type t) => cursor.Emit(OpCodes.Isinst, t);
        public static ILCursor Is_<T>(this ILCursor cursor) where T : class => cursor.Is_(typeof(T));
        public static ILCursor Jmp_(this ILCursor cursor, MethodInfo target) => cursor.Emit(OpCodes.Jmp, target);
        public static ILCursor LdArg_(this ILCursor cursor, UInt16 index) => index switch
        {
            0 => cursor.Emit(OpCodes.Ldarg_0),
            1 => cursor.Emit(OpCodes.Ldarg_1),
            2 => cursor.Emit(OpCodes.Ldarg_2),
            3 => cursor.Emit(OpCodes.Ldarg_3),
            UInt16 i when i >= Byte.MinValue && i <= Byte.MaxValue => cursor.Emit(OpCodes.Ldarg_S, (Byte)index),
            _ => cursor.Emit(OpCodes.Ldarg, index),
        };
        public static ILCursor LdArgA_(this ILCursor cursor, UInt16 index) => index switch
        {
            UInt16 i when i >= Byte.MinValue && i <= Byte.MaxValue => cursor.Emit(OpCodes.Ldarga_S, (Byte)index),
            _ => cursor.Emit(OpCodes.Ldarga, index),
        };
        public static ILCursor LdC_(this ILCursor cursor, String value) => cursor.Emit(OpCodes.Ldstr, value);
        public static ILCursor LdC_(this ILCursor cursor, Int32 value) => value switch
        {
            0 => cursor.Emit(OpCodes.Ldc_I4_0),
            1 => cursor.Emit(OpCodes.Ldc_I4_1),
            2 => cursor.Emit(OpCodes.Ldc_I4_2),
            3 => cursor.Emit(OpCodes.Ldc_I4_3),
            4 => cursor.Emit(OpCodes.Ldc_I4_4),
            5 => cursor.Emit(OpCodes.Ldc_I4_5),
            6 => cursor.Emit(OpCodes.Ldc_I4_6),
            7 => cursor.Emit(OpCodes.Ldc_I4_7),
            8 => cursor.Emit(OpCodes.Ldc_I4_8),
            -1 => cursor.Emit(OpCodes.Ldc_I4_M1),
            Int32 i when i >= SByte.MinValue && i <= SByte.MaxValue => cursor.Emit(OpCodes.Ldc_I4_S, (SByte)i),
            _ => cursor.Emit(OpCodes.Ldc_I4, value),
        };
        public static ILCursor LdC_(this ILCursor cursor, Int64 value) => cursor.Emit(OpCodes.Ldc_I8, value);
        public static ILCursor LdC_(this ILCursor cursor, Single value) => cursor.Emit(OpCodes.Ldc_R4, value);
        public static ILCursor LdC_(this ILCursor cursor, Double value) => cursor.Emit(OpCodes.Ldc_R8, value);
        public static ILCursor LdElem_(this ILCursor cursor, Type type) => type switch
        {
            Type t when t == typeof(IntPtr) => cursor.Emit(OpCodes.Ldelem_I),
            Type t when t == typeof(SByte) => cursor.Emit(OpCodes.Ldelem_I1),
            Type t when t == typeof(Int16) => cursor.Emit(OpCodes.Ldelem_I2),
            Type t when t == typeof(Int32) => cursor.Emit(OpCodes.Ldelem_I4),
            Type t when t == typeof(Int64) => cursor.Emit(OpCodes.Ldelem_I8),
            Type t when t == typeof(Single) => cursor.Emit(OpCodes.Ldelem_R4),
            Type t when t == typeof(Double) => cursor.Emit(OpCodes.Ldelem_R8),
            Type t when t == typeof(Byte) => cursor.Emit(OpCodes.Ldelem_U1),
            Type t when t == typeof(UInt16) => cursor.Emit(OpCodes.Ldelem_U2),
            Type t when t == typeof(UInt32) => cursor.Emit(OpCodes.Ldelem_U4),
            Type t when t == typeof(Object) => cursor.Emit(OpCodes.Ldelem_Ref),
            _ => cursor.Emit(OpCodes.Ldelem_Any, type),
        };
        public static ILCursor LdElem_<T>(this ILCursor cursor) => cursor.LdElem_(typeof(T));
        public static ILCursor LdElemA(this ILCursor cursor, Boolean readOnly = false) => (readOnly ? cursor.Emit(OpCodes.Readonly) : cursor).Emit(OpCodes.Ldelema);
        public static ILCursor LdFld_(this ILCursor cursor, FieldInfo field, Boolean vol = false) => (vol ? cursor.Emit(OpCodes.Volatile) : cursor).Emit(OpCodes.Ldfld, field);
        public static ILCursor LdFldA_(this ILCursor cursor, FieldInfo field) => cursor.Emit(OpCodes.Ldflda, field);
        public static ILCursor LdFtn_(this ILCursor cursor, MethodInfo method) => cursor.Emit(OpCodes.Ldftn, method);
        public static ILCursor LdInd_(this ILCursor cursor, Type type, Boolean vol = false) => type switch
        {
            Type t when t == typeof(IntPtr) => (vol ? cursor.Emit(OpCodes.Volatile) : cursor).Emit(OpCodes.Ldind_I),
            Type t when t == typeof(SByte) => (vol ? cursor.Emit(OpCodes.Volatile) : cursor).Emit(OpCodes.Ldind_I1),
            Type t when t == typeof(Int16) => (vol ? cursor.Emit(OpCodes.Volatile) : cursor).Emit(OpCodes.Ldind_I2),
            Type t when t == typeof(Int32) => (vol ? cursor.Emit(OpCodes.Volatile) : cursor).Emit(OpCodes.Ldind_I4),
            Type t when t == typeof(Int64) => (vol ? cursor.Emit(OpCodes.Volatile) : cursor).Emit(OpCodes.Ldind_I8),
            Type t when t == typeof(Single) => (vol ? cursor.Emit(OpCodes.Volatile) : cursor).Emit(OpCodes.Ldind_R4),
            Type t when t == typeof(Double) => (vol ? cursor.Emit(OpCodes.Volatile) : cursor).Emit(OpCodes.Ldind_R8),
            Type t when t == typeof(Byte) => (vol ? cursor.Emit(OpCodes.Volatile) : cursor).Emit(OpCodes.Ldind_U1),
            Type t when t == typeof(UInt16) => (vol ? cursor.Emit(OpCodes.Volatile) : cursor).Emit(OpCodes.Ldind_U2),
            Type t when t == typeof(UInt32) => (vol ? cursor.Emit(OpCodes.Volatile) : cursor).Emit(OpCodes.Ldind_U4),
            Type t when t == typeof(Object) => (vol ? cursor.Emit(OpCodes.Volatile) : cursor).Emit(OpCodes.Ldind_Ref),
            _ => throw new ArgumentException(),
        };
        public static ILCursor LdInd_<T>(this ILCursor cursor, Boolean vol = false) => cursor.LdInd_(typeof(T), vol);
        public static ILCursor LdLen_(this ILCursor cursor) => cursor.Emit(OpCodes.Ldlen);
        public static ILCursor LdLoc_(this ILCursor cursor, UInt16 index) => index switch
        {
            0 => cursor.Emit(OpCodes.Ldloc_0),
            1 => cursor.Emit(OpCodes.Ldloc_1),
            2 => cursor.Emit(OpCodes.Ldloc_2),
            3 => cursor.Emit(OpCodes.Ldloc_3),
            UInt16 i when i >= Byte.MinValue && i <= Byte.MaxValue => cursor.Emit(OpCodes.Ldloc_S, (Byte)i),
            _ => cursor.Emit(OpCodes.Ldloc, index),
        };
        public static ILCursor LdLocA_(this ILCursor cursor, UInt16 index) => index switch
        {
            UInt16 i when i >= Byte.MinValue && i <= Byte.MaxValue => cursor.Emit(OpCodes.Ldloca_S, (Byte)i),
            _ => cursor.Emit(OpCodes.Ldloca, index),
        };
        public static ILCursor LdNull_(this ILCursor cursor) => cursor.Emit(OpCodes.Ldnull);
        public static ILCursor LdObj_(this ILCursor cursor, Type type, Boolean vol = false) => (vol ? cursor.Emit(OpCodes.Volatile) : cursor).Emit(OpCodes.Ldobj, type);
        public static ILCursor LdObj_<T>(this ILCursor cursor, Boolean vol = false) where T : struct => cursor.LdObj_(typeof(T), vol);
        public static ILCursor LdSFld_(this ILCursor cursor, FieldInfo field) => cursor.Emit(OpCodes.Ldsfld, field);
        public static ILCursor LdSFldA_(this ILCursor cursor, FieldInfo field) => cursor.Emit(OpCodes.Ldsflda, field);
        public static ILCursor LdStr_(this ILCursor cursor, String text) => cursor.LdC_(text);
        public static ILCursor LdToken_(this ILCursor cursor, Type type) => cursor.Emit(OpCodes.Ldtoken, type);
        public static ILCursor LdToken_(this ILCursor cursor, FieldInfo field) => cursor.Emit(OpCodes.Ldtoken, field);
        public static ILCursor LdToken_(this ILCursor cursor, MethodInfo method) => cursor.Emit(OpCodes.Ldtoken, method);
        public static ILCursor LdToken_<T>(this ILCursor cursor) => cursor.Emit(OpCodes.Ldtoken, typeof(T));
        public static ILCursor LdVirtFtn_(this ILCursor cursor, MethodInfo method) => cursor.Emit(OpCodes.Ldvirtftn, method);
        public static ILCursor Leave_(this ILCursor cursor, ILLabel target) => cursor.Emit(OpCodes.Leave, target);
        public static ILCursor LocAlloc_(this ILCursor cursor) => cursor.Emit(OpCodes.Localloc);
        public static ILCursor MkRefAny_(this ILCursor cursor, Type type) => cursor.Emit(OpCodes.Mkrefany, type);
        public static ILCursor MkRefAny_<T>(this ILCursor cursor) => cursor.MkRefAny_(typeof(T));
        public static ILCursor Mul_(this ILCursor cursor) => cursor.Emit(OpCodes.Mul);
        public static ILCursor Mul_Ovf_(this ILCursor cursor, Boolean unsigned = false) => cursor.Emit(unsigned ? OpCodes.Mul_Ovf_Un : OpCodes.Mul_Ovf);
        public static ILCursor Neg_(this ILCursor cursor) => cursor.Emit(OpCodes.Neg);
        public static ILCursor NewArr_(this ILCursor cursor, Type type) => cursor.Emit(OpCodes.Newarr, type);
        public static ILCursor NewArr_<T>(this ILCursor cursor) => cursor.NewArr_(typeof(T));
        public static ILCursor NewObj_(this ILCursor cursor, ConstructorInfo ctor) => cursor.Emit(OpCodes.Newobj, ctor);
        public static ILCursor Nop_(this ILCursor cursor) => cursor.Emit(OpCodes.Nop);
        public static ILCursor Not_(this ILCursor cursor) => cursor.Emit(OpCodes.Not);
        public static ILCursor Or_(this ILCursor cursor) => cursor.Emit(OpCodes.Or);
        public static ILCursor Pop_(this ILCursor cursor) => cursor.Emit(OpCodes.Pop);
        public static ILCursor RefAnyType_(this ILCursor cursor) => cursor.Emit(OpCodes.Refanytype);
        public static ILCursor RefAnyVal_(this ILCursor cursor) => cursor.Emit(OpCodes.Refanyval);
        public static ILCursor Rem_(this ILCursor cursor, Boolean unsigned = false) => cursor.Emit(unsigned ? OpCodes.Rem_Un : OpCodes.Rem);
        public static ILCursor Ret_(this ILCursor cursor) => cursor.Emit(OpCodes.Ret);
        public static ILCursor ReThrow_(this ILCursor cursor) => cursor.Emit(OpCodes.Rethrow);
        public static ILCursor Shl_(this ILCursor cursor) => cursor.Emit(OpCodes.Shl);
        public static ILCursor Shr_(this ILCursor cursor, Boolean unsigned = false) => cursor.Emit(unsigned ? OpCodes.Shr_Un : OpCodes.Shr);
        public static ILCursor SizeOf_(this ILCursor cursor, Type type) => cursor.Emit(OpCodes.Sizeof, type);
        public static ILCursor SizeOf_<T>(this ILCursor cursor) => cursor.SizeOf_(typeof(T));
        public static ILCursor StArg_(this ILCursor cursor, UInt16 index) => index switch
        {
            UInt16 i when i >= Byte.MinValue && i <= Byte.MaxValue => cursor.Emit(OpCodes.Starg_S, (Byte)index),
            _ => cursor.Emit(OpCodes.Starg, index),
        };
        public static ILCursor StElem_(this ILCursor cursor, Type type) => type switch
        {
            Type t when t == typeof(IntPtr) => cursor.Emit(OpCodes.Stelem_I),
            Type t when t == typeof(SByte) => cursor.Emit(OpCodes.Stelem_I1),
            Type t when t == typeof(Int16) => cursor.Emit(OpCodes.Stelem_I2),
            Type t when t == typeof(Int32) => cursor.Emit(OpCodes.Stelem_I4),
            Type t when t == typeof(Int64) => cursor.Emit(OpCodes.Stelem_I8),
            Type t when t == typeof(Single) => cursor.Emit(OpCodes.Stelem_R4),
            Type t when t == typeof(Double) => cursor.Emit(OpCodes.Stelem_R8),
            Type t when t == typeof(Object) => cursor.Emit(OpCodes.Stelem_Ref),
            _ => cursor.Emit(OpCodes.Stelem_Any, type),
        };
        public static ILCursor StElem_<T>(this ILCursor cursor) => cursor.StElem_(typeof(T));
        public static ILCursor StFld_(this ILCursor cursor, FieldInfo field, Boolean vol = false) => (vol ? cursor.Emit(OpCodes.Volatile) : cursor).StFld_(field);
        public static ILCursor StInd_(this ILCursor cursor, Type type, Boolean vol = false) => type switch
        {
            Type t when t == typeof(IntPtr) => (vol ? cursor.Emit(OpCodes.Volatile) : cursor).Emit(OpCodes.Stind_I),
            Type t when t == typeof(SByte) => (vol ? cursor.Emit(OpCodes.Volatile) : cursor).Emit(OpCodes.Stind_I1),
            Type t when t == typeof(Int16) => (vol ? cursor.Emit(OpCodes.Volatile) : cursor).Emit(OpCodes.Stind_I2),
            Type t when t == typeof(Int32) => (vol ? cursor.Emit(OpCodes.Volatile) : cursor).Emit(OpCodes.Stind_I4),
            Type t when t == typeof(Int64) => (vol ? cursor.Emit(OpCodes.Volatile) : cursor).Emit(OpCodes.Stind_I8),
            Type t when t == typeof(Single) => (vol ? cursor.Emit(OpCodes.Volatile) : cursor).Emit(OpCodes.Stind_R4),
            Type t when t == typeof(Double) => (vol ? cursor.Emit(OpCodes.Volatile) : cursor).Emit(OpCodes.Stind_R8),
            Type t when t == typeof(Object) => (vol ? cursor.Emit(OpCodes.Volatile) : cursor).Emit(OpCodes.Stind_Ref),
            _ => throw new ArgumentException(),
        };
        public static ILCursor StInd_<T>(this ILCursor cursor, Boolean vol = false) => cursor.StInd_(typeof(T), vol);
        public static ILCursor StLoc_(this ILCursor cursor, UInt16 index) => index switch
        {
            0 => cursor.Emit(OpCodes.Stloc_0),
            1 => cursor.Emit(OpCodes.Stloc_1),
            2 => cursor.Emit(OpCodes.Stloc_2),
            3 => cursor.Emit(OpCodes.Stloc_3),
            UInt16 i when i >= Byte.MinValue && i <= Byte.MaxValue => cursor.Emit(OpCodes.Stloc_S, (Byte)i),
            _ => cursor.Emit(OpCodes.Stloc, index),
        };
        public static ILCursor StObj_(this ILCursor cursor, Type type, Boolean vol = false) => (vol ? cursor.Emit(OpCodes.Volatile) : cursor).Emit(OpCodes.Stobj, type);
        public static ILCursor StObj_<T>(this ILCursor cursor, Boolean vol = false) where T : struct => cursor.StObj_(typeof(T), vol);
        public static ILCursor StSFld_(this ILCursor cursor, FieldInfo field) => cursor.Emit(OpCodes.Stsfld, field);
        public static ILCursor Sub_(this ILCursor cursor) => cursor.Emit(OpCodes.Sub);
        public static ILCursor Sub_Ovf_(this ILCursor cursor, Boolean unsigned = false) => cursor.Emit(unsigned ? OpCodes.Sub_Ovf_Un : OpCodes.Sub_Ovf);
        public static ILCursor Switch_(this ILCursor cursor, params ILLabel[] targets) => cursor.Emit(OpCodes.Switch, targets);
        public static ILCursor Throw_(this ILCursor cursor) => cursor.Emit(OpCodes.Throw);
        public static ILCursor Unbox_(this ILCursor cursor, Type type) => cursor.Emit(OpCodes.Unbox, type);
        public static ILCursor Unbox_<T>(this ILCursor cursor) where T : struct => cursor.Unbox_(typeof(T));
        public static ILCursor Unbox_Any_(this ILCursor cursor, Type type) => cursor.Emit(OpCodes.Unbox_Any, type);
        public static ILCursor Unbox_Any_<T>(this ILCursor cursor) => cursor.Emit(OpCodes.Unbox_Any, typeof(T));
        public static ILCursor Xor_(this ILCursor cursor) => cursor.Emit(OpCodes.Xor);

        public static ILCursor Move(this ILCursor cursor, Int32 offset)
        {
            cursor.Index += offset;
            return cursor;
        }

        public static ILCursor AddLocal<T>(this ILCursor cursor, out Int32 index)
        {
            index = cursor.Context.Body.Variables.Count;
            cursor.Context.Body.Variables.Add(new VariableDefinition(cursor.Context.Import(typeof(T))));
            return cursor;
        }

        private static ILCursor EmitNewObj(this ILCursor cursor, ConstructorInfo constructor) => cursor.Emit(OpCodes.Newobj, constructor);
















        public static ILCursor Log(this ILCursor cursor)
        {
            global::ReinCore.Log.Warning(cursor);
            return cursor;
        }
        public static ILCursor LogFull(this ILCursor cursor)
        {
            global::ReinCore.Log.Warning(cursor.Context);
            return cursor;
        }
        public static ILCursor CallDel_<TDelegate>(this ILCursor cursor, TDelegate target, out Int32 index)
    where TDelegate : Delegate
        {
            index = cursor.EmitDelegate<TDelegate>(target);
            return cursor;
        }
        public static ILCursor CallDel_<TDelegate>(this ILCursor cursor, TDelegate target)
            where TDelegate : Delegate => cursor.CallDel_(target, out _);
    }
}

namespace ILHelpers
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
        public static ILCursor Position(this ILCursor cursor, out Int32 position)
        {
            position = cursor.Index;
            return cursor;
        }

        public static ILCursor Position(this ILCursor cursor, Int32 position)
        {
            cursor.Index = position;
            return cursor;
        }
        public static ILCursor Log(this ILCursor cursor, BepInEx.Logging.ManualLogSource logger)
        {
            logger.LogWarning(cursor);
            return cursor;
        }
        public static ILCursor LogFull(this ILCursor cursor, BepInEx.Logging.ManualLogSource logger)
        {
            logger.LogWarning(cursor.Context);
            return cursor;
        }
        public static ILCursor DefLabel(this ILCursor cursor, out ILLabel label)
        {
            label = cursor.DefineLabel();
            return cursor;
        }
        public static ILCursor Mark(this ILCursor cursor, out ILLabel label)
        {
            label = cursor.MarkLabel();
            return cursor;
        }
        public static ILCursor Mark(this ILCursor cursor, ILLabel label)
        {
            cursor.MarkLabel(label);
            return cursor;
        }
        public static ILCursor Move(this ILCursor cursor, Int32 offset)
        {
            cursor.Index += offset;
            return cursor;
        }
        public static ILCursor AddRef<T>(this ILCursor cursor, T value, out Int32 index)
        {
            index = cursor.AddReference<T>(value);
            return cursor;
        }
        public static ILCursor GetRef<T>(this ILCursor cursor, Int32 index)
        {
            cursor.EmitGetReference<T>(index);
            return cursor;
        }
        public static ILCursor EmitRef<T>(this ILCursor cursor, T value, out Int32 index)
        {
            index = cursor.EmitReference<T>(value);
            return cursor;
        }


        public static ILCursor Add_(this ILCursor cursor) => cursor.Emit(OpCodes.Add);
        public static ILCursor Add_Ovf_(this ILCursor cursor, Boolean unsigned = false) => cursor.Emit(unsigned ? OpCodes.Add_Ovf_Un : OpCodes.Add_Ovf);
        public static ILCursor And_(this ILCursor cursor) => cursor.Emit(OpCodes.And);
        public static ILCursor Arglist_(this ILCursor cursor) => cursor.Emit(OpCodes.Arglist);
        public static ILCursor Beq_(this ILCursor cursor, ILLabel target) => cursor.Emit(OpCodes.Beq, target);
        public static ILCursor Bge_(this ILCursor cursor, ILLabel target, Boolean unsigned = false) => cursor.Emit(unsigned ? OpCodes.Bge_Un : OpCodes.Bge_Un, target);
        public static ILCursor Bgt_(this ILCursor cursor, ILLabel target, Boolean unsigned = false) => cursor.Emit(unsigned ? OpCodes.Bgt_Un : OpCodes.Bgt_Un, target);
        public static ILCursor Ble_(this ILCursor cursor, ILLabel target, Boolean unsigned = false) => cursor.Emit(unsigned ? OpCodes.Ble_Un : OpCodes.Ble_Un, target);
        public static ILCursor Blt_(this ILCursor cursor, ILLabel target, Boolean unsigned = false) => cursor.Emit(unsigned ? OpCodes.Blt_Un : OpCodes.Blt_Un, target);
        public static ILCursor Bne_Un_(this ILCursor cursor, ILLabel target) => cursor.Emit(OpCodes.Bne_Un, target);
        public static ILCursor Box_<T>(this ILCursor cursor) where T : struct => cursor.Box_(typeof(T));
        public static ILCursor Box_(this ILCursor cursor, Type type) => cursor.Box_(type);
        public static ILCursor Br_(this ILCursor cursor, ILLabel label) => cursor.Emit(OpCodes.Br, label);
        public static ILCursor Break_(this ILCursor cursor) => cursor.Emit(OpCodes.Break);
        public static ILCursor BrFalse_(this ILCursor cursor, ILLabel target) => cursor.Emit(OpCodes.Brfalse, target);
        public static ILCursor BrTrue_(this ILCursor cursor, ILLabel target) => cursor.Emit(OpCodes.Brtrue, target);
        public static ILCursor Call_(this ILCursor cursor, MethodInfo method) => cursor.Emit(OpCodes.Call, method);
        public static ILCursor CallVirt_(this ILCursor cursor, MethodInfo method) => cursor.Emit(OpCodes.Callvirt, method);
        public static ILCursor CallVirt_Constr_(this ILCursor cursor, MethodInfo method, Type type) => cursor.Emit(OpCodes.Constrained, type).CallVirt_(method);
        public static ILCursor CallVirt_Constr_<T>(this ILCursor cursor, MethodInfo method) => cursor.CallVirt_Constr_(method, typeof(T));
        public static ILCursor Call_<TDelegate>(this ILCursor cursor, TDelegate method) where TDelegate : Delegate
        {
            cursor.EmitDelegate<TDelegate>(method);
            return cursor;
        }
        public static ILCursor Cast_(this ILCursor cursor, Type type) => cursor.Emit(OpCodes.Castclass, type);
        public static ILCursor Cast_<T>(this ILCursor cursor) => cursor.Cast_(typeof(T));
        public static ILCursor Ceq_(this ILCursor cursor) => cursor.Emit(OpCodes.Ceq);
        public static ILCursor Cgt_(this ILCursor cursor, Boolean unsigned = false) => cursor.Emit(unsigned ? OpCodes.Cgt_Un : OpCodes.Cgt);
        public static ILCursor CkFinite_(this ILCursor cursor) => cursor.Emit(OpCodes.Ckfinite);
        public static ILCursor Clt_(this ILCursor cursor, Boolean unsigned = false) => cursor.Emit(unsigned ? OpCodes.Clt_Un : OpCodes.Clt);
        public static ILCursor Is_(this ILCursor cursor, Type t) => cursor.Emit(OpCodes.Isinst, t);
        public static ILCursor Is_<T>(this ILCursor cursor) where T : class => cursor.Is_(typeof(T));
        public static ILCursor Switch_(this ILCursor cursor, params ILLabel[] targets) => cursor.Emit(OpCodes.Switch, targets);
        public static ILCursor New_(this ILCursor cursor, ConstructorInfo constructor) => cursor.EmitNewObj(constructor);
        public static ILCursor Dup_(this ILCursor cursor) => cursor.Emit(OpCodes.Dup);
        public static ILCursor Pop_(this ILCursor cursor) => cursor.Emit(OpCodes.Pop);
        public static ILCursor Nop_(this ILCursor cursor) => cursor.Emit(OpCodes.Nop);
        public static ILCursor LdArg_(this ILCursor cursor, UInt16 index) => index switch
        {
            0 => cursor.Emit(OpCodes.Ldarg_0),
            1 => cursor.Emit(OpCodes.Ldarg_1),
            2 => cursor.Emit(OpCodes.Ldarg_2),
            3 => cursor.Emit(OpCodes.Ldarg_3),
            UInt16 ind when ind < Byte.MaxValue => cursor.Emit(OpCodes.Ldarg_S, (Byte)index),
            _ => cursor.Emit(OpCodes.Ldarg, index),
        };
        public static ILCursor LdC_(this ILCursor cursor, String value) => cursor.Emit(OpCodes.Ldstr, value);
        public static ILCursor LdC_(this ILCursor cursor, Int32 value) => cursor.Emit(OpCodes.Ldc_I4, value);
        public static ILCursor LdC_(this ILCursor cursor, Int64 value) => cursor.Emit(OpCodes.Ldc_I8, value);
        public static ILCursor LdC_(this ILCursor cursor, Single value) => cursor.Emit(OpCodes.Ldc_R4, value);
        public static ILCursor LdC_(this ILCursor cursor, Double value) => cursor.Emit(OpCodes.Ldc_R8, value);
        public static ILCursor StLoc_(this ILCursor cursor, Int32 index) => cursor.Emit(OpCodes.Stloc, index);
        public static ILCursor LdLoc_(this ILCursor cursor, Int32 index) => index switch
        {
            0 => cursor.Emit(OpCodes.Ldloc_0),
            1 => cursor.Emit(OpCodes.Ldloc_1),
            2 => cursor.Emit(OpCodes.Ldloc_2),
            3 => cursor.Emit(OpCodes.Ldloc_3),
            Int32 i when i >= Byte.MinValue && i <= Byte.MaxValue => cursor.Emit(OpCodes.Ldloc_S, (Byte)index),
            _ => cursor.Emit(OpCodes.Ldloc, index),
        };
        public static ILCursor LdLocA_(this ILCursor cursor, Int32 index) => index switch
        {
            Int32 i when i >= Byte.MinValue && i <= Byte.MaxValue => cursor.Emit(OpCodes.Ldloca_S, (Byte)index),
            _ => cursor.Emit(OpCodes.Ldloca, index),
        };
        public static ILCursor LdFld_(this ILCursor cursor, FieldInfo field) => cursor.Emit(OpCodes.Ldfld, field);
        public static ILCursor LdSFld_(this ILCursor cursor, FieldInfo field) => cursor.Emit(OpCodes.Ldsfld, field);
        public static ILCursor LdSFldA_(this ILCursor cursor, FieldInfo field) => cursor.Emit(OpCodes.Ldsflda, field);
        public static ILCursor StSFld_(this ILCursor cursor, FieldInfo field) => cursor.Emit(OpCodes.Stsfld, field);
        public static ILCursor Ret_(this ILCursor cursor) => cursor.Emit(OpCodes.Ret);


        private static ILCursor EmitNewObj(this ILCursor cursor, ConstructorInfo constructor) => cursor.Emit(OpCodes.Newobj, constructor);
    }
}