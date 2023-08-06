namespace Paraclete.Ansi;

public class AnsiStringBuilder
{
    private readonly List<IAnsiStringPiece> _pieces;

    public AnsiStringBuilder()
    {
        _pieces = new ();
    }

    public int Length => _pieces.Sum(_ => _.Length);

    public AnsiStringBuilder Append(char chr)
    {
        return Append(chr.ToString());
    }

    public AnsiStringBuilder Append(string text)
    {
        _pieces.Add(new AnsiStringTextPiece(text));
        return this;
    }

    public AnsiStringBuilder Append(AnsiControlSequence sequence)
    {
        _pieces.Add(new AnsiStringControlSequencePart(sequence));
        return this;
    }

    public AnsiStringBuilder Append(AnsiString ansiString)
    {
        _pieces.AddRange(ansiString.Pieces);
        return this;
    }

    public AnsiStringBuilder Append(AnsiStringBuilder stringBuilder)
    {
        _pieces.AddRange(stringBuilder._pieces);
        return this;
    }

    public AnsiStringBuilder Clear()
    {
        _pieces.Clear();
        return this;
    }

    public AnsiString Build()
    {
        return new AnsiString(_pieces);
    }
}
