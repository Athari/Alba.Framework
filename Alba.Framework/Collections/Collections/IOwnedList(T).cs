namespace Alba.Framework.Collections;

public interface IOwnedList<T> : IList<T>
{
    void SwapAt(int index, int indexOther);
}