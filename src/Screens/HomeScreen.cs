namespace Paraclete.Screens;

using Paraclete.Ansi;
using Paraclete.Layouts;
using Paraclete.Menu;
using Paraclete.Menu.General;
using Paraclete.Painting;

public class HomeScreen : IScreen
{
    private const int _1stColumnWidth = 59;

    private readonly Stopwatch _stopWatch;
    private readonly ToDoListPainter _toDoListPainter;

    private readonly TimeFormatter _currentTimeFormatter;
    private readonly TimeFormatter _stopWatchFormatter;
    private readonly TimeFormatter _markTimeFormatter;

    private (int x, int y) _currentTimePosition;
    private (int x, int y) _stopWatchPosition;
    private (int x, int y) _markTimesPosition;

    public HomeScreen(Stopwatch stopWatch, MainMenu mainMenu, ToDoListPainter toDoListPainter)
    {
        Menu = mainMenu;

        _stopWatch = stopWatch;
        _toDoListPainter = toDoListPainter;

        _currentTimePosition = (x: 5, y: 1);
        _stopWatchPosition   = (x: 2, y: 1);
        _markTimesPosition   = (x: 2, y: 7);

        var currentTimeSettings = new TimeFormatterSettings() with {
            FontSize = Font.Size.L,
            SecondsFontSize = Font.Size.M,
            Color = AnsiSequences.ForegroundColors.White,
            SecondsColor = AnsiSequences.ForegroundColors.Gray,
            ShowHours = true,
            ShowSeconds = true,
            ShowMilliseconds = false,
            ShowDate = true
        };

        var stopWatchSettings = new TimeFormatterSettings() with {
            FontSize = Font.Size.M,
            MillisecondsFontSize = Font.Size.S,
            ShowHours = true,
            ShowSeconds = true,
            ShowMilliseconds = true,
            Color = AnsiSequences.ForegroundColors.Magenta,
            SecondsColor = AnsiSequences.ForegroundColors.Magenta,
            MillisecondsColor = AnsiSequences.ForegroundColors.DarkMagenta
        };

        var markTimeSettings = new TimeFormatterSettings() with {
            FontSize = Font.Size.XS,
            ShowHours = true,
            ShowSeconds = true,
            ShowMilliseconds = true,
            Color = AnsiSequences.ForegroundColors.Magenta,
            SecondsColor = AnsiSequences.ForegroundColors.Magenta,
            MillisecondsColor = AnsiSequences.ForegroundColors.DarkMagenta
        };

        _currentTimeFormatter = new (currentTimeSettings);
        _stopWatchFormatter = new (stopWatchSettings);
        _markTimeFormatter = new (markTimeSettings);
    }

    public string Name => "Home";
    public ConsoleKey Shortcut => ConsoleKey.F1;
    public bool ShowCurrentTime => false;
    public int[] AutoRefreshingPaneIndices => new[] { 0, 1 };

    public MenuBase Menu { get; }

    public ILayout Layout { get; } = new ColumnBasedLayout(new ColumnBasedLayout.ColumnDefinition[]
    {
        new (width: _1stColumnWidth, 9),
    });

    public Action GetPaintPaneAction(Painter painter, int paneIndex) => () =>
    {
        var pane = Layout.Panes[paneIndex];

        (paneIndex switch
        {
            0 => () =>
            {
                // Current time
                var now = DateTime.Now;
                painter.PaintRows(_currentTimeFormatter.Format(now), pane, _currentTimePosition);
            },

            1 => () =>
            {
                // Stopwatch
                if (_stopWatch.Start != default)
                {
                    var stopWatchTime = (_stopWatch.IsRunning ? DateTime.Now : _stopWatch.Stop)
                        - _stopWatch.Start;
                    painter.PaintRows(_stopWatchFormatter.Format(stopWatchTime), pane, _stopWatchPosition);
                }

                // Marked time
                var mx = _markTimesPosition.x;
                var my = _markTimesPosition.y;
                foreach (var mark in _stopWatch.MarkedTimes)
                {
                    painter.PaintRows(_markTimeFormatter.Format(mark), pane, (mx, my++));
                }
            },

            2 => () =>
            {
                // ToDos
                _toDoListPainter.Paint(Layout.Panes[2], (1, 1));
            },

            _ => (Action)(() => { })
        })();
    };
}
