namespace RuntimeCodeGen
{
    using System;
    using System.Runtime.CompilerServices;

    public class Class1
    {
        [MethodImpl( MethodImplOptions.ForwardRef )]
        public extern int Square( int number );
    }
}
/* dmd.Definition
 * dmd.GetDumpName( "Cecil" );
 * ReflectionImporterProvider
 * c_UnverifiableCodeAttribute
 * dmd.OriginalMethod
 * 
 * Relinker
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 */