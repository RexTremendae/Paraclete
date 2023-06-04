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

    public void Paint(IEnumerable<string> parts, IEnumerable<ConsoleColor> colors, (int x, int y)? position = null)
    => PaintRows(new[] { (parts, colors) }, position);

    public void Paint(AnsiString text, (int x, int y)? position = null, ConsoleColor? color = null)
    {
        var formattedText = (color.HasValue ? color.Value.ToAnsiColorCode() : "") + text;
        PaintRows(new[] { formattedText }, position);
    }

    public void PaintRows((IEnumerable<string> parts, IEnumerable<ConsoleColor> colors)[] rows, (int x, int y)? position = null)
    {
        var formattedRows = new List<AnsiString>();
        foreach (var r in rows)
        {
            var parts = r.parts.ToArray();
            var colors = r.colors.ToArray();
            var builder = new StringBuilder();
            for (int idx = 0; idx < parts.Length; idx++)
            {
                builder.Append(colors[idx].ToAnsiColorCode());
                builder.Append(parts[idx]);
            }
            builder.Append(AnsiSequences.Reset);
            formattedRows.Add(builder.ToString());
        }

        PaintRows(formattedRows.ToArray(), position);
    }

    public void PaintRows(AnsiString[] rows, (int x, int y)? position = null)
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

        var screen = _screenSelector.SelectedScreen;
        var menuItems = screen.Menu.MenuItems;
        var row = (parts: new List<string>(), colors: new List<ConsoleColor>());

        if (!string.IsNullOrEmpty(screen.Name))
        {
            var label = $"{AnsiSequences.BackgroundColors.Blue} {screen.Name} {AnsiSequences.Reset}";
            row.parts.AddRange(new[] { label, " |    " });
            row.colors.AddRange(new[] { ConsoleColor.Black, ConsoleColor.Gray });
        }

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

        Paint(row.parts, row.colors, (2, _windowHeight-2));
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

        parts.Add(key.ToDisplayString());
        colors.Add(shortcutColor);

        parts.Add(explicitBrackets ? "]" : "] ");
        colors.Add(bracketColor);

        parts.Add(description[(endBracketIndex+1)..]);
        colors.Add(descriptionColor);

        return (parts, colors);
    }
}
