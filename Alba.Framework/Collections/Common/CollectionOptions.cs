namespace Alba.Framework.Collections;

[Flags]
public enum CollectionOptions : byte
{
    None = 0,
    StrictApi = 1 << 0,
    ReadOnly = 1 << 1,
    Default = None,
}