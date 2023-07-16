namespace Paraclete.IO;

public class BoolInputDefinition : IInputDefinition
{
    public Type DataType => typeof(bool);
    public string Alphabet => "YyNn";
    public int? MinLength => 1;
    public int? MaxLength => 1;

    public bool TryCompleteInput(string inputData, out object result, out string errorMessage)
    {
        result = false;
        if (string.IsNullOrEmpty(inputData))
        {
            errorMessage = "Empty input";
            return false;
        }

        result = "Yy".Contains(inputData);
        errorMessage = string.Empty;
        return true;
    }
}
