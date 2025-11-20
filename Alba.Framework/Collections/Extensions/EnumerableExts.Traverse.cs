namespace Alba.Framework.Collections;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public static partial class EnumerableExts
{
    extension<T>(T root)
    {
        public IEnumerable<T> TraverseList(Func<T, T?> getNext)
        {
            for (T? current = root; current != null; current = getNext(current))
                yield return current;
        }

        public IEnumerable<T> TraverseTree(Func<T, IEnumerable<T>> getChildren)
        {
            return TraverseTreeDepth(root, getChildren);
        }

        public IEnumerable<T> TraverseTreeDepth(Func<T, IEnumerable<T>> getChildren)
        {
            var stack = new Stack<T>();
            stack.Push(root);

            while (stack.Count != 0) {
                T item = stack.Pop();
                yield return item;
                foreach (var child in getChildren(item).Inverse())
                    stack.Push(child);
            }
        }

        public IEnumerable<T> TraverseTreeBreadth(Func<T, IEnumerable<T>> getChildren)
        {
            var queue = new Queue<T>();
            queue.Enqueue(root);

            while (queue.Count != 0) {
                T item = queue.Dequeue();
                yield return item;
                queue.EnqueueRange(getChildren(item));
            }
        }

        public IEnumerable<T> TraverseGraph(Func<T, IEnumerable<T>> getChildren)
        {
            var seen = new HashSet<T>();
            var stack = new Stack<T>();
            stack.Push(root);

            while (stack.Count != 0) {
                T item = stack.Pop();
                if (!seen.Add(item))
                    continue;
                yield return item;
                stack.PushRange(getChildren(item));
            }
        }

        public IEnumerable<T> ReturnEnumerable()
        {
            yield return root;
        }
    }
}