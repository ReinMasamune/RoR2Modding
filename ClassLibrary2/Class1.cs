using System;
using System.Runtime.CompilerServices;

namespace ClassLibrary2
{
    public class Class1
    {
        [MethodImpl( MethodImplOptions.ForwardRef )]
        public extern int Square( int number );
    }
}
