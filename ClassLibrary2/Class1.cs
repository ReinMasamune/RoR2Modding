namespace ClassLibrary2
{
    using System;
    using System.Reflection.Emit;
    public class Class1
    {
        public Type Stuff()
        {
            return typeof( System.Reflection.Emit.AssemblyBuilder );
        }
    }

    public class Program
    {
        public int Main( string[] args)
        {
            return 0;
        }
    }
}
