//Generated
namespace ILHelper
{
	using System;

	using MonoMod.Cil;
	public static partial class StackedCursorExtensions
	{
		#region CallExtensions
		public static DMDCursor<TStack, TSig> Call<TStack, TSig>( ref this DMDCursor<TStack,TSig> stack, XAct func, Boolean doTailCall = false )
			where TStack : IStack
			where TSig : Delegate
		{
			return stack._Emit(stack.cursor.Call(func.Method, doTailCall));
		}
		public static DMDCursor<TStack, TSig> Call<T1, TStack, TSig>( ref this DMDCursor<IL<T1, TStack>,TSig> stack, XAct<T1> func, Boolean doTailCall = false )
			where TStack : IStack
			where TSig : Delegate
		{
			return stack._Pop(stack.cursor.Call(func.Method, doTailCall));
		}
		public static DMDCursor<TStack, TSig> Call<T1, T2, TStack, TSig>( ref this DMDCursor<IL<T2, IL<T1, TStack>>,TSig> stack, XAct<T1, T2> func, Boolean doTailCall = false )
			where TStack : IStack
			where TSig : Delegate
		{
			return stack._Pop()._Pop(stack.cursor.Call(func.Method, doTailCall));
		}
		public static DMDCursor<TStack, TSig> Call<T1, T2, T3, TStack, TSig>( ref this DMDCursor<IL<T3, IL<T2, IL<T1, TStack>>>,TSig> stack, XAct<T1, T2, T3> func, Boolean doTailCall = false )
			where TStack : IStack
			where TSig : Delegate
		{
			return stack._Pop()._Pop()._Pop(stack.cursor.Call(func.Method, doTailCall));
		}
		public static DMDCursor<TStack, TSig> Call<T1, T2, T3, T4, TStack, TSig>( ref this DMDCursor<IL<T4, IL<T3, IL<T2, IL<T1, TStack>>>>,TSig> stack, XAct<T1, T2, T3, T4> func, Boolean doTailCall = false )
			where TStack : IStack
			where TSig : Delegate
		{
			return stack._Pop()._Pop()._Pop()._Pop(stack.cursor.Call(func.Method, doTailCall));
		}
		public static DMDCursor<TStack, TSig> Call<T1, T2, T3, T4, T5, TStack, TSig>( ref this DMDCursor<IL<T5, IL<T4, IL<T3, IL<T2, IL<T1, TStack>>>>>,TSig> stack, XAct<T1, T2, T3, T4, T5> func, Boolean doTailCall = false )
			where TStack : IStack
			where TSig : Delegate
		{
			return stack._Pop()._Pop()._Pop()._Pop()._Pop(stack.cursor.Call(func.Method, doTailCall));
		}
		public static DMDCursor<TStack, TSig> Call<T1, T2, T3, T4, T5, T6, TStack, TSig>( ref this DMDCursor<IL<T6, IL<T5, IL<T4, IL<T3, IL<T2, IL<T1, TStack>>>>>>,TSig> stack, XAct<T1, T2, T3, T4, T5, T6> func, Boolean doTailCall = false )
			where TStack : IStack
			where TSig : Delegate
		{
			return stack._Pop()._Pop()._Pop()._Pop()._Pop()._Pop(stack.cursor.Call(func.Method, doTailCall));
		}
		public static DMDCursor<TStack, TSig> Call<T1, T2, T3, T4, T5, T6, T7, TStack, TSig>( ref this DMDCursor<IL<T7, IL<T6, IL<T5, IL<T4, IL<T3, IL<T2, IL<T1, TStack>>>>>>>,TSig> stack, XAct<T1, T2, T3, T4, T5, T6, T7> func, Boolean doTailCall = false )
			where TStack : IStack
			where TSig : Delegate
		{
			return stack._Pop()._Pop()._Pop()._Pop()._Pop()._Pop()._Pop(stack.cursor.Call(func.Method, doTailCall));
		}
		public static DMDCursor<TStack, TSig> Call<T1, T2, T3, T4, T5, T6, T7, T8, TStack, TSig>( ref this DMDCursor<IL<T8, IL<T7, IL<T6, IL<T5, IL<T4, IL<T3, IL<T2, IL<T1, TStack>>>>>>>>,TSig> stack, XAct<T1, T2, T3, T4, T5, T6, T7, T8> func, Boolean doTailCall = false )
			where TStack : IStack
			where TSig : Delegate
		{
			return stack._Pop()._Pop()._Pop()._Pop()._Pop()._Pop()._Pop()._Pop(stack.cursor.Call(func.Method, doTailCall));
		}
		public static DMDCursor<IL<TReturn,TStack>, TSig> Call<TReturn, TStack, TSig>( ref this DMDCursor<TStack,TSig> stack, XFunc<TReturn> func, Boolean doTailCall = false )
			where TStack : IStack
			where TSig : Delegate
		{
			return stack._Push<TReturn>(stack.cursor.Call(func.Method, doTailCall));
		}
		public static DMDCursor<IL<TReturn,TStack>, TSig> Call<T1, TReturn, TStack, TSig>( ref this DMDCursor<IL<T1, TStack>,TSig> stack, XFunc<T1, TReturn> func, Boolean doTailCall = false )
			where TStack : IStack
			where TSig : Delegate
		{
			return stack._Pop()._Push<TReturn>(stack.cursor.Call(func.Method, doTailCall));
		}
		public static DMDCursor<IL<TReturn,TStack>, TSig> Call<T1, T2, TReturn, TStack, TSig>( ref this DMDCursor<IL<T2, IL<T1, TStack>>,TSig> stack, XFunc<T1, T2, TReturn> func, Boolean doTailCall = false )
			where TStack : IStack
			where TSig : Delegate
		{
			return stack._Pop()._Pop()._Push<TReturn>(stack.cursor.Call(func.Method, doTailCall));
		}
		public static DMDCursor<IL<TReturn,TStack>, TSig> Call<T1, T2, T3, TReturn, TStack, TSig>( ref this DMDCursor<IL<T3, IL<T2, IL<T1, TStack>>>,TSig> stack, XFunc<T1, T2, T3, TReturn> func, Boolean doTailCall = false )
			where TStack : IStack
			where TSig : Delegate
		{
			return stack._Pop()._Pop()._Pop()._Push<TReturn>(stack.cursor.Call(func.Method, doTailCall));
		}
		public static DMDCursor<IL<TReturn,TStack>, TSig> Call<T1, T2, T3, T4, TReturn, TStack, TSig>( ref this DMDCursor<IL<T4, IL<T3, IL<T2, IL<T1, TStack>>>>,TSig> stack, XFunc<T1, T2, T3, T4, TReturn> func, Boolean doTailCall = false )
			where TStack : IStack
			where TSig : Delegate
		{
			return stack._Pop()._Pop()._Pop()._Pop()._Push<TReturn>(stack.cursor.Call(func.Method, doTailCall));
		}
		public static DMDCursor<IL<TReturn,TStack>, TSig> Call<T1, T2, T3, T4, T5, TReturn, TStack, TSig>( ref this DMDCursor<IL<T5, IL<T4, IL<T3, IL<T2, IL<T1, TStack>>>>>,TSig> stack, XFunc<T1, T2, T3, T4, T5, TReturn> func, Boolean doTailCall = false )
			where TStack : IStack
			where TSig : Delegate
		{
			return stack._Pop()._Pop()._Pop()._Pop()._Pop()._Push<TReturn>(stack.cursor.Call(func.Method, doTailCall));
		}
		public static DMDCursor<IL<TReturn,TStack>, TSig> Call<T1, T2, T3, T4, T5, T6, TReturn, TStack, TSig>( ref this DMDCursor<IL<T6, IL<T5, IL<T4, IL<T3, IL<T2, IL<T1, TStack>>>>>>,TSig> stack, XFunc<T1, T2, T3, T4, T5, T6, TReturn> func, Boolean doTailCall = false )
			where TStack : IStack
			where TSig : Delegate
		{
			return stack._Pop()._Pop()._Pop()._Pop()._Pop()._Pop()._Push<TReturn>(stack.cursor.Call(func.Method, doTailCall));
		}
		public static DMDCursor<IL<TReturn,TStack>, TSig> Call<T1, T2, T3, T4, T5, T6, T7, TReturn, TStack, TSig>( ref this DMDCursor<IL<T7, IL<T6, IL<T5, IL<T4, IL<T3, IL<T2, IL<T1, TStack>>>>>>>,TSig> stack, XFunc<T1, T2, T3, T4, T5, T6, T7, TReturn> func, Boolean doTailCall = false )
			where TStack : IStack
			where TSig : Delegate
		{
			return stack._Pop()._Pop()._Pop()._Pop()._Pop()._Pop()._Pop()._Push<TReturn>(stack.cursor.Call(func.Method, doTailCall));
		}
		public static DMDCursor<IL<TReturn,TStack>, TSig> Call<T1, T2, T3, T4, T5, T6, T7, T8, TReturn, TStack, TSig>( ref this DMDCursor<IL<T8, IL<T7, IL<T6, IL<T5, IL<T4, IL<T3, IL<T2, IL<T1, TStack>>>>>>>>,TSig> stack, XFunc<T1, T2, T3, T4, T5, T6, T7, T8, TReturn> func, Boolean doTailCall = false )
			where TStack : IStack
			where TSig : Delegate
		{
			return stack._Pop()._Pop()._Pop()._Pop()._Pop()._Pop()._Pop()._Pop()._Push<TReturn>(stack.cursor.Call(func.Method, doTailCall));
		}
		#endregion
		#region VirtCallExtensions
		public static DMDCursor<TStack, TSig> Call<TStack, TSig>( ref this DMDCursor<TStack,TSig> stack, XAct func, Type constrainedType = null, Boolean doTailCall = false )
			where TStack : IStack
			where TSig : Delegate
		{
			return stack._Emit(stack.cursor.CallVirtual(func.Method, constrainedType, doTailCall));
		}
		public static DMDCursor<TStack, TSig> Call<T1, TStack, TSig>( ref this DMDCursor<IL<T1, TStack>,TSig> stack, XAct<T1> func, Type constrainedType = null, Boolean doTailCall = false )
			where TStack : IStack
			where TSig : Delegate
		{
			return stack._Pop(stack.cursor.CallVirtual(func.Method, constrainedType, doTailCall));
		}
		public static DMDCursor<TStack, TSig> Call<T1, T2, TStack, TSig>( ref this DMDCursor<IL<T2, IL<T1, TStack>>,TSig> stack, XAct<T1, T2> func, Type constrainedType = null, Boolean doTailCall = false )
			where TStack : IStack
			where TSig : Delegate
		{
			return stack._Pop()._Pop(stack.cursor.CallVirtual(func.Method, constrainedType, doTailCall));
		}
		public static DMDCursor<TStack, TSig> Call<T1, T2, T3, TStack, TSig>( ref this DMDCursor<IL<T3, IL<T2, IL<T1, TStack>>>,TSig> stack, XAct<T1, T2, T3> func, Type constrainedType = null, Boolean doTailCall = false )
			where TStack : IStack
			where TSig : Delegate
		{
			return stack._Pop()._Pop()._Pop(stack.cursor.CallVirtual(func.Method, constrainedType, doTailCall));
		}
		public static DMDCursor<TStack, TSig> Call<T1, T2, T3, T4, TStack, TSig>( ref this DMDCursor<IL<T4, IL<T3, IL<T2, IL<T1, TStack>>>>,TSig> stack, XAct<T1, T2, T3, T4> func, Type constrainedType = null, Boolean doTailCall = false )
			where TStack : IStack
			where TSig : Delegate
		{
			return stack._Pop()._Pop()._Pop()._Pop(stack.cursor.CallVirtual(func.Method, constrainedType, doTailCall));
		}
		public static DMDCursor<TStack, TSig> Call<T1, T2, T3, T4, T5, TStack, TSig>( ref this DMDCursor<IL<T5, IL<T4, IL<T3, IL<T2, IL<T1, TStack>>>>>,TSig> stack, XAct<T1, T2, T3, T4, T5> func, Type constrainedType = null, Boolean doTailCall = false )
			where TStack : IStack
			where TSig : Delegate
		{
			return stack._Pop()._Pop()._Pop()._Pop()._Pop(stack.cursor.CallVirtual(func.Method, constrainedType, doTailCall));
		}
		public static DMDCursor<TStack, TSig> Call<T1, T2, T3, T4, T5, T6, TStack, TSig>( ref this DMDCursor<IL<T6, IL<T5, IL<T4, IL<T3, IL<T2, IL<T1, TStack>>>>>>,TSig> stack, XAct<T1, T2, T3, T4, T5, T6> func, Type constrainedType = null, Boolean doTailCall = false )
			where TStack : IStack
			where TSig : Delegate
		{
			return stack._Pop()._Pop()._Pop()._Pop()._Pop()._Pop(stack.cursor.CallVirtual(func.Method, constrainedType, doTailCall));
		}
		public static DMDCursor<TStack, TSig> Call<T1, T2, T3, T4, T5, T6, T7, TStack, TSig>( ref this DMDCursor<IL<T7, IL<T6, IL<T5, IL<T4, IL<T3, IL<T2, IL<T1, TStack>>>>>>>,TSig> stack, XAct<T1, T2, T3, T4, T5, T6, T7> func, Type constrainedType = null, Boolean doTailCall = false )
			where TStack : IStack
			where TSig : Delegate
		{
			return stack._Pop()._Pop()._Pop()._Pop()._Pop()._Pop()._Pop(stack.cursor.CallVirtual(func.Method, constrainedType, doTailCall));
		}
		public static DMDCursor<TStack, TSig> Call<T1, T2, T3, T4, T5, T6, T7, T8, TStack, TSig>( ref this DMDCursor<IL<T8, IL<T7, IL<T6, IL<T5, IL<T4, IL<T3, IL<T2, IL<T1, TStack>>>>>>>>,TSig> stack, XAct<T1, T2, T3, T4, T5, T6, T7, T8> func, Type constrainedType = null, Boolean doTailCall = false )
			where TStack : IStack
			where TSig : Delegate
		{
			return stack._Pop()._Pop()._Pop()._Pop()._Pop()._Pop()._Pop()._Pop(stack.cursor.CallVirtual(func.Method, constrainedType, doTailCall));
		}
		public static DMDCursor<IL<TReturn,TStack>, TSig> CallVirtual<TReturn, TStack, TSig>( ref this DMDCursor<TStack,TSig> stack, XFunc<TReturn> func, Type constrainedType = null, Boolean doTailCall = false )
			where TStack : IStack
			where TSig : Delegate
		{
			return stack._Push<TReturn>(stack.cursor.CallVirtual(func.Method, constrainedType, doTailCall));
		}
		public static DMDCursor<IL<TReturn,TStack>, TSig> CallVirtual<T1, TReturn, TStack, TSig>( ref this DMDCursor<IL<T1, TStack>,TSig> stack, XFunc<T1, TReturn> func, Type constrainedType = null, Boolean doTailCall = false )
			where TStack : IStack
			where TSig : Delegate
		{
			return stack._Pop()._Push<TReturn>(stack.cursor.CallVirtual(func.Method, constrainedType, doTailCall));
		}
		public static DMDCursor<IL<TReturn,TStack>, TSig> CallVirtual<T1, T2, TReturn, TStack, TSig>( ref this DMDCursor<IL<T2, IL<T1, TStack>>,TSig> stack, XFunc<T1, T2, TReturn> func, Type constrainedType = null, Boolean doTailCall = false )
			where TStack : IStack
			where TSig : Delegate
		{
			return stack._Pop()._Pop()._Push<TReturn>(stack.cursor.CallVirtual(func.Method, constrainedType, doTailCall));
		}
		public static DMDCursor<IL<TReturn,TStack>, TSig> CallVirtual<T1, T2, T3, TReturn, TStack, TSig>( ref this DMDCursor<IL<T3, IL<T2, IL<T1, TStack>>>,TSig> stack, XFunc<T1, T2, T3, TReturn> func, Type constrainedType = null, Boolean doTailCall = false )
			where TStack : IStack
			where TSig : Delegate
		{
			return stack._Pop()._Pop()._Pop()._Push<TReturn>(stack.cursor.CallVirtual(func.Method, constrainedType, doTailCall));
		}
		public static DMDCursor<IL<TReturn,TStack>, TSig> CallVirtual<T1, T2, T3, T4, TReturn, TStack, TSig>( ref this DMDCursor<IL<T4, IL<T3, IL<T2, IL<T1, TStack>>>>,TSig> stack, XFunc<T1, T2, T3, T4, TReturn> func, Type constrainedType = null, Boolean doTailCall = false )
			where TStack : IStack
			where TSig : Delegate
		{
			return stack._Pop()._Pop()._Pop()._Pop()._Push<TReturn>(stack.cursor.CallVirtual(func.Method, constrainedType, doTailCall));
		}
		public static DMDCursor<IL<TReturn,TStack>, TSig> CallVirtual<T1, T2, T3, T4, T5, TReturn, TStack, TSig>( ref this DMDCursor<IL<T5, IL<T4, IL<T3, IL<T2, IL<T1, TStack>>>>>,TSig> stack, XFunc<T1, T2, T3, T4, T5, TReturn> func, Type constrainedType = null, Boolean doTailCall = false )
			where TStack : IStack
			where TSig : Delegate
		{
			return stack._Pop()._Pop()._Pop()._Pop()._Pop()._Push<TReturn>(stack.cursor.CallVirtual(func.Method, constrainedType, doTailCall));
		}
		public static DMDCursor<IL<TReturn,TStack>, TSig> CallVirtual<T1, T2, T3, T4, T5, T6, TReturn, TStack, TSig>( ref this DMDCursor<IL<T6, IL<T5, IL<T4, IL<T3, IL<T2, IL<T1, TStack>>>>>>,TSig> stack, XFunc<T1, T2, T3, T4, T5, T6, TReturn> func, Type constrainedType = null, Boolean doTailCall = false )
			where TStack : IStack
			where TSig : Delegate
		{
			return stack._Pop()._Pop()._Pop()._Pop()._Pop()._Pop()._Push<TReturn>(stack.cursor.CallVirtual(func.Method, constrainedType, doTailCall));
		}
		public static DMDCursor<IL<TReturn,TStack>, TSig> CallVirtual<T1, T2, T3, T4, T5, T6, T7, TReturn, TStack, TSig>( ref this DMDCursor<IL<T7, IL<T6, IL<T5, IL<T4, IL<T3, IL<T2, IL<T1, TStack>>>>>>>,TSig> stack, XFunc<T1, T2, T3, T4, T5, T6, T7, TReturn> func, Type constrainedType = null, Boolean doTailCall = false )
			where TStack : IStack
			where TSig : Delegate
		{
			return stack._Pop()._Pop()._Pop()._Pop()._Pop()._Pop()._Pop()._Push<TReturn>(stack.cursor.CallVirtual(func.Method, constrainedType, doTailCall));
		}
		public static DMDCursor<IL<TReturn,TStack>, TSig> CallVirtual<T1, T2, T3, T4, T5, T6, T7, T8, TReturn, TStack, TSig>( ref this DMDCursor<IL<T8, IL<T7, IL<T6, IL<T5, IL<T4, IL<T3, IL<T2, IL<T1, TStack>>>>>>>>,TSig> stack, XFunc<T1, T2, T3, T4, T5, T6, T7, T8, TReturn> func, Type constrainedType = null, Boolean doTailCall = false )
			where TStack : IStack
			where TSig : Delegate
		{
			return stack._Pop()._Pop()._Pop()._Pop()._Pop()._Pop()._Pop()._Pop()._Push<TReturn>(stack.cursor.CallVirtual(func.Method, constrainedType, doTailCall));
		}
		#endregion
		#region IndirectCallExtensions
		public static DMDCursor<TStack, TSig> CallIndirect<TStack, TSig>( ref this DMDCursor<IL<ILFunctionPointer<XAct>, TStack>,TSig> stack, Boolean doTailCall = false )
			where TStack : IStack
			where TSig : Delegate
		{
			return stack._Pop(stack.cursor.CallIndirect(CallInfo<XAct>.site, doTailCall));
		}
		public static DMDCursor<TStack, TSig> CallIndirect<T1, TStack, TSig>( ref this DMDCursor<IL<T1, IL<ILFunctionPointer<XAct<T1>>, TStack>>,TSig> stack, Boolean doTailCall = false )
			where TStack : IStack
			where TSig : Delegate
		{
			return stack._Pop()._Pop(stack.cursor.CallIndirect(CallInfo<XAct<T1>>.site, doTailCall));
		}
		public static DMDCursor<TStack, TSig> CallIndirect<T1, T2, TStack, TSig>( ref this DMDCursor<IL<T2, IL<T1, IL<ILFunctionPointer<XAct<T1, T2>>, TStack>>>,TSig> stack, Boolean doTailCall = false )
			where TStack : IStack
			where TSig : Delegate
		{
			return stack._Pop()._Pop()._Pop(stack.cursor.CallIndirect(CallInfo<XAct<T1, T2>>.site, doTailCall));
		}
		public static DMDCursor<TStack, TSig> CallIndirect<T1, T2, T3, TStack, TSig>( ref this DMDCursor<IL<T3, IL<T2, IL<T1, IL<ILFunctionPointer<XAct<T1, T2, T3>>, TStack>>>>,TSig> stack, Boolean doTailCall = false )
			where TStack : IStack
			where TSig : Delegate
		{
			return stack._Pop()._Pop()._Pop()._Pop(stack.cursor.CallIndirect(CallInfo<XAct<T1, T2, T3>>.site, doTailCall));
		}
		public static DMDCursor<TStack, TSig> CallIndirect<T1, T2, T3, T4, TStack, TSig>( ref this DMDCursor<IL<T4, IL<T3, IL<T2, IL<T1, IL<ILFunctionPointer<XAct<T1, T2, T3, T4>>, TStack>>>>>,TSig> stack, Boolean doTailCall = false )
			where TStack : IStack
			where TSig : Delegate
		{
			return stack._Pop()._Pop()._Pop()._Pop()._Pop(stack.cursor.CallIndirect(CallInfo<XAct<T1, T2, T3, T4>>.site, doTailCall));
		}
		public static DMDCursor<TStack, TSig> CallIndirect<T1, T2, T3, T4, T5, TStack, TSig>( ref this DMDCursor<IL<T5, IL<T4, IL<T3, IL<T2, IL<T1, IL<ILFunctionPointer<XAct<T1, T2, T3, T4, T5>>, TStack>>>>>>,TSig> stack, Boolean doTailCall = false )
			where TStack : IStack
			where TSig : Delegate
		{
			return stack._Pop()._Pop()._Pop()._Pop()._Pop()._Pop(stack.cursor.CallIndirect(CallInfo<XAct<T1, T2, T3, T4, T5>>.site, doTailCall));
		}
		public static DMDCursor<TStack, TSig> CallIndirect<T1, T2, T3, T4, T5, T6, TStack, TSig>( ref this DMDCursor<IL<T6, IL<T5, IL<T4, IL<T3, IL<T2, IL<T1, IL<ILFunctionPointer<XAct<T1, T2, T3, T4, T5, T6>>, TStack>>>>>>>,TSig> stack, Boolean doTailCall = false )
			where TStack : IStack
			where TSig : Delegate
		{
			return stack._Pop()._Pop()._Pop()._Pop()._Pop()._Pop()._Pop(stack.cursor.CallIndirect(CallInfo<XAct<T1, T2, T3, T4, T5, T6>>.site, doTailCall));
		}
		public static DMDCursor<TStack, TSig> CallIndirect<T1, T2, T3, T4, T5, T6, T7, TStack, TSig>( ref this DMDCursor<IL<T7, IL<T6, IL<T5, IL<T4, IL<T3, IL<T2, IL<T1, IL<ILFunctionPointer<XAct<T1, T2, T3, T4, T5, T6, T7>>, TStack>>>>>>>>,TSig> stack, Boolean doTailCall = false )
			where TStack : IStack
			where TSig : Delegate
		{
			return stack._Pop()._Pop()._Pop()._Pop()._Pop()._Pop()._Pop()._Pop(stack.cursor.CallIndirect(CallInfo<XAct<T1, T2, T3, T4, T5, T6, T7>>.site, doTailCall));
		}
		public static DMDCursor<TStack, TSig> CallIndirect<T1, T2, T3, T4, T5, T6, T7, T8, TStack, TSig>( ref this DMDCursor<IL<T8, IL<T7, IL<T6, IL<T5, IL<T4, IL<T3, IL<T2, IL<T1, IL<ILFunctionPointer<XAct<T1, T2, T3, T4, T5, T6, T7, T8>>, TStack>>>>>>>>>,TSig> stack, Boolean doTailCall = false )
			where TStack : IStack
			where TSig : Delegate
		{
			return stack._Pop()._Pop()._Pop()._Pop()._Pop()._Pop()._Pop()._Pop()._Pop(stack.cursor.CallIndirect(CallInfo<XAct<T1, T2, T3, T4, T5, T6, T7, T8>>.site, doTailCall));
		}
		public static DMDCursor<IL<TReturn,TStack>, TSig> CallIndirect<TReturn, TStack, TSig>( ref this DMDCursor<IL<ILFunctionPointer<XFunc<TReturn>>, TStack>,TSig> stack, Boolean doTailCall = false )
			where TStack : IStack
			where TSig : Delegate
		{
			return stack._Pop()._Push<TReturn>(stack.cursor.CallIndirect(CallInfo<XFunc<TReturn>>.site, doTailCall));
		}
		public static DMDCursor<IL<TReturn,TStack>, TSig> CallIndirect<T1, TReturn, TStack, TSig>( ref this DMDCursor<IL<T1, IL<ILFunctionPointer<XFunc<T1, TReturn>>, TStack>>,TSig> stack, Boolean doTailCall = false )
			where TStack : IStack
			where TSig : Delegate
		{
			return stack._Pop()._Pop()._Push<TReturn>(stack.cursor.CallIndirect(CallInfo<XFunc<T1, TReturn>>.site, doTailCall));
		}
		public static DMDCursor<IL<TReturn,TStack>, TSig> CallIndirect<T1, T2, TReturn, TStack, TSig>( ref this DMDCursor<IL<T2, IL<T1, IL<ILFunctionPointer<XFunc<T1, T2, TReturn>>, TStack>>>,TSig> stack, Boolean doTailCall = false )
			where TStack : IStack
			where TSig : Delegate
		{
			return stack._Pop()._Pop()._Pop()._Push<TReturn>(stack.cursor.CallIndirect(CallInfo<XFunc<T1, T2, TReturn>>.site, doTailCall));
		}
		public static DMDCursor<IL<TReturn,TStack>, TSig> CallIndirect<T1, T2, T3, TReturn, TStack, TSig>( ref this DMDCursor<IL<T3, IL<T2, IL<T1, IL<ILFunctionPointer<XFunc<T1, T2, T3, TReturn>>, TStack>>>>,TSig> stack, Boolean doTailCall = false )
			where TStack : IStack
			where TSig : Delegate
		{
			return stack._Pop()._Pop()._Pop()._Pop()._Push<TReturn>(stack.cursor.CallIndirect(CallInfo<XFunc<T1, T2, T3, TReturn>>.site, doTailCall));
		}
		public static DMDCursor<IL<TReturn,TStack>, TSig> CallIndirect<T1, T2, T3, T4, TReturn, TStack, TSig>( ref this DMDCursor<IL<T4, IL<T3, IL<T2, IL<T1, IL<ILFunctionPointer<XFunc<T1, T2, T3, T4, TReturn>>, TStack>>>>>,TSig> stack, Boolean doTailCall = false )
			where TStack : IStack
			where TSig : Delegate
		{
			return stack._Pop()._Pop()._Pop()._Pop()._Pop()._Push<TReturn>(stack.cursor.CallIndirect(CallInfo<XFunc<T1, T2, T3, T4, TReturn>>.site, doTailCall));
		}
		public static DMDCursor<IL<TReturn,TStack>, TSig> CallIndirect<T1, T2, T3, T4, T5, TReturn, TStack, TSig>( ref this DMDCursor<IL<T5, IL<T4, IL<T3, IL<T2, IL<T1, IL<ILFunctionPointer<XFunc<T1, T2, T3, T4, T5, TReturn>>, TStack>>>>>>,TSig> stack, Boolean doTailCall = false )
			where TStack : IStack
			where TSig : Delegate
		{
			return stack._Pop()._Pop()._Pop()._Pop()._Pop()._Pop()._Push<TReturn>(stack.cursor.CallIndirect(CallInfo<XFunc<T1, T2, T3, T4, T5, TReturn>>.site, doTailCall));
		}
		public static DMDCursor<IL<TReturn,TStack>, TSig> CallIndirect<T1, T2, T3, T4, T5, T6, TReturn, TStack, TSig>( ref this DMDCursor<IL<T6, IL<T5, IL<T4, IL<T3, IL<T2, IL<T1, IL<ILFunctionPointer<XFunc<T1, T2, T3, T4, T5, T6, TReturn>>, TStack>>>>>>>,TSig> stack, Boolean doTailCall = false )
			where TStack : IStack
			where TSig : Delegate
		{
			return stack._Pop()._Pop()._Pop()._Pop()._Pop()._Pop()._Pop()._Push<TReturn>(stack.cursor.CallIndirect(CallInfo<XFunc<T1, T2, T3, T4, T5, T6, TReturn>>.site, doTailCall));
		}
		public static DMDCursor<IL<TReturn,TStack>, TSig> CallIndirect<T1, T2, T3, T4, T5, T6, T7, TReturn, TStack, TSig>( ref this DMDCursor<IL<T7, IL<T6, IL<T5, IL<T4, IL<T3, IL<T2, IL<T1, IL<ILFunctionPointer<XFunc<T1, T2, T3, T4, T5, T6, T7, TReturn>>, TStack>>>>>>>>,TSig> stack, Boolean doTailCall = false )
			where TStack : IStack
			where TSig : Delegate
		{
			return stack._Pop()._Pop()._Pop()._Pop()._Pop()._Pop()._Pop()._Pop()._Push<TReturn>(stack.cursor.CallIndirect(CallInfo<XFunc<T1, T2, T3, T4, T5, T6, T7, TReturn>>.site, doTailCall));
		}
		public static DMDCursor<IL<TReturn,TStack>, TSig> CallIndirect<T1, T2, T3, T4, T5, T6, T7, T8, TReturn, TStack, TSig>( ref this DMDCursor<IL<T8, IL<T7, IL<T6, IL<T5, IL<T4, IL<T3, IL<T2, IL<T1, IL<ILFunctionPointer<XFunc<T1, T2, T3, T4, T5, T6, T7, T8, TReturn>>, TStack>>>>>>>>>,TSig> stack, Boolean doTailCall = false )
			where TStack : IStack
			where TSig : Delegate
		{
			return stack._Pop()._Pop()._Pop()._Pop()._Pop()._Pop()._Pop()._Pop()._Pop()._Push<TReturn>(stack.cursor.CallIndirect(CallInfo<XFunc<T1, T2, T3, T4, T5, T6, T7, T8, TReturn>>.site, doTailCall));
		}
		#endregion
		#region InlineCallExtensions
		#endregion
	}
	#region Delegate Types
	public delegate void XAct();
	public delegate void XAct<T1>(T1 arg1);
	public delegate void XAct<T1, T2>(T1 arg1, T2 arg2);
	public delegate void XAct<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3);
	public delegate void XAct<T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);
	public delegate void XAct<T1, T2, T3, T4, T5>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);
	public delegate void XAct<T1, T2, T3, T4, T5, T6>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);
	public delegate void XAct<T1, T2, T3, T4, T5, T6, T7>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);
	public delegate void XAct<T1, T2, T3, T4, T5, T6, T7, T8>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8);
	public delegate TReturn XFunc<TReturn>();
	public delegate TReturn XFunc<T1, TReturn>(T1 arg1);
	public delegate TReturn XFunc<T1, T2, TReturn>(T1 arg1, T2 arg2);
	public delegate TReturn XFunc<T1, T2, T3, TReturn>(T1 arg1, T2 arg2, T3 arg3);
	public delegate TReturn XFunc<T1, T2, T3, T4, TReturn>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);
	public delegate TReturn XFunc<T1, T2, T3, T4, T5, TReturn>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);
	public delegate TReturn XFunc<T1, T2, T3, T4, T5, T6, TReturn>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);
	public delegate TReturn XFunc<T1, T2, T3, T4, T5, T6, T7, TReturn>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);
	public delegate TReturn XFunc<T1, T2, T3, T4, T5, T6, T7, T8, TReturn>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8);
	#endregion
	#region DMDTypes
	public sealed class DMDAction : DirectDMD<DMDAction.EmitDelegate, XAct>
	{
		public delegate DMDReturn EmitDelegate(DMDCursor<Empty, XAct> stack);
		public DMDAction( String name, EmitDelegate emitter ) : base( name, emitter ) {}
		protected sealed override Type returnType { get => null; }
		protected sealed override Type[] argTypes { get => new Type[] {  }; }
		protected sealed override void InvokeEmitter( ILContext context, EmitDelegate emitter )
		{
			var st = new Empty();
			var stack = new DMDCursor<Empty, XAct>( st, context );
			emitter(stack);
		}
	}

	public sealed class DMDAction<T1> : DirectDMD<DMDAction<T1>.EmitDelegate, XAct<T1>>
	{
		public delegate DMDReturn EmitDelegate(DMDCursor<Empty, XAct<T1>> stack, ILArg<T1> arg1);
		public DMDAction( String name, EmitDelegate emitter ) : base( name, emitter ) {}
		protected sealed override Type returnType { get => null; }
		protected sealed override Type[] argTypes { get => new Type[] { typeof(T1) }; }
		protected sealed override void InvokeEmitter( ILContext context, EmitDelegate emitter )
		{
			var st = new Empty();
			var stack = new DMDCursor<Empty, XAct<T1>>( st, context );
			var arg1 = new ILArg<T1>(1);
			emitter(stack, arg1);
		}
	}

	public sealed class DMDAction<T1, T2> : DirectDMD<DMDAction<T1, T2>.EmitDelegate, XAct<T1, T2>>
	{
		public delegate DMDReturn EmitDelegate(DMDCursor<Empty, XAct<T1, T2>> stack, ILArg<T1> arg1, ILArg<T2> arg2);
		public DMDAction( String name, EmitDelegate emitter ) : base( name, emitter ) {}
		protected sealed override Type returnType { get => null; }
		protected sealed override Type[] argTypes { get => new Type[] { typeof(T1), typeof(T2) }; }
		protected sealed override void InvokeEmitter( ILContext context, EmitDelegate emitter )
		{
			var st = new Empty();
			var stack = new DMDCursor<Empty, XAct<T1, T2>>( st, context );
			var arg1 = new ILArg<T1>(1);
			var arg2 = new ILArg<T2>(2);
			emitter(stack, arg1, arg2);
		}
	}

	public sealed class DMDAction<T1, T2, T3> : DirectDMD<DMDAction<T1, T2, T3>.EmitDelegate, XAct<T1, T2, T3>>
	{
		public delegate DMDReturn EmitDelegate(DMDCursor<Empty, XAct<T1, T2, T3>> stack, ILArg<T1> arg1, ILArg<T2> arg2, ILArg<T3> arg3);
		public DMDAction( String name, EmitDelegate emitter ) : base( name, emitter ) {}
		protected sealed override Type returnType { get => null; }
		protected sealed override Type[] argTypes { get => new Type[] { typeof(T1), typeof(T2), typeof(T3) }; }
		protected sealed override void InvokeEmitter( ILContext context, EmitDelegate emitter )
		{
			var st = new Empty();
			var stack = new DMDCursor<Empty, XAct<T1, T2, T3>>( st, context );
			var arg1 = new ILArg<T1>(1);
			var arg2 = new ILArg<T2>(2);
			var arg3 = new ILArg<T3>(3);
			emitter(stack, arg1, arg2, arg3);
		}
	}

	public sealed class DMDAction<T1, T2, T3, T4> : DirectDMD<DMDAction<T1, T2, T3, T4>.EmitDelegate, XAct<T1, T2, T3, T4>>
	{
		public delegate DMDReturn EmitDelegate(DMDCursor<Empty, XAct<T1, T2, T3, T4>> stack, ILArg<T1> arg1, ILArg<T2> arg2, ILArg<T3> arg3, ILArg<T4> arg4);
		public DMDAction( String name, EmitDelegate emitter ) : base( name, emitter ) {}
		protected sealed override Type returnType { get => null; }
		protected sealed override Type[] argTypes { get => new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) }; }
		protected sealed override void InvokeEmitter( ILContext context, EmitDelegate emitter )
		{
			var st = new Empty();
			var stack = new DMDCursor<Empty, XAct<T1, T2, T3, T4>>( st, context );
			var arg1 = new ILArg<T1>(1);
			var arg2 = new ILArg<T2>(2);
			var arg3 = new ILArg<T3>(3);
			var arg4 = new ILArg<T4>(4);
			emitter(stack, arg1, arg2, arg3, arg4);
		}
	}

	public sealed class DMDAction<T1, T2, T3, T4, T5> : DirectDMD<DMDAction<T1, T2, T3, T4, T5>.EmitDelegate, XAct<T1, T2, T3, T4, T5>>
	{
		public delegate DMDReturn EmitDelegate(DMDCursor<Empty, XAct<T1, T2, T3, T4, T5>> stack, ILArg<T1> arg1, ILArg<T2> arg2, ILArg<T3> arg3, ILArg<T4> arg4, ILArg<T5> arg5);
		public DMDAction( String name, EmitDelegate emitter ) : base( name, emitter ) {}
		protected sealed override Type returnType { get => null; }
		protected sealed override Type[] argTypes { get => new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5) }; }
		protected sealed override void InvokeEmitter( ILContext context, EmitDelegate emitter )
		{
			var st = new Empty();
			var stack = new DMDCursor<Empty, XAct<T1, T2, T3, T4, T5>>( st, context );
			var arg1 = new ILArg<T1>(1);
			var arg2 = new ILArg<T2>(2);
			var arg3 = new ILArg<T3>(3);
			var arg4 = new ILArg<T4>(4);
			var arg5 = new ILArg<T5>(5);
			emitter(stack, arg1, arg2, arg3, arg4, arg5);
		}
	}

	public sealed class DMDAction<T1, T2, T3, T4, T5, T6> : DirectDMD<DMDAction<T1, T2, T3, T4, T5, T6>.EmitDelegate, XAct<T1, T2, T3, T4, T5, T6>>
	{
		public delegate DMDReturn EmitDelegate(DMDCursor<Empty, XAct<T1, T2, T3, T4, T5, T6>> stack, ILArg<T1> arg1, ILArg<T2> arg2, ILArg<T3> arg3, ILArg<T4> arg4, ILArg<T5> arg5, ILArg<T6> arg6);
		public DMDAction( String name, EmitDelegate emitter ) : base( name, emitter ) {}
		protected sealed override Type returnType { get => null; }
		protected sealed override Type[] argTypes { get => new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6) }; }
		protected sealed override void InvokeEmitter( ILContext context, EmitDelegate emitter )
		{
			var st = new Empty();
			var stack = new DMDCursor<Empty, XAct<T1, T2, T3, T4, T5, T6>>( st, context );
			var arg1 = new ILArg<T1>(1);
			var arg2 = new ILArg<T2>(2);
			var arg3 = new ILArg<T3>(3);
			var arg4 = new ILArg<T4>(4);
			var arg5 = new ILArg<T5>(5);
			var arg6 = new ILArg<T6>(6);
			emitter(stack, arg1, arg2, arg3, arg4, arg5, arg6);
		}
	}

	public sealed class DMDAction<T1, T2, T3, T4, T5, T6, T7> : DirectDMD<DMDAction<T1, T2, T3, T4, T5, T6, T7>.EmitDelegate, XAct<T1, T2, T3, T4, T5, T6, T7>>
	{
		public delegate DMDReturn EmitDelegate(DMDCursor<Empty, XAct<T1, T2, T3, T4, T5, T6, T7>> stack, ILArg<T1> arg1, ILArg<T2> arg2, ILArg<T3> arg3, ILArg<T4> arg4, ILArg<T5> arg5, ILArg<T6> arg6, ILArg<T7> arg7);
		public DMDAction( String name, EmitDelegate emitter ) : base( name, emitter ) {}
		protected sealed override Type returnType { get => null; }
		protected sealed override Type[] argTypes { get => new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7) }; }
		protected sealed override void InvokeEmitter( ILContext context, EmitDelegate emitter )
		{
			var st = new Empty();
			var stack = new DMDCursor<Empty, XAct<T1, T2, T3, T4, T5, T6, T7>>( st, context );
			var arg1 = new ILArg<T1>(1);
			var arg2 = new ILArg<T2>(2);
			var arg3 = new ILArg<T3>(3);
			var arg4 = new ILArg<T4>(4);
			var arg5 = new ILArg<T5>(5);
			var arg6 = new ILArg<T6>(6);
			var arg7 = new ILArg<T7>(7);
			emitter(stack, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
		}
	}

	public sealed class DMDAction<T1, T2, T3, T4, T5, T6, T7, T8> : DirectDMD<DMDAction<T1, T2, T3, T4, T5, T6, T7, T8>.EmitDelegate, XAct<T1, T2, T3, T4, T5, T6, T7, T8>>
	{
		public delegate DMDReturn EmitDelegate(DMDCursor<Empty, XAct<T1, T2, T3, T4, T5, T6, T7, T8>> stack, ILArg<T1> arg1, ILArg<T2> arg2, ILArg<T3> arg3, ILArg<T4> arg4, ILArg<T5> arg5, ILArg<T6> arg6, ILArg<T7> arg7, ILArg<T8> arg8);
		public DMDAction( String name, EmitDelegate emitter ) : base( name, emitter ) {}
		protected sealed override Type returnType { get => null; }
		protected sealed override Type[] argTypes { get => new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8) }; }
		protected sealed override void InvokeEmitter( ILContext context, EmitDelegate emitter )
		{
			var st = new Empty();
			var stack = new DMDCursor<Empty, XAct<T1, T2, T3, T4, T5, T6, T7, T8>>( st, context );
			var arg1 = new ILArg<T1>(1);
			var arg2 = new ILArg<T2>(2);
			var arg3 = new ILArg<T3>(3);
			var arg4 = new ILArg<T4>(4);
			var arg5 = new ILArg<T5>(5);
			var arg6 = new ILArg<T6>(6);
			var arg7 = new ILArg<T7>(7);
			var arg8 = new ILArg<T8>(8);
			emitter(stack, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
		}
	}

	public sealed class DMDFunc<TReturn> : DirectDMD<DMDFunc<TReturn>.EmitDelegate, XFunc<TReturn>>
	{
		public delegate DMDReturn<TReturn> EmitDelegate(DMDCursor<Empty, XFunc<TReturn>> stack);
		public DMDFunc( String name, EmitDelegate emitter ) : base( name, emitter ) {}
		protected sealed override Type returnType { get => typeof(TReturn); }
		protected sealed override Type[] argTypes { get => new Type[] {  }; }
		protected sealed override void InvokeEmitter( ILContext context, EmitDelegate emitter )
		{
			var st = new Empty();
			var stack = new DMDCursor<Empty, XFunc<TReturn>>( st, context );
			emitter(stack);
		}
	}

	public sealed class DMDFunc<T1, TReturn> : DirectDMD<DMDFunc<T1, TReturn>.EmitDelegate, XFunc<T1, TReturn>>
	{
		public delegate DMDReturn<TReturn> EmitDelegate(DMDCursor<Empty, XFunc<T1, TReturn>> stack, ILArg<T1> arg1);
		public DMDFunc( String name, EmitDelegate emitter ) : base( name, emitter ) {}
		protected sealed override Type returnType { get => typeof(TReturn); }
		protected sealed override Type[] argTypes { get => new Type[] { typeof(T1) }; }
		protected sealed override void InvokeEmitter( ILContext context, EmitDelegate emitter )
		{
			var st = new Empty();
			var stack = new DMDCursor<Empty, XFunc<T1, TReturn>>( st, context );
			var arg1 = new ILArg<T1>(1);
			emitter(stack, arg1);
		}
	}

	public sealed class DMDFunc<T1, T2, TReturn> : DirectDMD<DMDFunc<T1, T2, TReturn>.EmitDelegate, XFunc<T1, T2, TReturn>>
	{
		public delegate DMDReturn<TReturn> EmitDelegate(DMDCursor<Empty, XFunc<T1, T2, TReturn>> stack, ILArg<T1> arg1, ILArg<T2> arg2);
		public DMDFunc( String name, EmitDelegate emitter ) : base( name, emitter ) {}
		protected sealed override Type returnType { get => typeof(TReturn); }
		protected sealed override Type[] argTypes { get => new Type[] { typeof(T1), typeof(T2) }; }
		protected sealed override void InvokeEmitter( ILContext context, EmitDelegate emitter )
		{
			var st = new Empty();
			var stack = new DMDCursor<Empty, XFunc<T1, T2, TReturn>>( st, context );
			var arg1 = new ILArg<T1>(1);
			var arg2 = new ILArg<T2>(2);
			emitter(stack, arg1, arg2);
		}
	}

	public sealed class DMDFunc<T1, T2, T3, TReturn> : DirectDMD<DMDFunc<T1, T2, T3, TReturn>.EmitDelegate, XFunc<T1, T2, T3, TReturn>>
	{
		public delegate DMDReturn<TReturn> EmitDelegate(DMDCursor<Empty, XFunc<T1, T2, T3, TReturn>> stack, ILArg<T1> arg1, ILArg<T2> arg2, ILArg<T3> arg3);
		public DMDFunc( String name, EmitDelegate emitter ) : base( name, emitter ) {}
		protected sealed override Type returnType { get => typeof(TReturn); }
		protected sealed override Type[] argTypes { get => new Type[] { typeof(T1), typeof(T2), typeof(T3) }; }
		protected sealed override void InvokeEmitter( ILContext context, EmitDelegate emitter )
		{
			var st = new Empty();
			var stack = new DMDCursor<Empty, XFunc<T1, T2, T3, TReturn>>( st, context );
			var arg1 = new ILArg<T1>(1);
			var arg2 = new ILArg<T2>(2);
			var arg3 = new ILArg<T3>(3);
			emitter(stack, arg1, arg2, arg3);
		}
	}

	public sealed class DMDFunc<T1, T2, T3, T4, TReturn> : DirectDMD<DMDFunc<T1, T2, T3, T4, TReturn>.EmitDelegate, XFunc<T1, T2, T3, T4, TReturn>>
	{
		public delegate DMDReturn<TReturn> EmitDelegate(DMDCursor<Empty, XFunc<T1, T2, T3, T4, TReturn>> stack, ILArg<T1> arg1, ILArg<T2> arg2, ILArg<T3> arg3, ILArg<T4> arg4);
		public DMDFunc( String name, EmitDelegate emitter ) : base( name, emitter ) {}
		protected sealed override Type returnType { get => typeof(TReturn); }
		protected sealed override Type[] argTypes { get => new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) }; }
		protected sealed override void InvokeEmitter( ILContext context, EmitDelegate emitter )
		{
			var st = new Empty();
			var stack = new DMDCursor<Empty, XFunc<T1, T2, T3, T4, TReturn>>( st, context );
			var arg1 = new ILArg<T1>(1);
			var arg2 = new ILArg<T2>(2);
			var arg3 = new ILArg<T3>(3);
			var arg4 = new ILArg<T4>(4);
			emitter(stack, arg1, arg2, arg3, arg4);
		}
	}

	public sealed class DMDFunc<T1, T2, T3, T4, T5, TReturn> : DirectDMD<DMDFunc<T1, T2, T3, T4, T5, TReturn>.EmitDelegate, XFunc<T1, T2, T3, T4, T5, TReturn>>
	{
		public delegate DMDReturn<TReturn> EmitDelegate(DMDCursor<Empty, XFunc<T1, T2, T3, T4, T5, TReturn>> stack, ILArg<T1> arg1, ILArg<T2> arg2, ILArg<T3> arg3, ILArg<T4> arg4, ILArg<T5> arg5);
		public DMDFunc( String name, EmitDelegate emitter ) : base( name, emitter ) {}
		protected sealed override Type returnType { get => typeof(TReturn); }
		protected sealed override Type[] argTypes { get => new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5) }; }
		protected sealed override void InvokeEmitter( ILContext context, EmitDelegate emitter )
		{
			var st = new Empty();
			var stack = new DMDCursor<Empty, XFunc<T1, T2, T3, T4, T5, TReturn>>( st, context );
			var arg1 = new ILArg<T1>(1);
			var arg2 = new ILArg<T2>(2);
			var arg3 = new ILArg<T3>(3);
			var arg4 = new ILArg<T4>(4);
			var arg5 = new ILArg<T5>(5);
			emitter(stack, arg1, arg2, arg3, arg4, arg5);
		}
	}

	public sealed class DMDFunc<T1, T2, T3, T4, T5, T6, TReturn> : DirectDMD<DMDFunc<T1, T2, T3, T4, T5, T6, TReturn>.EmitDelegate, XFunc<T1, T2, T3, T4, T5, T6, TReturn>>
	{
		public delegate DMDReturn<TReturn> EmitDelegate(DMDCursor<Empty, XFunc<T1, T2, T3, T4, T5, T6, TReturn>> stack, ILArg<T1> arg1, ILArg<T2> arg2, ILArg<T3> arg3, ILArg<T4> arg4, ILArg<T5> arg5, ILArg<T6> arg6);
		public DMDFunc( String name, EmitDelegate emitter ) : base( name, emitter ) {}
		protected sealed override Type returnType { get => typeof(TReturn); }
		protected sealed override Type[] argTypes { get => new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6) }; }
		protected sealed override void InvokeEmitter( ILContext context, EmitDelegate emitter )
		{
			var st = new Empty();
			var stack = new DMDCursor<Empty, XFunc<T1, T2, T3, T4, T5, T6, TReturn>>( st, context );
			var arg1 = new ILArg<T1>(1);
			var arg2 = new ILArg<T2>(2);
			var arg3 = new ILArg<T3>(3);
			var arg4 = new ILArg<T4>(4);
			var arg5 = new ILArg<T5>(5);
			var arg6 = new ILArg<T6>(6);
			emitter(stack, arg1, arg2, arg3, arg4, arg5, arg6);
		}
	}

	public sealed class DMDFunc<T1, T2, T3, T4, T5, T6, T7, TReturn> : DirectDMD<DMDFunc<T1, T2, T3, T4, T5, T6, T7, TReturn>.EmitDelegate, XFunc<T1, T2, T3, T4, T5, T6, T7, TReturn>>
	{
		public delegate DMDReturn<TReturn> EmitDelegate(DMDCursor<Empty, XFunc<T1, T2, T3, T4, T5, T6, T7, TReturn>> stack, ILArg<T1> arg1, ILArg<T2> arg2, ILArg<T3> arg3, ILArg<T4> arg4, ILArg<T5> arg5, ILArg<T6> arg6, ILArg<T7> arg7);
		public DMDFunc( String name, EmitDelegate emitter ) : base( name, emitter ) {}
		protected sealed override Type returnType { get => typeof(TReturn); }
		protected sealed override Type[] argTypes { get => new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7) }; }
		protected sealed override void InvokeEmitter( ILContext context, EmitDelegate emitter )
		{
			var st = new Empty();
			var stack = new DMDCursor<Empty, XFunc<T1, T2, T3, T4, T5, T6, T7, TReturn>>( st, context );
			var arg1 = new ILArg<T1>(1);
			var arg2 = new ILArg<T2>(2);
			var arg3 = new ILArg<T3>(3);
			var arg4 = new ILArg<T4>(4);
			var arg5 = new ILArg<T5>(5);
			var arg6 = new ILArg<T6>(6);
			var arg7 = new ILArg<T7>(7);
			emitter(stack, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
		}
	}

	public sealed class DMDFunc<T1, T2, T3, T4, T5, T6, T7, T8, TReturn> : DirectDMD<DMDFunc<T1, T2, T3, T4, T5, T6, T7, T8, TReturn>.EmitDelegate, XFunc<T1, T2, T3, T4, T5, T6, T7, T8, TReturn>>
	{
		public delegate DMDReturn<TReturn> EmitDelegate(DMDCursor<Empty, XFunc<T1, T2, T3, T4, T5, T6, T7, T8, TReturn>> stack, ILArg<T1> arg1, ILArg<T2> arg2, ILArg<T3> arg3, ILArg<T4> arg4, ILArg<T5> arg5, ILArg<T6> arg6, ILArg<T7> arg7, ILArg<T8> arg8);
		public DMDFunc( String name, EmitDelegate emitter ) : base( name, emitter ) {}
		protected sealed override Type returnType { get => typeof(TReturn); }
		protected sealed override Type[] argTypes { get => new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8) }; }
		protected sealed override void InvokeEmitter( ILContext context, EmitDelegate emitter )
		{
			var st = new Empty();
			var stack = new DMDCursor<Empty, XFunc<T1, T2, T3, T4, T5, T6, T7, T8, TReturn>>( st, context );
			var arg1 = new ILArg<T1>(1);
			var arg2 = new ILArg<T2>(2);
			var arg3 = new ILArg<T3>(3);
			var arg4 = new ILArg<T4>(4);
			var arg5 = new ILArg<T5>(5);
			var arg6 = new ILArg<T6>(6);
			var arg7 = new ILArg<T7>(7);
			var arg8 = new ILArg<T8>(8);
			emitter(stack, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
		}
	}

	#endregion
}
