namespace Paraclete.IO;

public class IntInputDefinition : IInputDefinition
{
    public Type DataType => typeof(int);
    public string Alphabet => IInputDefinition.NumericAlphabet;

    public bool TryCompleteInput(string inputData, out OutResult<object> result)
    {
        if (int.TryParse(inputData, out var intResult))
        {
            result = OutResult<object>.CreateSuccessful(intResult);
            return true;
        }

        result = OutResult<object>.CreateFailed("Invalid integer");
        return false;
    }
}
