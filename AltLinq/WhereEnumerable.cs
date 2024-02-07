using System.Collections;

namespace AltLinq
{
    public class WhereEnumerable<T> : IAltEnumerable<T>
    {
        public T Current { get; private set; }
        object IEnumerator.Current => Current;

        private IEnumerable<T> sourceEnumerable;
        private IEnumerator<T> sourceEnumerator;
        private Func<T, bool> predicate;

        public WhereEnumerable() { }
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
            ObjectPool<WhereEnumerable<T>>.Push(this);
        }
    }
}
