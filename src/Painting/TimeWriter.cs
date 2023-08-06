namespace Paraclete.Painting;

using Paraclete.Ansi;
using Paraclete.Layouts;

public class TimeWriter
{
    private readonly TimeWriterSettings _settings;
    private (int hour, int minute, int second, int millisecond, (int x, int y) cursorPos) _cache;

    public TimeWriter(TimeWriterSettings settings)
    {
        _settings = settings;
    }

    public void Write(TimeSpan timestamp, (int x, int y) cursorPos, Painter painter, Pane? pane = null)
    {
        Write(timestamp.Hours, timestamp.Minutes, timestamp.Seconds, timestamp.Milliseconds, cursorPos, painter, pane);
    }

    public void Write(DateTime timestamp, (int x, int y) cursorPos, Painter painter, Pane? pane = null)
    {
        Write(timestamp.Hour, timestamp.Minute, timestamp.Second, timestamp.Millisecond, cursorPos, painter, pane);
    }

    private void Write(int hour, int minute, int second, int millisecond, (int x, int y) cursorPos, Painter painter, Pane? pane)
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
        var partColors = new List<AnsiControlSequence>();
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

        Write(parts, partColors, partFonts, cursorPos, painter, pane);
    }

    private void Write(List<string> textParts, List<AnsiControlSequence> colors, List<Font> fonts, (int x, int y) cursorPos, Painter painter, Pane? pane)
    {
        var textHeight = fonts.Max(_ => _.CharacterHeight);
        var rows = Enumerable.Range(0, textHeight)
            .Select(_ => new AnsiStringBuilder())
            .ToArray();

        0.To(textParts.Count).Foreach(idx =>
        {
            var font = fonts[idx];
            var textPartRows = new string[textHeight];

            var text = textParts[idx];
            foreach (var c in text)
            {
                var fontCharacter = font[c];
                var yOffset = textHeight - font.CharacterHeight;
                0.To(textHeight).Foreach(y =>
                {
                    if (y < yOffset)
                    {
                        textPartRows[y] += string.Empty.PadLeft(font.CharacterWidth);
                    }
                    else
                    {
                        textPartRows[y] += fontCharacter[y - yOffset];
                    }
                });
            }

            0.To(textHeight).Foreach(y =>
            {
                rows[y].Append(colors[idx]).Append(textPartRows[y]);
            });
        });

        var content = rows.Select(_ => _.Build());

        if (pane != null)
        {
            painter.PaintRows(content, pane, cursorPos);
        }
        else
        {
            painter.PaintRows(content, cursorPos);
        }
    }
}
