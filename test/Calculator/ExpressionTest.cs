using CalculatorExpression = Paraclete.Calculator.Expression;
using TokenType = Paraclete.Calculator.Expression.TokenType;

namespace ParacleteTest.Calculator;

public class ExpressionTest
{
    public class Expression
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(1293845869211)]
        public void SimpleNumericExpression(long input)
        {
            // Arrange
            // Act
            var success = CalculatorExpression.TryCreate(input.ToString(), out var expression);
            var tokenList = expression.Tokens.ToList();
            var token = tokenList.First();

            // Assert
            success.Should().BeTrue();
            tokenList.Count.Should().Be(1, because: "token list count mismatch");
            token.token.Should().Be(input.ToString(), because: "token value mismatch");
            token.type.Should().Be(TokenType.Numeric, because: "token type mismatch");
        }
    }

    public class Evaluation
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(1293845869211)]
        public void SimpleNumericExpression(long input)
        {
            // Arrange
            var success = CalculatorExpression.TryCreate(input.ToString(), out var expression);

            // Act
            var evaluated = expression.Evaluate();

            // Assert
            success.Should().BeTrue();
            evaluated.Should().Be(input, because: "evaluated value mismatch");
        }

        [Theory]
        [InlineData("7 + 1",   8)]
        [InlineData("1+2",     3)]
        [InlineData("2-3",    -1)]
        [InlineData("3*2",     6)]
        [InlineData("10/4",  2.5)]
        [InlineData("7%3",     1)]
        public void OneOperationExpression(string input, double expected)
        {
            // Arrange
            var success = CalculatorExpression.TryCreate(input, out var expression);

            // Act
            var evaluated = expression.Evaluate();

            // Assert
            success.Should().BeTrue();
            evaluated.Should().Be(expected, because: "evaluated value mismatch");
        }

        [Theory]
        [InlineData("7 + 1 - 2",  6)]
        [InlineData("10/2 + 3",   8)]
        [InlineData("8 / 4 - 9", -7)]
        public void TwoOperationsExpression(string input, double expected)
        {
            // Arrange
            var success = CalculatorExpression.TryCreate(input, out var expression);

            // Act
            var evaluated = expression.Evaluate();

            // Assert
            success.Should().BeTrue();
            evaluated.Should().Be(expected, because: "evaluated value mismatch");
        }
    }
}
