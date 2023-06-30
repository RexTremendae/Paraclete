namespace Paraclete.Calculator;

using System.Text;

public class NumericTokenNode : TokenNodeBase
{
    private readonly double _number;

    public NumericTokenNode(string input)
    {
        _number = double.Parse(input);
    }

    public override void AddToString(StringBuilder builder)
    {
        builder.Append(_number);
    }

    public override double Evaluate() => _number;

    public override string ToString()
    {
        return _number.ToString();
    }
}
