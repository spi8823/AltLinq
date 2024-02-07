using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltLinq
{
    public class SelectEnumerable<TSource, TDistination> : IAltEnumerable<TDistination>
    {
        public TDistination Current { get; private set; }
        object IEnumerator.Current => Current;

        private IEnumerable<TSource> sourceEnumerable;
        private IEnumerator<TSource> sourceEnumerator;
        private Func<TSource, TDistination> selector;

        public SelectEnumerable() { }

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
            ObjectPool<SelectEnumerable<TSource, TDistination>>.Push(this);
        }
    }
}
