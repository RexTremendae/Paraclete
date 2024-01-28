namespace Paraclete;

public class NullableGeneric<T>(bool hasValue, T value)
{
    public bool HasValue { get; } = hasValue;

    public T Value { get; } = value;

    public static NullableGeneric<T> Create(T value)
    {
        return new NullableGeneric<T>(hasValue: true, value);
    }

    public static NullableGeneric<T> CreateNullValue()
    {
        return new NullableGeneric<T>(hasValue: false, default!);
    }
}
