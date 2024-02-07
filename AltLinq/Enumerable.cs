using System.Collections;
using System.Collections.Generic;

namespace AltLinq
{
    public interface IAltEnumerable<T> : IEnumerable<T>, IEnumerator<T>, IDisposable
    {
        object IEnumerator.Current => Current;
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => this;
        IEnumerator IEnumerable.GetEnumerator() => this;
    }
    
    public static partial class Enumerable
    {
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
