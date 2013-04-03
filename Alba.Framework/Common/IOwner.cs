using System.Collections.Generic;

namespace Alba.Framework.Common
{
    public interface IOwner
    {
        IEnumerable<object> Owned { get; }
    }
}