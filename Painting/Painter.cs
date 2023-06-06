using System.Text;
using Paraclete.Screens;

using static System.Console;

namespace Paraclete.Painting;

public class Painter
{
    private readonly ScreenInvalidator _screenInvalidator;
    private readonly ScreenSelector _screenSelector;
    private readonly MenuPainter _menuPainter;

    private int _windowHeight;
    private int _windowWidth;

    public Painter(ScreenSelector screenSelector, ScreenInvalidator screenInvalidator, MenuPainter menuPainter)
    {
        _screenInvalidator = screenInvalidator;
        _screenSelector = screenSelector;
        _menuPainter = menuPainter;
    }

    public void PaintScreen()
    {
        if (!_screenInvalidator.IsValid || _windowHeight != WindowHeight || _windowWidth != WindowWidth)
        {
            Write(AnsiSequences.ClearScreen);
            CursorVisible = false;

            _windowHeight = WindowHeight;
            _windowWidth = WindowWidth;
            _screenSelector.SelectedScreen.Layout.Paint(this, _windowWidth, _windowHeight);
            _screenInvalidator.Reset();
        }

        _screenSelector.SelectedScreen.PaintContent(this);
        _menuPainter.PaintMenu(this);
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
}
