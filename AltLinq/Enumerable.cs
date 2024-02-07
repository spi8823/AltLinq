using System.Collections;
using System.Collections.Generic;

namespace AltLinq
{
    public interface IAltEnumerable<T> : IEnumerable<T>, IEnumerator<T>, IDisposable
    {
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => this;
        IEnumerator IEnumerable.GetEnumerator() => this;
    }
    
    public static class Enumerable
    {
        public static IAltEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            var enumerable = ObjectPool<SelectEnumerable<TSource, TResult>>.Pop();
            enumerable.Init(source, selector);
            return enumerable;
        }

        public static IAltEnumerable<T> Where<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            var enumerable = ObjectPool<WhereEnumerable<T>>.Pop();
            enumerable.Init(source, predicate);
            return enumerable;
        }

        public static bool All<TSource>(this IEnumerable<TSource> sources, Func<TSource, bool> p) => System.Linq.Enumerable.All(sources, p);
        public static bool Any<TSource>(this IEnumerable<TSource> sources, Func<TSource, bool> predicate) => System.Linq.Enumerable.Any(sources, predicate);

        public static void Test()
        {
            var listlist = new List<List<long>>();
            using var result = from list in listlist
                               from i in list
                               where i < 0
                               select i;
        }
    }
}
