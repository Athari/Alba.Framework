namespace Alba.Framework.Collections
{
    public class IndexDictionary<TValue> : IndexDictionaryBase<int, TValue>
    {
        protected override int KeyToIndex (int key)
        {
            return key;
        }

        protected override int IndexToKey (int index)
        {
            return index;
        }
    }
}