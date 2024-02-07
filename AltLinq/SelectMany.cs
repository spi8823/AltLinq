using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltLinq
{
    public static partial class Enumerable
    {
        public static IAltEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, IEnumerable<TCollection>> collectionSelector,
            Func<TSource, TCollection, TResult> resultSelector)
        {
            var iterator = ObjectPool<SelectManyIterator<TSource, TCollection, TResult>>.Pop();
            iterator.Init(source, collectionSelector, resultSelector);
            return iterator;
        }
    }

    internal class SelectManyIterator<TSource, TCollection, TResult> : IAltEnumerable<TResult>
    {
        public TResult Current { get; private set; }
        object IEnumerator.Current => Current;

        private IEnumerator<TSource> sourceEnumerator;
        private IEnumerator<TCollection> collectionEnumerator;
        Func<TSource, IEnumerable<TCollection>> collectionSelector;
        Func<TSource, TCollection, TResult> resultSelector;

        public SelectManyIterator() { }
        public void Init(
            IEnumerable<TSource> source,
            Func<TSource, IEnumerable<TCollection>> collectionSelector,
            Func<TSource, TCollection, TResult> resultSelector)
        {
            sourceEnumerator = source.GetEnumerator();
            this.collectionSelector = collectionSelector;
            this.resultSelector = resultSelector;
        }

        public bool MoveNext()
        {
            if(collectionEnumerator != null && collectionEnumerator.MoveNext())
            {
                Current = resultSelector(sourceEnumerator.Current, collectionEnumerator.Current);
                return true;
            }
            else if(sourceEnumerator != null && sourceEnumerator.MoveNext())
            {
                collectionEnumerator = collectionSelector(sourceEnumerator.Current).GetEnumerator();
                if(collectionEnumerator != null && collectionEnumerator.MoveNext())
                {
                    Current = resultSelector(sourceEnumerator.Current, collectionEnumerator.Current);
                    return true;
                }
            }
            return false;
        }

        public void Reset()
        {
            sourceEnumerator.Reset();
            collectionEnumerator = null;
        }

        public void Dispose()
        {
            sourceEnumerator?.Dispose();
            sourceEnumerator = null;
            collectionEnumerator?.Dispose();
            collectionEnumerator = null;
            
            collectionSelector = null;
            resultSelector = null;

            ObjectPool<SelectManyIterator<TSource, TCollection, TResult>>.Push(this);
        }
    }
}
