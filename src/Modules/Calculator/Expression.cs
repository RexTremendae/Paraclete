namespace Paraclete.Modules.Calculator;

public partial class Expression
{
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

    public static Expression Empty { get; } = new ();
    public ITokenNode RootNode { get; }
    public IEnumerable<(string token, TokenType type)> Tokens { get; }

    public static bool TryCreate(string inputData, out Expression expression)
    {
        expression = Empty;

        var (root, tokens) = new Factory().CreateTokens(inputData);
        if (!tokens.Any())
        {
            return false;
        }

        expression = new Expression(root, tokens);
        return true;
    }

    public double Evaluate()
    {
        return RootNode.Evaluate();
    }
}
