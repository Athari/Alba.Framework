namespace Alba.Framework.Common;

public interface IOwned<TOwner>
{
    TOwner? Owner { get; set; }
}