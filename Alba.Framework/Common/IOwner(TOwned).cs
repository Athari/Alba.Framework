namespace Alba.Framework.Common;

public interface IOwner<out TOwned>
{
    IEnumerable<TOwned> Owned { get; }
}