namespace Paraclete.Modules.Calculator;

public class NumericTokenNode(string input) : TokenNodeBase
{
    private readonly double _number = double.Parse(input);

    public override double Evaluate() => _number;

    public override string ToString()
    {
        return _number.ToString();
    }
}
