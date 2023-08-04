namespace ParacleteTest.IO;

using System.Numerics;
using Paraclete.IO;

public class RadixInputDefinitionTest
{
    public class TryCompleteInput
    {
        [Theory]

        /* Decimal input */
        [InlineData("0",     0)]
        [InlineData("1",     1)]
        [InlineData("67",    67)]
        [InlineData("666",   666)]
        [InlineData("d11",   11)]
        [InlineData("0d55",  55)]
        [InlineData("D23",   23)]
        [InlineData("0D925", 925)]

        /* Hexadecimal input */
        [InlineData("x10",  16)]
        [InlineData("X20",  32)]
        [InlineData("0xff", 255)]
        [InlineData("0Xfe", 254)]

        /* Binary input */
        [InlineData("b101",    5)]
        [InlineData("0b111",   7)]
        [InlineData("B1011",  11)]
        [InlineData("0B1111", 15)]

        /* Octal input */
        [InlineData("o17",  15)]
        [InlineData("0o10",  8)]
        [InlineData("O20",  16)]
        [InlineData("0O24", 20)]

        public void SuccessfulDecimalInput(string input, int expectedResult)
        {
            // Arrange
            var definition = new RadixInputDefinition();

            // Act
            var success = definition.TryCompleteInput(input, out var actualObjectResult, out var errorMessage);
            var actualResult = (BigInteger)actualObjectResult;

            // Assert
            errorMessage.Should().BeNullOrEmpty();
            success.Should().BeTrue();
            actualResult.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData("")]
        [InlineData("x")]
        [InlineData("d")]
        [InlineData("b")]
        [InlineData("o")]
        [InlineData("0X")]
        [InlineData("0D")]
        [InlineData("0B")]
        [InlineData("0O")]
        [InlineData("0b12")]
        [InlineData("0dff")]
        [InlineData("xx")]
        public void InvalidInput(string input)
        {
            // Arrange
            var definition = new RadixInputDefinition();

            // Act
            var success = definition.TryCompleteInput(input, out var _, out var errorMessage);

            // Assert
            errorMessage.Should().NotBeNullOrEmpty();
            success.Should().BeFalse();
        }
    }
}
