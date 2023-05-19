using static System.Console;

namespace Time;

public class TimeWriter
{
    private Dictionary<char, string[]> _font;

    public TimeWriter(int fontSize)
    {
        _font = Font.OfSize(fontSize);
    }

    public void Write(DateTime timestamp, (int x, int y) cursorPos)
    {
        var hour = timestamp.Hour.ToString().PadLeft(2, '0');
        var minute = timestamp.Minute.ToString().PadLeft(2, '0');
        var second = timestamp.Second.ToString().PadLeft(2, '0');
        var millisecond = timestamp.Millisecond.ToString().PadLeft(3, '0');

        Write(
            new[] { $"{hour}:{minute}:{second}", $".{millisecond}" },
            new[] { ConsoleColor.Gray, ConsoleColor.DarkGray },
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
