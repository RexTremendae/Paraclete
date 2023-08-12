namespace Paraclete.Ansi;

public interface IAnsiStringPiece
{
    public int Length { get; }
}

public class AnsiStringTextPiece : IAnsiStringPiece
{
    private readonly string _text;

    public AnsiStringTextPiece(string text)
    {
        _text = text;
    }

    public int Length => _text.Length;
    public override string ToString() => _text;
}

public class AnsiStringControlSequencePart : IAnsiStringPiece
{
    private readonly AnsiControlSequence _sequence;

    public AnsiStringControlSequencePart(AnsiControlSequence sequence)
    {
        _sequence = sequence;
    }

    public int Length => 0;
    public override string ToString() => _sequence.ToString();
}
