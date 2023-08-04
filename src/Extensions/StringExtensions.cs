namespace Paraclete.Extensions;

using System.Numerics;

public static class StringExtensions
{
    public static string RemoveBeginning(this string text, string beginning)
    {
        return string.IsNullOrEmpty(text)
            ? text
            : (text.StartsWith(beginning) ? text[beginning.Length..] : text);
    }

    public static string RemoveEnding(this string text, string ending)
    {
        return string.IsNullOrEmpty(text)
            ? text
            : (text.EndsWith(ending) ? text[..^ending.Length] : text);
    }

    public static bool TryParseBinary(this string input, out BigInteger result, out string errorMessage)
    {
        return TryParse(input, "binary", "01", out result, out errorMessage);
    }

    public static bool TryParseDecimal(this string input, out BigInteger result, out string errorMessage)
    {
        return TryParse(input, "decimal", "0123456789", out result, out errorMessage);
    }

    public static bool TryParseHexadecimal(this string input, out BigInteger result, out string errorMessage)
    {
        return TryParse(input, "hexadecimal", "0123456789abcdef", out result, out errorMessage);
    }

    public static bool TryParseOctal(this string input, out BigInteger result, out string errorMessage)
    {
        return TryParse(input, "octal", "01234567", out result, out errorMessage);
    }

    private static bool TryParse(this string input, string baseName, string alphabet, out BigInteger result, out string errorMessage)
    {
        input = input.ToLower();
        result = 0;
        errorMessage = string.Empty;

        if (input.Length == 0)
        {
            errorMessage = "Invalid input.";
            return false;
        }

        var value = new BigInteger();
        var multiplier = new BigInteger(1);

        for (int idx = input.Length - 1; idx >= 0; idx--)
        {
            var data = alphabet.IndexOf(input[idx]);
            if (data < 0)
            {
                errorMessage = $"Invalid {baseName} input: '{input[idx]}'";
                return false;
            }

            value += multiplier * data;
            multiplier *= alphabet.Length;
        }

        result = value;
        return true;
    }
}
