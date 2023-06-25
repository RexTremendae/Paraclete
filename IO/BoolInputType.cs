namespace Paraclete.IO;

public class BoolInputType : IDataTypeInputter
{
    public Type DataType => typeof(bool);
    public string Alphabet => "YyNn";
    public int? MinLength => 1;
    public int? MaxLength => 1;

    public object CompleteInput(string inputData)
    {
        return !string.IsNullOrEmpty(inputData) && "Yy".Contains(inputData);
    }
}
