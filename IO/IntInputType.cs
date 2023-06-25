namespace Paraclete.IO;

public class IntInputType : IDataTypeInputter
{
    public Type DataType => typeof(int);
    public string Alphabet => "0123456789";

    public object CompleteInput(string inputData) => int.Parse(inputData);
}
