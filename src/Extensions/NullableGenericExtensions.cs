namespace Paraclete.Extensions;

using System.Diagnostics.CodeAnalysis;

public static class NullableGenericExtensions
{
#pragma warning disable CS8607 // A possible null value may not be used for a type marked with [NotNull] or [DisallowNull]
    [return: NotNull]
    public static T GetNonNullValue<T>(this NullableGeneric<T>? value)
    {
        return (value?.HasValue ?? false)
            ? value.Value
            : throw new InvalidOperationException($"Value is null - check with {nameof(HasNonNullValue)} before accessing the value.");
    }
#pragma warning restore CS8607 // A possible null value may not be used for a type marked with [NotNull] or [DisallowNull]

    public static bool HasNonNullValue<T>(this NullableGeneric<T>? value)
    {
        return value?.HasValue ?? false;
    }
}