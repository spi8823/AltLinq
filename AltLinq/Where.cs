namespace AltLinq
{
    public static partial class Enumerable
    {
        public static IAltEnumerable<T> Where<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            var enumerable = ObjectPool<WhereIterator<T>>.Pop();
            enumerable.Init(source, predicate);
            return enumerable;
        }
    }

    public class WhereIterator<T> : IAltEnumerable<T>
    {
        public T Current { get; private set; }

        private IEnumerable<T> sourceEnumerable;
        private IEnumerator<T> sourceEnumerator;
        private Func<T, bool> predicate;

        public WhereIterator() { }
        internal void Init(IEnumerable<T> source, Func<T, bool> predicate)
        {
            sourceEnumerable = source;
            sourceEnumerator = sourceEnumerable.GetEnumerator();
            this.predicate = predicate;
        }

        public bool MoveNext()
        {
            while(sourceEnumerator.MoveNext())
            {
                if(predicate(sourceEnumerator.Current))
                {
                    Current = sourceEnumerator.Current;
                    return true;
                }
            }
            return false;
        }

        public void Reset()
        {
            sourceEnumerator.Reset();
        }

        public void Dispose()
        {
            Current = default;
            sourceEnumerable = null;
            sourceEnumerator = null;
            predicate = null;
            ObjectPool<WhereIterator<T>>.Push(this);
        }
    }
}
