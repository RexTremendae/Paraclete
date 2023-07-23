namespace Paraclete.Screens;

using Paraclete.Ansi;
using Paraclete.Painting;
using static System.Console;

public class ScreenSaver
{
    private readonly TimeSpan _changeInterval;
    private readonly TimeSpan _activationInterval;

    private readonly int _currentTimeWidth;
    private readonly int _currentTimeHeight;

    private readonly Painter _painter;

    private TimeWriterSettings _currentTimeSettings;

    private (int x, int y) _currentTimePosition;
    private DateTime _lastChange;
    private DateTime _inactivationTime;

    private (AnsiControlSequence color, AnsiControlSequence secondColor)[] _timeColors = new[]
    {
        (AnsiSequences.ForegroundColors.White,   AnsiSequences.ForegroundColors.Gray),
        (AnsiSequences.ForegroundColors.Yellow,  AnsiSequences.ForegroundColors.DarkYellow),
        (AnsiSequences.ForegroundColors.Cyan,    AnsiSequences.ForegroundColors.DarkCyan),
        (AnsiSequences.ForegroundColors.Blue,    AnsiSequences.ForegroundColors.DarkBlue),
        (AnsiSequences.ForegroundColors.Green,   AnsiSequences.ForegroundColors.DarkGreen),
        (AnsiSequences.ForegroundColors.Magenta, AnsiSequences.ForegroundColors.DarkMagenta),
        (AnsiSequences.ForegroundColors.Red,     AnsiSequences.ForegroundColors.DarkRed),
    };

    public ScreenSaver(Painter painter, Settings settings)
    {
        _painter = painter;
        _changeInterval = settings.ScreenSaver.ContentChangeInterval;
        _activationInterval = settings.ScreenSaver.ActivationInterval;

        _currentTimeSettings = new TimeWriterSettings() with {
            FontSize = settings.ScreenSaver.FontSize,
            SecondsFontSize = settings.ScreenSaver.SecondsFontSize,
            Color = AnsiSequences.ForegroundColors.White,
            SecondsColor = AnsiSequences.ForegroundColors.DarkGray,
            ShowHours = true,
            ShowSeconds = true,
            ShowMilliseconds = false,
        };

        var font = Font.OfSize(settings.ScreenSaver.FontSize);
        var secondsFont = Font.OfSize(settings.ScreenSaver.SecondsFontSize);

        _currentTimeHeight = int.Max(font.CharacterHeight, secondsFont.CharacterHeight);
        _currentTimeWidth = (font.CharacterWidth * 5) + (secondsFont.CharacterWidth * 3);
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
            _currentTimePosition.x + _currentTimeWidth >= WindowWidth ||
            _currentTimePosition.y + _currentTimeHeight >= WindowHeight)
        {
            _lastChange = now;
            Write(AnsiSequences.ClearScreen);
            Write(AnsiSequences.HideCursor);

            var x = Random.Shared.Next(WindowWidth - _currentTimeWidth);
            var y = Random.Shared.Next(WindowHeight - _currentTimeHeight);
            _currentTimePosition = (x, y);
            var (color, secondColor) = _timeColors[Random.Shared.Next(_timeColors.Length)];
            _currentTimeSettings = _currentTimeSettings with
            {
                Color = color,
                SecondsColor = secondColor
            };
        }

        WriteTime(now);
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

    private void WriteTime(DateTime now)
    {
        var timeWriter = new TimeWriter(_currentTimeSettings);
        timeWriter.Write(now, _currentTimePosition, _painter);
    }
}
