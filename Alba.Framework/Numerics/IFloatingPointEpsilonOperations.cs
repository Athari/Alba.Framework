using System.Numerics;

namespace Alba.Framework.Numerics;

public interface IFloatingPointEpsilonOperations<TSelf>
    where TSelf : IFloatingPoint<TSelf>?
{
    static abstract TSelf Epsilon { get; }

    static abstract bool IsCloseTo(TSelf @this, TSelf v, TSelf epsilon);
    static abstract bool IsGreaterThan(TSelf @this, TSelf v);
    static abstract bool IsGreaterThanOrClose(TSelf @this, TSelf v);
    static abstract bool IsLessThan(TSelf @this, TSelf v);
    static abstract bool IsLessThanOrClose(TSelf @this, TSelf v);
    static abstract bool IsOne(TSelf @this);
    static abstract bool IsZero(TSelf @this);
    static abstract bool IsBetweenZeroAndOne(TSelf @this);
}