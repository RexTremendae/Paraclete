namespace Paraclete;

using System.Diagnostics.CodeAnalysis;

public class NullableGeneric<T>
{
    public NullableGeneric(T value)
    {
        if (value == null)
        {
            throw new InvalidOperationException("NullableGeneric wrapper cannot contain a null value. When needed, just set the whole NullableGeneric object to null.");
        }

        Value = value;
    }

    [NotNull]
    public T Value { get; }
}
