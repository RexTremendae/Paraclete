namespace Paraclete.Painting;

public class TimeWriter
{
    private readonly TimeWriterSettings _settings;
    private (int hour, int minute, int second, int millisecond, (int x, int y) cursorPos) _cache;

    public TimeWriter(TimeWriterSettings settings)
    {
        _settings = settings;
    }

    public void Write(TimeSpan timestamp, (int x, int y) cursorPos, Painter painter)
    {
        Write(timestamp.Hours, timestamp.Minutes, timestamp.Seconds, timestamp.Milliseconds, cursorPos, painter);
    }

    public void Write(DateTime timestamp, (int x, int y) cursorPos, Painter painter)
    {
        Write(timestamp.Hour, timestamp.Minute, timestamp.Second, timestamp.Millisecond, cursorPos, painter);
    }

    private void Write(int hour, int minute, int second, int millisecond, (int x, int y) cursorPos, Painter painter)
    {
        var newCache = (hour, minute, second, millisecond, cursorPos);
        if (newCache == _cache)
        {
            return;
        }
        _cache = newCache;

        var hourPart = hour.ToString().PadLeft(2, '0');
        var minutePart = minute.ToString().PadLeft(2, '0');
        var secondPart = second.ToString().PadLeft(2, '0');
        var millisecondPart = millisecond.ToString().PadLeft(3, '0');

        var parts = new List<string>();
        var partColors = new List<ConsoleColor>();
        var partFonts = new List<Font>();
        var font = Font.OfSize(_settings.FontSize);

        if (_settings.ShowHours)
        {
            parts.Add($"{hourPart}:");
            partColors.Add(_settings.Color);
            partFonts.Add(font);
        }

        parts.Add($"{minutePart}");
        partColors.Add(_settings.Color);
        partFonts.Add(font);

        if (_settings.ShowSeconds)
        {
            parts.Add($":{secondPart}");
            partColors.Add(_settings.SecondsColor);
            partFonts.Add(_settings.SecondsFontSize == Font.Size.Undefined
                ? font
                : Font.OfSize(_settings.SecondsFontSize));
        }

        if (_settings.ShowMilliseconds)
        {
            parts.Add($".{millisecondPart}");
            partColors.Add(_settings.MillisecondsColor);
            partFonts.Add(_settings.MillisecondsFontSize == Font.Size.Undefined
                ? font
                : Font.OfSize(_settings.MillisecondsFontSize));
        }

        Write(parts, partColors, partFonts, cursorPos, painter);
    }

    private void Write(List<string> textParts, List<ConsoleColor> colors, List<Font> fonts, (int x, int y) cursorPos, Painter painter)
    {
        var textHeight = fonts.Max(_ => _.CharacterHeight);
        var rows = Enumerable.Range(0, textHeight)
            .Select(_ => new AnsiStringBuilder())
            .ToArray();

        for (int idx = 0; idx < textParts.Count; idx++)
        {
            var font = fonts[idx];
            var textPartRows = new string[textHeight];

            var text = textParts[idx];
            foreach (var c in text)
            {
                var fontCharacter = font[c];
                var yOffset = textHeight-font.CharacterHeight;
                for (var y = 0; y < textHeight; y++)
                {
                    if (y < yOffset)
                    {
                        textPartRows[y] += "".PadLeft(font.CharacterWidth);
                    }
                    else
                    {
                        textPartRows[y] += fontCharacter[y-yOffset];
                    }
                }
            }

            for (var y = 0; y < textHeight; y++)
            {
                rows[y].Append((textPartRows[y], colors[idx]));
            }
        }

        painter.PaintRows(rows.Select(_ => _.Build()).ToArray(), cursorPos);
    }
}
