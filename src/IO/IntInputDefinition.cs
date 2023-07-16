namespace Paraclete.IO;

public class IntInputDefinition : IInputDefinition
{
    public Type DataType => typeof(int);
    public string Alphabet => IInputDefinition.NumericAlphabet;

    public bool TryCompleteInput(string inputData, out object result, out string errorMessage)
    {
        result = default(int);
        if (int.TryParse(inputData, out var intResult))
        {
            errorMessage = "Invalid integer";
            result = intResult;
            return true;
        }

        errorMessage = string.Empty;
        return false;
    }
}
