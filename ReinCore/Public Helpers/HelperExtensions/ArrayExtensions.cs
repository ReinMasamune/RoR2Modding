namespace ReinCore
{
    using System;
    using System.Collections.Generic;

    public static class ArrayExtensions
    {
        public static IEnumerator<T> GetEnumerator<T>(this T[] self)
        {
            for(Int32 i = 0; i < self.Length; ++i)
            {
                yield return self[i];
            }
        }

    }
}
