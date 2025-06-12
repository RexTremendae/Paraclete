namespace ParacleteTest.Extensions;

using System.Numerics;
using Paraclete.Extensions;

public class BigIntegerExtensionsTest
{
    [Theory]
    [InlineData(     0, false, false,     "0")]
    [InlineData(     0,  true, false,     "0")]
    [InlineData(     0,  true,  true,    "00")]
    [InlineData(     1, false, false,     "1")]
    [InlineData(     9, false, false,     "9")]
    [InlineData(    10, false, false,     "a")]
    [InlineData(    15, false, false,     "f")]
    [InlineData(    16, false, false,    "10")]
    [InlineData(   255, false, false,    "ff")]
    [InlineData(   256, false, false,   "100")]
    [InlineData(   256,  true, false,  "1 00")]
    [InlineData(   256,  true, true,  "01 00")]
    [InlineData(65_535,  true, false, "ff ff")]
    public void ToHexadecimalString(int input, bool useGrouping, bool padGroups, string expected)
    {
        // Arrange
        // Act
        var result = ((BigInteger)input).ToHexadecimalString(useGrouping: useGrouping, padGroups: padGroups);

        // Assert
        result.ShouldBe(expected);
    }

    [Theory]
    [InlineData(  0, false, false,         "0")]
    [InlineData(  0,  true, false,         "0")]
    [InlineData(  0,  true,  true,      "0000")]
    [InlineData(  1, false, false,         "1")]
    [InlineData( 10, false, false,      "1010")]
    [InlineData( 16, false, false,     "10000")]
    [InlineData( 16,  true, false,    "1 0000")]
    [InlineData( 16,  true,  true, "0001 0000")]
    [InlineData(255, false, false,  "11111111")]
    public void ToBinaryString(int input, bool useGrouping, bool padGroups, string expected)
    {
        // Arrange
        // Act
        var result = ((BigInteger)input).ToBinaryString(useGrouping: useGrouping, padGroups: padGroups);

        // Assert
        result.ShouldBe(expected);
    }

    [Theory]
    [InlineData(    0, false, false,        "0")]
    [InlineData(    1, false, false,        "1")]
    [InlineData(    8, false, false,       "10")]
    [InlineData(    9, false, false,       "11")]
    [InlineData(   63, false, false,       "77")]
    [InlineData(   64, false, false,      "100")]
    [InlineData(4_100, false, false,    "10004")]
    [InlineData(4_100,  true, false,  "1 00 04")]
    [InlineData(4_100,  true,  true, "01 00 04")]
    public void ToOctalString(int input, bool useGrouping, bool padGroups, string expected)
    {
        // Arrange
        // Act
        var result = ((BigInteger)input).ToOctalString(useGrouping: useGrouping, padGroups: padGroups);

        // Assert
        result.ShouldBe(expected);
    }

    [Theory]
    [InlineData(        0, false, false,           "0")]
    [InlineData(        0,  true, false,           "0")]
    [InlineData(        0,  true,  true,         "000")]
    [InlineData(        1, false, false,           "1")]
    [InlineData(1_234_567, false, false,     "1234567")]
    [InlineData(1_234_567,  true, false,   "1 234 567")]
    [InlineData(1_234_567,  true,  true, "001 234 567")]
    public void ToDecimalString(int input, bool useGrouping, bool padGroups, string expected)
    {
        // Arrange
        // Act
        var result = ((BigInteger)input).ToDecimalString(useGrouping: useGrouping, padGroups: padGroups);

        // Assert
        result.ShouldBe(expected);
    }
}
