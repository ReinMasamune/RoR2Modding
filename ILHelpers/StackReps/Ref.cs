namespace ILHelper
{
    using System;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Runtime.CompilerServices;

    using MonoMod.Cil;

    using Object = System.Object;

    [_PATCH.MakeByRef]
    public struct ByRef<T>
    {
        [_PATCH.MakeManagedPointerType]
        private readonly T _backing;

        [property:_PATCH.MakeManagedPointerType]
        public T val
        {
            [method: _PATCH.ILBody("ldarg.0", "ret")]
            [return: _PATCH.MakeManagedPointerType]
            get => this._backing;
        }
    }
}
