using static System.Console;

namespace Time;

public class TimeWriter
{
    private readonly Dictionary<char, string[]> _font;
    private readonly Settings _settings;

    public TimeWriter(Settings settings)
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

    public void Write(DateTime timestamp, (int x, int y) cursorPos)
    {
        var hour = timestamp.Hour.ToString().PadLeft(2, '0');
        var minute = timestamp.Minute.ToString().PadLeft(2, '0');
        var second = timestamp.Second.ToString().PadLeft(2, '0');
        var millisecond = timestamp.Millisecond.ToString().PadLeft(3, '0');

        var parts = new[] {
            $"{hour}:{minute}:{second}",
            $".{millisecond}"
        };

        var partColors = new[] {
            _settings.Color,
            _settings.MillisecondsColor
        };

        Write(
            _settings.ShowMilliseconds ? parts : parts.Take(1).ToArray(),
            _settings.ShowMilliseconds ? partColors : partColors.Take(1).ToArray(),
            cursorPos);
    }

    private void Write(string[] textParts, ConsoleColor[] colors, (int x, int y) cursorPos)
    {
        cursorPos = (cursorPos.x, cursorPos.y+1);
        var charWidth = _font['0'][0].Length;
        var charHeight = _font['0'].Length;

        CursorLeft = cursorPos.x;
        CursorTop = cursorPos.y;

        var charCount = 0;
        for (int idx = 0; idx < textParts.Length; idx++)
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
