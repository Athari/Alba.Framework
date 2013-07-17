using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Alba.Framework.Text;
using NcChangedAction = System.Collections.Specialized.NotifyCollectionChangedAction;
using NcChangedEventArgs = System.Collections.Specialized.NotifyCollectionChangedEventArgs;
using Alba.Framework.Sys;

// TODO Collection view for ObservableQueue is ListCollectionView which uses IList internally. Implement ICollectionViewFactory and custom collection view?
// ReSharper disable StaticFieldInGenericType
namespace Alba.Framework.Collections
{
    [Serializable]
    [DebuggerDisplay ("Count = {Count}")]
    [DebuggerTypeProxy (typeof(ObservableQueue<>.ObservableQueue_DebugView))]
    public class ObservableQueue<T> : IList<T>, IList, IReadOnlyList<T>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        private const string Error_ReentrancyNotAllowed = "ObservableQueue reentrancy not allowed.";
        private const string Count_PropName = "Count";
        private const string Indexer_PropName = "Item[]";
        // HACK Use Fasterflect if too slooow
        private static readonly MethodInfo _queueGetElement = typeof(Queue<T>).GetMethod("GetElement", BindingFlags.Instance | BindingFlags.NonPublic);

        private readonly Queue<T> _queue = new Queue<T>();
        private readonly SimpleMonitor _monitor = new SimpleMonitor();

        [field: NonSerialized]
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableQueue ()
        {
            _queue = new Queue<T>();
        }

        public ObservableQueue (IEnumerable<T> collection)
        {
            _queue = new Queue<T>(collection);
        }

        public ObservableQueue (int capacity)
        {
            _queue = new Queue<T>(capacity);
        }

        int ICollection.Count
        {
            get { return Count; }
        }

        int ICollection<T>.Count
        {
            get { return Count; }
        }

        public int Count
        {
            get { return _queue.Count; }
        }

        public bool IsFixedSize
        {
            get { return false; }
        }

        bool IList.IsReadOnly
        {
            get { return false; }
        }

        bool ICollection<T>.IsReadOnly
        {
            get { return false; }
        }

        bool ICollection.IsSynchronized
        {
            get { return false; }
        }

        object ICollection.SyncRoot
        {
            get { return _queue; }
        }

        void ICollection<T>.Add (T item)
        {
            Enqueue(item);
        }

        int IList.Add (object item)
        {
            Enqueue((T)item);
            return Count - 1;
        }

        public void Clear ()
        {
            CheckReentrancy();
            _queue.Clear();
            OnPropertyChanged(Count_PropName);
            OnPropertyChanged(Indexer_PropName);
            OnCollectionChanged(new NcChangedEventArgs(NcChangedAction.Reset));
        }

        bool IList.Contains (object item)
        {
            return IsCompatibleObject(item) && Contains((T)item);
        }

        public bool Contains (T item)
        {
            return _queue.Contains(item);
        }

        void ICollection<T>.CopyTo (T[] array, int index)
        {
            CopyTo(array, index);
        }

        void ICollection.CopyTo (Array array, int index)
        {
            var tArray = array as T[];
            if (tArray != null)
                CopyTo(tArray, index);
            else {
                tArray = new T[Count];
                CopyTo(tArray, 0);
                tArray.CopyTo(array, index);
            }
        }

        private void CopyTo (T[] array, int index)
        {
            _queue.CopyTo(array, index);
        }

        public T Dequeue ()
        {
            CheckReentrancy();
            T item = _queue.Dequeue();
            OnPropertyChanged(Count_PropName);
            OnPropertyChanged(Indexer_PropName);
            OnCollectionChanged(new NcChangedEventArgs(NcChangedAction.Remove, item, 0));
            return item;
        }

        public void Enqueue (T item)
        {
            CheckReentrancy();
            _queue.Enqueue(item);
            OnPropertyChanged(Count_PropName);
            OnPropertyChanged(Indexer_PropName);
            OnCollectionChanged(new NcChangedEventArgs(NcChangedAction.Add, item, Count));
        }

