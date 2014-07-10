using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Globalization;
using System.Runtime.CompilerServices;
using Alba.Framework.Attributes;
using Alba.Framework.Sys;

namespace Alba.Framework.Windows.Mvvm
{
    public class EnumFlagsModel<T> : DynamicObject, INotifyPropertyChanged
        where T : struct, IComparable, IFormattable, IConvertible //, Enum
    {
        private T _enumValue;

        public event PropertyChangedEventHandler PropertyChanged;

        public EnumFlagsModel (T enumValue = default(T))
        {
            EnumValue = enumValue;
        }

        public T EnumValue
        {
            get { return _enumValue; }
            set
            {
                if (_enumValue.Equals(value))
                    return;
                _enumValue = value;
                OnPropertyChanged();
                foreach (string memberName in GetDynamicMemberNames())
                    OnPropertyChanged(memberName);
            }
        }

        public override IEnumerable<string> GetDynamicMemberNames ()
        {
            return Enum.GetNames(typeof(T));
            /*IEnumerable<string> names = Enum.GetNames(typeof(T));
            IEnumerable<ulong> values = Enum.GetValues(typeof(T)).Cast<Enum>().Select(Convert.ToUInt64);
            return names.Zip(values, (name, value) => new { name, value })
                .Where(e => GetBitCount(e.value) == 1)
                .Select(e => e.name);*/
        }

        public override bool TryGetMember (GetMemberBinder binder, out object result)
        {
            T flag = GetFlag(binder.Name, binder.IgnoreCase);
            result = EnumValue.Has(flag);
            return true;
        }

        public override bool TrySetMember (SetMemberBinder binder, object value)
        {
            T flag = GetFlag(binder.Name, binder.IgnoreCase);
            T oldValue = EnumValue;
            EnumValue = EnumValue.With(flag, (bool)value);
            if (!EnumValue.Equals(oldValue))
                OnPropertyChanged(binder.Name);
            return true;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged ([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private static T GetFlag (string binderName, bool ignoreCase)
        {
            return (T)Enum.Parse(typeof(T), binderName, ignoreCase);
        }

        /*private static int GetBitCount (ulong i)
        {
            i = i - ((i >> 1) & 0x5555555555555555);
            i = (i & 0x3333333333333333) + ((i >> 2) & 0x3333333333333333);
            return (int)((((i + (i >> 4)) & 0xF0F0F0F0F0F0F0F) * 0x101010101010101) >> 56);
        }*/

        public override string ToString ()
        {
            return EnumValue.ToString(CultureInfo.InvariantCulture);
        }
    }
}