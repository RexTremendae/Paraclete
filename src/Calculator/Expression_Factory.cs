namespace Paraclete.Calculator;

using System.Text;

public partial class Expression
{
    private class Factory
    {
        private readonly List<(string token, TokenType type)> _tokens = new ();
        private readonly StringBuilder _currentToken = new ();
        private TokenType _currentTokenType = TokenType.None;

        private static IEnumerable<(string token, TokenType type)> NoTokens { get; } =
            Enumerable.Empty<(string token, TokenType type)>();

        public (ITokenNode root, IEnumerable<(string token, TokenType type)> tokens) CreateTokens(string inputData)
        {
            _currentTokenType = TokenType.None;
            _currentToken.Clear();

            foreach (var chr in inputData)
            {
                if (!HandleCharacter(chr))
                {
                    return (ITokenNode.Empty, NoTokens);
                }
            }

            CompleteCurrentToken();

            return ExpressionTreeBuilder.TryBuildTree(_tokens, out var rootTokenNode)
                ? (rootTokenNode, _tokens)
                : (ITokenNode.Empty, NoTokens);
        }

        private bool HandleCharacter(char chr)
        {
            if (chr == ' ')
            {
                CompleteCurrentToken();
                return true;
            }

            var nextTokenType = chr switch
            {
                var x when x >= '0' && x <= '9' => TokenType.Numeric,
                var _ when "+-*/%^".Contains(chr) => TokenType.Operator,
                _ => TokenType.Invalid
            };

            if (_currentTokenType == TokenType.Operator && nextTokenType == TokenType.Operator)
            {
                if (chr == '+' || chr == '-')
                {
                    CompleteCurrentToken();
                    _currentTokenType = TokenType.Numeric;
                    _currentToken.Append(chr);
                    return true;
                }

                return false;
            }

            if (nextTokenType == TokenType.Invalid)
            {
                return false;
            }

            if (_currentTokenType == TokenType.None || _currentTokenType == nextTokenType)
            {
                _currentTokenType = nextTokenType;
            }
            else
            {
                CompleteCurrentToken();
                _currentTokenType = nextTokenType;
            }

            _currentToken.Append(chr);
            return true;
        }

        private void CompleteCurrentToken()
        {
            if (_currentToken.Length == 0)
            {
                return;
            }

            if (_currentTokenType == TokenType.Invalid || _currentTokenType == TokenType.None)
            {
                throw new InvalidOperationException($"Cannot add token of type {_currentTokenType}.");
            }

            _tokens.Add((_currentToken.ToString().Trim(), _currentTokenType));
            _currentToken.Clear();
            _currentTokenType = TokenType.None;
        }
    }
}
