namespace Paraclete.Calculator;

public class NumericTokenNode : TokenNodeBase
{
    private readonly double _number;

    public NumericTokenNode(string input)
    {
        _number = double.Parse(input);
    }

    public override double Evaluate() => _number;

    public override string ToString()
    {
        return _number.ToString();
    }
}
