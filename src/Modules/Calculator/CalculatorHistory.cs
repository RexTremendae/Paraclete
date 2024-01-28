namespace Paraclete.Modules.Calculator;

using System.Numerics;

public class CalculatorHistory
{
    private List<Expression> _entries = [];

    public IEnumerable<Expression> Entries => _entries;
    public BigInteger? RadixConversion { get; set; }

    public void Clear()
    {
        _entries = [];
        RadixConversion = null;
    }

    public void AddEntry(Expression data)
    {
        _entries = _entries
            .AsEnumerable()
            .Append(data)
            .ToList();
    }
}
