namespace Paraclete.IO;

public class StringInputType : IDataTypeInputter
{
    public Type DataType => typeof(string);
    public string Alphabet =>
        "abcdefghijklmnopqrstuvwxyzåäö" +
        "ABCDEFGHIJKLMNOPQRSTUVWXYZÅÄÖ" +
        "0123456789" +
        ".,:;/\\@#$%& !?_-+=()[]{}<>";

    public object CompleteInput(string inputData)
    {
        return inputData;
    }
}
