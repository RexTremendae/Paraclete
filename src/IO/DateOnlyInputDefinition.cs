namespace Paraclete.IO;

using System.Globalization;

public class DateOnlyInputDefinition : IInputDefinition
{
    public Type DataType => typeof(DateOnly);
    public string Alphabet => IInputDefinition.NumericAlphabet + "-";

    public bool TryCompleteInput(string inputData, out object result, out string errorMessage)
    {
        result = default(DateOnly);

        if (string.IsNullOrEmpty(inputData))
        {
            errorMessage = "Date cannot be empty";
            return true;
        }

        if (DateOnly.TryParse(inputData, CultureInfo.InvariantCulture, out var dateOnlyResult))
        {
            errorMessage = string.Empty;
            result = dateOnlyResult;
            return true;
        }

        errorMessage = "Invalid date";
        return false;
    }
}
