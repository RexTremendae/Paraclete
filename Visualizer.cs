using static System.Console;

namespace Time;

public class Visualizer
{
    private readonly StopWatch _stopWatch;

    private int _windowHeight;
    private int _windowWidth;
    private bool _isFrameValid;

    private (int x, int y) _currentTimePosition;
    private (int x, int y) _stopWatchPosition;
    private (int x, int y) _markTimesPosition;

    private TimeWriter _currentTimeWriter;
    private TimeWriter _stopWatchWriter;
    private TimeWriter _markTimeWriter;

    public Visualizer(StopWatch stopWatch)
    {
        _stopWatch = stopWatch;

        _currentTimePosition = (x: 3, y:  2);
        _stopWatchPosition   = (x: 3, y: 12);
        _markTimesPosition   = (x: 3, y: 18);

        var currentTimeSettings = new TimeWriterSettings() with {
            FontSize = 3,
            Color = ConsoleColor.White,
            SecondsColor = ConsoleColor.DarkGray,
            ShowHours = true,
            ShowSeconds = true,
            ShowMilliseconds = false
        };

        var stopWatchSettings = new TimeWriterSettings() with {
            FontSize = 2,
            ShowHours = true,
            ShowSeconds = true,
            ShowMilliseconds = true,
            Color = ConsoleColor.Magenta,
            SecondsColor = ConsoleColor.Magenta,
            MillisecondsColor = ConsoleColor.DarkMagenta
        };

        var markTimeSettings = new TimeWriterSettings() with {
            FontSize = 1,
            ShowHours = true,
            ShowSeconds = true,
            ShowMilliseconds = true,
            Color = ConsoleColor.Magenta,
            SecondsColor = ConsoleColor.Magenta,
            MillisecondsColor = ConsoleColor.DarkMagenta
        };

        _currentTimeWriter = new TimeWriter(currentTimeSettings);
        _stopWatchWriter = new TimeWriter(stopWatchSettings);
        _markTimeWriter = new TimeWriter(markTimeSettings);
    }

    public void PaintFrame(params (ConsoleKey key, string description)[] menuOptions)
    {
        if (_isFrameValid && _windowHeight == WindowHeight && _windowWidth == WindowWidth)
        {
            return;
        }

        _windowHeight = WindowHeight;
        _windowWidth = WindowWidth;

        CursorLeft = 0;
        CursorTop = 0;
        var frameRows = new string[_windowHeight];
        frameRows[0] = $"╔{"".PadLeft(_windowWidth-2, '═')}╗";
        for (int y = 1; y < WindowHeight-1; y++)
        {
            if (y == 10 || y == WindowHeight-3)
            {
                frameRows[y] = $"╟{"".PadLeft(_windowWidth-2, '─')}╢";
            }
            else
            {
                frameRows[y] = $"║{"".PadLeft(_windowWidth-2)}║";
            }
        }
        frameRows[_windowHeight-1] = $"╚{"".PadLeft(_windowWidth-2, '═')}╝";

        for (int y = 0; y < _windowHeight; y++)
        {
            CursorLeft = 0;
            CursorTop = y;
            if (_windowHeight != WindowHeight || _windowWidth != WindowWidth)
            {
                return;
            }
            Write(frameRows[y]);
        }

        CursorLeft = 2;
        CursorTop = _windowHeight-2;

        foreach (var (key, description) in menuOptions)
        {
            Write("[");
            ForegroundColor = ConsoleColor.Green;
            Write(key);
            ResetColor();
            Write("] ");
            Write(description + "    ");
        }

        _isFrameValid = true;
    }

    public void InvalidateFrame()
    {
        _isFrameValid = false;
    }

    public void PaintContent()
    {
        var now = DateTime.Now;
        _currentTimeWriter.Write(now, _currentTimePosition);

        if (_stopWatch.Start != default)
        {
            var stopWatchTime = (_stopWatch.IsRunning ? DateTime.Now : _stopWatch.Stop)
                - _stopWatch.Start;
            _stopWatchWriter.Write(stopWatchTime, _stopWatchPosition);
        }

        var mx = _markTimesPosition.x;
        var my = _markTimesPosition.y;
        foreach (var mark in _stopWatch.MarkedTimes)
        {
            _markTimeWriter.Write(mark, (mx, my++));
        }
    }
}
