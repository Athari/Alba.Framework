using System.Collections;
using System.Collections.Generic;

// ReSharper disable PossibleInterfaceMemberAmbiguity
namespace Alba.Framework.Collections
{
    public interface IOwnedList<T> : IList<T>, IList, IReadOnlyList<T>
    {
        void SwapAt (int index, int indexOther);
    }
}