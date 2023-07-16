namespace Paraclete.Calculator;

using System.Text;

public class OperatorTokenNode : TokenNodeBase
{
    private readonly string _operator;

    public OperatorTokenNode(string input)
    {
        _operator = input;
    }

    public ITokenNode LeftOperand { get; private set; } = ITokenNode.Empty;
    public ITokenNode RightOperand { get; private set; } = ITokenNode.Empty;

    public void SetLeftOperand(ITokenNode token)
    {
        if (LeftOperand != ITokenNode.Empty)
        {
            throw new InvalidOperationException("Operator already has a left operand.");
        }

        LeftOperand = token;
        token.SetParent(this);
    }

    public void SetRightOperand(ITokenNode token)
    {
        if (RightOperand != ITokenNode.Empty)
        {
            throw new InvalidOperationException("Operator already has a right operand.");
        }

        RightOperand = token;
        token.SetParent(this);
    }

    public override double Evaluate()
    {
        var left = LeftOperand.Evaluate();
        var right = RightOperand.Evaluate();

        return _operator switch
        {
            "+" => left + right,
            "-" => left - right,
            "*" => left * right,
            "/" => left / right,
            "%" => left % right,
            "^" => double.Pow(left, right),
            _ => throw new NotImplementedException($"Operator {_operator} is not implemented.")
        };
    }

    public override string ToString()
    {
        return _operator;
    }
}
