using System.ComponentModel;
using Alba.Framework.Reflection;

namespace Alba.Framework.ComponentModel;

public static class InvalidEnumArgumentExceptionExts
{
    extension(InvalidEnumArgumentException @this)
    {
        public static InvalidEnumArgumentException Create<T>(T value)
            where T : struct, Enum =>
            new($"Invalid {typeof(T).GetName()} value: {value}");

        [DoesNotReturn, ContractAnnotation("=> halt")]
        public static void Throw<T>(T value)
            where T : struct, Enum =>
            throw Create(value);
    }
}