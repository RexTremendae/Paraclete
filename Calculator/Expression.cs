namespace Paraclete.Calculator;

using System;
using System.Text;

public class Expression
{
    private static Expression _empty = new Expression();

    private Expression()
    {
        RootNode = ITokenNode.Empty;
    }

    private Expression(ITokenNode rootNode)
    {
        RootNode = rootNode;
    }

    public enum TokenType
    {
        Invalid = -1,
        None = 0,
        Operator,
        Numeric,
    }

    public static Expression Empty => _empty;
    public ITokenNode RootNode { get; }

    public static bool TryCreate(string inputData, out Expression expression)
    {
        expression = Expression.Empty;
        var currentTokenType = TokenType.None;
        var currentToken = new StringBuilder();
        var tokens = new List<(string token, TokenType type)>();

        foreach (var ch in inputData)
        {
            if (ch == ' ')
            {
                tokens.Add((currentToken.ToString(), currentTokenType));
                currentToken.Clear();
                currentTokenType = TokenType.None;
                continue;
            }

            var nextTokenType = ch switch
            {
                var x when x >= '0' && x <= '9' => TokenType.Numeric,
                var x when "+-*/%^".Contains(ch) => TokenType.Operator,
                _ => TokenType.Invalid
            };

            if (currentTokenType == TokenType.Operator && nextTokenType == TokenType.Operator)
            {
                if (ch == '+' || ch == '-')
                {
                    tokens.Add((currentToken.ToString(), currentTokenType));
                    currentToken.Clear();
                    currentTokenType = TokenType.Numeric;
                    currentToken.Append(ch);
                    continue;
                }

                return false;
            }

            if (nextTokenType == TokenType.Invalid)
            {
                return false;
            }

            if (currentTokenType == TokenType.None || currentTokenType == nextTokenType)
            {
                currentTokenType = nextTokenType;
            }
            else
            {
                tokens.Add((currentToken.ToString(), currentTokenType));
                currentToken.Clear();
                currentTokenType = nextTokenType;
            }

            currentToken.Append(ch);
        }

        if (currentToken.Length > 0)
        {
            tokens.Add((currentToken.ToString(), currentTokenType));
        }

        if (!ExpressionTreeBuilder.TryBuildTree(tokens, out var rootToken))
        {
            return false;
        }

        expression = new Expression(rootToken);
        return true;
    }

    public double Evaluate()
    {
        return RootNode.Evaluate();
    }

    public void AddToString(StringBuilder builder)
    {
        RootNode.AddToString(builder);
    }

    private static ITokenNode CreateNode(string token, TokenType type)
    {
        return type switch
        {
            TokenType.Operator => new OperatorTokenNode(token),
            TokenType.Numeric => new NumericTokenNode(token),
            _ => throw new InvalidOperationException($"Cannot create token from type {type}.")
        };
    }
}
