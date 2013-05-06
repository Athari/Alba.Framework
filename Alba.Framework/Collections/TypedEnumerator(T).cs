using System;
using System.Collections;
using System.Collections.Generic;

namespace Alba.Framework.Collections
{
    internal class TypedEnumerator<T> : IEnumerator<T>
    {
        private readonly IEnumerator _enumerator;

        public TypedEnumerator (IEnumerator enumerator)
        {
            _enumerator = enumerator;
        }

        public bool MoveNext ()
        {
            return _enumerator.MoveNext();
        }

        public void Reset ()
        {
            _enumerator.Reset();
        }

        public T Current
        {
            get { return (T)_enumerator.Current; }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }

        void IDisposable.Dispose ()
        {}
    }
}