using System.Text;
using Time.Screens;

using static System.Console;

namespace Time;

public class Painter
{
    private readonly FrameInvalidator _frameInvalidator;
    private readonly ScreenSelector _screenSelector;

    private int _windowHeight;
    private int _windowWidth;

    public Painter(ScreenSelector screenSelector, FrameInvalidator frameInvalidator)
    {
        _frameInvalidator = frameInvalidator;
        _screenSelector = screenSelector;
    }

    public void PaintScreen()
    {
        if (!_frameInvalidator.IsValid || _windowHeight != WindowHeight || _windowWidth != WindowWidth)
        {
            Write(AnsiSequences.ClearScreen);
            CursorVisible = false;

            _windowHeight = WindowHeight;
            _windowWidth = WindowWidth;
            _screenSelector.SelectedScreen.PaintFrame(this, _windowWidth, _windowHeight);
            _frameInvalidator.Reset();
        }

        _screenSelector.SelectedScreen.PaintContent(this);

        PaintMenu();
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
                fr.Append(r.colors[idx].GetAnsiColorCode());
                fr.Append(r.parts[idx]);
            }
            fr.Append(AnsiSequences.Reset);
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

        var menuItems = _screenSelector.SelectedScreen.Menu.MenuItems;
        var row = (parts: new List<string>(), colors: new List<ConsoleColor>());

        var isFirst = true;
        foreach (var (key, description) in menuItems.Select(_ => (_.Key, _.Value.Description)))
        {
            if (isFirst)
            {
                isFirst = false;
            }
            else
            {
                row.parts.Add("    ");
                row.colors.Add(ConsoleColor.Gray);
            }

            var (parts, colors) = GetMenuParts(key, description);
            row.parts.AddRange(parts);
            row.colors.AddRange(colors);
        }

        var data = new[] { row }.Select(_ => (_.parts.ToArray(), _.colors.ToArray())).ToArray();
        Paint(data, (2, _windowHeight-2), true);
    }

    private (IEnumerable<string> parts, IEnumerable<ConsoleColor> colors) GetMenuParts(ConsoleKey key, string description)
    {
        var bracketColor = ConsoleColor.White;
        var shortcutColor = ConsoleColor.Green;
        var descriptionColor = ConsoleColor.Gray;

        var parts = new List<string>();
        var colors = new List<ConsoleColor>();

        var startBracketIndex = description.IndexOf('[');
        var endBracketIndex = -1;
        if (startBracketIndex >= 0)
        {
            endBracketIndex = description.IndexOf(']', startBracketIndex);
        }

        var explicitBrackets = startBracketIndex >= 0 && endBracketIndex >= 0;

        if (explicitBrackets)
        {
            parts.Add(description[..startBracketIndex]);
            colors.Add(descriptionColor);
        }

        parts.Add("[");
        colors.Add(bracketColor);

        parts.Add(key.ToString());
        colors.Add(shortcutColor);

        parts.Add(explicitBrackets ? "]" : "] ");
        colors.Add(bracketColor);

        parts.Add(description[(endBracketIndex+1)..]);
        colors.Add(descriptionColor);

        return (parts, colors);
    }
}
