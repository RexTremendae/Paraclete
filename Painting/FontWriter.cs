using System.Text;

namespace Paraclete.Painting;

public class FontWriter
{
    public Font Font { get; }

    private FontWriter(Font font)
    {
        Font = font;
    }

    public static FontWriter Create(Font.Size fontSize)
    {
        var font = Font.OfSize(fontSize);
        return new FontWriter(font);
    }

    public void Write(string text, ConsoleColor color, (int x, int y) cursorPos, Painter painter)
    {
        var textRows = Enumerable
            .Range(0, Font.CharacterHeight)
            .Select(_ => new AnsiStringBuilder())
            .ToArray();

        foreach (var c in text)
        {
            var fontCharacter = Font[c];
            for (var y = 0; y < Font.CharacterHeight; y++)
            {
                textRows[y].Append((fontCharacter[y], color));
            }
        }

        painter.PaintRows(textRows.Select(_ => _.Build()).ToArray(), cursorPos);
    }
}
