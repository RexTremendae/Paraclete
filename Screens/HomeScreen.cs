using Time.Menu;

namespace Time.Screens;

public class HomeScreen : ScreenBase
{
    private IMenu _menu;
    public override IMenu Menu => _menu;

    private Stopwatch _stopWatch;
    private FrameInvalidator _frameInvalidator;

    private (int x, int y) _currentTimePosition;
    private (int x, int y) _stopWatchPosition;
    private (int x, int y) _markTimesPosition;

    private TimeWriter _currentTimeWriter;
    private TimeWriter _stopWatchWriter;
    private TimeWriter _markTimeWriter;

    private int _1stColumnWidth = 63;

    public HomeScreen(Stopwatch stopWatch, MainMenu mainMenu, FrameInvalidator frameInvalidator)
    {
        _menu = mainMenu;

        _stopWatch = stopWatch;
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

    public override void PaintFrame(Visualizer visualizer, int windowWidth, int windowHeight)
    {
        var _2ndColumnWidth = windowWidth-3-_1stColumnWidth;

        if (_1stColumnWidth < 0)
            throw new ArgumentOutOfRangeException(nameof(_1stColumnWidth));

        if (_2ndColumnWidth < 0)
            throw new ArgumentOutOfRangeException(nameof(_2ndColumnWidth));

        var frameRows = new string[windowHeight];
        frameRows[0] = $"╔{"".PadLeft(_1stColumnWidth, '═')}╤{"".PadLeft(_2ndColumnWidth, '═')}╗";
        for (int y = 1; y < windowHeight-1; y++)
        {
            if (y == 10)
            {
                frameRows[y] = $"╟{"".PadLeft(_1stColumnWidth, '─')}┤{"".PadLeft(_2ndColumnWidth)}║";
            }
            else if (y == windowHeight-3)
            {
                frameRows[y] = $"╟{"".PadLeft(_1stColumnWidth, '─')}┴{"".PadLeft(_2ndColumnWidth, '─')}╢";
            }
            else if (y == windowHeight-2)
            {
                frameRows[y] = $"║{"".PadLeft(windowWidth-2)}║";
            }
            else
            {
                frameRows[y] = $"║{"".PadLeft(_1stColumnWidth)}│{"".PadLeft(_2ndColumnWidth)}║";
            }
        }
        frameRows[windowHeight-1] = $"╚{"".PadLeft(windowWidth-2, '═')}╝";

        visualizer.Paint(frameRows);
    }

    public override void PaintContent(Visualizer visualizer)
    {
        // Current time
        var now = DateTime.Now;
        _currentTimeWriter.Write(now, _currentTimePosition, visualizer);

        // Stopwatch
        if (_stopWatch.Start != default)
        {
            var stopWatchTime = (_stopWatch.IsRunning ? DateTime.Now : _stopWatch.Stop)
                - _stopWatch.Start;
            _stopWatchWriter.Write(stopWatchTime, _stopWatchPosition, visualizer);
        }

        // Marked time
        var mx = _markTimesPosition.x;
        var my = _markTimesPosition.y;
        foreach (var mark in _stopWatch.MarkedTimes)
        {
            _markTimeWriter.Write(mark, (mx, my++), visualizer);
        }

        // TODOs
        visualizer.Paint(
            position: (_1stColumnWidth+4, 2),
            rows: new[]
            {
                "\x1b[97mTODO:\x1b[m",
                "- \x1b[9m\x1b[90mAdd TODO section\x1b[m",
                "- \x1b[93mEnable add/edit/remove TODO items\x1b[m",
                "- \x1b[93mPersist TODO items\x1b[m"
            });
    }
}
