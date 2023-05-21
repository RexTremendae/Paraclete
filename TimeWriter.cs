using static System.Console;

namespace Time;

public class TimeWriter
{
    private readonly Dictionary<char, string[]> _font;
    private readonly TimeWriterSettings _settings;

    public TimeWriter(TimeWriterSettings settings)
    {
        _settings = settings;
        _font = Font.OfSize(settings.FontSize);
        if (settings.DigitDisplayChar != '#')
        {
            foreach (var key in _font.Keys)
            {
                _font[key] = _font[key].Select(_ => _.Replace('#', settings.DigitDisplayChar)).ToArray();
            }
        }
    }

    public void Write(TimeSpan timestamp, (int x, int y) cursorPos)
    {
        Write(timestamp.Hours, timestamp.Minutes, timestamp.Seconds, timestamp.Milliseconds, cursorPos);
    }

    public void Write(DateTime timestamp, (int x, int y) cursorPos)
    {
        Write(timestamp.Hour, timestamp.Minute, timestamp.Second, timestamp.Millisecond, cursorPos);
    }

    private void Write(int hour, int minute, int second, int millisecond, (int x, int y) cursorPos)
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

        Write(parts, partColors, cursorPos);
    }

    private void Write(List<string> textParts, List<ConsoleColor> colors, (int x, int y) cursorPos)
    {
        var charWidth = _font['0'][0].Length;
        var charHeight = _font['0'].Length;

        CursorLeft = cursorPos.x;
        CursorTop = cursorPos.y;

        var charCount = 0;
        for (int idx = 0; idx < textParts.Count; idx++)
        {
            var text = textParts[idx];
            ForegroundColor = colors[idx];
            foreach (var c in text)
            {
                var chr = _font[c];
                for (var y = 0; y < chr.Length; y++)
                {
                    CursorLeft = cursorPos.x + charCount*charWidth;
                    CursorTop = cursorPos.y + y;
                    Console.Write(chr[y]);
                }

                charCount++;
            }
        }
        ResetColor();
    }
}
