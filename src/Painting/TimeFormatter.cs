namespace Paraclete.Painting;

using Paraclete.Ansi;

public class TimeFormatter
{
    private readonly TimeFormatterSettings _settings;
    private (int hour, int minute, int second, int millisecond) _cacheKey;
    private IEnumerable<AnsiString> _cache;

    public TimeFormatter(TimeFormatterSettings settings)
    {
        _settings = settings;
        _cache = Enumerable.Empty<AnsiString>();
    }

    public IEnumerable<AnsiString> Format(TimeSpan timestamp)
    {
        return Format(
            timestamp.Hours,
            timestamp.Minutes,
            timestamp.Seconds,
            timestamp.Milliseconds,
            string.Empty,
            0,
            string.Empty
        );
    }

    public IEnumerable<AnsiString> Format(DateTime timestamp)
    {
        return Format(
            timestamp.Hour,
            timestamp.Minute,
            timestamp.Second,
            timestamp.Millisecond,
            timestamp.DayOfWeek.ToString(),
            timestamp.Day,
            GetMothName(timestamp.Month));
    }

    private static string GetMothName(int month)
    {
        return month switch
        {
             1 => "January",
             2 => "February",
             3 => "March",
             4 => "April",
             5 => "May",
             6 => "June",
             7 => "July",
             8 => "August",
             9 => "September",
            10 => "October",
            11 => "November",
            12 => "December",
            _ => throw new InvalidDataException($"Invalid month: {month}")
        };
    }

    private IEnumerable<AnsiString> Format(int hour, int minute, int second, int millisecond, string dayName, int day, string monthName)
    {
        var newCacheKey = (hour, minute, second, millisecond);
        if (newCacheKey == _cacheKey)
        {
            return _cache;
        }

        _cacheKey = newCacheKey;

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

        _cache = Format(parts, partColors, partFonts, GetFormattedDate(dayName, day, monthName));
        return _cache;
    }

    private IEnumerable<AnsiString> Format(List<string> textParts, List<AnsiControlSequence> colors, List<Font> fonts, AnsiString formattedDate)
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

        var result = rows.Select(_ => _.Build());

        return (_settings.ShowDate && formattedDate.Length > 0)
            ? result.Concat(new[] { formattedDate })
            : result;
    }

    private AnsiString GetFormattedDate(string dayName, int day, string monthName)
    {
        return _settings.DateColor + $"{dayName} {day} {monthName}";
    }
}
