using System;
using System.Collections.Generic;
using System.Text;
namespace AltLinq
{
    public static class ObjectPool<T> where T : class, new()
    {
        private static readonly Stack<T> stack = new Stack<T>();

        public static T Pop()
        {
            if(stack.TryPop(out var result))
            {
                return result;
            }
            return new T();
        }

        public static void Push(T t)
        {
            stack.Push(t);
        }
    }
}
