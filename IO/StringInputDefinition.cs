namespace Paraclete.IO;

public class StringInputDefinition : IInputDefinition
{
    public Type DataType => typeof(string);
    public string Alphabet =>
        "abcdefghijklmnopqrstuvwxyzåäö" +
        "ABCDEFGHIJKLMNOPQRSTUVWXYZÅÄÖ" +
        ".,:;/\\@#$%& !?_-+=()[]{}<>" +
        IInputDefinition.NumericAlphabet;

    public bool TryCompleteInput(string inputData, out object result)
    {
        result = inputData;
        return true;
    }
}
