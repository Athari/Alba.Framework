namespace Alba.Framework.Common
{
    public interface IIdentifiable<out TKey>
    {
        TKey Id { get; }
    }
}