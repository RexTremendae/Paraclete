namespace Paraclete.Painting;

using Paraclete.Ansi;

public class FontFormatter
{
    private FontFormatter(Font font)
    {
        Font = font;
    }

    public Font Font { get; }

    public static FontFormatter Create(Font.Size fontSize)
    {
        var font = Font.OfSize(fontSize);
        return new FontFormatter(font);
    }

    public IEnumerable<AnsiString> Format(string text, AnsiControlSequence color)
    {
        var textRows = Enumerable
            .Range(0, Font.CharacterHeight)
            .Select(_ => new AnsiStringBuilder())
            .ToArray();

        foreach (var c in text)
        {
            var fontCharacter = Font[c];
            0.To(Font.CharacterHeight).Foreach(y =>
            {
                textRows[y].Append(color).Append(fontCharacter[y]);
            });
        }

        return textRows.Select(_ => _.Build());
    }
}
