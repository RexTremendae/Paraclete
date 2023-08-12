namespace Paraclete.IO;

public interface IInputDefinition
{
    public const string NumericAlphabet = "0123456789";
    public static readonly IInputDefinition NoInputter = new NoInputDefinitionImplementation();

    Type DataType { get; }
    string Alphabet { get; }
    int? MinLength => null;
    int? MaxLength => null;

    bool TryCompleteInput(string inputData, out OutResult<object> result);

    [ExcludeFromEnumeration]
    private sealed class NoInputDefinitionImplementation : IInputDefinition
    {
        public Type DataType => throw new NotSupportedException();
        public string Alphabet => throw new NotSupportedException();

        public bool TryCompleteInput(string inputData, out OutResult<object> result)
        {
            throw new NotSupportedException();
        }
    }
}
