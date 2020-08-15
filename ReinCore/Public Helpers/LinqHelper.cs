namespace ReinCore
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;

    using UnityEngine;

    public static class LinqHelper
    {
        public delegate Boolean Selector<TSource, TResult>(TSource item, out TResult result);
        public static IEnumerable<TResult> SelectWhere<TSource, TResult>(this IEnumerable<TSource> source, Selector<TSource, TResult> selector )
        {
            var em = Enumerable.Empty<TResult>();
            IEnumerable<TResult> GetResult(TSource item) => selector(item, out var res) ? em.Append(res) : em;
            return source.SelectMany(GetResult);
        }
    }
}
