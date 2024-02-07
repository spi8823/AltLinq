using System.Diagnostics.Metrics;

namespace AltLinq
{
    public static partial class Enumerable
    {
        //int
        public static IAltEnumerable<TResult> Join<TOuter, TInner, TResult>(
            this IEnumerable<TOuter> outer,
            IEnumerable<TInner> inner,
            Func<TOuter, int> outerKeySelector,
            Func<TInner, int> innerKeySelector,
            Func<TOuter, TInner, TResult> resultSelector)
        {
            var join = ObjectPool<JoinIterator<TOuter, TInner>>.Pop();
            join.Init(outer, inner);
            return join.Where(pair => outerKeySelector(pair.Item1) == innerKeySelector(pair.Item2)).Select(pair => resultSelector(pair.Item1, pair.Item2));
        }

        //long
        public static IAltEnumerable<TResult> Join<TOuter, TInner, TResult>(
            this IEnumerable<TOuter> outer,
            IEnumerable<TInner> inner,
            Func<TOuter, long> outerKeySelector,
            Func<TInner, long> innerKeySelector,
            Func<TOuter, TInner, TResult> resultSelector)
        {
            var join = ObjectPool<JoinIterator<TOuter, TInner>>.Pop();
            join.Init(outer, inner);
            return join.Where(pair => outerKeySelector(pair.Item1) == innerKeySelector(pair.Item2)).Select(pair => resultSelector(pair.Item1, pair.Item2));
        }

        //float
        public static IAltEnumerable<TResult> Join<TOuter, TInner, TResult>(
            this IEnumerable<TOuter> outer,
            IEnumerable<TInner> inner,
            Func<TOuter, float> outerKeySelector,
            Func<TInner, float> innerKeySelector,
            Func<TOuter, TInner, TResult> resultSelector)
        {
            var join = ObjectPool<JoinIterator<TOuter, TInner>>.Pop();
            join.Init(outer, inner);
            return join.Where(pair => outerKeySelector(pair.Item1) == innerKeySelector(pair.Item2)).Select(pair => resultSelector(pair.Item1, pair.Item2));
        }

        //double
        public static IAltEnumerable<TResult> Join<TOuter, TInner, TResult>(
            this IEnumerable<TOuter> outer,
            IEnumerable<TInner> inner,
            Func<TOuter, double> outerKeySelector,
            Func<TInner, double> innerKeySelector,
            Func<TOuter, TInner, TResult> resultSelector)
        {
            var join = ObjectPool<JoinIterator<TOuter, TInner>>.Pop();
            join.Init(outer, inner);
            return join.Where(pair => outerKeySelector(pair.Item1) == innerKeySelector(pair.Item2)).Select(pair => resultSelector(pair.Item1, pair.Item2));
        }

        public static IAltEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(
            this IEnumerable<TOuter> outer,
            IEnumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TOuter, TInner, TResult> resultSelector) where TKey : IEquatable<TKey>
        {
            var join = ObjectPool<JoinIterator<TOuter, TInner>>.Pop();
            join.Init(outer, inner);
            return join.Where(pair => outerKeySelector(pair.Item1).Equals(innerKeySelector(pair.Item2))).Select(pair => resultSelector(pair.Item1, pair.Item2));
        }
    }

    public class JoinIterator<TOuter, TInner> : IAltEnumerable<(TOuter, TInner)>
    {
        public (TOuter, TInner) Current { get; private set; }

        private IEnumerator<TOuter> outerEnumerator;
        private IEnumerator<TInner> innerEnumerator;
        private bool outerEnumeratorCouldMoved = false;

        public JoinIterator() { }
        public void Init(
            IEnumerable<TOuter> outer,
            IEnumerable<TInner> inner)
        {
            outerEnumerator = outer.GetEnumerator();
            innerEnumerator = inner.GetEnumerator();

            outerEnumeratorCouldMoved = false;
        }

        public bool MoveNext()
        {
            if (!outerEnumeratorCouldMoved)
            {
                if (outerEnumerator.MoveNext())
                {
                    outerEnumeratorCouldMoved = true;
                }
                else
                {
                    return false;
                }
            }

            if (innerEnumerator.MoveNext())
            {
                Current = (outerEnumerator.Current, innerEnumerator.Current);
                return true;
            }
            else if (outerEnumerator.MoveNext())
            {
                innerEnumerator.Reset();
                if (innerEnumerator.MoveNext())
                {
                    Current = (outerEnumerator.Current, innerEnumerator.Current);
                    return true;
                }
            }
            else
            {
                outerEnumeratorCouldMoved = false;
            }
            return false;
        }

        public void Reset()
        {
            outerEnumerator.Reset();
            innerEnumerator.Reset();
            outerEnumeratorCouldMoved = false;
        }

        public void Dispose()
        {
            outerEnumerator.Dispose();
            outerEnumerator = null;
            innerEnumerator.Dispose();
            innerEnumerator = null;

            ObjectPool<JoinIterator<TOuter, TInner>>.Push(this);
        }
    }
}
