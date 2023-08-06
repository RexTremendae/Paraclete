namespace Paraclete.Painting;

using Paraclete.Ansi;

public class FontWriter
{
    private FontWriter(Font font)
    {
        Font = font;
    }

    public Font Font { get; }

    public static FontWriter Create(Font.Size fontSize)
    {
        var font = Font.OfSize(fontSize);
        return new FontWriter(font);
    }

    public void Write(string text, AnsiControlSequence color, (int x, int y) cursorPos, Painter painter)
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

        painter.PaintRows(textRows.Select(_ => _.Build()), cursorPos);
    }
}
