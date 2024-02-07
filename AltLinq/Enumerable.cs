using System.Collections;

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
        public static bool All<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate) => System.Linq.Enumerable.All(source, predicate);
        public static bool Any<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate) => System.Linq.Enumerable.Any(source, predicate);
        public static int Count<TSource>(this IEnumerable<TSource> source) => System.Linq.Enumerable.Count(source);
        public static int Count<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate) => System.Linq.Enumerable.Count(source, predicate);

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
