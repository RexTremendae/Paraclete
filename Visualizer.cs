using System.Text;
using Time.Screens;

using static System.Console;

namespace Time;

public class Visualizer
{
    private readonly FrameInvalidator _frameInvalidator;

    private ScreenBase _currentScreen;

    private int _windowHeight;
    private int _windowWidth;

    public Visualizer(HomeScreen homeScreen, FrameInvalidator frameInvalidator)
    {
        _currentScreen = homeScreen;
        _frameInvalidator = frameInvalidator;
    }

    public void PaintScreen()
    {
        if (!_frameInvalidator.IsValid || _windowHeight != WindowHeight || _windowWidth != WindowWidth)
        {
            _windowHeight = WindowHeight;
            _windowWidth = WindowWidth;
            _currentScreen.PaintFrame(this, _windowWidth, _windowHeight);
        }

        _currentScreen.PaintContent(this);

        PaintMenu();
        _frameInvalidator.Reset();
    }

    public void Paint((string[] parts, ConsoleColor[] colors) data, (int x, int y)? position = null, bool debug = false)
    {
        Paint(new[] { data }, position, debug);
    }

    public void Paint((string[] parts, ConsoleColor[] colors)[] rows, (int x, int y)? position = null, bool debug = false)
    {
        var formattedRows = new List<string>();
        foreach (var r in rows)
        {
            var fr = new StringBuilder();
            for (int idx = 0; idx < r.parts.Length; idx++)
            {
                fr.Append(GetAnsiColorCode(r.colors[idx]));
                fr.Append(r.parts[idx]);
            }
            fr.Append(AnsiConstants.Reset);
            formattedRows.Add(fr.ToString());
        }

        Paint(formattedRows.ToArray(), position, debug);
    }

    public void Paint(string[] rows, (int x, int y)? position = null, bool debug = false)
    {
        var pos = position ?? (0, 0);

        for (int y = 0; y < rows.Length; y++)
        {
            var yy = y + pos.y;
            if (yy >= WindowHeight)
            {
                break;
            }
            CursorTop = yy;

            if (pos.x >= WindowWidth)
            {
                break;
            }
            CursorLeft = pos.x;

            Write(rows[y]);
        }
    }

    private void PaintMenu()
    {
        CursorLeft = 2;
        CursorTop = _windowHeight-2;

        var row = (parts: new List<string>(), colors: new List<ConsoleColor>());
        foreach (var (key, description) in _currentScreen.Menu.MenuItems.Select(_ => (_.Shortcut, _.Description)))
        {
            row.parts.Add("[");
            row.colors.Add(ConsoleColor.White);

            row.parts.Add(key.ToString());
            row.colors.Add(ConsoleColor.Green);

            row.parts.Add("] ");
            row.colors.Add(ConsoleColor.White);

            row.parts.Add(description + "    ");
            row.colors.Add(ConsoleColor.Gray);
        }

        var data = new[] { row }.Select(_ => (_.parts.ToArray(), _.colors.ToArray())).ToArray();
        Paint(data, (2, _windowHeight-2), true);
    }

    private string GetAnsiColorCode(ConsoleColor color) => color switch
    {
        ConsoleColor.Black       => AnsiConstants.ForegroundColor.Black,
        ConsoleColor.DarkRed     => AnsiConstants.ForegroundColor.DarkRed,
        ConsoleColor.DarkGreen   => AnsiConstants.ForegroundColor.DarkGreen,
        ConsoleColor.DarkYellow  => AnsiConstants.ForegroundColor.DarkYellow,
        ConsoleColor.DarkBlue    => AnsiConstants.ForegroundColor.DarkBlue,
        ConsoleColor.DarkMagenta => AnsiConstants.ForegroundColor.DarkMagenta,
        ConsoleColor.DarkCyan    => AnsiConstants.ForegroundColor.DarkCyan,
        ConsoleColor.Gray        => AnsiConstants.ForegroundColor.Gray,
        ConsoleColor.DarkGray    => AnsiConstants.ForegroundColor.DarkGray,
        ConsoleColor.Red         => AnsiConstants.ForegroundColor.Red,
        ConsoleColor.Green       => AnsiConstants.ForegroundColor.Green,
        ConsoleColor.Yellow      => AnsiConstants.ForegroundColor.Yellow,
        ConsoleColor.Blue        => AnsiConstants.ForegroundColor.Blue,
        ConsoleColor.Magenta     => AnsiConstants.ForegroundColor.Magenta,
        ConsoleColor.Cyan        => AnsiConstants.ForegroundColor.Cyan,
        ConsoleColor.White       => AnsiConstants.ForegroundColor.White,
        _ => throw new ArgumentException(message: $"Undefined color: {color}", paramName: nameof(color))
    };
}
