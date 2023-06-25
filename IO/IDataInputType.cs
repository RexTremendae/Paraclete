namespace Paraclete.IO;

public interface IDataTypeInputter
{
    Type DataType { get; }
    string Alphabet { get; }
    object CompleteInput(string inputData);
    int? MinLength => null;
    int? MaxLength => null;

    public static readonly IDataTypeInputter NoInputter = new NoInputterImplementation();

    [ExcludeFromEnumeration]
    private class NoInputterImplementation : IDataTypeInputter
    {
        public Type DataType => throw new NotSupportedException();
        public string Alphabet => throw new NotSupportedException();

        public object CompleteInput(string inputData)
        {
            throw new NotSupportedException();
        }
    }
}
