namespace Paraclete.Ansi;

public interface IAnsiStringPart
{
}

public class AnsiStringTextPart : IAnsiStringPart
{
    private readonly string _text;

    public AnsiStringTextPart(string text)
    {
        _text = text;
    }

    public override string ToString() => _text;
}

public class AnsiStringControlSequencePart : IAnsiStringPart
{
    private readonly AnsiControlSequence _sequence;

    public AnsiStringControlSequencePart(AnsiControlSequence sequence)
    {
        _sequence = sequence;
    }

    public override string ToString() => _sequence.ToString();
}
