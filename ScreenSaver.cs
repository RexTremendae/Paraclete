using static System.Console;

namespace Time;

public class ScreenSaver
{
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
                _inactivationTime = default;
                _lastChange = default;
                return true;
            }

            return false;
        }
    }

    TimeWriterSettings _currentTimeSettings = new TimeWriterSettings() with {
        FontSize = 3,
        Color = ConsoleColor.White,
        SecondsColor = ConsoleColor.DarkGray,
        ShowHours = true,
        ShowSeconds = true,
        ShowMilliseconds = false
    };

    private (int x, int y) _currentTimePosition;
    private TimeSpan _changeInterval = TimeSpan.FromSeconds(30);
    private TimeSpan _activationInterval = TimeSpan.FromMinutes(5);
    private DateTime _lastChange;
    private DateTime _inactivationTime;

    private (ConsoleColor color, ConsoleColor secondColor)[] _timeColors = new[] {
        (ConsoleColor.White, ConsoleColor.DarkGray),
        (ConsoleColor.Yellow, ConsoleColor.DarkYellow),
        (ConsoleColor.Cyan, ConsoleColor.DarkCyan),
        (ConsoleColor.Blue, ConsoleColor.DarkBlue),
        (ConsoleColor.Green, ConsoleColor.DarkGreen),
        (ConsoleColor.Magenta, ConsoleColor.DarkMagenta),
        (ConsoleColor.Red, ConsoleColor.DarkRed)
    };

    public void PaintScreen()
    {
        var currentTimeWidth = 56;
        var currentTimeHeight = 7;

        var now = DateTime.Now;
        if (now - _lastChange > _changeInterval ||
            _currentTimePosition.x + currentTimeWidth >= WindowWidth ||
            _currentTimePosition.y + currentTimeHeight >= WindowHeight)
        {
            _lastChange = now;
            Clear();
            _currentTimePosition = (Random.Shared.Next(WindowWidth-56), Random.Shared.Next(WindowHeight-7));
            var (color, secondColor) = _timeColors[Random.Shared.Next(_timeColors.Length)];
            _currentTimeSettings = _currentTimeSettings with
            {
                Color = color,
                SecondsColor = secondColor
            };
        }

        WriteTime(now);
    }

    public void Inactivate()
    {
        _inactivationTime = DateTime.Now;
    }

    private void WriteTime(DateTime now)
    {
        var timeWriter = new TimeWriter(_currentTimeSettings);
        timeWriter.Write(now, _currentTimePosition);
    }
}
