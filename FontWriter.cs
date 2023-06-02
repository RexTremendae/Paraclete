namespace Time;

public class FontWriter
{
    private readonly int _fontSize;
    private readonly char _digitDisplayChar;

    private FontWriter(int fontSize, char digitDisplayChar)
    {
        _fontSize = fontSize;
        _digitDisplayChar = digitDisplayChar;
    }

    public static FontWriter Create(int fontSize, char digitDisplayChar)
    {
        return new FontWriter(fontSize, digitDisplayChar);
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
        var font = Font.OfSize(_fontSize, _digitDisplayChar);
        var rows = new List<(List<string> parts, List<ConsoleColor> colors)>();
        rows.AddRange(
            Enumerable.Range(0, font.CharacterHeight)
            .Select(_ => (new List<string>(), new List<ConsoleColor>())));

        for (int idx = 0; idx < textParts.Length; idx++)
        {
            var textPartRows = new List<string>(
                Enumerable.Range(0, font.CharacterHeight)
                    .Select(_ => ""));

            var text = textParts[idx];
            foreach (var c in text)
            {
                var fontCharacter = font[c];
                for (var y = 0; y < font.CharacterHeight; y++)
                {
                    textPartRows[y] += fontCharacter[y];
                }
            }

            for (var y = 0; y < font.CharacterHeight; y++)
            {
                rows[y].parts.Add(textPartRows[y]);
                rows[y].colors.Add(colors[idx]);
            }
        }

        var data = rows
            .Select(_ => (_.parts.ToArray(), _.colors.ToArray()))
            .ToArray();
        painter.Paint(data, cursorPos, true);
    }
}
