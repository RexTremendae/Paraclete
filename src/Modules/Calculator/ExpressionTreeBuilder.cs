namespace Paraclete.Modules.Calculator;

using System;
using TokenType = Expression.TokenType;

public static class ExpressionTreeBuilder
{
    public static bool TryBuildTree(List<(string token, TokenType type)> tokens, out ITokenNode rootToken)
    {
        var root = ITokenNode.Empty;

        foreach (var (token, type) in tokens.Where(_ => _.type != TokenType.None))
        {
            var newNode = CreateNode(token, type);

            if (root == ITokenNode.Empty)
            {
                root = newNode;
            }
            else if (type == TokenType.Operator && root is NumericTokenNode)
            {
                ((OperatorTokenNode)newNode).SetLeftOperand(root);
                root = newNode;
            }
            else if (root is OperatorTokenNode rootOperatorNode)
            {
                switch (type)
                {
                    case TokenType.Numeric:
                    case TokenType.ParenthesisStart:
                        rootOperatorNode.SetRightOperand(newNode);
                        break;

                    case TokenType.Operator:
                        var newOperatorNode = (OperatorTokenNode)newNode;
                        newOperatorNode.SetLeftOperand(root);
                        root = newOperatorNode;
                        break;

                    case TokenType.Invalid:
                    case TokenType.None:
                    case TokenType.ParenthesisEnd:
                    default:
                        rootToken = ITokenNode.Empty;
                        return false;
                }
            }
            else
            {
                rootToken = ITokenNode.Empty;
                return false;
            }
        }

        rootToken = root;
        return true;
    }

    private static ITokenNode CreateNode(string token, TokenType type)
    {
        return type switch
        {
            TokenType.Operator => new OperatorTokenNode(token),
            TokenType.Numeric => new NumericTokenNode(token),
            TokenType.ParenthesisStart => new ParenthesisTokenNode(),
            TokenType.ParenthesisEnd => new ParenthesisTokenNode(),
            _ => throw new InvalidOperationException($"Cannot create token from type {type}.")
        };
    }
}
