namespace Paraclete.Screens;

using Paraclete.Ansi;
using Paraclete.Painting;
using static System.Console;

public class ScreenSaver
{
    private readonly TimeSpan _changeInterval;
    private readonly TimeSpan _activationInterval;

    private readonly Painter _painter;

    private readonly (AnsiControlSequence color, AnsiControlSequence secondColor)[] _timeColors = new[]
    {
        (AnsiSequences.ForegroundColors.White,   AnsiSequences.ForegroundColors.Gray),
        (AnsiSequences.ForegroundColors.Yellow,  AnsiSequences.ForegroundColors.DarkYellow),
        (AnsiSequences.ForegroundColors.Cyan,    AnsiSequences.ForegroundColors.DarkCyan),
        (AnsiSequences.ForegroundColors.Blue,    AnsiSequences.ForegroundColors.DarkBlue),
        (AnsiSequences.ForegroundColors.Green,   AnsiSequences.ForegroundColors.DarkGreen),
        (AnsiSequences.ForegroundColors.Magenta, AnsiSequences.ForegroundColors.DarkMagenta),
        (AnsiSequences.ForegroundColors.Red,     AnsiSequences.ForegroundColors.DarkRed),
    };

    private readonly int _timeWidth;
    private readonly int _timeHeight;
    private TimeFormatterSettings _currentTimeSettings;

    private (int x, int y) _currentTimePosition;
    private DateTime _lastChange;
    private DateTime _inactivationTime;

    public ScreenSaver(Painter painter, Settings settings)
    {
        _painter = painter;
        _changeInterval = settings.ScreenSaver.ContentChangeInterval;
        _activationInterval = settings.ScreenSaver.ActivationInterval;

        _currentTimeSettings = new TimeFormatterSettings() with
        {
            FontSize = settings.ScreenSaver.FontSize,
            SecondsFontSize = settings.ScreenSaver.SecondsFontSize,
            Color = AnsiSequences.ForegroundColors.White,
            DateColor = AnsiSequences.ForegroundColors.White,
            SecondsColor = AnsiSequences.ForegroundColors.DarkGray,
            ShowHours = true,
            ShowSeconds = true,
            ShowMilliseconds = false,
            ShowDate = true
        };

        var timeRows = new TimeFormatter(_currentTimeSettings).Format(DateTime.Now).ToArray();
        _timeWidth = timeRows.Max(_ => _.Length);
        _timeHeight = timeRows.Length;
    }

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

        if (now - _lastChange > _changeInterval ||
            _currentTimePosition.x + _timeWidth >= WindowWidth ||
            _currentTimePosition.y + _timeHeight >= WindowHeight)
        {
            _lastChange = now;
            Write(AnsiSequences.ClearScreen);
            Write(AnsiSequences.HideCursor);

            var x = Random.Shared.Next(WindowWidth - _timeWidth);
            var y = Random.Shared.Next(WindowHeight - _timeHeight);
            _currentTimePosition = (x, y);
            var (color, secondColor) = _timeColors[Random.Shared.Next(_timeColors.Length)];
            _currentTimeSettings = _currentTimeSettings with
            {
                Color = color,
                SecondsColor = secondColor
            };
        }

        _painter.PaintRows(new TimeFormatter(_currentTimeSettings).Format(DateTime.Now), _currentTimePosition);
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
}
