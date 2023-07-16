namespace Paraclete.Screens;

using Paraclete.Layouts;
using Paraclete.Menu;
using Paraclete.Menu.General;
using Paraclete.Painting;

public class HomeScreen : IScreen
{
    private const int _1stColumnWidth = 59;
    private ColumnBasedLayout _layout = new (new ColumnBasedLayout.ColumnDefinition[] { new (width: _1stColumnWidth, 9) });

    private Stopwatch _stopWatch;
    private ScreenInvalidator _screenInvalidator;
    private ToDoListPainter _toDoListPainter;

    private (int x, int y) _currentTimePosition;
    private (int x, int y) _stopWatchPosition;
    private (int x, int y) _markTimesPosition;

    private TimeWriter _currentTimeWriter;
    private TimeWriter _stopWatchWriter;
    private TimeWriter _markTimeWriter;

    public HomeScreen(Stopwatch stopWatch, MainMenu mainMenu, ScreenInvalidator screenInvalidator, ToDoListPainter toDoListPainter)
    {
        Menu = mainMenu;

        _stopWatch = stopWatch;
        _screenInvalidator = screenInvalidator;
        _toDoListPainter = toDoListPainter;

        _currentTimePosition = (x: 6, y:  2);
        _stopWatchPosition   = (x: 3, y: 12);
        _markTimesPosition   = (x: 3, y: 18);

        var currentTimeSettings = new TimeWriterSettings() with {
            FontSize = Font.Size.L,
            SecondsFontSize = Font.Size.M,
            Color = ConsoleColor.White,
            SecondsColor = ConsoleColor.Gray,
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

    public string Name => "Home";
    public ConsoleKey Shortcut => ConsoleKey.F1;

    public MenuBase Menu { get; }

    public ILayout Layout => _layout;

    public void PaintContent(Painter painter, int windowWidth, int windowHeight)
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

        // ToDos
        _toDoListPainter.Paint((_1stColumnWidth + 4, 2));
    }
}
