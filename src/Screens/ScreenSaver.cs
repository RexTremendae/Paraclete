namespace Paraclete.Screens;

using Paraclete.Ansi;
using Paraclete.Painting;
using static System.Console;

public class ScreenSaver(Painter painter, Settings settings)
{
    private readonly TimeSpan _changeInterval = settings.ScreenSaver.ContentChangeInterval;
    private readonly TimeSpan _activationInterval = settings.ScreenSaver.ActivationInterval;

    private readonly Painter _painter = painter;

    private readonly (AnsiControlSequence Color, AnsiControlSequence SecondColor)[] _timeColors =
    [
        (AnsiSequences.ForegroundColors.White,   AnsiSequences.ForegroundColors.Gray),
        (AnsiSequences.ForegroundColors.Yellow,  AnsiSequences.ForegroundColors.DarkYellow),
        (AnsiSequences.ForegroundColors.Cyan,    AnsiSequences.ForegroundColors.DarkCyan),
        (AnsiSequences.ForegroundColors.Blue,    AnsiSequences.ForegroundColors.DarkBlue),
        (AnsiSequences.ForegroundColors.Green,   AnsiSequences.ForegroundColors.DarkGreen),
        (AnsiSequences.ForegroundColors.Magenta, AnsiSequences.ForegroundColors.DarkMagenta),
        (AnsiSequences.ForegroundColors.Red,     AnsiSequences.ForegroundColors.DarkRed),
    ];

    private int _timeWidth;
    private int _timeHeight;
    private TimeFormatterSettings _currentTimeSettings = new TimeFormatterSettings() with
        {
            FontSize = settings.ScreenSaver.FontSize,
            SecondsFontSize = settings.ScreenSaver.SecondsFontSize,
            Color = AnsiSequences.ForegroundColors.White,
            DateColor = AnsiSequences.ForegroundColors.White,
            SecondsColor = AnsiSequences.ForegroundColors.DarkGray,
            ShowHours = true,
            ShowSeconds = true,
            ShowMilliseconds = false,
            ShowDate = true,
        };

    private (int X, int Y) _currentTimePosition;
    private DateTime _lastChange;
    private DateTime _lastPaint;
    private DateTime _inactivationTime;

    public bool IsActive
    {
        get
        {
            if (_inactivationTime == default)
            {
                return true;
            }

            if (_inactivationTime + _activationInterval < DateTime.Now)
            {
                Activate();
                return true;
            }

            return false;
        }
    }

    public void PaintScreen()
    {
        var now = DateTime.Now;

        var repaint = now - _lastPaint > TimeSpan.FromSeconds(1);

        if (now - _lastChange > _changeInterval ||
            _currentTimePosition.X + _timeWidth >= WindowWidth ||
            _currentTimePosition.Y + _timeHeight >= WindowHeight)
        {
            var x = Random.Shared.Next(WindowWidth - _timeWidth);
            var y = Random.Shared.Next(WindowHeight - _timeHeight);
            _currentTimePosition = (x, y);

            var (color, secondColor) = _timeColors[Random.Shared.Next(_timeColors.Length)];
            _currentTimeSettings = _currentTimeSettings with
            {
                Color = color,
                SecondsColor = secondColor,
            };

            Write(AnsiSequences.ClearScreen);
            Write(AnsiSequences.EraseScrollbackBuffer);
            Write(AnsiSequences.HideCursor);

            _lastChange = now;
            repaint = true;
        }

        if (repaint)
        {
            var timeRows = GenerateTimeRows().ToArray();
            _timeWidth = timeRows.First().Length;
            _timeHeight = timeRows.Length;

            _painter.PaintRows(timeRows, _currentTimePosition, boundary: (WindowWidth, WindowHeight));
            _lastPaint = now;
        }
    }

    public void Activate()
    {
        _inactivationTime = default;
        _lastChange = default;
    }

    public void Inactivate()
    {
        _inactivationTime = DateTime.Now;
    }

    private IEnumerable<AnsiString> GenerateTimeRows()
    {
        var frameColor = AnsiSequences.ForegroundColors.White;

        var timeRows = new TimeFormatter(_currentTimeSettings)
            .Format(DateTime.Now)
            .Select(_ => " " + _)
            .ToList();
        var width = timeRows.Max(_ => _.Length);

        timeRows.Insert(0, new(string.Empty.PadRight(width)));

        yield return frameColor +  "╭" + string.Empty.PadLeft(width, '─') + "╮";
        foreach (var row in timeRows)
        {
            yield return frameColor + "│" + row.PadRight(width) + frameColor + "│";
        }

        yield return frameColor +  "╰" + string.Empty.PadLeft(width, '─') + "╯";
    }
}
