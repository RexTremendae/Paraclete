namespace Time;

public class TimeWriter
{
    private readonly TimeWriterSettings _settings;

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
        var hourPart = hour.ToString().PadLeft(2, '0');
        var minutePart = minute.ToString().PadLeft(2, '0');
        var secondPart = second.ToString().PadLeft(2, '0');
        var millisecondPart = millisecond.ToString().PadLeft(3, '0');

        var parts = new List<string>();
        var partColors = new List<ConsoleColor>();

        if (_settings.ShowHours)
        {
            parts.Add($"{hourPart}:");
            partColors.Add(_settings.Color);
        }

        parts.Add($"{minutePart}");
        partColors.Add(_settings.Color);

        if (_settings.ShowSeconds)
        {
            parts.Add($":{secondPart}");
            partColors.Add(_settings.SecondsColor);
        }

        if (_settings.ShowMilliseconds)
        {
            parts.Add($".{millisecondPart}");
            partColors.Add(_settings.MillisecondsColor);
        }

        Write(parts, partColors, cursorPos, painter);
    }

    private void Write(List<string> textParts, List<ConsoleColor> colors, (int x, int y) cursorPos, Painter painter)
    {
        var font = Font.OfSize(_settings.FontSize);
        var rows = new List<(List<string> parts, List<ConsoleColor> colors)>();
        rows.AddRange(
            Enumerable.Range(0, font.CharacterHeight)
            .Select(_ => (new List<string>(), new List<ConsoleColor>())));

        for (int idx = 0; idx < textParts.Count; idx++)
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
        painter.Paint(data, cursorPos);
    }
}
