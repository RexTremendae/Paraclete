namespace Paraclete;

public class NullableGeneric<T>
{
    private NullableGeneric(bool hasValue, T value)
    {
        Value = value;
        HasValue = hasValue;
    }

    public bool HasValue { get; }

    public T Value { get; }

    public static NullableGeneric<T> Create(T value)
    {
        return new NullableGeneric<T>(hasValue: true, value);
    }

    public static NullableGeneric<T> CreateNullValue()
    {
        return new NullableGeneric<T>(hasValue: false, default!);
    }
}
