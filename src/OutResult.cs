namespace Paraclete;

public class OutResult<T>
{
    private OutResult(NullableGeneric<T> result, string errorMessage)
    {
        Result = result;
        ErrorMessage = errorMessage;
    }

    public bool IsSuccess => Result.HasNonNullValue();
    public NullableGeneric<T> Result { get; }
    public string ErrorMessage { get; }

    public static OutResult<T> CreateSuccessful(T result)
    {
        return new OutResult<T>(NullableGeneric<T>.Create(result), string.Empty);
    }

    public static OutResult<T> CreateFailed(string errorMessage)
    {
        return new OutResult<T>(NullableGeneric<T>.CreateNullValue(), errorMessage);
    }
}