        public IEnumerator<T> GetEnumerator ()
        {
            return _queue.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator ()
        {
            return GetEnumerator();
        }

        int IList.IndexOf (object item)
        {
            return IsCompatibleObject(item) ? IndexOf((T)item) : -1;
        }

        public int IndexOf (T item)
        {
            int index = 0;
            using (var items = GetEnumerator()) {
                while (items.MoveNext()) {
                    if (items.Current.EqualsValue(item))
                        return index;
                    index++;
                }
            }
            return -1;
        }

        void IList.Insert (int index, object item)
        {
            Insert(index, (T)item);
        }

        void IList<T>.Insert (int index, T item)
        {
            Insert(index, item);
        }

        private void Insert (int index, T item)
        {
            if (index == Count)
                Enqueue(item);
            else
                throw ThrowNotSupported();
        }

        public T Peek ()
        {
            return _queue.Peek();
        }

        void IList.Remove (object item)
        {
            if (IsCompatibleObject(item))
                Remove((T)item);
        }

        public bool Remove (T item)
        {
            if (Peek().EqualsValue(item)) {
                Dequeue();
                return true;
            }
            else if (Contains(item))
                throw ThrowNotSupported();
            else
                return false;
        }

        void IList.RemoveAt (int index)
        {
            RemoveAt(index);
        }

        void IList<T>.RemoveAt (int index)
        {
            RemoveAt(index);
        }

        private void RemoveAt (int index)
        {
            if (index == 0)
                Dequeue();
            else
                throw ThrowNotSupported();
        }

        public void TrimExcess ()
        {
            _queue.TrimExcess();
        }

        T IReadOnlyList<T>.this [int index]
        {
            get { return this[index]; }
        }

        object IList.this [int index]
        {
            get { return this[index]; }
            set { this[index] = (T)value; }
        }

        T IList<T>.this [int index]
        {
            get { return this[index]; }
            set { this[index] = value; }
        }

        private T this [int index]
        {
            get { return GetElement(index); }
            set { throw ThrowNotSupported(); }
        }

        private T GetElement (int index)
        {
            return (T)_queueGetElement.Invoke(_queue, new object[] { index });
        }

        private IDisposable BlockReentrancy ()
        {
            _monitor.Enter();
            return _monitor;
        }

        private void CheckReentrancy ()
        {
            if (_monitor.Busy && CollectionChanged != null && CollectionChanged.GetInvocationList().Length > 1)
                throw new InvalidOperationException(Error_ReentrancyNotAllowed);
        }

        private void OnPropertyChanged (string propName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propName));
        }

        private void OnPropertyChanged (PropertyChangedEventArgs e)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, e);
        }

        private void OnCollectionChanged (NcChangedEventArgs e)
        {
            var handler = CollectionChanged;
            if (handler != null)
                using (BlockReentrancy())
                    handler(this, e);
        }

        private static bool IsCompatibleObject (object item)
        {
            // ReSharper disable CompareNonConstrainedGenericWithNull
            return item is T || item == null && default(T) == null;
            // ReSharper restore CompareNonConstrainedGenericWithNull
        }

        private Exception ThrowNotSupported ([CallerMemberName] string callerName = null)
        {
            return new NotSupportedException("Operation {0} is not supported for {1}."
                .Fmt(callerName, GetType().Name));
        }

        internal class ObservableQueue_DebugView
        {
            private readonly ObservableQueue<T> _queue;

            public ObservableQueue_DebugView (ObservableQueue<T> queue)
            {
                if (queue == null)
                    throw new ArgumentNullException("queue");
                _queue = queue;
            }

            [DebuggerBrowsable (DebuggerBrowsableState.RootHidden)]
            public T[] Items
            {
                get { return _queue.ToArray(); }
            }
        }

        [Serializable]
        private class SimpleMonitor : IDisposable
        {
            private int _busyCount;

            public void Dispose ()
            {
                _busyCount--;
            }

            public void Enter ()
            {
                _busyCount++;
            }

            public bool Busy
            {
                get { return _busyCount > 0; }
            }
        }
    }
}