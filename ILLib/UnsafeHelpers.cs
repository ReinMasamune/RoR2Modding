using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace ILLib
{
    public static class UnsafeHelpers
    {
        [MethodImpl( MethodImplOptions.ForwardRef )]
        public unsafe static extern ref T AsRef<T>( void* source );
    }
}
