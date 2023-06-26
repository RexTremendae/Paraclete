namespace Paraclete.IO;

public class IntInputDefinition : IInputDefinition
{
    public Type DataType => typeof(int);
    public string Alphabet => IInputDefinition.NumericAlphabet;

    public bool TryCompleteInput(string inputData, out object result)
    {
        result = default(int);
        if (int.TryParse(inputData, out var intResult))
        {
            result = intResult;
            return true;
        }

        return false;
    }
}
