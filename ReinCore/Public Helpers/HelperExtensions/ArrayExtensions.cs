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


        //public static void AddTo<T>(this T self, ref T[] destination, params T[] items)
        //{
        //    var start = self.Length;
        //    var added = 1 + items.Length;
        //    Array.Resize<T>(ref self, start + added);
        //    self[start++] = item;
        //    self.CopyItemsFrom(ref items, start);
        //}

        //public static void CopyItemsFrom<T>(ref this T[] self, ref T[] items, Int32 start)
        //{
        //    for(Int32 i = 0; i < items.Length; ++i)
        //    {

        //    }
        //}
    }
}
