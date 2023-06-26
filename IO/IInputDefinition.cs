namespace Paraclete.IO;

public interface IInputDefinition
{
    Type DataType { get; }
    string Alphabet { get; }
    bool TryCompleteInput(string inputData, out object result);
    int? MinLength => null;
    int? MaxLength => null;

    public static string NumericAlphabet = "0123456789";
    public static readonly IInputDefinition NoInputter = new NoInputDefinitionImplementation();

    [ExcludeFromEnumeration]
    private class NoInputDefinitionImplementation : IInputDefinition
    {
        public Type DataType => throw new NotSupportedException();
        public string Alphabet => throw new NotSupportedException();

        public bool TryCompleteInput(string inputData, out object result)
        {
            throw new NotSupportedException();
        }
    }
}
