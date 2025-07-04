using System.ComponentModel;
using ObservableComputations;

namespace Alba.Framework.Collections.ObservableComputations;

public static class ObservableComputationsExts
{
    public static IReadScalar<T> ToReadOnlyScalar<T>(this ScalarComputing<T> @this) =>
        new ReadOnlyScalar<T>(@this);

    private class ReadOnlyScalar<T>(IReadScalar<T> scalar) : IReadScalar<T>
    {
        public event PropertyChangedEventHandler? PropertyChanged
        {
            add => scalar.PropertyChanged += value;
            remove => scalar.PropertyChanged -= value;
        }

        public T Value => scalar.Value;
    }
}