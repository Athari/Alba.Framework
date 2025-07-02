using System.Reflection;
using Alba.Framework.Collections;
using Alba.Framework.Text;

namespace Alba.Framework.Reflection;

[PublicAPI]
public static class TypeExts
{
    /// <summary>Find non-abstract classes either inheriting from a type or implementing an interface.</summary>
    /// <param name="this">Class or interface.</param>
    /// <param name="types">Types to search within. If not specified, types of the assembly <paramref name="this"/> originates from are searched.</param>
    public static IEnumerable<Type> GetConcreteSubtypes(this Type @this, IEnumerable<Type>? types = null)
    {
        types ??= @this.Assembly.GetTypes();
        return @this.IsClass // else is interface
            ? types.Where(t => t is { IsClass: true, IsAbstract: false } && t.IsSubclassOf(@this))
            : types.Where(t => t is { IsClass: true, IsAbstract: false } && t.IsAssignableTo(@this));
    }

    /// <summary>Return name of a type. Unlike <see cref="MemberInfo.Name"/>, includes name of container types, if the type is nested.</summary>
    public static string GetName(this Type @this)
    {
        return @this.IsNested
            ? @this.TraverseList(t => t.DeclaringType).Select(t => t.Name).Inverse().JoinString(".")
            : @this.Name;
    }

    /// <summary>Return "medium name" of a type: full name for type, short names for type arguments.</summary>
    public static string GetMediumName(this Type @this) => FixGenericArgumentsNames(@this, @this.FullName ?? "?", GetName);

    /// <summary>Return "full sharp name" of a type: full name for type, full names for type arguments. Unlike <see cref="Type.FullName"/> property, type arguments do not include full assembly info.</summary>
    public static string GetFullName(this Type @this) => FixGenericArgumentsNames(@this, @this.FullName ?? "?", GetFullName);

    /// <summary>Equivalent to "is" keyword.</summary>
    /// <remarks><see cref="Type.IsAssignableFrom"/> is "reverse is" and as such is confusing.</remarks>
    [Pure]
    public static bool IsAssignableTo(this Type @this, Type type) => type.IsAssignableFrom(@this);

    /// <summary>Equivalent to "is" keyword.</summary>
    [Pure]
    public static bool Is(this Type @this, Type type) => @this.IsAssignableTo(type);

    /// <summary>Equivalent to "is" keyword.</summary>
    [Pure]
    public static bool Is<T>(this Type @this) => @this.IsAssignableTo(typeof(T));

    public static string GetShortAssemblyName(this Type @this) => @this.Assembly.GetName().Name ?? "?";

    private static string FixGenericArgumentsNames(Type type, string name, Func<Type, string> getName)
    {
        int genericGrave = name.IndexOf('`');
        if (genericGrave != -1)
            name = $"{name.Sub(0, genericGrave)}<{type.GenericTypeArguments.Select(getName).JoinString(", ")}>";
        return name;
    }
}