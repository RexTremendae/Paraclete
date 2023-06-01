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
            fr.Append("\x1b[m");
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

    private string GetAnsiColorCode(ConsoleColor color)
    {
        var colorCode = color switch
        {
            ConsoleColor.Black => "30",
            ConsoleColor.DarkRed => "31",
            ConsoleColor.DarkGreen => "32",
            ConsoleColor.DarkYellow => "33",
            ConsoleColor.DarkBlue => "34",
            ConsoleColor.DarkMagenta => "35",
            ConsoleColor.DarkCyan => "36",
            ConsoleColor.Gray => "37",
            ConsoleColor.DarkGray => "90",
            ConsoleColor.Red => "91",
            ConsoleColor.Green => "92",
            ConsoleColor.Yellow => "93",
            ConsoleColor.Blue => "94",
            ConsoleColor.Magenta => "95",
            ConsoleColor.Cyan => "96",
            ConsoleColor.White => "97",
            _ => throw new ArgumentException(message: $"Undefined color: {color}", paramName: nameof(color))
        };

        return $"\x1b[{colorCode}m";
    }
}
