namespace Paraclete.Ansi;

public partial class AnsiString
{
    private readonly string _fullString;

    public AnsiString(params string[] texts)
        : this(texts.Select(_ => new AnsiStringTextPiece(_)))
    {
    }

    public AnsiString(params AnsiControlSequence[] sequences)
        : this(sequences.Select(_ => new AnsiStringControlSequencePart(_)))
    {
    }

    public AnsiString(IEnumerable<IAnsiStringPiece> pieces)
    {
        Pieces = pieces.ToArray();
        _fullString = string.Concat(pieces.Select(_ => _.ToString()));
        Length = Pieces.OfType<AnsiStringTextPiece>().Select(_ => _.ToString().Length).Sum();
    }

    public static AnsiString Empty => new(string.Empty);

    public IAnsiStringPiece[] Pieces { get; }
    public int Length { get; }

    public static implicit operator AnsiString(string input) => new(input);

    public static AnsiString operator +(AnsiString ansiString1, AnsiString ansiString2)
    {
        return new AnsiString(ansiString1.Pieces.Concat(ansiString2.Pieces));
    }

    public static AnsiString operator +(string text, AnsiString ansiString)
    {
        return new AnsiStringBuilder().Append(text).Append(ansiString).Build();
    }

    public static AnsiString operator +(AnsiString ansiString, string text)
    {
        return new AnsiStringBuilder().Append(ansiString).Append(text).Build();
    }

    public static AnsiString operator +(AnsiControlSequence sequence, AnsiString ansiString)
    {
        return new AnsiStringBuilder().Append(sequence).Append(ansiString).Build();
    }

    public static AnsiString operator +(AnsiString ansiString, AnsiControlSequence sequence)
    {
        return new AnsiStringBuilder().Append(ansiString).Append(sequence).Build();
    }

    public override string ToString() => _fullString;

    public AnsiString PadRight(int totalWidth, char paddingChar = ' ')
    {
        var pieces = Pieces.ToList();
        var paddingWidth = (totalWidth - Length).ZeroFloor();
        if (paddingWidth > 0)
        {
            pieces.Add(new AnsiStringTextPiece(string.Empty.PadRight(paddingWidth, paddingChar)));
        }

        return new(pieces);
    }

    public AnsiString Truncate(int maxLength)
    {
        var totalLength = 0;
        var truncatedPieces = new List<IAnsiStringPiece>();

        foreach (var piece in Pieces)
        {
            if (piece is AnsiStringControlSequencePart)
            {
                truncatedPieces.Add(piece);
                continue;
            }

            var textPart = piece.ToString() ?? string.Empty;
            var lengthLeft = (maxLength - totalLength).ZeroFloor();

            if (textPart.Length >= lengthLeft)
            {
                var tpart = textPart[..lengthLeft];
                truncatedPieces.Add(new AnsiStringTextPiece(tpart));
                break;
            }

            truncatedPieces.Add(piece);
            totalLength += textPart.Length;
        }

        return new(truncatedPieces);
    }
}
