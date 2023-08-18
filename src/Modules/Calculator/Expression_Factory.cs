namespace Paraclete.Modules.Calculator;

using System.Text;

public partial class Expression
{
    private sealed class Factory
    {
        private readonly List<(string token, TokenType type)> _tokens = new ();
        private readonly StringBuilder _currentToken = new ();
        private readonly StringBuilder _log = new ();
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

            return (ITokenNode.Empty, _tokens);
/*
            return ExpressionTreeBuilder.TryBuildTree(_tokens, out var rootTokenNode)
                ? (rootTokenNode, _tokens)
                : (ITokenNode.Empty, NoTokens);
*/
        }

        private static TokenType GetTokenType(char chr)
        {
            return chr switch
            {
                _ when chr >= '0' && chr <= '9' => TokenType.Numeric,
                _ when "+-*/%^".Contains(chr) => TokenType.Operator,
                _ when chr == '(' => TokenType.ParenthesisStart,
                _ when chr == ')' => TokenType.ParenthesisEnd,
                _ => TokenType.Invalid
            };
        }

        private bool HandleCharacter(char chr)
        {
            _log.AppendLine($"Handling '{chr}'");
            if (chr == ' ')
            {
                CompleteCurrentToken();
                return true;
            }

            var nextTokenType = GetTokenType(chr);

            if (nextTokenType == TokenType.ParenthesisStart || nextTokenType == TokenType.ParenthesisEnd)
            {
                CompleteCurrentToken();
                _currentTokenType = nextTokenType;
                _currentToken.Append(chr);
                CompleteCurrentToken();
                return true;
            }

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
            _log.AppendLine($"Completing current token: [{_currentTokenType}] {_currentToken.ToString()}");
            if (_currentToken.Length == 0 || _currentTokenType == TokenType.None)
            {
                return;
            }

            if (_currentTokenType == TokenType.Invalid)
            {
                throw new InvalidOperationException($"Cannot add token of type {_currentTokenType}.");
            }

            _tokens.Add((_currentToken.ToString().Trim(), _currentTokenType));
            _currentToken.Clear();
            _currentTokenType = TokenType.None;
        }
    }
}
