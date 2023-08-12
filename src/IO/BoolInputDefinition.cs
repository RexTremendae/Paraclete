namespace Paraclete.IO;

public class BoolInputDefinition : IInputDefinition
{
    public Type DataType => typeof(bool);
    public string Alphabet => "YyNn";
    public int? MinLength => 1;
    public int? MaxLength => 1;

    public bool TryCompleteInput(string inputData, out OutResult<object> result)
    {
        if (string.IsNullOrEmpty(inputData))
        {
            result = OutResult<object>.CreateFailed("Empty input");
            return false;
        }

        result = OutResult<object>.CreateSuccessful("Yy".Contains(inputData));
        return true;
    }
}
