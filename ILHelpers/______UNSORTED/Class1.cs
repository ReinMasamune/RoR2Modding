namespace ILHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public static class TypeRef<T>
    {
        private static readonly TRef<T> cached = new TRef<T>();
    }

    public class TRef<T> 
    {
        internal Type t = typeof(T);
    }
}
