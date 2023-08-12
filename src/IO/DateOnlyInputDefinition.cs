namespace Paraclete.IO;

using System.Globalization;

public class DateOnlyInputDefinition : IInputDefinition
{
    public Type DataType => typeof(DateOnly);
    public string Alphabet => IInputDefinition.NumericAlphabet + "-";

    public bool TryCompleteInput(string inputData, out OutResult<object> result)
    {
        if (string.IsNullOrEmpty(inputData))
        {
            result = OutResult<object>.CreateSuccessful(default(DateOnly));
            return true;
        }

        if (DateOnly.TryParse(inputData, CultureInfo.InvariantCulture, out var dateOnlyResult))
        {
            result = OutResult<object>.CreateSuccessful(dateOnlyResult);
            return true;
        }

        result = OutResult<object>.CreateFailed("Date cannot be empty");
        return false;
    }
}
