.class public abstract sealed PointerLib.Unsafe
{
	.method public static !!T& AsRef<T>(void* ptr) cil managed
	{
		.maxstack 1
		ldarg.0
		ret
	}

	.method public static int32 SizeOf<T>() cil managed
	{
		.maxstack 1
		sizeof		!!T
		ret
	}
}