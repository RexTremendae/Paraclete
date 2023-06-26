namespace Paraclete.IO;

public class DateOnlyInputDefinition : IInputDefinition
{
    public Type DataType => typeof(DateOnly);
    public string Alphabet => IInputDefinition.NumericAlphabet + "-";

    public bool TryCompleteInput(string inputData, out object result)
    {
        result = default(DateOnly);

        if (string.IsNullOrEmpty(inputData))
        {
            return true;
        }

        if (DateOnly.TryParse(inputData, out var dateOnlyResult))
        {
            result = dateOnlyResult;
            return true;
        }

        return false;
    }
}
