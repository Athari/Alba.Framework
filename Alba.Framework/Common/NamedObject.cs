namespace Alba.Framework.Common;

public class NamedObject
{
    private string _name;

    public NamedObject(string name)
    {
        Guard.IsNotNullOrEmpty(name, nameof(name));
        _name = name;
    }

    public override string ToString()
    {
        if (_name[0] != '{')
            _name = $"{{{_name}}}";
        return _name;
    }
}