using Time.Menu;
using Time.Menu.General;

namespace Time.Screens;

public class HomeScreen : IScreen
{
    private MenuBase _menu;
    public MenuBase Menu => _menu;

    private Stopwatch _stopWatch;
    private FrameInvalidator _frameInvalidator;

    private (int x, int y) _currentTimePosition;
    private (int x, int y) _stopWatchPosition;
    private (int x, int y) _markTimesPosition;

    private TimeWriter _currentTimeWriter;
    private TimeWriter _stopWatchWriter;
    private TimeWriter _markTimeWriter;

    private int _1stColumnWidth = 60;

    public HomeScreen(Stopwatch stopWatch, _MainMenu mainMenu, FrameInvalidator frameInvalidator)
    {
        _menu = mainMenu;

        _stopWatch = stopWatch;
        _frameInvalidator = frameInvalidator;

        _currentTimePosition = (x: 6, y:  2);
        _stopWatchPosition   = (x: 3, y: 12);
        _markTimesPosition   = (x: 3, y: 18);

        var currentTimeSettings = new TimeWriterSettings() with {
            FontSize = Font.Size.L,
            SecondsFontSize = Font.Size.M,
            Color = ConsoleColor.White,
            SecondsColor = ConsoleColor.DarkGray,
            ShowHours = true,
            ShowSeconds = true,
            ShowMilliseconds = false
        };

        var stopWatchSettings = new TimeWriterSettings() with {
            FontSize = Font.Size.M,
            MillisecondsFontSize = Font.Size.S,
            ShowHours = true,
            ShowSeconds = true,
            ShowMilliseconds = true,
            Color = ConsoleColor.Magenta,
            SecondsColor = ConsoleColor.Magenta,
            MillisecondsColor = ConsoleColor.DarkMagenta
        };

        var markTimeSettings = new TimeWriterSettings() with {
            FontSize = Font.Size.XS,
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

    public void PaintFrame(Painter painter, int windowWidth, int windowHeight)
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

        painter.Paint(frameRows);
    }

    public void PaintContent(Painter painter)
    {
        // Current time
        var now = DateTime.Now;
        _currentTimeWriter.Write(now, _currentTimePosition, painter);

        // Stopwatch
        if (_stopWatch.Start != default)
        {
            var stopWatchTime = (_stopWatch.IsRunning ? DateTime.Now : _stopWatch.Stop)
                - _stopWatch.Start;
            _stopWatchWriter.Write(stopWatchTime, _stopWatchPosition, painter);
        }

        // Marked time
        var mx = _markTimesPosition.x;
        var my = _markTimesPosition.y;
        foreach (var mark in _stopWatch.MarkedTimes)
        {
            _markTimeWriter.Write(mark, (mx, my++), painter);
        }

        // TODOs
        painter.Paint(
            position: (_1stColumnWidth+4, 2),
            rows: new[]
            {
                $"{AnsiSequences.ForegroundColors.White}TODO:{AnsiSequences.Reset}",
                $"- {AnsiSequences.ForegroundColors.DarkGray}{AnsiSequences.StrikeThrough}Add TODO section{AnsiSequences.Reset}",
                $"- {AnsiSequences.ForegroundColors.Yellow}Enable add/edit/remove TODO items{AnsiSequences.Reset}",
                $"- {AnsiSequences.ForegroundColors.Yellow}Persist TODO items{AnsiSequences.Reset}"
            });
    }
}
