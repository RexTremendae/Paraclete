namespace Paraclete.Screens;

using Paraclete.Ansi;
using Paraclete.Layouts;
using Paraclete.Menu;
using Paraclete.Menu.General;
using Paraclete.Painting;

public class HomeScreen(Stopwatch stopWatch, MainMenu mainMenu, ToDoListPainter toDoListPainter) : IScreen
{
    private const int _1stColumnWidth = 59;

    private readonly Stopwatch _stopWatch = stopWatch;
    private readonly ToDoListPainter _toDoListPainter = toDoListPainter;

    private readonly TimeFormatter _currentTimeFormatter = new(new TimeFormatterSettings() with {
            FontSize = Font.Size.L,
            SecondsFontSize = Font.Size.M,
            Color = AnsiSequences.ForegroundColors.White,
            SecondsColor = AnsiSequences.ForegroundColors.Gray,
            ShowHours = true,
            ShowSeconds = true,
            ShowMilliseconds = false,
            ShowDate = true,
        });

    private readonly TimeFormatter _stopWatchFormatter = new(new TimeFormatterSettings() with {
            FontSize = Font.Size.M,
            MillisecondsFontSize = Font.Size.S,
            ShowHours = true,
            ShowSeconds = true,
            ShowMilliseconds = true,
            Color = AnsiSequences.ForegroundColors.Magenta,
            SecondsColor = AnsiSequences.ForegroundColors.Magenta,
            MillisecondsColor = AnsiSequences.ForegroundColors.DarkMagenta,
        });

    private readonly TimeFormatter _markTimeFormatter = new(new TimeFormatterSettings() with {
            FontSize = Font.Size.XS,
            ShowHours = true,
            ShowSeconds = true,
            ShowMilliseconds = true,
            Color = AnsiSequences.ForegroundColors.Magenta,
            SecondsColor = AnsiSequences.ForegroundColors.Magenta,
            MillisecondsColor = AnsiSequences.ForegroundColors.DarkMagenta,
        });

    private (int X, int Y) _currentTimePosition = (X: 5, Y: 1);
    private (int X, int Y) _stopWatchPosition   = (X: 2, Y: 1);
    private (int X, int Y) _markTimesPosition   = (X: 2, Y: 7);

    public string Name => "Home";
    public ConsoleKey Shortcut => ConsoleKey.F1;
    public bool ShowCurrentTime => false;
    public bool ShowTitle => false;
    public int[] AutoRefreshingPaneIndices => [0, 1];

    public MenuBase Menu { get; } = mainMenu;

    public ILayout Layout { get; } = new ColumnBasedLayout(new ColumnBasedLayout.ColumnDefinition(width: _1stColumnWidth, 9));

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
                    painter.PaintRows(_stopWatchFormatter.Format(_stopWatch.CurrentDuration), pane, _stopWatchPosition);
                }

                // Marked time
                var mx = _markTimesPosition.X;
                var my = _markTimesPosition.Y;
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

            _ => (Action)(() => { }),
        })();
    };
}
