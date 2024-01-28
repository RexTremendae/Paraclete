namespace Paraclete.IO;

using System.Numerics;

[ExcludeFromEnumeration]
public class RadixInputDefinition : IInputDefinition
{
    private const string ValidPrefixes = "xdbo";
    public static string ValidPrefixesDescription => string.Join(", ", ValidPrefixes.ToCharArray().Select(_ => $"\"(0){_}\""));

    public Type DataType => typeof(string);
    public string Alphabet => "xXoOabcdefABCDEF" + IInputDefinition.NumericAlphabet;

    public bool TryCompleteInput(string inputData, out OutResult<object> result)
    {
        if (inputData.Length == 0)
        {
            result = OutResult<object>.CreateFailed("No input");
            return false;
        }

        inputData = EnsurePrefix(inputData);

        if (inputData[0] != '0' || !ValidPrefixes.Contains(inputData[1], StringComparison.OrdinalIgnoreCase))
        {
            result = OutResult<object>.CreateFailed($"Invalid radix (valid prefixes: {ValidPrefixesDescription})");
            return false;
        }

        var type = char.ToLower(inputData[1]);
        inputData = inputData[2..];
        var bigIntResult = OutResult<BigInteger>.CreateFailed($"Invalid prefix: '{type}'");

        var success = type switch
        {
            'd' => inputData.TryParseDecimal(out bigIntResult),
            'b' => inputData.TryParseBinary(out bigIntResult),
            'x' => inputData.TryParseHexadecimal(out bigIntResult),
            'o' => inputData.TryParseOctal(out bigIntResult),
            _ => false,
        };

        result = success
            ? OutResult<object>.CreateSuccessful(bigIntResult.Result.GetNonNullValue())
            : OutResult<object>.CreateFailed(bigIntResult.ErrorMessage);

        return success;
    }

    private static string EnsurePrefix(string inputData)
    {
        return inputData.Length < 2
            ? $"0d{inputData}"
            : inputData switch
            {
                var _ when ValidPrefixes.Contains(inputData[0], StringComparison.OrdinalIgnoreCase)
                    => $"0{char.ToLower(inputData[0])}{inputData[1..]}",
                var _ when inputData[0] == '0' && ValidPrefixes.Contains(inputData[1], StringComparison.OrdinalIgnoreCase)
                    => inputData,
                var _ when inputData.All(_ => IInputDefinition.NumericAlphabet.Contains(inputData[0]))
                    => $"0d{inputData}",
                _ => inputData,
            };
    }
}
