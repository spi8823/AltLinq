namespace AltLinq
{
    public static partial class Enumerable
    {
        public static IAltEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            var enumerable = ObjectPool<SelectIterator<TSource, TResult>>.Pop();
            enumerable.Init(source, selector);
            return enumerable;
        }
    }

    public class SelectIterator<TSource, TDistination> : IAltEnumerable<TDistination>
    {
        public TDistination Current { get; private set; }

        private IEnumerable<TSource> sourceEnumerable;
        private IEnumerator<TSource> sourceEnumerator;
        private Func<TSource, TDistination> selector;

        public SelectIterator() { }

        internal void Init(IEnumerable<TSource> source, Func<TSource, TDistination> selector)
        {
            sourceEnumerable = source;
            sourceEnumerator = sourceEnumerable.GetEnumerator();
            this.selector = selector;
        }

        public bool MoveNext()
        {
            if (sourceEnumerator.MoveNext())
            {
                Current = selector(sourceEnumerator.Current);
                return true;
            }
            return false;
        }

        public void Reset()
        {
            sourceEnumerator.Reset();
        }

        public void Dispose()
        {
            sourceEnumerable = null;
            sourceEnumerator = null;
            selector = null;
            ObjectPool<SelectIterator<TSource, TDistination>>.Push(this);
        }
    }
}
