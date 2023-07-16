namespace Paraclete.Extensions;

using System.Numerics;
using System.Text;

public static class BigIntegerExtensions
{
    public static string ToBinaryString(this BigInteger input)
    {
        return input.ToRadixString(2);
    }

    public static string ToHexadecimalString(this BigInteger input)
    {
        return input.ToRadixString(16);
    }

    public static string ToOctalString(this BigInteger input)
    {
        return input.ToRadixString(8);
    }

    public static string ToRadixString(this BigInteger input, int radix)
    {
        var builder = new StringBuilder();

        if (input < 0)
        {
            throw new NotSupportedException("Negative numbers are not supported.");
        }

        while (input > 0)
        {
            var current = input % radix;
            var chr = (char)((current < 10)
                ? '0' + current
                : 'a' + (current - 10));

            builder.Insert(0, chr);
            input /= radix;
        }

        var result = builder.ToString();
        return result.Length > 0 ? result : "0";
    }
}
