namespace Paraclete.Extensions;

using System.Numerics;
using System.Text;

public static class BigIntegerExtensions
{
    public static string ToDecimalString(this BigInteger input, bool useGrouping = false, bool padGroups = false)
    {
        return input.ToRadixString(10, useGrouping ? 3 : 0, padGroups);
    }

    public static string ToBinaryString(this BigInteger input, bool useGrouping = false, bool padGroups = false)
    {
        return input.ToRadixString(2, useGrouping ? 4 : 0, padGroups);
    }

    public static string ToHexadecimalString(this BigInteger input, bool useGrouping = false, bool padGroups = false)
    {
        return input.ToRadixString(16, useGrouping ? 2 : 0, padGroups);
    }

    public static string ToOctalString(this BigInteger input, bool useGrouping = false, bool padGroups = false)
    {
        return input.ToRadixString(8, useGrouping ? 2 : 0, padGroups);
    }

    public static string ToRadixString(this BigInteger input, int radix, int groupSize = 0, bool padGroups = false)
    {
        var builder = new StringBuilder();

        if (input < 0)
        {
            throw new NotSupportedException("Negative numbers are not supported.");
        }

        var groupCount = 0;
        while (input > 0)
        {
            var current = input % radix;
            var chr = ((char)((current < 10)
                ? '0' + current
                : 'a' + (current - 10))
            ).ToString();

            if (groupSize > 0 && groupCount >= groupSize)
            {
                groupCount = 0;
                chr = $"{chr} ";
            }

            builder.Insert(0, chr);
            groupCount++;
            input /= radix;
        }

        if (padGroups)
        {
            while (groupCount < groupSize)
            {
                builder.Insert(0, '0');
                groupCount++;
            }
        }

        var result = builder.ToString();
        return result.Length > 0 ? result : "0";
    }
}
