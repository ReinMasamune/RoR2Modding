using System;
using System.Runtime.CompilerServices;

namespace InterfaceThings
{
    public class Class1
    {
        [MethodImpl( MethodImplOptions.ForwardRef )]
        public extern int Square( int number );
    }

    public interface ITestInterface
    {
        ITestInterface ConvertMe( System.Object obj )
        {

        }
    }

    public abstract class ExampleClass
    {
        public static explicit operator ExampleClass( Class1 obj )
        {
            Console.WriteLine( "I am being converted?" );
            return default;
        }
    }
}
