namespace ParacleteTest.Extensions;

using System.Numerics;
using Paraclete.Extensions;

public class BigIntegerExtensionsTest
{
    [Theory]
    [InlineData(  0,  "0")]
    [InlineData(  1,  "1")]
    [InlineData(  9,  "9")]
    [InlineData( 10,  "a")]
    [InlineData( 15,  "f")]
    [InlineData( 16, "10")]
    [InlineData(255, "ff")]
    public void ToHexadecimalString(int input, string expected)
    {
        // Arrange
        // Act
        var result = ((BigInteger)input).ToHexadecimalString();

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(  0,        "0")]
    [InlineData(  1,        "1")]
    [InlineData( 10,     "1010")]
    [InlineData(255, "11111111")]
    public void ToBinaryString(int input, string expected)
    {
        // Arrange
        // Act
        var result = ((BigInteger)input).ToBinaryString();

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData( 0,   "0")]
    [InlineData( 1,   "1")]
    [InlineData( 8,  "10")]
    [InlineData( 9,  "11")]
    [InlineData(63,  "77")]
    [InlineData(64, "100")]
    public void ToOctalString(int input, string expected)
    {
        // Arrange
        // Act
        var result = ((BigInteger)input).ToOctalString();

        // Assert
        result.Should().Be(expected);
    }
}