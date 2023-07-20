namespace Paraclete.Ansi;

public partial class AnsiString
{
    private readonly string _fullString;

    public AnsiString(params string[] texts)
        : this(texts.Select(_ => new AnsiStringTextPart(_)))
    {
    }

    public AnsiString(params AnsiControlSequence[] sequences)
        : this(sequences.Select(_ => new AnsiStringControlSequencePart(_)))
    {
    }

    public AnsiString(IEnumerable<IAnsiStringPart> parts)
    {
        Parts = parts.ToArray();
        _fullString = string.Concat(parts.Select(_ => _.ToString()));
        Length = Parts.OfType<AnsiStringTextPart>().Select(_ => _.ToString().Length).Sum();
    }

    public static AnsiString Empty => new (string.Empty);

    public IAnsiStringPart[] Parts { get; }
    public int Length { get; }

    public static implicit operator AnsiString(string input) => new AnsiString(input);

    public static AnsiString operator +(AnsiString ansiString1, AnsiString ansiString2)
    {
        return new AnsiString(ansiString1.Parts.Concat(ansiString2.Parts));
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
        var parts = Parts.ToList();
        var paddingWidth = int.Max(0, totalWidth - Length);
        if (paddingWidth > 0)
        {
            parts.Add(new AnsiStringTextPart(string.Empty.PadRight(paddingWidth, paddingChar)));
        }

        return new (parts);
    }

    public AnsiString Truncate(int maxLength)
    {
        var totalLength = 0;
        var truncatedParts = new List<IAnsiStringPart>();

        foreach (var part in Parts)
        {
            if (part is AnsiStringControlSequencePart ctrlPart)
            {
                truncatedParts.Add(part);
                continue;
            }

            var textPart = part.ToString() ?? string.Empty;
            var lengthLeft = maxLength - totalLength;

            if (textPart.Length >= lengthLeft)
            {
                var tpart = textPart[..lengthLeft];
                truncatedParts.Add(new AnsiStringTextPart(tpart));
                totalLength += tpart.Length;
                break;
            }

            truncatedParts.Add(part);
            totalLength += textPart.Length;
        }

        return new (truncatedParts);
    }
}
