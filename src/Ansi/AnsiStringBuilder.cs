namespace Paraclete.Ansi;

public class AnsiStringBuilder
{
    private readonly List<IAnsiStringPart> _parts;

    public AnsiStringBuilder()
    {
        _parts = new ();
    }

    public AnsiStringBuilder Append(string text)
    {
        _parts.Add(new AnsiStringTextPart(text));
        return this;
    }

    public AnsiStringBuilder Append(AnsiControlSequence sequence)
    {
        _parts.Add(new AnsiStringControlSequencePart(sequence));
        return this;
    }

    public AnsiStringBuilder Append(AnsiString ansiString)
    {
        _parts.AddRange(ansiString.Parts);
        return this;
    }

    public AnsiString Build()
    {
        return new AnsiString(_parts);
    }
}
