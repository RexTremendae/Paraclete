using System.Collections;
using System.Numerics;
using CalculatorExpression = Paraclete.Modules.Calculator.Expression;
using TokenType = Paraclete.Modules.Calculator.Expression.TokenType;

namespace ParacleteTest.Modules.Calculator;

public class ExpressionTest
{
    public class Expression
    {
        public class ExpressionTokensData : IEnumerable<object[]>
        {
            private readonly List<object[]> _data = new()
            {
/*
                new object[]
                {
                    "123",
                    new Token[]
                    {
                        Numeric(123)
                    }
                },

                new object[]
                {
                    "   1 ",
                    new Token[]
                    {
                        Numeric(1)
                    }
                },

                new object[]
                {
                    "   1293845869211   ",
                    new Token[]
                    {
                        Numeric(1293845869211)
                    }
                },

                new object[]
                {
                    "1+2",
                    new Token[]
                    {
                        Numeric(1),
                        Operator("+"),
                        Numeric(2)
                    }
                },

                new object[]
                {
                    " 5 -   7  ",
                    new Token[]
                    {
                        Numeric(5),
                        Operator("-"),
                        Numeric(7)
                    }
                },

                new object[]
                {
                    "8+-9",
                    new Token[]
                    {
                        Numeric(8),
                        Operator("+"),
                        Numeric(-9)
                    }
                },

                new object[]
                {
                    " -  1 ",
                    new Token[]
                    {
                        Operator("-"),
                        Numeric(1)
                    }
                },
*/
                new object[]
                {
                    "(9-6)",
                    new Token[]
                    {
                        ParenthesisStart(),
                        Numeric(9),
                        Operator("-"),
                        Numeric(6),
                        ParenthesisEnd(),
                    }
                },
            };

            private static Token Numeric(BigInteger value) => new Token(TokenType.Numeric, value.ToString());
            private static Token Operator(string value) => new Token(TokenType.Operator, value);
            private static Token ParenthesisStart() => new Token(TokenType.ParenthesisStart, "(");
            private static Token ParenthesisEnd() => new Token(TokenType.ParenthesisEnd, ")");

            public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public class Token
            {
                public Token(TokenType type, string token)
                {
                    TokenType = type;
                    TokenValue = token;
                }

                public TokenType TokenType { get; }
                public string TokenValue { get; }
            }
        }

        [Theory]
        [ClassData(typeof(ExpressionTokensData))]
        public void ExpressionTokens(string input, ExpressionTokensData.Token[] expectedTokens)
        {
            // Arrange
            // Act
            var success = CalculatorExpression.TryCreate(input, out var expression);
            var tokenList = expression.Tokens.ToList();

            // Assert
            success.Should().BeTrue();
            tokenList.Count.Should().Be(expectedTokens.Length, because: "token list count mismatch");
            for (int idx = 0; idx < tokenList.Count; idx++)
            {
                var (actualTokenValue, actualTokenType) = tokenList[idx];
                var expected = expectedTokens[idx];
                actualTokenValue.Should().Be(expected.TokenValue);
                actualTokenType.Should().Be(expected.TokenType);
            }
            //token.token.Should().Be(input.ToString(), because: "token value mismatch");
            //token.type.Should().Be(TokenType.Numeric, because: "token type mismatch");
        }
    }
/*
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
/*
        [Theory]
        [InlineData("(7 + 1) * 2", 16)]
        public void ParenthesisExpression(string input, double expected)
        {
            // Arrange
            var success = CalculatorExpression.TryCreate(input, out var expression);

            // Act
            var evaluated = expression.Evaluate();

            // Assert
            success.Should().BeTrue();
            evaluated.Should().Be(expected, because: "evaluated value mismatch");
        }
* /
    }
*/
}
