namespace Alba.Framework.Collections;

[PublicAPI]
public class IndexDictionary<TValue> : IndexDictionaryBase<int, TValue>
{
    protected override int KeyToIndex(int key) => key;

    protected override int IndexToKey(int index) => index;
}