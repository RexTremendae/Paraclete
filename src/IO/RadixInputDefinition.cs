namespace Paraclete.IO;

using System.Numerics;

[ExcludeFromEnumeration]
public class RadixInputDefinition : IInputDefinition
{
    private const string ValidPrefixes = "xdbo";
    public static string ValidPrefixesDescription => string.Join(", ", ValidPrefixes.ToCharArray().Select(_ => $"\"(0){_}\""));

    public Type DataType => typeof(string);
    public string Alphabet => "xXoOabcdefABCDEF" + IInputDefinition.NumericAlphabet;

    public bool TryCompleteInput(string inputData, out object result, out string errorMessage)
    {
        result = new BigInteger();
        errorMessage = string.Empty;

        if (inputData.Length == 0)
        {
            return false;
        }

        if (ValidPrefixes.Contains(inputData[0], StringComparison.OrdinalIgnoreCase))
        {
            inputData = $"0{char.ToLower(inputData[0])}{inputData[1..]}";
        }
        else if (inputData.All(_ => IInputDefinition.NumericAlphabet.Contains(inputData[0])))
        {
            inputData = $"0d{inputData}";
        }

        if (inputData[0] != '0' || !ValidPrefixes.Contains(inputData[1], StringComparison.OrdinalIgnoreCase))
        {
            errorMessage = $"Invalid radix (valid prefixes: {ValidPrefixesDescription})";
            return false;
        }

        var type = char.ToLower(inputData[1]);
        inputData = inputData[2..];
        var bigIntResult = new BigInteger();

        var success = type switch
        {
            'd' => inputData.TryParseDecimal(out bigIntResult, out errorMessage),
            'b' => inputData.TryParseBinary(out bigIntResult, out errorMessage),
            'x' => inputData.TryParseHexadecimal(out bigIntResult, out errorMessage),
            'o' => inputData.TryParseOctal(out bigIntResult, out errorMessage),
            _ => false,
        };

        result = bigIntResult;
        return success;
    }
}