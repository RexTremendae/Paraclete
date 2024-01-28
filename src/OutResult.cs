namespace Paraclete;

public class OutResult<T>(NullableGeneric<T> result, string errorMessage)
{
    public bool IsSuccess => Result.HasNonNullValue();
    public NullableGeneric<T> Result { get; } = result;
    public string ErrorMessage { get; } = errorMessage;

    public static OutResult<T> CreateSuccessful(T result)
    {
        return new OutResult<T>(NullableGeneric<T>.Create(result), string.Empty);
    }

    public static OutResult<T> CreateFailed(string errorMessage)
    {
        return new OutResult<T>(NullableGeneric<T>.CreateNullValue(), errorMessage);
    }
}
