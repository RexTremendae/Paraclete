namespace Paraclete.Ansi;

public interface IAnsiStringPiece
{
    public int Length { get; }
}

public class AnsiStringTextPiece(string text) : IAnsiStringPiece
{
    private readonly string _text = text;

    public int Length => _text.Length;
    public override string ToString() => _text;
}

public class AnsiStringControlSequencePart(AnsiControlSequence sequence) : IAnsiStringPiece
{
    private readonly AnsiControlSequence _sequence = sequence;

    public int Length => 0;
    public override string ToString() => _sequence.ToString();
}
