using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Alba.Framework.Collections;

namespace Alba.Framework.Windows.Markup
{
    public class DictionaryStyleSelector : StyleSelector, IDictionary
    {
        private readonly Dictionary<Type, Style> _typeToStyle = new Dictionary<Type, Style>();

        public override Style SelectStyle (object item, DependencyObject container)
        {
            if (item == null)
                return null;
            Type itemType = item.GetType();
            return _typeToStyle.GetOrDefault(itemType);
        }

        public void Add (object key, object value)
        {
            _typeToStyle.Add((Type)key, (Style)value);
        }

        bool IDictionary.Contains (object key)
        {
            throw BuckYouMicrosoft;
        }

        void IDictionary.Clear ()
        {
            throw BuckYouMicrosoft;
        }

        IDictionaryEnumerator IDictionary.GetEnumerator ()
        {
            throw BuckYouMicrosoft;
        }

        IEnumerator IEnumerable.GetEnumerator ()
        {
            throw BuckYouMicrosoft;
        }

        void IDictionary.Remove (object key)
        {
            throw BuckYouMicrosoft;
        }

        object IDictionary.this [object key]
        {
            get { throw BuckYouMicrosoft; }
            set { throw BuckYouMicrosoft; }
        }

        ICollection IDictionary.Keys
        {
            get { throw BuckYouMicrosoft; }
        }

        ICollection IDictionary.Values
        {
            get { throw BuckYouMicrosoft; }
        }

        bool IDictionary.IsReadOnly
        {
            get { throw BuckYouMicrosoft; }
        }

        bool IDictionary.IsFixedSize
        {
            get { throw BuckYouMicrosoft; }
        }

        void ICollection.CopyTo (Array array, int index)
        {
            throw BuckYouMicrosoft;
        }

        int ICollection.Count
        {
            get { throw BuckYouMicrosoft; }
        }

        object ICollection.SyncRoot
        {
            get { throw BuckYouMicrosoft; }
        }

        bool ICollection.IsSynchronized
        {
            get { throw BuckYouMicrosoft; }
        }

        private static Exception BuckYouMicrosoft
        {
            get { return new NotSupportedException("Buck you Microsoft for making StyleSelector a class instead of an interface."); }
        }
    }
}