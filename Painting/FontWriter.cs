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
        Write(new[] { text }, new[] { color }, cursorPos, painter);
    }

    public void Write(IEnumerable<string> textParts, IEnumerable<ConsoleColor> colors, (int x, int y) cursorPos, Painter painter)
    {
        Write(textParts, colors, cursorPos, painter);
    }

    private void Write(string[] textParts, ConsoleColor[] colors, (int x, int y) cursorPos, Painter painter)
    {
        var rows = new List<(List<string> parts, List<ConsoleColor> colors)>();
        rows.AddRange(
            Enumerable.Range(0, Font.CharacterHeight)
            .Select(_ => (new List<string>(), new List<ConsoleColor>())));

        for (int idx = 0; idx < textParts.Length; idx++)
        {
            var textPartRows = new List<string>(
                Enumerable.Range(0, Font.CharacterHeight)
                    .Select(_ => string.Empty));

            var text = textParts[idx];
            foreach (var c in text)
            {
                var fontCharacter = Font[c];
                for (var y = 0; y < Font.CharacterHeight; y++)
                {
                    textPartRows[y] += fontCharacter[y];
                }
            }

            for (var y = 0; y < Font.CharacterHeight; y++)
            {
                rows[y].parts.Add(textPartRows[y]);
                rows[y].colors.Add(colors[idx]);
            }
        }

        var data = rows
            .Select(_ => ((IEnumerable<string>)_.parts, (IEnumerable<ConsoleColor>)_.colors))
            .ToArray();
        painter.PaintRows(data, cursorPos);
    }
}
