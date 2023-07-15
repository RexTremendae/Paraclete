namespace Paraclete.Calculator;

using System.Text;

public class Expression
{
    private static Expression _empty = new Expression();

    private Expression()
    {
        RootNode = ITokenNode.Empty;
        Tokens = Enumerable.Empty<(string, TokenType)>();
    }

    private Expression(ITokenNode rootNode, IEnumerable<(string, TokenType)> tokens)
    {
        RootNode = rootNode;
        Tokens = tokens;
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
    public IEnumerable<(string token, TokenType type)> Tokens { get; }

    public static bool TryCreate(string inputData, out Expression expression)
    {
        expression = Empty;
        var currentTokenType = TokenType.None;
        var currentToken = new StringBuilder();
        var tokens = new List<(string token, TokenType type)>();

        var addToken = (StringBuilder tokenBuilder, TokenType type) =>
        {
            if (type == TokenType.Invalid || type == TokenType.None)
            {
                throw new InvalidOperationException($"Cannot add token of type {type}.");
            }

            tokens.Add((tokenBuilder.ToString().Trim(), type));
        };

        foreach (var ch in inputData)
        {
            if (ch == ' ')
            {
                addToken(currentToken, currentTokenType);
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
                    addToken(currentToken, currentTokenType);
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
                addToken(currentToken, currentTokenType);
                currentToken.Clear();
                currentTokenType = nextTokenType;
            }

            currentToken.Append(ch);
        }

        if (currentToken.Length > 0)
        {
            addToken(currentToken, currentTokenType);
        }

        if (!ExpressionTreeBuilder.TryBuildTree(tokens, out var rootTokenNode))
        {
            return false;
        }

        expression = new Expression(rootTokenNode, tokens);
        return true;
    }

    public double Evaluate()
    {
        return RootNode.Evaluate();
    }
}
