using CalculatorExpression = Paraclete.Modules.Calculator.Expression;
using TokenType = Paraclete.Modules.Calculator.Expression.TokenType;

namespace ParacleteTest.Modules.Calculator;

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
            success.ShouldBeTrue();
            tokenList.Count.ShouldBe(1, "token list count mismatch");
            token.Token.ShouldBe(input.ToString(), "token value mismatch");
            token.Type.ShouldBe(TokenType.Numeric, "token type mismatch");
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
            success.ShouldBeTrue();
            evaluated.ShouldBe(input, "evaluated value mismatch");
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
            success.ShouldBeTrue();
            evaluated.ShouldBe(expected, "evaluated value mismatch");
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
            success.ShouldBeTrue();
            evaluated.ShouldBe(expected, "evaluated value mismatch");
        }
    }
}
