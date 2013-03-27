using System;
using System.Collections.Generic;

namespace Alba.Framework.Collections
{
    public static class EnumerableExts
    {
        public static string JoinString<T> (this IEnumerable<T> @this, string separator)
        {
            return string.Join(separator, @this);
        }

        public static string JoinString (this IEnumerable<string> @this, string separator)
        {
            return string.Join(separator, @this);
        }

        public static string ConcatString<T> (this IEnumerable<T> @this)
        {
            return string.Concat(@this);
        }

        public static string ConcatString (this IEnumerable<string> @this)
        {
            return string.Concat(@this);
        }

        public static IEnumerable<T> TraverseList<T> (this T root, Func<T, T> getNext) where T : class
        {
            for (T current = root; current != null; current = getNext(current))
                yield return current;
        }

        public static IEnumerable<T> TraverseTree<T> (this T root, Func<T, IEnumerable<T>> getChildren)
        {
            return TraverseTreeDepth(root, getChildren);
        }

        public static IEnumerable<T> TraverseTreeDepth<T> (this T root, Func<T, IEnumerable<T>> getChildren)
        {
            var stack = new Stack<T>();
            stack.Push(root);

            while (stack.Count != 0) {
                T item = stack.Pop();
                yield return item;
                foreach (var child in getChildren(item))
                    stack.Push(child);
            }
        }

        public static IEnumerable<T> TraverseTreeBreadth<T> (this T root, Func<T, IEnumerable<T>> getChildren)
        {
            var queue = new Queue<T>();
            queue.Enqueue(root);

            while (queue.Count != 0) {
                T item = queue.Dequeue();
                yield return item;
                queue.EnqueueRange(getChildren(item));
            }
        }

        public static IEnumerable<T> TraverseGraph<T> (this T root, Func<T, IEnumerable<T>> getChildren)
        {
            var seen = new HashSet<T>();
            var stack = new Stack<T>();
            stack.Push(root);

            while (stack.Count != 0) {
                T item = stack.Pop();
                if (seen.Contains(item))
                    continue;
                seen.Add(item);
                yield return item;
                stack.PushRange(getChildren(item));
            }
        }
    }
}