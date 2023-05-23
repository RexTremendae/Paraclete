using Time.Menu;
using static System.Console;

namespace Time;

public class Visualizer
{
    private readonly Stopwatch _stopWatch;
    private readonly MainMenu _mainMenu;
    private readonly FrameInvalidator _frameInvalidator;

    private int _1stColumnWidth = 63;

    private int _windowHeight;
    private int _windowWidth;

    private (int x, int y) _currentTimePosition;
    private (int x, int y) _stopWatchPosition;
    private (int x, int y) _markTimesPosition;

    private TimeWriter _currentTimeWriter;
    private TimeWriter _stopWatchWriter;
    private TimeWriter _markTimeWriter;

    public Visualizer(Stopwatch stopWatch, MainMenu mainMenu, FrameInvalidator frameInvalidator)
    {
        _stopWatch = stopWatch;
        _mainMenu = mainMenu;
        _frameInvalidator = frameInvalidator;

        _currentTimePosition = (x: 5, y:  2);
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

    public void PaintScreen()
    {
        PaintFrame();
        PaintContent();
    }

    private void PaintFrame()
    {
        if (_frameInvalidator.IsValid && _windowHeight == WindowHeight && _windowWidth == WindowWidth)
        {
            return;
        }

        _windowHeight = WindowHeight;
        _windowWidth = WindowWidth;

        var _2ndColumnWidth = _windowWidth-3-_1stColumnWidth;

        CursorLeft = 0;
        CursorTop = 0;
        var frameRows = new string[_windowHeight];
        frameRows[0] = $"╔{"".PadLeft(_1stColumnWidth, '═')}╤{"".PadLeft(_2ndColumnWidth, '═')}╗";
        for (int y = 1; y < WindowHeight-1; y++)
        {
            if (y == 10)
            {
                frameRows[y] = $"╟{"".PadLeft(_1stColumnWidth, '─')}┤{"".PadLeft(_2ndColumnWidth)}║";
            }
            else if (y == WindowHeight-3)
            {
                frameRows[y] = $"╟{"".PadLeft(_1stColumnWidth, '─')}┴{"".PadLeft(_2ndColumnWidth, '─')}╢";
            }
            else if (y == WindowHeight-2)
            {
                frameRows[y] = $"║{"".PadLeft(_windowWidth-2)}║";
            }
            else
            {
                frameRows[y] = $"║{"".PadLeft(_1stColumnWidth)}│{"".PadLeft(_2ndColumnWidth)}║";
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

        PaintMenu();
        _frameInvalidator.Reset();
    }

    private void PaintMenu()
    {
        CursorLeft = 2;
        CursorTop = _windowHeight-2;

        foreach (var (key, description) in _mainMenu.MenuItems.Select(_ => (_.Shortcut, _.Description)))
        {
            Write("[");
            ForegroundColor = ConsoleColor.Green;
            Write(key);
            ResetColor();
            Write("] ");
            Write(description + "    ");
        }
    }

    private void PaintContent()
    {
        // Current time
        var now = DateTime.Now;
        _currentTimeWriter.Write(now, _currentTimePosition);

        // Stopwatch
        if (_stopWatch.Start != default)
        {
            var stopWatchTime = (_stopWatch.IsRunning ? DateTime.Now : _stopWatch.Stop)
                - _stopWatch.Start;
            _stopWatchWriter.Write(stopWatchTime, _stopWatchPosition);
        }

        // Marked time
        var mx = _markTimesPosition.x;
        var my = _markTimesPosition.y;
        foreach (var mark in _stopWatch.MarkedTimes)
        {
            _markTimeWriter.Write(mark, (mx, my++));
        }

        // \x1b[m   - reset
        // \x1b[90m - change fg color to 90 (dark gray)
        // \x1b[93m - change fg color to 93 (yellow)
        // \x1b[97m - change fg color to 97 (white)
        // \x1b[9m  - start strike-through

        // TODOs
        SetCursorPosition(_1stColumnWidth+4, 2);
        Write("\x1b[97mTODO:\x1b[m");
        SetCursorPosition(_1stColumnWidth+4, 3);
        Write("- \x1b[9m\x1b[90mAdd TODO section\x1b[m");
        SetCursorPosition(_1stColumnWidth+4, 4);
        Write("- \x1b[93mEnable add/edit/remove TODO items\x1b[m");
        SetCursorPosition(_1stColumnWidth+4, 5);
        Write("- \x1b[93mPersist TODO items\x1b[m");
    }
}
