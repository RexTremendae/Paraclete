using System.Text;
using Paraclete.IO;
using Paraclete.Screens;

using static System.Console;

namespace Paraclete.Painting;

public class Painter
{
    private readonly ScreenInvalidator _screenInvalidator;
    private readonly ScreenSelector _screenSelector;
    private readonly MenuPainter _menuPainter;
    private readonly DataInputter _dataInputter;
    private readonly DataInputPainter _dataInputPainter;

    private int _windowHeight;
    private int _windowWidth;

    public Painter(ScreenSelector screenSelector, ScreenInvalidator screenInvalidator, MenuPainter menuPainter, DataInputter dataInputter, DataInputPainter dataInputPainter)
    {
        _screenInvalidator = screenInvalidator;
        _screenSelector = screenSelector;
        _menuPainter = menuPainter;
        _dataInputter = dataInputter;
        _dataInputPainter = dataInputPainter;
    }

    public void PaintScreen(bool shortcutsMenuActive)
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
        if (_dataInputter.IsActive)
        {
            _dataInputPainter.PaintInput(this, _windowWidth, _windowHeight);
        }
        else
        {
            _menuPainter.PaintMenu(this, shortcutsMenuActive, _windowWidth);
        }
    }

    public void Paint(IEnumerable<PaintSection> sections, (int x, int y)? position = null)
    => PaintRows(new[] { new PaintRow(sections.ToArray()) }, position);

    public void Paint(AnsiString text, (int x, int y)? position = null, ConsoleColor? color = null)
    {
        var formattedText = (color.HasValue ? color.Value.ToAnsiForegroundColorCode() : string.Empty) + text;
        PaintRows(new[] { formattedText }, position);
    }

    public void PaintRows(PaintRow[] rows, (int x, int y)? position = null)
    {
        var formattedRows = new List<AnsiString>();
        foreach (var r in rows)
        {
            var builder = new StringBuilder();
            for (int idx = 0; idx < r.Sections.Length; idx++)
            {
                var part = r.Sections[idx];
                if (part.ForegroundColor.HasValue)
                {
                    builder.Append(part.ForegroundColor.Value.ToAnsiForegroundColorCode());
                }
                if (part.BackgroundColor.HasValue)
                {
                    builder.Append(part.BackgroundColor.Value.ToAnsiBackgroundColorCode());
                }
                builder.Append(part.Text);
            }
            builder.Append(AnsiSequences.Reset);
            formattedRows.Add(builder.ToString());
        }

        PaintRows(formattedRows.ToArray(), position);
    }

    public void PaintRows(AnsiString[] rows, (int x, int y)? position = null)
    {
        var pos = position ?? (0, 0);
        if (pos.x < 0)
        {
            pos = (pos.x + _windowWidth, pos.y);
        }
        if (pos.y < 0)
        {
            pos = (pos.x, pos.y + _windowHeight);
        }

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
