namespace Paraclete.Calculator;

public class CalculatorHistory
{
    private List<Expression> _entries = new ();

    public IEnumerable<Expression> Entries => _entries;

    public void Clear()
    {
        _entries = new ();
    }

    public void AddEntry(Expression data)
    {
        _entries = _entries
            .AsEnumerable()
            .Append(data)
            .ToList();
    }
}
