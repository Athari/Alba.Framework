using System.Reflection;
using Alba.Framework.Collections;
using Alba.Framework.Text;

namespace Alba.Framework.Reflection;

public static class TypeExts
{
    /// <param name="this">Class or interface.</param>
    extension(Type @this)
    {
        /// <summary>Find non-abstract classes either inheriting from a type or implementing an interface.</summary>
        /// <param name="types">Types to search within. If not specified, types of the assembly <paramref name="this"/> originates from are searched.</param>
        public IEnumerable<Type> GetConcreteSubtypes(IEnumerable<Type>? types = null)
        {
            types ??= @this.Assembly.GetTypes();
            return @this.IsClass // else is interface
                ? types.Where(t => t is { IsClass: true, IsAbstract: false } && t.IsSubclassOf(@this))
                : types.Where(t => t is { IsClass: true, IsAbstract: false } && t.IsAssignableTo(@this));
        }

        /// <summary>Return name of a type. Unlike <see cref="MemberInfo.Name"/>, includes name of container types, if the type is nested.</summary>
        public string GetName()
        {
            return @this.IsNested
                ? @this.TraverseList(t => t.DeclaringType).Select(t => t.Name).Inverse().JoinString(".")
                : @this.Name;
        }

        /// <summary>Return "medium name" of a type: full name for type, short names for type arguments.</summary>
        public string GetMediumName() => FixGenericArgumentsNames(@this, @this.FullName ?? "?", GetName);

        /// <summary>Return "full sharp name" of a type: full name for type, full names for type arguments. Unlike <see cref="Type.FullName"/> property, type arguments do not include full assembly info.</summary>
        public string GetFullName() => FixGenericArgumentsNames(@this, @this.FullName ?? "?", GetFullName);

        /// <summary>Equivalent to "is" keyword.</summary>
        /// <remarks><see cref="Type.IsAssignableFrom"/> is "reverse is" and as such is confusing.</remarks>
        [Pure]
        public bool IsAssignableTo(Type type) => type.IsAssignableFrom(@this);

        /// <summary>Equivalent to "is" keyword.</summary>
        [Pure]
        public bool Is(Type type) => @this.IsAssignableTo(type);

        /// <summary>Equivalent to "is" keyword.</summary>
        [Pure]
        public bool Is<T>() => @this.IsAssignableTo(typeof(T));

        public string GetShortAssemblyName() => @this.Assembly.GetName().Name ?? "?";

        public IEnumerable<string> GetResourceNames()
        {
            var prefix = $"{@this.Namespace}.";
            return @this.Assembly.GetManifestResourceNames()
                .Where(n => n.StartsWith(prefix))
                .Select(n => n[prefix.Length..]);
        }

        public Stream ReadResourceStream(string resourceName) =>
            @this.Assembly.GetManifestResourceStream($"{@this.Namespace}.{resourceName}")
         ?? throw new InvalidOperationException($"Resource '{resourceName}' not found.");

        public string ReadResourceString(string resourceName)
        {
            using var reader = new StreamReader(ReadResourceStream(@this, resourceName));
            return reader.ReadToEnd();
        }
    }

    private static string FixGenericArgumentsNames(Type type, string name, Func<Type, string> getName)
    {
        int genericGrave = name.IndexOf('`');
        if (genericGrave != -1)
            name = $"{name.Sub(0, genericGrave)}<{type.GenericTypeArguments.Select(getName).JoinString(", ")}>";
        return name;
    }
}